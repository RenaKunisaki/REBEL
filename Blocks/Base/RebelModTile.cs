using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;

using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks.Base {
    public abstract class RebelModTile: ModTile {
        /** Base class for all our blocks.
         */

        public int getFrameWidth()  => 18;
        public int getFrameHeight() => 18;

        public Point getFrameBlock(int i, int j) {
            Tile tile = Framing.GetTileSafely(i, j);
            return new Point(tile.TileFrameX / getFrameWidth(),
                tile.TileFrameY / getFrameHeight());
        }

        public void setFrame(int i, int j, int frameX, int frameY,
        bool local=false) {
            Tile tile = Framing.GetTileSafely(i, j);
            tile.TileFrameX = (short)(frameX * getFrameWidth());
            tile.TileFrameY = (short)(frameY * getFrameHeight());
            if((!local) && Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1, Player.tileTargetX,
                    Player.tileTargetY, 1, TileChangeType.None);
			}
        }
    }
}
