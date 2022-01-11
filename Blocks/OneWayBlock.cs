using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
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

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            int mode = (int)(tile.frameX / 18) & 3;
            //0:up 1:right 2:down 3:left
            switch(mode) {
                case 0: whom.velocity.Y = Math.Min(whom.velocity.Y, 0); break;
                case 1: whom.velocity.X = Math.Max(whom.velocity.X, 0); break;
                case 2: whom.velocity.Y = Math.Max(whom.velocity.Y, 0); break;
                case 3: whom.velocity.X = Math.Min(whom.velocity.X, 0); break;
                default: break;
            }
        }
    }
}
