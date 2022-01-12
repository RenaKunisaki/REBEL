using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Items.Placeable {
    public class TestBlock : ModItem {
		public override String Texture {
            get => "REBEL/Items/Placeable/Misc/TestBlock/TestBlock";
        }
        public override void SetStaticDefaults() {
            Tooltip.SetDefault("A block for debugging.");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults() {
			Item.width = 16; //hitbox size in pixels
			Item.height = 16;
			Item.maxStack = 7511;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1; //ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.value = 10000000;
			Item.createTile = ModContent.TileType<Blocks.TestBlock>();
		}

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.TestBlock>();
			resultItem.CreateRecipe(7511)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
