using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public class HealHurtBlock:
    ItemDropBlock<Items.Placeable.HealHurtBlock>,
    IReactsToTouch {
        /** A block that hurts to touch, or heals you,
         *  depending on its mode.
         */
        public override String Texture {
            get => "REBEL/Blocks/Misc/HealHurtBlock/Block";
        }

        public override void SetStaticDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
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
			Point p = getFrameBlock(i, j);
            setFrame(i, j, 0, (p.Y+1) & 3);
            return false;
		}

        static String[] deathMessages = {
            "{0} touched a Hurt Block too much.",
            "{0} didn't pay attention to their pain.",
            "{0} touched something they shouldn't.",
            "{0} never learned about hot stoves.",
        };

        protected void _onTouchedHurt(Entity whom, Point location,
        TouchDirection direction, bool fast) {
            int msgNo = Main.rand.Next(0, deathMessages.Length-1);
            String msg = deathMessages[msgNo];

            //NPCs don't have hitstun, so only hurt them every second.
            uint frame = Main.GameUpdateCount % 60;
            if(whom is Player p) {
                p.Hurt(PlayerDeathReason.ByCustomReason(
                    String.Format(msg, p.name)), fast ? 20 : 1, 0);
            }
            else if(whom is NPC n && (frame == 0 || fast)) {
                n.StrikeNPC(1, 0, 0);
                if(n.life <= 0 && n.isLikeATownNPC ) {
                    Main.NewText(String.Format(msg, n.FullName),
                        0xFF, 0x00, 0x00);
                }
            }
        }

        protected void _onTouchedHeal(Entity whom, Point location,
        TouchDirection direction, bool fast) {
            uint frame = Main.GameUpdateCount % 60;
            if(frame != 0 && !fast) return;
            if(whom is Player p) {
                p.HealEffect(1); //visual only
                p.statLife = Math.Min(p.statLife+1, p.statLifeMax);
            }
            else if(whom is NPC n) {
                n.HealEffect(1);
                n.life = Math.Min(n.life+1, n.lifeMax);
            }
        }

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            if(tile.IsActuated) return; //don't react when turned off.

            int mode = (int)(tile.frameY / getFrameHeight()) & 3;
            switch(mode) {
                case 0: _onTouchedHeal(whom, location, direction, false); break;
                case 1: _onTouchedHurt(whom, location, direction, false); break;
                case 2: _onTouchedHeal(whom, location, direction, true); break;
                case 3: _onTouchedHurt(whom, location, direction, true); break;
                default: break;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class HealHurtBlock: TilePlaceItem<Blocks.HealHurtBlock, HealHurtBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Misc/HealHurtBlock/Item";
        }
        public override String _getName() => "Heal/Hurt Block";
        public override String _getDescription() =>
            "A block that hurts or heals on contact. Hammer it to cycle between heal, hurt, heal lots, hurt lots.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.HealHurtBlock>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.HealingPotion, 1)
				.AddIngredient(ItemID.Spike, 1)
                .AddTile(TileID.Bottles)
				.Register();
		}
    }
}
