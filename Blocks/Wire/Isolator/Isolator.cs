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
        Dictionary<ushort, Action<Point, Point>> Receivers;
        enum Mode {
            Bidirectional,
            Transmit,
            Receive
        }
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

            registerReceiver(Type, _activate);
        }

        public void registerReceiver(ushort type,
        Action<Point, Point> handler) {
            if(Receivers is null) {
                Receivers = new Dictionary<ushort, Action<Point, Point>>();
            }
            if(!Receivers.ContainsKey(type)) {
                Receivers[type] = handler;
            }
        }

        public void _activate(Point thisLoc, Point origin) {
            /** Called when activated by an Isolator.
             */
            Tile tile = Main.tile[thisLoc.X, thisLoc.Y];
            int mode = tile.frameX / getFrameWidth();
            if(mode == (int)Mode.Transmit) return;
            setFrame(thisLoc.X, thisLoc.Y, mode, 1, true); //use lit-up version

            REBEL mod = Mod as REBEL;
            mod.tripWire(thisLoc.X, thisLoc.Y);
        }

        public override bool RightClick(int x, int y) {
            REBEL mod = Mod as REBEL;
            Point p = getFrameBlock(x, y);
            int anim = mod.wireAlreadyHit(x, y) ? 1 : 0;
            setFrame(x, y, (p.X+1) % 3, anim);
            return true;
        }

        public bool activateTile(int i, int j, Point thisLoc) {
            Tile tile = Main.tile[i, j];
            if(Receivers.ContainsKey(tile.type)) {
                Receivers[tile.type](new Point(i,j), thisLoc);
                return true;
            }
            return false;
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            int mode = tile.frameX / getFrameWidth();
            setFrame(i, j, mode, 1, true); //use lit-up version
            if(mode == (int)Mode.Receive) return;

            REBEL mod = Mod as REBEL;
            if(mod.wireAlreadyHit(i, j)) return;

            //Mod.Logger.Info($"[{Main.GameUpdateCount}] Isolator({i},{j}) hit (mode {mode} wire {Wiring._currentWireColor})");

            //look for a corresponding outlet.
            Point ourLoc = new Point(i, j);
            int x1 = Math.Max(i-2, (int)Main.leftWorld);
            int x2 = Math.Min(i+2, (int)Main.rightWorld-1);
            int y1 = Math.Max(j-2, (int)Main.topWorld);
            int y2 = Math.Min(j+2, (int)Main.bottomWorld-1);
            for(int y=y1; y<=y2; y++) {
                for(int x=x1; x<=x2; x++) {
                    activateTile(x, y, ourLoc);
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
    public class Isolator: TilePlaceItem<Blocks.Isolator, Isolator> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Isolator/Item";
        }
        public override String _getName() => "Isolator";
        public override String _getDescription() => "Passes signals across small gaps.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Isolator>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.Wire, 5)
				.AddIngredient(ItemID.CrystalShard, 1)
				.Register();
		}
    }
}
