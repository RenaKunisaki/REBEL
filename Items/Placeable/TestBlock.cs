using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Items.Placeable {
    public class TestBlock : ModItem
    {
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("A test item you shouldn't have got. Probably just does something stupid.");
        }

        public override void SetDefaults() {
			Item.width = 1;
			Item.height = 1;
			Item.maxStack = 7511;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1; //ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Blocks.TestBlock>();
		}

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.TestBlock>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
