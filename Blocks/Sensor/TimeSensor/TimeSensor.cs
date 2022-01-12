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

        Dictionary<Point, int> prevTime;
        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            prevTime = new Dictionary<Point, int>();
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            if(Main.gamePaused) return;
            if(Main.dayRate == 0) return; //time paused

            //what the fuck
            double time = Main.time;
			if (!Main.dayTime) time += 54000.0;
            time = (time + (4.5 * 3600.0)) % 86400.0;
            int hour = (int)(time / 3600.0);
            int minute = (int)((time / 60.0) % 60.0);
            int second = (int)(time % 60.0);

            //use this to ensure we don't miss a tick if the game lags,
            //and don't signal twice on the same minute.
            Point pt = new Point(i, j);
            int prev = 0;
            if(prevTime.ContainsKey(pt)) prev = prevTime[pt];
            //Main.NewText($"{hour:00}:{minute:00}:{second:00} prev {prev}");

            //check for < 5 to ensure that even if the player manually
            //changes the clock backward we don't signal unless it's very
            //near the top of the hour. don't just check for == 0 because
            //then we can miss an hour if there's lag.
            if(minute < prev && minute < 5) {
                Wiring.TripWire(i, j, 1, 1); //send a signal
            }
            prevTime[pt] = minute;
        }
    }
}

namespace REBEL.Items.Placeable {
    public class TimeSensor : ModItem {
		public override String Texture {
            get => "REBEL/Blocks/Sensor/TimeSensor/Item";
        }
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Emits a signal every in-game hour.");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1; //ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.value = 1;
			Item.createTile = ModContent.TileType<Blocks.TimeSensor>();
		}

        public override void AddRecipes() {
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<Items.Placeable.TimeSensor>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
