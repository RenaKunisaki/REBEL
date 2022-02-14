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
    public class HealBlock:
    ItemDropBlock<Items.Placeable.HealBlock>,
    IReactsToTouch {
        /** A block that restores HP on touch.
         */
        public override String Texture {
            get => "REBEL/Blocks/Misc/HealBlock/Block";
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
            setFrame(i, j, 0, (p.Y+1) & 1);
            return false;
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
            var tile = Framing.GetTileSafely(location.X, location.Y);
            if(tile.IsActuated) return; //don't react when turned off.

            int mode = (int)(tile.TileFrameY / getFrameHeight()) & 1;
            switch(mode) {
                case 0: _onTouchedHeal(whom, location, direction, false); break;
                case 1: _onTouchedHeal(whom, location, direction, true); break;
                default: break;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class HealBlock: TilePlaceItem<Blocks.HealBlock, HealBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Misc/HealBlock/Item";
        }
        public override String _getName() => "Heal Block";
        public override String _getDescription() =>
            "A block that restores HP on contact. Hammer it to change speed.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.HealBlock>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.HealingPotion, 1)
				.AddRecipeGroup("IronBar", 1)
                .AddTile(TileID.Bottles)
				.Register();
		}
    }
}
