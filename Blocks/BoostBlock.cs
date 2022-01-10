using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class BoostBlock:
    Base.ItemDropBlock<Items.Placeable.BoostBlock>,
    IReactsToTouch {
        /** A block that, when you touch it, gives you a boost in
         *  some direction.
         */
        
        //XXX use slope to determine which directions it works in?
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        static int[] frameXCycle = { 1, 2, 3, 0 };
        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Main.tile[i, j];
			int style = tile.frameY / 18;
            int mode  = (tile.frameX / 18) % frameXCycle.Length;
			int nextFrameX = frameXCycle[mode];
			tile.frameX = (short)(nextFrameX * 18);
            tile.frameY = 0;
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

        public void OnTouched(Player player, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            int mode = (int)(tile.frameX / 18) & 3;
            //Main.NewText(
            //    String.Format("Touched {0} {1}", direction, mode),
            //    0x00, 0x9D, 0xF3);
            
            switch(mode) {
                case 0: player.velocity.Y = -10; break;
                case 1: player.velocity.X =  10; break;
                case 2: player.velocity.Y =  10; break;
                case 3: player.velocity.X = -10; break;
                default: break;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (Main.tileFrame[Type] % 5) * 18;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 10) {
                frameCounter = 0;
                frame = ++frame % 5;
            }
        }
    }
}
