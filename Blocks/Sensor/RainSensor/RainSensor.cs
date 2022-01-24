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
    public class RainSensor:
    ItemDropBlock<Items.Placeable.RainSensor> {
        /** A block that emits a signal when rain starts/ends.
         */
        enum Mode {Both, OnStart, OnEnd};
        public override String Texture {
            get => "REBEL/Blocks/Sensor/RainSensor/Block";
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
            //x: mode
            //y: animation (lit up or not)
            Point p = getFrameBlock(x, y);
            setFrame(x, y, (p.X+1) % 3, p.Y);
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            (Mod as REBEL).deleteWire(i, j);
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            if(Main.gamePaused) return;
            Point frame   = getFrameBlock(i, j);
            int   mode    = frame.X;
            bool  isOn    = frame.Y != 0;
            var   tile    = Framing.GetTileSafely(i, j);
            bool  trigger = false;
            switch((Mode)mode) {
                case Mode.Both:
                    trigger = (Main.raining != isOn);
                    break;
                case Mode.OnStart:
                    trigger = Main.raining && !isOn;
                    break;
                case Mode.OnEnd:
                    trigger = isOn && !Main.raining;
                    break;
            }
            if(trigger) (Mod as REBEL).tripWire(i, j);
            setFrame(i, j, mode, Main.raining ? 1 : 0);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class RainSensor: TilePlaceItem<Blocks.RainSensor, RainSensor> {
		public override String Texture {
            get => "REBEL/Blocks/Sensor/RainSensor/Item";
        }
        public override String _getName() => "Rain Sensor";
        public override String _getDescription() => "Sends a signal when rain begins/ends.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.RainSensor>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.Wire, 20)
				.AddIngredient(ItemID.Lens, 1)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
