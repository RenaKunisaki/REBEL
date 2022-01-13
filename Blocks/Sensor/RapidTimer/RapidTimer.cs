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
    public class RapidTimer:
    ItemDropBlock<Items.Placeable.RapidTimer> {
        /** Emits a signal every two frames.
         */
        public override String Texture {
            get => "REBEL/Blocks/Sensor/RapidTimer/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            if(Main.gamePaused) return;
            Tile tile = Main.tile[i, j];
            if(tile.frameX == 0) return; //inactive
            if((Main.GameUpdateCount & 1) == 0) {
                Wiring.TripWire(i, j, 1, 1); //send a signal
            }
        }

        protected void toggle(int i, int j) {
            Tile tile = Main.tile[i, j];
            tile.frameX ^= 18;
            if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1,
                    Player.tileTargetX, Player.tileTargetY,
                    1, TileChangeType.None);
			}
        }

        public override bool RightClick(int x, int y) {
            toggle(x, y); //toggle like normal timers
            return true; //we did something, don't do default right click
        }

        public override void HitWire(int i, int j) {
            toggle(i, j); //toggle like normal timers
        }
    }
}

namespace REBEL.Items.Placeable {
    public class RapidTimer: TilePlaceItem<Blocks.RapidTimer, RapidTimer> {
		public override String Texture {
            get => "REBEL/Blocks/Sensor/RapidTimer/Item";
        }
        public override String _getName() => "Rapid Timer";
        public override String _getDescription() => "Emits a signal every two frames.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override bool _showsWires() => true;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.RapidTimer>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
