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
    public abstract class Consumable: RebelModTile, IReactsToTouch {
        /** A non-solid block which, when touched, drops an item
         *  on you and becomes inactive.
         */
        public abstract int _getItemID();
        public override void SetStaticDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
            Main.tileSolid[Type] = false;
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
            if(tile.HasActuator && tile.IsActuated) return; //don't react when turned off.
            if(tile.frameX != 0) return; //actuator hack

            if(whom is Player p) {
                if(tile.HasActuator) {
                    //turn off (actuator hack)
                    Point pt = getFrameBlock(location.X, location.Y);
                    setFrame(location.X, location.Y, 1, pt.Y);
                    //spawn the item right on the player
                    Item.NewItem((int)whom.position.X, (int)whom.position.Y,
                        16, 16, _getItemID());
                }
                //don't drop an item if we're about to destroy the block,
                //because doing so will drop the item anyway.
                else WorldGen.KillTile(location.X, location.Y);
            }
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            Point p = getFrameBlock(i, j);
            if(tile.HasActuator) {
                //actuation doesn't work properly for non-solid blocks.
                //anyway, we want actuators to only enable, not disable,
                //so we can use them to reset a bunch without worrying
                //about ones that weren't collected.
                setFrame(i, j, 0, p.Y);
            }
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            /** Called when this tile is hit by a pickaxe.
             *  i, j: this tile's world tile-coordinates.
             *  fail: whether we didn't hit hard enough to destroy it.
             *  effectOnly: only create dust
             *  noItem: don't drop anything.
             */
            if(fail || effectOnly || noItem) return;
            Item.NewItem(i * 16, j * 16, 16, 16, _getItemID());
        }

        //repeat the middle frame to avoid duplicating it in memory.
        static int[] animFrames = {0, 1, 2, 1};
        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (animFrames[Main.tileFrame[Type] & 3]) * getFrameHeight();
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 16) {
                frameCounter = 0;
                frame = (frame+1) & 3;
            }
        }
    }
}
