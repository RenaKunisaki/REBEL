using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks.Base {
    public abstract class ItemDropBlock<DropItemType> : RebelModTile
    where DropItemType: ModItem {
        /** Base class for blocks that drop an item when destroyed.
         */

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            /** Called when this tile is hit by a pickaxe.
             *  i, j: this tile's world tile-coordinates.
             *  fail: whether we didn't hit hard enough to destroy it.
             *  effectOnly: only create dust
             *  noItem: don't drop anything.
             */
            if(fail || effectOnly || noItem) return;
            Item.NewItem(new EntitySource_TileBreak(i, j),
                new Rectangle(i * 16, j * 16, 16, 16),
                ModContent.ItemType<DropItemType>());
        }
    }
}
