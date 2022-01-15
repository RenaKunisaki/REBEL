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
    public class RainbowLamp:
    ItemDropBlock<Items.Placeable.RainbowLamp> {
        /** Emits light that cycles through colours.
         */
        Dictionary<ushort, ushort> PureTiles;
        Dictionary<int, int> PureNPCs;
        public override String Texture {
            get => "REBEL/Blocks/Decorative/RainbowLamp/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            if(getFrameBlock(i, j).X != 0) return; //turned off
            float hue = (float)Main.tileFrame[Type] / 2048f;
            Color color = Main.hslToRgb(hue, 1f, 0.5f);
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(),
                color.ToVector3());
            //the vector can be scaled for more light.
            //at 255 it's basically a portable sun.
            //could make multiple variants with different brightness/speed.
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            frame = ++frame % 2048;
        }

        public override void HitWire(int i, int j) {
            Point p = getFrameBlock(i, j);
            p.X ^= 1;
            setFrame(i, j, p.X, p.Y);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class RainbowLamp: TilePlaceItem<Blocks.RainbowLamp, RainbowLamp> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/RainbowLamp/Block"; //XXX
        }
        public override String _getName() => "Rainbow Lamp";
        public override String _getDescription() => "Cycles through colors.";
        public override int _getResearchNeeded() => 3;
        public override int _getValue() => 2000;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.RainbowLamp>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.RedTorch, 1)
				.AddIngredient(ItemID.GreenTorch, 1)
				.AddIngredient(ItemID.BlueTorch, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
