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
    public class Mana: Consumable {
        /** A mana refill star floating in the air.
         */
        public override int _getItemID() => ItemID.Star;
        public override String Texture {
            get => "REBEL/Blocks/Consumable/Mana/Block";
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Mana: TilePlaceItem<Blocks.Mana, Mana> {
		public override String Texture {
            get => "REBEL/Blocks/Consumable/Mana/Item";
        }
        public override String _getName() => "Floating Mana";
        public override String _getDescription() => "A mana refill star floating in the air.";
        public override int _getResearchNeeded() => 10;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Mana>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.LesserManaPotion, 1)
				.Register();
		}
    }
}
