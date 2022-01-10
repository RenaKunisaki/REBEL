using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class HealHurtBlock:
    Base.ItemDropBlock<Items.Placeable.HealHurtBlock>,
    IReactsToTouch {
        /** A block that hurts to touch, or heals you,
         *  depending on its mode.
         */
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Main.tile[i, j];
            tile.frameX = 0;
			tile.frameY ^= 18;
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1,
                    Player.tileTargetX, Player.tileTargetY,
                    1, TileChangeType.None);
			}
            return false;
		}

        public override bool RightClick(int x, int y) {
            /** Called when right-clicked.
             *  Used because apparently we can't hammer non-solid tiles.
             */
            Slope(x, y);
            return true; //we did something, don't do default right click
        }

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            int mode = (int)(tile.frameY / 18) & 3;
            switch(mode) {
                case 0: { //heal
                    if(whom is Player p) {
                        p.HealEffect(1); //visual only
                        p.statLife = Math.Min(p.statLife+1, p.statLifeMax);
                    }
                    else if(whom is NPC n) {
                        n.HealEffect(1);
                        n.life = Math.Min(n.life+1, n.lifeMax);
                    }
                    break;
                }
                case 1: { //hurt
                    if(whom is Player p) {
                        p.Hurt(PlayerDeathReason.ByCustomReason(
                            String.Format(
                                "{0} touched a Hurt Block too much.",
                                p.name)),
                            1, 0);
                    }
                    else if(whom is NPC n) {
                        n.StrikeNPC(1, 0, 0);
                    }
                    break;
                }
                default: break;
            }
        }
    }
}
