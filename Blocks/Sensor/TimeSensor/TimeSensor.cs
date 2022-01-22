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
    public class TimeSensor:
    ItemDropBlock<Items.Placeable.TimeSensor> {
        /** Emits a signal every in-game hour.
         */
        public override String Texture {
            get => "REBEL/Blocks/Sensor/TimeSensor/Block";
        }

        Dictionary<Point, float> prevTime;
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(ModContent.GetInstance<TimeSensorEntity>()
                    .Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            prevTime = new Dictionary<Point, float>();
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            if(Main.gamePaused) return;
            if(Main.dayRate == 0) return; //time paused

            //what the fuck
            float time = (float)Main.time;
			if (!Main.dayTime) time += 54000.0f;
            time = (time + (4.5f * 3600.0f)) % 86400.0f;
            //int hour = (int)(time / 3600.0);
            //int minute = (int)((time / 60.0) % 60.0);
            //int second = (int)(time % 60.0);

            //use this to ensure we don't miss a tick if the game lags,
            //and don't signal twice on the same minute.
            Point pt = new Point(i, j);
            float prev = 0;
            if(prevTime.ContainsKey(pt)) prev = prevTime[pt];
            //Main.NewText();

            float frequency = 60f;
            int index = ModContent.GetInstance<TimeSensorEntity>().Find(i, j);
            if(index >= 0) {
                var entity = (TimeSensorEntity)TileEntity.ByID[index];
                frequency = entity.frequency;
            }

            if(Math.Abs(time - prev) >= (frequency * 60f)) {
                //Main.NewText($"{hour:00}:{minute:00}:{second:00}");
                (Mod as REBEL).tripWire(i, j); //send a signal
                prevTime[pt] = time;
            }
        }

        public override bool RightClick(int x, int y) {
            (Mod as REBEL).showUIForTile<TimeSensorEntity>(x, y);
            return true;
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            if(!(fail || effectOnly)) {
                ModContent.GetInstance<TimeSensorEntity>().Kill(i, j);
            }
        }
    } //class

    public class TimeSensorEntity: RebelModTileEntity<TimeSensor> {
        //Stores parameters for individual TimeSensor tiles.

        //The name displayed in the config UI.
        public override String displayName {get => "Time Sensor";}

        [TileFloatAttribute("Frequency", "Trigger every this many minutes.",
            defaultValue: 60f, minValue: 1f, maxValue: 3600f, step: 5f,
            bigStep: 60f)]
        public float frequency = 60f;
    } //class
} //namespace

namespace REBEL.Items.Placeable {
    public class TimeSensor: TilePlaceItem<Blocks.TimeSensor, TimeSensor> {
		public override String Texture {
            get => "REBEL/Blocks/Sensor/TimeSensor/Item";
        }
        public override String _getName() => "Time Sensor";
        public override String _getDescription() => "Emits a signal every N in-game minutes.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.TimeSensor>();
			resultItem.CreateRecipe(20)
				.AddIngredient(ItemID.Wire, 20)
				.AddIngredient(ItemID.CopperWatch, 1)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
    } //class
} //namespace
