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
    public class Counter:
    ItemDropBlock<Items.Placeable.Counter> {
        /** A block that emits a signal for every N signals
         *  received from Isolators.
         */
        Dictionary<Point, byte> buffers; //signal counter per tile
        public override String Texture {
            get => "REBEL/Blocks/Wire/Counter/Block";
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
            buffers = new Dictionary<Point, byte>();
        }

        public void _activate(Point thisLoc, Point origin) {
            /** Called when activated by an Isolator.
             */
            if(!buffers.ContainsKey(thisLoc)) {
                buffers[thisLoc] = 0;
            }
            buffers[thisLoc]++;

            Point p = getFrameBlock(thisLoc.X, thisLoc.Y);
            byte count = (byte)((p.X == 7) ? Main.rand.Next(1, 64) : (2 << p.X));
            if(buffers[thisLoc] >= count) {
                (Mod as REBEL).tripWire(thisLoc.X, thisLoc.Y);
                setFrame(thisLoc.X, thisLoc.Y, p.X, 1); //highlight
                buffers[thisLoc] = 0; //reset
            }
        }

        public override bool RightClick(int x, int y) {
            //x: mode (how many signals needed) 2,4,8,16,32,64,128,random
            //y: animation (lit up or not)
            Point p = getFrameBlock(x, y);
            setFrame(x, y, (p.X+1) & 7, 0);
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            Point p = new Point(i, j);
            (Mod as REBEL).deleteWire(i, j);
            buffers.Remove(p);
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            //clear highlight
            REBEL mod = Mod as REBEL;
            Point p = getFrameBlock(i, j);
            setFrame(i, j, p.X, 0, true);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Counter: TilePlaceItem<Blocks.Counter, Counter> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Counter/Item";
        }
        public override String _getName() => "Signal Counter";
        public override String _getDescription() => "Receives signals from Isolator; emits one when enough have been received.";
        public override bool _showsWires() => true;
    }
}
