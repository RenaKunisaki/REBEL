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
    public class Coin: Consumable {
        /** A coin floating in the air.
         */
        public override int _getItemID() => ItemID.CopperCoin;
        public override String Texture {
            get => "REBEL/Blocks/Consumable/Coin/Block";
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Coin: TilePlaceItem<Blocks.Coin, Coin> {
		public override String Texture {
            get => "REBEL/Blocks/Consumable/Coin/Item";
        }
        public override String _getName() => "Floating Coin";
        public override String _getDescription() => "A coin floating in the air.";
        public override int _getResearchNeeded() => 100;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Coin>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.CopperCoin, 1)
				.Register();
		}
    }
}
