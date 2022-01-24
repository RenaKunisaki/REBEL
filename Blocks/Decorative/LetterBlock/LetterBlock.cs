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
    public abstract class LetterBlock<TDropItem>:
    ItemDropBlock<TDropItem>
    where TDropItem: ModItem {
        /** A block that's a letter.
         */
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/Block";
        }

        abstract public int _getFrame();

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            setFrame(i, j, 0, _getFrame());
        }

        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Framing.GetTileSafely(i, j);
            Point p = getFrameBlock(i, j);
            setFrame(i, j, p.X ^ 1, _getFrame());
            return false;
		}

        public override bool RightClick(int x, int y) {
            /** Called when right-clicked.
             *  Used because apparently we can't hammer non-solid tiles.
             */
            Slope(x, y);
            return true; //we did something, don't do default right click
        }
    }

    //generated code go!
    public class LetterBlockA: LetterBlock<Items.Placeable.LetterBlockA> {
        public override int _getFrame() { return 0; }
    }
    public class LetterBlockB: LetterBlock<Items.Placeable.LetterBlockB> {
        public override int _getFrame() { return 1; }
    }
    public class LetterBlockC: LetterBlock<Items.Placeable.LetterBlockC> {
        public override int _getFrame() { return 2; }
    }
    public class LetterBlockD: LetterBlock<Items.Placeable.LetterBlockD> {
        public override int _getFrame() { return 3; }
    }
    public class LetterBlockE: LetterBlock<Items.Placeable.LetterBlockE> {
        public override int _getFrame() { return 4; }
    }
    public class LetterBlockF: LetterBlock<Items.Placeable.LetterBlockF> {
        public override int _getFrame() { return 5; }
    }
    public class LetterBlockG: LetterBlock<Items.Placeable.LetterBlockG> {
        public override int _getFrame() { return 6; }
    }
    public class LetterBlockH: LetterBlock<Items.Placeable.LetterBlockH> {
        public override int _getFrame() { return 7; }
    }
    public class LetterBlockI: LetterBlock<Items.Placeable.LetterBlockI> {
        public override int _getFrame() { return 8; }
    }
    public class LetterBlockJ: LetterBlock<Items.Placeable.LetterBlockJ> {
        public override int _getFrame() { return 9; }
    }
    public class LetterBlockK: LetterBlock<Items.Placeable.LetterBlockK> {
        public override int _getFrame() { return 10; }
    }
    public class LetterBlockL: LetterBlock<Items.Placeable.LetterBlockL> {
        public override int _getFrame() { return 11; }
    }
    public class LetterBlockM: LetterBlock<Items.Placeable.LetterBlockM> {
        public override int _getFrame() { return 12; }
    }
    public class LetterBlockN: LetterBlock<Items.Placeable.LetterBlockN> {
        public override int _getFrame() { return 13; }
    }
    public class LetterBlockO: LetterBlock<Items.Placeable.LetterBlockO> {
        public override int _getFrame() { return 14; }
    }
    public class LetterBlockP: LetterBlock<Items.Placeable.LetterBlockP> {
        public override int _getFrame() { return 15; }
    }
    public class LetterBlockQ: LetterBlock<Items.Placeable.LetterBlockQ> {
        public override int _getFrame() { return 16; }
    }
    public class LetterBlockR: LetterBlock<Items.Placeable.LetterBlockR> {
        public override int _getFrame() { return 17; }
    }
    public class LetterBlockS: LetterBlock<Items.Placeable.LetterBlockS> {
        public override int _getFrame() { return 18; }
    }
    public class LetterBlockT: LetterBlock<Items.Placeable.LetterBlockT> {
        public override int _getFrame() { return 19; }
    }
    public class LetterBlockU: LetterBlock<Items.Placeable.LetterBlockU> {
        public override int _getFrame() { return 20; }
    }
    public class LetterBlockV: LetterBlock<Items.Placeable.LetterBlockV> {
        public override int _getFrame() { return 21; }
    }
    public class LetterBlockW: LetterBlock<Items.Placeable.LetterBlockW> {
        public override int _getFrame() { return 22; }
    }
    public class LetterBlockX: LetterBlock<Items.Placeable.LetterBlockX> {
        public override int _getFrame() { return 23; }
    }
    public class LetterBlockY: LetterBlock<Items.Placeable.LetterBlockY> {
        public override int _getFrame() { return 24; }
    }
    public class LetterBlockZ: LetterBlock<Items.Placeable.LetterBlockZ> {
        public override int _getFrame() { return 25; }
    }
    public class LetterBlock0: LetterBlock<Items.Placeable.LetterBlock0> {
        public override int _getFrame() { return 26; }
    }
    public class LetterBlock1: LetterBlock<Items.Placeable.LetterBlock1> {
        public override int _getFrame() { return 27; }
    }
    public class LetterBlock2: LetterBlock<Items.Placeable.LetterBlock2> {
        public override int _getFrame() { return 28; }
    }
    public class LetterBlock3: LetterBlock<Items.Placeable.LetterBlock3> {
        public override int _getFrame() { return 29; }
    }
    public class LetterBlock4: LetterBlock<Items.Placeable.LetterBlock4> {
        public override int _getFrame() { return 30; }
    }
    public class LetterBlock5: LetterBlock<Items.Placeable.LetterBlock5> {
        public override int _getFrame() { return 31; }
    }
    public class LetterBlock6: LetterBlock<Items.Placeable.LetterBlock6> {
        public override int _getFrame() { return 32; }
    }
    public class LetterBlock7: LetterBlock<Items.Placeable.LetterBlock7> {
        public override int _getFrame() { return 33; }
    }
    public class LetterBlock8: LetterBlock<Items.Placeable.LetterBlock8> {
        public override int _getFrame() { return 34; }
    }
    public class LetterBlock9: LetterBlock<Items.Placeable.LetterBlock9> {
        public override int _getFrame() { return 35; }
    }
    public class LetterBlockZHyphen: LetterBlock<Items.Placeable.LetterBlockZHyphen> {
        public override int _getFrame() { return 36; }
    }
    public class LetterBlockZEqual: LetterBlock<Items.Placeable.LetterBlockZEqual> {
        public override int _getFrame() { return 37; }
    }
    public class LetterBlockZLeftBracket: LetterBlock<Items.Placeable.LetterBlockZLeftBracket> {
        public override int _getFrame() { return 38; }
    }
    public class LetterBlockZRightBracket: LetterBlock<Items.Placeable.LetterBlockZRightBracket> {
        public override int _getFrame() { return 39; }
    }
    public class LetterBlockZBackslash: LetterBlock<Items.Placeable.LetterBlockZBackslash> {
        public override int _getFrame() { return 40; }
    }
    public class LetterBlockZSemicolon: LetterBlock<Items.Placeable.LetterBlockZSemicolon> {
        public override int _getFrame() { return 41; }
    }
    public class LetterBlockZApostrophe: LetterBlock<Items.Placeable.LetterBlockZApostrophe> {
        public override int _getFrame() { return 42; }
    }
    public class LetterBlockZComma: LetterBlock<Items.Placeable.LetterBlockZComma> {
        public override int _getFrame() { return 43; }
    }
    public class LetterBlockZPeriod: LetterBlock<Items.Placeable.LetterBlockZPeriod> {
        public override int _getFrame() { return 44; }
    }
    public class LetterBlockZSlash: LetterBlock<Items.Placeable.LetterBlockZSlash> {
        public override int _getFrame() { return 45; }
    }
    public class LetterBlockZGrave: LetterBlock<Items.Placeable.LetterBlockZGrave> {
        public override int _getFrame() { return 46; }
    }
}

namespace REBEL.Items.Placeable {
    public abstract class LetterBlock<PlaceTile, DropItem>: ModItem
	where PlaceTile: ModTile
	where DropItem: ModItem {
        abstract public String _getLetter1();
		abstract public String _getLetter2();

        public override String Texture {
            get {
                String letter1 = _getLetter1();
                return $"REBEL/Blocks/Decorative/LetterBlock/LetterBlock{letter1}";
            }
        }

		public override void SetStaticDefaults() {
			String letter1 = _getLetter1();
			String letter2 = _getLetter2();
            Tooltip.SetDefault($"A decorative block. Right-click to switch between {letter1} and {letter2}.");
            DisplayName.SetDefault($"Letter Block {letter1} / {letter2}");
            Terraria.GameContent.Creative.CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
			Item.value = 0;
			Item.createTile = ModContent.TileType<PlaceTile>();
		}

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<DropItem>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.StoneBlock, 1)
                .AddTile(TileID.WorkBenches)
				.Register();
		}
    }

	//so much repetition... fortunately it's generated.
	//still, there really must be a better way... why can we not specify
	//constants in the template parameters?
    public class LetterBlockA: LetterBlock<Blocks.LetterBlockA, Items.Placeable.LetterBlockA> {
        public override String _getLetter1() { return "A"; }
        public override String _getLetter2() { return "a"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/A";
        }
    }
    public class LetterBlockB: LetterBlock<Blocks.LetterBlockB, Items.Placeable.LetterBlockB> {
        public override String _getLetter1() { return "B"; }
        public override String _getLetter2() { return "b"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/B";
        }
    }
    public class LetterBlockC: LetterBlock<Blocks.LetterBlockC, Items.Placeable.LetterBlockC> {
        public override String _getLetter1() { return "C"; }
        public override String _getLetter2() { return "c"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/C";
        }
    }
    public class LetterBlockD: LetterBlock<Blocks.LetterBlockD, Items.Placeable.LetterBlockD> {
        public override String _getLetter1() { return "D"; }
        public override String _getLetter2() { return "d"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/D";
        }
    }
    public class LetterBlockE: LetterBlock<Blocks.LetterBlockE, Items.Placeable.LetterBlockE> {
        public override String _getLetter1() { return "E"; }
        public override String _getLetter2() { return "e"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/E";
        }
    }
    public class LetterBlockF: LetterBlock<Blocks.LetterBlockF, Items.Placeable.LetterBlockF> {
        public override String _getLetter1() { return "F"; }
        public override String _getLetter2() { return "f"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/F";
        }
    }
    public class LetterBlockG: LetterBlock<Blocks.LetterBlockG, Items.Placeable.LetterBlockG> {
        public override String _getLetter1() { return "G"; }
        public override String _getLetter2() { return "g"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/G";
        }
    }
    public class LetterBlockH: LetterBlock<Blocks.LetterBlockH, Items.Placeable.LetterBlockH> {
        public override String _getLetter1() { return "H"; }
        public override String _getLetter2() { return "h"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/H";
        }
    }
    public class LetterBlockI: LetterBlock<Blocks.LetterBlockI, Items.Placeable.LetterBlockI> {
        public override String _getLetter1() { return "I"; }
        public override String _getLetter2() { return "i"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/I";
        }
    }
    public class LetterBlockJ: LetterBlock<Blocks.LetterBlockJ, Items.Placeable.LetterBlockJ> {
        public override String _getLetter1() { return "J"; }
        public override String _getLetter2() { return "j"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/J";
        }
    }
    public class LetterBlockK: LetterBlock<Blocks.LetterBlockK, Items.Placeable.LetterBlockK> {
        public override String _getLetter1() { return "K"; }
        public override String _getLetter2() { return "k"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/K";
        }
    }
    public class LetterBlockL: LetterBlock<Blocks.LetterBlockL, Items.Placeable.LetterBlockL> {
        public override String _getLetter1() { return "L"; }
        public override String _getLetter2() { return "l"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/L";
        }
    }
    public class LetterBlockM: LetterBlock<Blocks.LetterBlockM, Items.Placeable.LetterBlockM> {
        public override String _getLetter1() { return "M"; }
        public override String _getLetter2() { return "m"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/M";
        }
    }
    public class LetterBlockN: LetterBlock<Blocks.LetterBlockN, Items.Placeable.LetterBlockN> {
        public override String _getLetter1() { return "N"; }
        public override String _getLetter2() { return "n"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/N";
        }
    }
    public class LetterBlockO: LetterBlock<Blocks.LetterBlockO, Items.Placeable.LetterBlockO> {
        public override String _getLetter1() { return "O"; }
        public override String _getLetter2() { return "o"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/O";
        }
    }
    public class LetterBlockP: LetterBlock<Blocks.LetterBlockP, Items.Placeable.LetterBlockP> {
        public override String _getLetter1() { return "P"; }
        public override String _getLetter2() { return "p"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/P";
        }
    }
    public class LetterBlockQ: LetterBlock<Blocks.LetterBlockQ, Items.Placeable.LetterBlockQ> {
        public override String _getLetter1() { return "Q"; }
        public override String _getLetter2() { return "q"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Q";
        }
    }
    public class LetterBlockR: LetterBlock<Blocks.LetterBlockR, Items.Placeable.LetterBlockR> {
        public override String _getLetter1() { return "R"; }
        public override String _getLetter2() { return "r"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/R";
        }
    }
    public class LetterBlockS: LetterBlock<Blocks.LetterBlockS, Items.Placeable.LetterBlockS> {
        public override String _getLetter1() { return "S"; }
        public override String _getLetter2() { return "s"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/S";
        }
    }
    public class LetterBlockT: LetterBlock<Blocks.LetterBlockT, Items.Placeable.LetterBlockT> {
        public override String _getLetter1() { return "T"; }
        public override String _getLetter2() { return "t"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/T";
        }
    }
    public class LetterBlockU: LetterBlock<Blocks.LetterBlockU, Items.Placeable.LetterBlockU> {
        public override String _getLetter1() { return "U"; }
        public override String _getLetter2() { return "u"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/U";
        }
    }
    public class LetterBlockV: LetterBlock<Blocks.LetterBlockV, Items.Placeable.LetterBlockV> {
        public override String _getLetter1() { return "V"; }
        public override String _getLetter2() { return "v"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/V";
        }
    }
    public class LetterBlockW: LetterBlock<Blocks.LetterBlockW, Items.Placeable.LetterBlockW> {
        public override String _getLetter1() { return "W"; }
        public override String _getLetter2() { return "w"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/W";
        }
    }
    public class LetterBlockX: LetterBlock<Blocks.LetterBlockX, Items.Placeable.LetterBlockX> {
        public override String _getLetter1() { return "X"; }
        public override String _getLetter2() { return "x"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/X";
        }
    }
    public class LetterBlockY: LetterBlock<Blocks.LetterBlockY, Items.Placeable.LetterBlockY> {
        public override String _getLetter1() { return "Y"; }
        public override String _getLetter2() { return "y"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Y";
        }
    }
    public class LetterBlockZ: LetterBlock<Blocks.LetterBlockZ, Items.Placeable.LetterBlockZ> {
        public override String _getLetter1() { return "Z"; }
        public override String _getLetter2() { return "z"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Z";
        }
    }
    public class LetterBlock0: LetterBlock<Blocks.LetterBlock0, Items.Placeable.LetterBlock0> {
        public override String _getLetter1() { return "0"; }
        public override String _getLetter2() { return ")"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/0";
        }
    }
    public class LetterBlock1: LetterBlock<Blocks.LetterBlock1, Items.Placeable.LetterBlock1> {
        public override String _getLetter1() { return "1"; }
        public override String _getLetter2() { return "!"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/1";
        }
    }
    public class LetterBlock2: LetterBlock<Blocks.LetterBlock2, Items.Placeable.LetterBlock2> {
        public override String _getLetter1() { return "2"; }
        public override String _getLetter2() { return "@"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/2";
        }
    }
    public class LetterBlock3: LetterBlock<Blocks.LetterBlock3, Items.Placeable.LetterBlock3> {
        public override String _getLetter1() { return "3"; }
        public override String _getLetter2() { return "#"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/3";
        }
    }
    public class LetterBlock4: LetterBlock<Blocks.LetterBlock4, Items.Placeable.LetterBlock4> {
        public override String _getLetter1() { return "4"; }
        public override String _getLetter2() { return "$"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/4";
        }
    }
    public class LetterBlock5: LetterBlock<Blocks.LetterBlock5, Items.Placeable.LetterBlock5> {
        public override String _getLetter1() { return "5"; }
        public override String _getLetter2() { return "%"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/5";
        }
    }
    public class LetterBlock6: LetterBlock<Blocks.LetterBlock6, Items.Placeable.LetterBlock6> {
        public override String _getLetter1() { return "6"; }
        public override String _getLetter2() { return "^"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/6";
        }
    }
    public class LetterBlock7: LetterBlock<Blocks.LetterBlock7, Items.Placeable.LetterBlock7> {
        public override String _getLetter1() { return "7"; }
        public override String _getLetter2() { return "&"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/7";
        }
    }
    public class LetterBlock8: LetterBlock<Blocks.LetterBlock8, Items.Placeable.LetterBlock8> {
        public override String _getLetter1() { return "8"; }
        public override String _getLetter2() { return "*"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/8";
        }
    }
    public class LetterBlock9: LetterBlock<Blocks.LetterBlock9, Items.Placeable.LetterBlock9> {
        public override String _getLetter1() { return "9"; }
        public override String _getLetter2() { return "("; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/9";
        }
    }
    public class LetterBlockZHyphen: LetterBlock<Blocks.LetterBlockZHyphen, Items.Placeable.LetterBlockZHyphen> {
        public override String _getLetter1() { return "-"; }
        public override String _getLetter2() { return "_"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZHyphen";
        }
    }
    public class LetterBlockZEqual: LetterBlock<Blocks.LetterBlockZEqual, Items.Placeable.LetterBlockZEqual> {
        public override String _getLetter1() { return "="; }
        public override String _getLetter2() { return "+"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZEqual";
        }
    }
    public class LetterBlockZLeftBracket: LetterBlock<Blocks.LetterBlockZLeftBracket, Items.Placeable.LetterBlockZLeftBracket> {
        public override String _getLetter1() { return "["; }
        public override String _getLetter2() { return "{"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZLeftBracket";
        }
    }
    public class LetterBlockZRightBracket: LetterBlock<Blocks.LetterBlockZRightBracket, Items.Placeable.LetterBlockZRightBracket> {
        public override String _getLetter1() { return "]"; }
        public override String _getLetter2() { return "}"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZRightBracket";
        }
    }
    public class LetterBlockZBackslash: LetterBlock<Blocks.LetterBlockZBackslash, Items.Placeable.LetterBlockZBackslash> {
        public override String _getLetter1() { return "\\"; }
        public override String _getLetter2() { return "|"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZBackslash";
        }
    }
    public class LetterBlockZSemicolon: LetterBlock<Blocks.LetterBlockZSemicolon, Items.Placeable.LetterBlockZSemicolon> {
        public override String _getLetter1() { return ";"; }
        public override String _getLetter2() { return ":"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZSemicolon";
        }
    }
    public class LetterBlockZApostrophe: LetterBlock<Blocks.LetterBlockZApostrophe, Items.Placeable.LetterBlockZApostrophe> {
        public override String _getLetter1() { return "'"; }
        public override String _getLetter2() { return "\""; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZApostrophe";
        }
    }
    public class LetterBlockZComma: LetterBlock<Blocks.LetterBlockZComma, Items.Placeable.LetterBlockZComma> {
        public override String _getLetter1() { return ","; }
        public override String _getLetter2() { return "<"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZComma";
        }
    }
    public class LetterBlockZPeriod: LetterBlock<Blocks.LetterBlockZPeriod, Items.Placeable.LetterBlockZPeriod> {
        public override String _getLetter1() { return "."; }
        public override String _getLetter2() { return ">"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZPeriod";
        }
    }
    public class LetterBlockZSlash: LetterBlock<Blocks.LetterBlockZSlash, Items.Placeable.LetterBlockZSlash> {
        public override String _getLetter1() { return "/"; }
        public override String _getLetter2() { return "?"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZSlash";
        }
    }
    public class LetterBlockZGrave: LetterBlock<Blocks.LetterBlockZGrave, Items.Placeable.LetterBlockZGrave> {
        public override String _getLetter1() { return "`"; }
        public override String _getLetter2() { return "~"; }
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/ZGrave";
        }
    }
}
