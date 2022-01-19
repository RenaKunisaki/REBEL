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
    public class PortableSun:
    ItemDropBlock<Items.Placeable.PortableSun> {
        /** Emits a ton of light.
         */
        public override String Texture {
            get => "REBEL/Blocks/Misc/PortableSun/Block";
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
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(),
                new Vector3(255f, 255f, 255f));
        }

        public override void HitWire(int i, int j) {
            Point p = getFrameBlock(i, j);
            p.X ^= 1;
            setFrame(i, j, p.X, p.Y);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class PortableSun: TilePlaceItem<Blocks.PortableSun, PortableSun> {
		public override String Texture {
            get => "REBEL/Blocks/Misc/PortableSun/Item";
        }
        public override String _getName() => "Portable Sun";
        public override String _getDescription() => "Do not look directly at it.";
        public override int _getResearchNeeded() => 1;
        public override int _getValue() => 10000;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.PortableSun>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.UltrabrightTorch, 100)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
