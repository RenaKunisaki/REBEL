using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class BoostBlock : ModTile, IReactsToTouch {
        /** A block that, when you touch it, gives you a boost in
         *  some direction.
         */
        //XXX use slope to determine which directions it works in?

        public override void PostSetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
        }

        /* public override bool RightClick(int x, int y) {
            Main.NewText(
                String.Format("I'm at {0} {1}", x, y),
                0x00, 0x9D, 0xF3);
            return true; //we did something, don't do default right click
        } */

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            //Called when this tile is hit by a pickaxe.
            //i, j: this tile's world tile-coordinates.
            //fail: whether we didn't hit hard enough to destroy it.
            //effectOnly: only create dust
            //noItem: don't drop anything.
            if(fail) return;
            if(!noItem) {
                Item.NewItem(i * 16, j * 16, 16, 16,
                    ModContent.ItemType<Items.Placeable.BoostBlock>());
            }
		}

        public void OnTouched(Player player, Point location,
        TouchDirection direction) {
            var player = Main.LocalPlayer;

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
    }
}
