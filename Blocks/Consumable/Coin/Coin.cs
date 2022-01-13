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
    public class Coin: RebelModTile, IReactsToTouch {
        /** A coin floating in the air.
         */
        public override String Texture {
            get => "REBEL/Blocks/Consumable/Coin/Block";
        }
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
            var tile = Main.tile[location.X, location.Y];
            if(tile.IsActuated) return; //don't react when turned off.
            if(tile.frameX != 0) return; //actuator hack

            if(whom is Player p) {
                p.DoCoins(1);
                WorldGen.KillTile(location.X, location.Y);
                Item.NewItem(location.X * 16, location.Y * 16, 16, 16,
                    ItemID.CopperCoin);
            }
        }

        public override void HitWire(int i, int j) {
            Tile tile = Main.tile[i, j];
            if(tile.HasActuator) {
                //actuation doesn't work properly for non-solid blocks.
                Point p = getFrameBlock(i, j);
                setFrame(i, j, p.X ^ 1, p.Y);
            }
        }

        //repeat the middle frame to avoid duplicating it in memory.
        static int[] animFrames = {0, 1, 2, 1};
        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (animFrames[Main.tileFrame[Type] & 3]) * getFrameHeight();
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 16) {
                frameCounter = 0;
                frame = (frame+1) & 3;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class Coin: TilePlaceItem<Blocks.Coin, Coin> {
		public override String Texture {
            get => "REBEL/Blocks/Consumable/Coin/Item";
        }
        public override String _getName() => "Coin";
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
