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
        Dictionary<Point, ulong> buffers; //signal buffer per tile
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
            buffers = new Dictionary<Point, ulong>();
        }

        public void _activate(Point thisLoc, Point origin) {
            /** Called when activated by an Isolator.
             */
            bufferAdd(thisLoc.X, thisLoc.Y);
        }

        public bool bufferGet(int i, int j, bool peek=false) {
            //pop/peek one bit out of the buffer
            Point p = new Point(i, j);
            if(!buffers.ContainsKey(p)) {
                buffers[p] = 0;
            }
            ulong buf = buffers[p];
            if(!peek) buffers[p] = buf >> 1;
            return (buf & 1) != 0;
        }

        public void bufferAdd(int i, int j) {
            //push a bit into the buffer
            Point p = new Point(i, j);
            if(!buffers.ContainsKey(p)) {
                buffers[p] = 0;
            }
            int time = getFrameBlock(i, j).X;
            if(time == 7) time = Main.rand.Next(0, 6);
            buffers[p] |= (ulong)(1 << time);
        }

        public override bool RightClick(int x, int y) {
            //x: mode (how long to delay) 1,2,4,8,16,32,64,random?
            //y: animation (lit up or not)
            Point p = getFrameBlock(x, y);
            setFrame(x, y, (p.X+1) & 7, 0);
            return true;
        }

        public override void HitWire(int i, int j) {
            bufferAdd(i, j); //no problem if done multiple times
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            Point p = getFrameBlock(i, j);
            if(bufferGet(i, j)) {
                (Mod as REBEL).tripWire(i, j);
                setFrame(i, j, p.X, 1); //highlight
            }
            else setFrame(i, j, p.X, 0); //no highlight
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            Point p = new Point(i, j);
            (Mod as REBEL).deleteWire(i, j);
            buffers.Remove(p);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Delay: TilePlaceItem<Blocks.Delay, Delay> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Delay/Item";
        }
        public override String _getName() => "Delay Timer";
        public override String _getDescription() => "Receives signals from Isolator and passes them on a few frames later.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Delay>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.Wire, 20)
				.AddIngredient(ItemID.HoneyBlock, 1)
				.Register();
		}
    }
}
