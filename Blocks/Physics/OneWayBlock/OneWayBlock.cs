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
    public class OneWayBlock:
    ItemDropBlock<Items.Placeable.OneWayBlock>,
    IReactsToTouch {
        /** A block that only lets you pass through in one direction.
         */
        public override String Texture {
            get => "REBEL/Blocks/Physics/OneWayBlock/Block";
        }

        public override void SetStaticDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            //(Mod as REBEL).registerTouchHandler(Type, OnTouched);
        }

        static int[] frameXCycle = { 1, 2, 3, 0 };
        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
            Point p = getFrameBlock(i, j);
            setFrame(i, j, frameXCycle[p.X % frameXCycle.Length], p.Y);
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
            if(tile.IsActuated) return; //don't react when turned off.

            int mode = (int)(tile.frameX / 18) & 3;
            //0:up 1:right 2:down 3:left
            //these numbers are the order of the graphics in the image
            switch(mode) {
                case 0: { //up
                    if(direction == TouchDirection.Top
                    || direction == TouchDirection.TopLeft
                    || direction == TouchDirection.TopRight) {
                        //XXX there must be a better way to do this.
                        //we could try to change solidity but that would
                        //affect all instances of the tile.
                        //as it is, this causes a weird jittering instead
                        //of simply standing on top.
                        whom.velocity.Y = Math.Min(whom.velocity.Y, 0);
                        whom.position.Y -= 0.5f;
                    }
                    break;
                }
                case 1: { //right
                    if(direction == TouchDirection.Right
                    || direction == TouchDirection.BottomRight
                    || direction == TouchDirection.TopRight) {
                        whom.velocity.X = Math.Max(whom.velocity.X,  0);
                        whom.position.X += 0.5f;
                    }
                    break;
                }
                case 2: { //down
                    if(direction == TouchDirection.Bottom
                    || direction == TouchDirection.BottomLeft
                    || direction == TouchDirection.BottomRight) {
                        whom.velocity.Y = Math.Max(whom.velocity.Y,  0);
                        whom.position.Y += 0.5f;
                    }
                    break;
                }
                case 3: { //left
                    if(direction == TouchDirection.Left
                    || direction == TouchDirection.TopLeft
                    || direction == TouchDirection.BottomLeft) {
                        whom.velocity.X = Math.Min(whom.velocity.X, 0);
                        whom.position.X -= 0.5f;
                    }
                    break;
                }
                default: break;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class OneWayBlock: TilePlaceItem<Blocks.OneWayBlock, OneWayBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Physics/OneWayBlock/Item";
        }
        public override String _getName() => "One-Way Block";
        public override String _getDescription() =>
            "A block that can only be passed through from one direction. Right-click to change direction.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.OneWayBlock>();
			resultItem.CreateRecipe(20)
				.AddRecipeGroup("PresurePlate", 1) //lolspelling
				.AddRecipeGroup("IronBar", 1)
                .AddTile(TileID.Anvils)
				.Register();
		}
    }
}
