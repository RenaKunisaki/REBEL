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
        }

        public override bool RightClick(int x, int y) {
            REBEL mod = Mod as REBEL;
            Point p = getFrameBlock(x, y);
            int anim = mod.wireAlreadyHit(x, y) ? 1 : 0;
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
    public class Isolator: TilePlaceItem<Blocks.Isolator, Isolator> {
		public override String Texture {
            get => "REBEL/Blocks/Wire/Isolator/Item";
        }
        public override String _getName() => "Isolator";
        public override String _getDescription() => "Passes signals in one direction.";
        public override bool _showsWires() => true;
    }
}