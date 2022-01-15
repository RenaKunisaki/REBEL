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
    public class BounceBlock:
    ItemDropBlock<Items.Placeable.BounceBlock>,
    IReactsToTouch {
        /** A block that, when you touch it, makes you bounce
         *  in the opposite direction.
         */
        public override String Texture {
            get => "REBEL/Blocks/Physics/BounceBlock/Block";
        }

        //XXX use slope to determine which directions it works in?
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

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            if(tile.IsActuated) return; //don't react when turned off.

            //apply vertical motion
            //10 units seems good. 100 will cause you to smack your head
            //against the top of the world. 20 can clip you through blocks.
            switch(direction) {
                case TouchDirection.TopLeft:
                case TouchDirection.Top:
                case TouchDirection.TopRight:
                    whom.velocity.Y = -10; break;

                case TouchDirection.BottomLeft:
                case TouchDirection.Bottom:
                case TouchDirection.BottomRight:
                    whom.velocity.Y = 10; break;

                default: break;
            }

            //apply horizontal motion
            //ignore corners here or else we get bounced around in random
            //directions when we land on a solid platform.
            switch(direction) {
                //case TouchDirection.TopLeft:
                case TouchDirection.Left:
                //case TouchDirection.BottomLeft:
                    whom.velocity.X = -10; break;

                //case TouchDirection.TopRight:
                case TouchDirection.Right:
                //case TouchDirection.BottomRight:
                    whom.velocity.X = 10; break;

                default: break;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (Main.tileFrame[Type] % 3) * getFrameHeight();
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 10) {
                frameCounter = 0;
                frame = ++frame % 3;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class BounceBlock: TilePlaceItem<Blocks.BounceBlock, BounceBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Physics/BounceBlock/Item";
        }
        public override String _getName() => "Bounce Block";
        public override String _getDescription() =>
            "A block that bounces away anything that touches it.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.BounceBlock>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.GravitationPotion, 1)
                .AddRecipeGroup("IronBar", 1)
				.Register();
		}
    }
}
