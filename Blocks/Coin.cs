using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public class Coin: ModTile, IReactsToTouch {
        /** A coin floating in the air.
         */
        public override void SetStaticDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            if(whom is Player p) {
                p.DoCoins(1);
                WorldGen.KillTile(location.X, location.Y);
                Item.NewItem(location.X * 16, location.Y * 16, 16, 16,
                    ItemID.CopperCoin);
            }
        }
    }
}
