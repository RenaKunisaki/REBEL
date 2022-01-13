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
    public class Delay:
    ItemDropBlock<Items.Placeable.Delay> {
        /** A block that receives a signal from an Isolator and
         *  re-sends it some time later.
         */
        Dictionary<Point, uint> buffers;
        public override String Texture {
            get => "REBEL/Blocks/Wire/Delay/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            ModContent.GetInstance<Isolator>().registerReceiver(Type, _activate);
        }

        public void _activate(Point thisLoc, Point origin) {
            /** Called when activated by an Isolator.
             */
            int mode = tile.frameX / getFrameWidth();
            setFrame(thisLoc.X, thisLoc.Y, mode, 1, true); //use lit-up version
            if(mode != 0) return; //we're a transmitter; nothing to do

            REBEL mod = Mod as REBEL;
            mod.tripWire(thisLoc.X, thisLoc.Y);
        }

        public void _updateBuffer(int i, int j) {
            //ensure the buffer size is correct for this tile
            Point p = new Point(i, j);
            if(!buffers.ContainsKey(p)) {
                buffers[p] = 0;
            }
            //buffer should be a bitflags, where each frame, if the lowest bit
            //is set, we signal; then we shift it down by 1.
            //when activated, we set the Nth bit, where N is how many frames
            //of delay we want.
            //can change this method to something like getBuffer?
        }

        public override bool RightClick(int x, int y) {
            //x: mode (how long to delay)
            //y: animation (lit up or not)
            //here we should change the delay time. I think we don't need to do
            //anything with the existing buffer.
            Point p = getFrameBlock(x, y);
            setFrame(x, y, p.X ^ 1, anim);
            return true;
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            int mode = tile.frameX / getFrameWidth();
            setFrame(i, j, mode, 1, true); //use lit-up version
            if(mode != 0) return; //nothing to do

            REBEL mod = Mod as REBEL;
            if(mod.wireAlreadyHit(i, j)) return;

            //Mod.Logger.Info($"[{Main.GameUpdateCount}] Delay({i},{j}) hit (mode {mode} wire {Wiring._currentWireColor})");

            //look for a corresponding outlet.
            int x1 = Math.Max(i-4, (int)Main.leftWorld);
            int x2 = Math.Min(i+4, (int)Main.rightWorld-1);
            int y1 = Math.Max(j-4, (int)Main.topWorld);
            int y2 = Math.Min(j+4, (int)Main.bottomWorld-1);
            for(int y=y1; y<=y2; y++) {
                for(int x=x1; x<=x2; x++) {
                    Tile t2 = Main.tile[x, y];
                    if(t2.type == this.Type && t2.frameX != 0) {
                        //Mod.Logger.Info($"Delay trip at {x},{y}");
                        mod.tripWire(x, y);
                        setFrame(x, y, 1, 1, true); //use lit-up version
                    }
                }
            }
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            (Mod as REBEL).deleteWire(i, j);
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            //clear highlight
            REBEL mod = Mod as REBEL;
            Point p = getFrameBlock(i, j);
            setFrame(i, j, p.X, 0, true);
            //frameXOffset = mode * 18;
            //frameYOffset = anim * 18;
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Delay: TilePlaceItem<Blocks.Delay, Delay> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Delay/Item";
        }
        public override String _getName() => "Delay";
        public override String _getDescription() => "Passes signals in one direction.";
        public override bool _showsWires() => true;
    }
}
