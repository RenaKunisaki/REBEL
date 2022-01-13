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

        public void setFrame(int i, int j, int frameX, int frameY,
        bool local=false) {
            Tile tile = Main.tile[i, j];
            tile.frameX = (short)(frameX * 18);
            tile.frameY = (short)(frameY * 18);
            if((!local) && Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1, Player.tileTargetX,
                    Player.tileTargetY, 1, TileChangeType.None);
			}
        }
    }
}
