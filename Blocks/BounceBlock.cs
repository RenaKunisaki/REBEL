using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class BounceBlock:
    Base.ItemDropBlock<Items.Placeable.BounceBlock>,
    IReactsToTouch {
        /** A block that, when you touch it, makes you bounce
         *  in the opposite direction.
         */

        //XXX use slope to determine which directions it works in?
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public void OnTouched(Player player, Point location,
        TouchDirection direction) {
            //var player = Main.LocalPlayer;

            //Main.NewText(
            //    String.Format("Touched {0}", direction),
            //    0x00, 0x9D, 0xF3);

            //apply vertical motion
            //10 units seems good. 100 will cause you to smack your head
            //against the top of the world.
            switch(direction) {
                case TouchDirection.TopLeft:
                case TouchDirection.Top:
                case TouchDirection.TopRight:
                    player.velocity.Y = -10; break;

                case TouchDirection.BottomLeft:
                case TouchDirection.Bottom:
                case TouchDirection.BottomRight:
                    player.velocity.Y = 10; break;

                default: break;
            }

            //apply horizontal motion
            //ignore corners here or else we get bounced around in random
            //directions when we land on a solid platform.
            switch(direction) {
                //case TouchDirection.TopLeft:
                case TouchDirection.Left:
                //case TouchDirection.BottomLeft:
                    player.velocity.X = -10; break;

                //case TouchDirection.TopRight:
                case TouchDirection.Right:
                //case TouchDirection.BottomRight:
                    player.velocity.X = 10; break;

                default: break;
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (Main.tileFrame[Type] % 3) * 18;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 10) {
                frameCounter = 0;
                frame = ++frame % 3;
            }
        }
    }
}
