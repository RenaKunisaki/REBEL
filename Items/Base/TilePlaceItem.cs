using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Items {
    public abstract class TilePlaceItem<TPlaceTile, TDropItem> : RebelItem
    where TPlaceTile: ModTile
    where TDropItem: ModItem {
        /** Base class for an item that simply places a tile.
         */
		public override void SetDefaults() {
			base.SetDefaults();
			Item.createTile = ModContent.TileType<TPlaceTile>();
		}

		public override void AddRecipes() {
			//default recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<TDropItem>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
