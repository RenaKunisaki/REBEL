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
    public class Diode:
    ItemDropBlock<Items.Placeable.Diode> {
        /** A block that passes signals in one direction.
         */
        public override String Texture {
            get => "REBEL/Blocks/Wire/Diode/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        /* public override void PlaceInWorld(int i, int j, Item item) {
            setFrame(i, j, 0, 0);
        } */

        public override bool RightClick(int x, int y) {
            Tile tile = Main.tile[x, y];
            int mode = tile.frameX / 18;
            setFrame(x, y, (mode+1) & 3, 0);
            return true;
        }

        static int count = 0;
        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            int mode = tile.frameX / 18;
            Mod.Logger.Info($"Diode({i},{j}) hit (mode {mode})");
            count++;
            if(count >= 100) {
                Mod.Logger.Info("Infinite loop, aborting");
                return;
            }
            //this doesn't work because for one, the wire has to be *on* the
            //Diode to activate it, and if another wire of the same color is
            //directly next to it, it will just connect right through.
            //That can be avoided by using a different color but that doesn't
            //work either and I'm not entirely sure why.
            //probably best method is to figure out how the logic gate lamps
            //work and use them as inputs.

            Wiring.SkipWire(i, j); //no idea what this does.
            try {
                switch(mode) {
                    case 0: { //up
                        if(j > Main.topWorld) Wiring.TripWire(i, j-1, 1, 1);
                        break;
                    }
                    case 1: { //right
                        if(i < Main.rightWorld) Wiring.TripWire(i+1, j, 1, 1);
                        break;
                    }
                    case 2: { //down
                        if(j < Main.bottomWorld) Wiring.TripWire(i, j+1, 1, 1);
                        break;
                    }
                    case 3: { //left
                        if(i > Main.leftWorld) Wiring.TripWire(i-1, j, 1, 1);
                        break;
                    }
                    default: break;
                }
            }
            catch(System.ArgumentException) {
                //ignore. this happens if the wire is already tripped.
                Mod.Logger.Info("Got ArgumentException in TripWire");
            }
        }

        public void setFrame(int i, int j, int frameX, int frameY) {
            Tile tile = Main.tile[i, j];
            tile.frameX = (short)(frameX * 18);
            tile.frameY = (short)(frameY * 18);
            if(Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1, Player.tileTargetX,
                    Player.tileTargetY, 1, TileChangeType.None);
			}
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Diode: TilePlaceItem<Blocks.Diode, Diode> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Diode/Item";
        }
        public override String _getName() => "Diode";
        public override String _getDescription() => "Passes signals in one direction.";
        public override bool _showsWires() => true;
    }
}
