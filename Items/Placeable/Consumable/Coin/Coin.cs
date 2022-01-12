using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Items.Placeable {
    public class Coin : ModItem {
		public override String Texture {
            get => "REBEL/Items/Placeable/Consumable/Coin/Coin";
        }

		public override void SetStaticDefaults() {
            Tooltip.SetDefault("A coin floating in the air.");
			DisplayName.SetDefault("Coin");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults() {
			Item.width = 16; //hitbox size in pixels
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1; //ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.value = 1;
			Item.createTile = ModContent.TileType<Blocks.Coin>();
		}

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Coin>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.CopperCoin, 1)
				.Register();
		}
    }
}
