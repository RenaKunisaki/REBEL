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
    public class LetterBlock:
    ItemDropBlock<Items.Placeable.LetterBlock> {
        /** A block that's a letter.
         */
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(ModContent.GetInstance<LetterBlockEntity>()
                    .Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            //XXX be able to select a default?
            //this item should be "letter placer" or something
            setFrame(i, j, 0, 0);
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            if(!(fail || effectOnly)) {
                ModContent.GetInstance<LetterBlockEntity>().Kill(i, j);
            }
        }

        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Framing.GetTileSafely(i, j);
            Point p = getFrameBlock(i, j);
            setFrame(i, j, p.X ^ 1, p.Y); //swap case
            return false;
		}

        public override bool RightClick(int x, int y) {
            /** Called when right-clicked.
             *  Used because apparently we can't hammer non-solid tiles.
             */
            //Slope(x, y);
            (Mod as REBEL).showUIForTile<LetterBlockEntity>(x, y);
            return true; //we did something, don't do default right click
        }
    } //class

    public class LetterBlockEntity: RebelModTileEntity<LetterBlock> {
        //Stores parameters for individual LetterBlock tiles.

        //The name displayed in the config UI.
        public override String displayName {get => "Letter Block";}

        [TileEnumAttribute("Letter", "Which letter to display",
            defaultValue: 0, values:
                //horrible hack because we can't pass a dict here.
                //XXX any way to generate this at runtime?
                //this is not in ASCII order because it would be
                //gross in the UI. but this is dreadful. should just
                //redo the texture to be in ASCII order, then we don't
                //need to worry about sorting either.
                //0-25
                "A\nB\nC\nD\nE\nF\nG\nH\nI\nJ\nK\nL\nM\nN\nO\nP\n"+
                "Q\nR\nS\nT\nU\nV\nW\nX\nY\nZ\n"+
                //26-51
                "a\nb\nc\nd\ne\nf\ng\nh\ni\nj\nk\nl\nm\nn\no\np\n"+
                "q\nr\ns\nt\nu\nv\nw\nx\ny\nz\n"+
                //52-61
                "0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n"+
                //62-71
                "!\n@\n#\n$\n%\n^\n&\n*\n(\n)\n"+
                //72-81
                "-\n=\n`\n[\n]\n\\\n;\n'\n,\n.\n/\n"+
                //82-91
                "_\n+\n~\n{\n}\n|\n:\n\"\n<\n>\n?"
            /* sort: //XXX needs to be converted to IDs
                "`1234567890-=\n"+
                "QWERTYUIOP{}|\n"+
                "ASDFGHJKL:\"\n"+
                "ZXCVBNM<>?\n"+
                "~!@#$%^&*()_+\n"+
                "qwertyuiop[]\\\n"+
                "asdfghjkl;'\n"+
                "zxcvbnm,./", */
            )]
        public int letter = 0;

        public override void refresh(int i, int j) {
            Tile tile = Framing.GetTileSafely(i, j);
            int x=0, y=0;
            if(letter < 26) y = letter;
            else if(letter >= 26 && letter <= 51) { x = 1; y = letter - 26; }
            else if(letter >= 52 && letter <= 61) { x = 0; y = letter - 36; }
            else if(letter >= 62 && letter <= 71) { x = 1; y = letter - 36; }
            else if(letter >= 72 && letter <= 81) { x = 0; y = letter - 46; }
            else if(letter >= 82 && letter <= 91) { x = 1; y = letter - 46; }
            Mod.Logger.Info($"Letter({i}, {j}) is {letter}: ({x}, {y})");
            ModContent.GetInstance<LetterBlock>().setFrame(i, j, x, y);
        }
    } //class
}

namespace REBEL.Items.Placeable {
    public class LetterBlock: TilePlaceItem<Blocks.LetterBlock, LetterBlock> {
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/Item";
        }

        public override String _getName() => "Letter Block";
        public override String _getDescription() =>
            "A decorative block. Right-click to change letter.";
        public override int _getResearchNeeded() => 1;
        public override int _getValue() => 0;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<LetterBlock>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.StoneBlock, 1)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
