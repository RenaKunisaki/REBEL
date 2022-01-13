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
    public class RandomSensor:
    ItemDropBlock<Items.Placeable.RandomSensor> {
        /** Emits a signal every in-game hour.
         */
        public override String Texture {
            get => "REBEL/Blocks/Sensor/RandomSensor/Block";
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

        public override void NearbyEffects(int i, int j, bool closer) {
            if(Main.gamePaused) return;
            if(Main.rand.Next(60) == 0) {
                (Mod as REBEL).tripWire(i, j); //send a signal
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class RandomSensor: TilePlaceItem<Blocks.RandomSensor, RandomSensor> {
		public override String Texture {
            //reuse this
            get => "REBEL/Blocks/Sensor/RandomSensor/Block";
        }
        public override String _getName() => "Random Sensor";
        public override String _getDescription() => "Emits a signal at random.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.OneWayBlock>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
