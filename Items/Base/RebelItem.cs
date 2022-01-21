using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.UI;

namespace REBEL.Items {
    public abstract class RebelItem: ModItem {
        /** Base class for our items. Wraps some things to make
         *  the code neater.
         */
        abstract public String _getName();
        abstract public String _getDescription();
        virtual public int _getResearchNeeded() => 1;
        virtual public int _getValue() => 1;
        virtual public bool _showsWires() => false;

        public override void SetStaticDefaults() {
			DisplayName.SetDefault(_getName());
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
			Item.mech = _showsWires();
		}
    }
}
