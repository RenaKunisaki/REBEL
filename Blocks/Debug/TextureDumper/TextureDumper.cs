using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public class TextureDumperBlock:
    ItemDropBlock<Items.Placeable.TextureDumperBlock> {
        //This is implemented as a block because it was the easiest way
        //I could find (since I already had block code framework) to
        //safely dump. Doing it in other places doesn't always work
        //because texture access is restricted to the main thread.
        public override String Texture {
            get => "REBEL/Blocks/Debug/TextureDumper/Block";
        }

        public override void PostSetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public override bool RightClick(int x, int y) {
            Main.NewText("Dumping, please wait...", 0xFF, 0x80, 0xFF);
            (new TextureDumper(Mod as REBEL)).dumpAllLoadedTextures();
            Main.NewText("Dump OK");

            return true; //we did something, don't do default right click
        }
    }
}

namespace REBEL.Items.Placeable {
    public class TextureDumperBlock: TilePlaceItem<Blocks.TextureDumperBlock, TextureDumperBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Debug/TextureDumper/Block";
        }
        public override String _getName() => "Texture Dumper";
        public override String _getDescription() => "Right-click to dump all loaded textures";

        public override void AddRecipes() {
			//do nothing. use Cheat Sheet or etc to instantiate.
		}
    }
}
