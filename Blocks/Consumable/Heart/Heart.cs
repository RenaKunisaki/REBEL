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
    public class Heart: Consumable {
        /** A recovery heart floating in the air.
         */
        public override int _getItemID() => ItemID.Heart;
        public override String Texture {
            get => "REBEL/Blocks/Consumable/Heart/Block";
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Heart: TilePlaceItem<Blocks.Heart, Heart> {
		public override String Texture {
            get => "REBEL/Blocks/Consumable/Heart/Item";
        }
        public override String _getName() => "Floating Heart";
        public override String _getDescription() => "A recovery heart floating in the air.";
        public override int _getResearchNeeded() => 10;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.Heart>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.LesserHealingPotion, 1)
				.Register();
		}
    }
}
