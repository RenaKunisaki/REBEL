using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Items.Placeable {
    public abstract class TilePlaceItem<TPlaceTile, TDropItem> : ModItem
    where TPlaceTile: ModTile
    where TDropItem: ModItem {
        /** Base class for an item that simply places a tile.
         */
		//public override String Texture;
        abstract public String _getName();
        abstract public String _getDescription();
        public int _getResearchNeeded() { return 1; }
        public int _getValue() { return 1; }

        public override void SetStaticDefaults() {
            Tooltip.SetDefault(_getDescription());
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.
                Instance.SacrificeCountNeededByItemId[Type] =
                    _getResearchNeeded();
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
			Item.value = _getValue();
			Item.createTile = ModContent.TileType<TPlaceTile>();
		}

        public override void AddRecipes() {
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<TDropItem>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
