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
    public class Isolator:
    ItemDropBlock<Items.Placeable.Isolator> {
        /** A pair of blocks that pass signals in one direction
         *  across short gaps.
         */
        protected Dictionary<Point, uint> lastHit;
        public override String Texture {
            get => "REBEL/Blocks/Wire/Isolator/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            lastHit = new Dictionary<Point, uint>();
        }

        public override bool RightClick(int x, int y) {
            Tile tile = Main.tile[x, y];
            int mode = tile.frameX / 18;
            setFrame(x, y, (mode+1) & 1, 0);
            return true;
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            int mode = tile.frameX / 18;
            if(mode != 0) return; //nothing to do

            //for whatever reason, tripping another wire elsewhere can
            //trip this tile again too. make sure it doesn't trip more
            //than once per tick.
            uint tick = Main.GameUpdateCount;
            Point p = new Point(i, j);
            if(!lastHit.ContainsKey(p)) lastHit[p] = 0;
            if(lastHit[p] == tick) return;
            lastHit[p] = tick;

            //Mod.Logger.Info($"[{Main.GameUpdateCount}] Isolator({i},{j}) hit (mode {mode} wire {Wiring._currentWireColor})");

            //look for a corresponding outlet.
            int x1 = Math.Max(i-4, (int)Main.leftWorld);
            int x2 = Math.Min(i+4, (int)Main.rightWorld-1);
            int y1 = Math.Max(j-4, (int)Main.topWorld);
            int y2 = Math.Min(j+4, (int)Main.bottomWorld-1);
            for(int y=y1; y<=y2; y++) {
                for(int x=x1; x<=x2; x++) {
                    Tile t2 = Main.tile[x, y];
                    if(t2.type == this.Type && t2.frameX != 0) {
                        //Mod.Logger.Info($"Isolator trip at {x},{y}");
                        Point p2 = new Point(x, y);
                        if(!lastHit.ContainsKey(p2)) lastHit[p2] = 0;
                        if(lastHit[p2] != tick) {
                            lastHit[p] = tick;
                            try {
                                Wiring.TripWire(x, y, 1, 1);
                            }
                            catch(System.ArgumentException) {
                                //ignore. this happens if the wire is already tripped.
                                Mod.Logger.Info("Got ArgumentException in TripWire");
                            }
                        }
                    }
                }
            }
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            if(!(lastHit is null)) {
                lastHit.Remove(new Point(i, j)); //don't leak memory
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
    public class Isolator: TilePlaceItem<Blocks.Isolator, Isolator> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Isolator/Item";
        }
        public override String _getName() => "Isolator";
        public override String _getDescription() => "Passes signals in one direction.";
        public override bool _showsWires() => true;
    }
}
