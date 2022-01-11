using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Items.Placeable {
    public class HealHurtBlock : ModItem {
		public override void SetStaticDefaults() {
            Tooltip.SetDefault(
				"A block that hurts or heals on contact. Hammer it to cycle between heal, hurt, heal lots, hurt lots.");
			DisplayName.SetDefault("Heal/Hurt Block");
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
			Item.value = 500;
			Item.createTile = ModContent.TileType<Blocks.HealHurtBlock>();
		}

        public override void AddRecipes() {
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<Items.Placeable.HealHurtBlock>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
