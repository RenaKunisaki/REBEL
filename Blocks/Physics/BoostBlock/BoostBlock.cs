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
    public class BoostBlock:
    ItemDropBlock<Items.Placeable.BoostBlock>,
    IReactsToTouch {
        /** A block that, when you touch it, gives you a boost in
         *  some direction.
         */
        public override String Texture {
            get => "REBEL/Blocks/Physics/BoostBlock/Block";
        }

        //XXX use slope to determine which directions it works in?
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

        static int[] frameXCycle = { 1, 2, 3, 0 };
        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Main.tile[i, j];
			int style = tile.frameY / 18;
            int mode  = (tile.frameX / 18) % frameXCycle.Length;
			int nextFrameX = frameXCycle[mode];
			tile.frameX = (short)(nextFrameX * 18);
            //tile.frameY = 0;
			if (Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1,
                    Player.tileTargetX, Player.tileTargetY,
                    1, TileChangeType.None);
			}
            return false;
		}

        public override bool RightClick(int x, int y) {
            /** Called when right-clicked.
             *  Used because apparently we can't hammer non-solid tiles.
             */
            Slope(x, y);
            return true; //we did something, don't do default right click
        }

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            if(tile.IsActuated) return; //don't react when turned off.

            int mode = (int)(tile.frameX / 18) & 7;
            switch(mode) {
                case 0: whom.velocity.Y = -10; break;
                case 1: whom.velocity.X =  10; break;
                case 2: whom.velocity.Y =  10; break;
                case 3: whom.velocity.X = -10; break;
                //other states are deactivated (actuator hack)
                default: break;
            }
        }

        public override void HitWire(int i, int j) {
            //called when a signal passes through this tile via wire.
            Tile tile = Main.tile[i, j];
            if(tile.HasActuator) {
                //actuation doesn't work properly for non-solid blocks.
                int mode = (int)(tile.frameX / 18) & 7;
                mode ^= 4;
                tile.frameX = (short)(mode * 18);
            }
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (Main.tileFrame[Type] % 5) * 18;
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 10) {
                frameCounter = 0;
                frame = ++frame % 5;
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class BoostBlock : ModItem {
		public override String Texture {
            get => "REBEL/Blocks/Physics/BoostBlock/Item";
        }
        public override void SetStaticDefaults() {
            Tooltip.SetDefault(
				"A block that accelerates you in a direction. Right-click to change direction.");
			Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults() {
			Item.width = 16; //hitbox size in pixels
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1; //ItemUseStyleID.SwingThrow;
			Item.consumable = true;
			Item.value = 500;
			Item.createTile = ModContent.TileType<Blocks.BoostBlock>();
		}

        public override void AddRecipes() {
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<Items.Placeable.BoostBlock>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
