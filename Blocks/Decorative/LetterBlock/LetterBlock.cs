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
            setFrame(i, j, 1, 2); //A
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            if(!(fail || effectOnly)) {
                ModContent.GetInstance<LetterBlockEntity>().Kill(i, j);
            }
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
            values:
                //horrible hack because we can't pass a dict here.
                //XXX any way to generate this at runtime?
                "33\t!\n\"\n#\n$\n%\n&\n'\n(\n)\n*\n+\n,\n-\n.\n/\n"+
                "0\n1\n2\n3\n4\n5\n6\n7\n8\n9\n:\n;\n<\n=\n>\n?\n"+
                "@\nA\nB\nC\nD\nE\nF\nG\nH\nI\nJ\nK\nL\nM\nN\nO\n"+
                "P\nQ\nR\nS\nT\nU\nV\nW\nX\nY\nZ\n[\n\\\n]\n^\n_\n"+
                "`\na\nb\nc\nd\ne\nf\ng\nh\ni\nj\nk\nl\nm\nn\no\n"+
                "p\nq\nr\ns\nt\nu\nv\nw\nx\ny\nz\n{\n|\n}\n~\n"+
                "3\tUp\nUpRight\nRight\nDownRight\nDown\nDownLeft\nLeft\nUpLeft",
            sort: //ASCII values in QWERTY order
                "96,49,50,51,52,53,54,55,56,57,48,45,61\n"+ //`1234567890-=
                "81,87,69,82,84,89,85,73,79,80,123,125,124\n"+ //QWERTYUIOP{}|
                "65,83,68,70,71,72,74,75,76,58,34\n"+ //ASDFGHJKL:"
                "90,88,67,86,66,78,77,60,62,63\n"+ //ZXCVBNM<>?
                "126,33,64,35,36,37,94,38,42,40,41,95,43\n"+ //~!@#$%^&*()_+
                "113,119,101,114,116,121,117,105,111,112,91,93,92\n"+ //qwertyuiop[]\ .
                "97,115,100,102,103,104,106,107,108,59,39\n"+ //asdfghjkl;'
                "122,120,99,118,98,110,109,44,46,47\n"+ //zxcvbnm,./
                "3,4,5,6\n7,8,9,10", //arrows
            defaultValue: 0x41 //A
        )]
        public int letter = 0x41;

        public override void refresh(int i, int j) {
            Tile tile = Framing.GetTileSafely(i, j);
            ModContent.GetInstance<LetterBlock>().setFrame(i, j,
                letter & 0xF, letter >> 4);
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
