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
                Wiring.TripWire(i, j, 1, 1); //send a signal
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class RandomSensor : ModItem {
		public override String Texture {
            //use same texture for both
            get => "REBEL/Blocks/Sensor/RandomSensor/Block";
        }
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("Emits a signal at random.");
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
			Item.createTile = ModContent.TileType<Blocks.RandomSensor>();
		}

        public override void AddRecipes() {
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<Items.Placeable.RandomSensor>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
