using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Items.Placeable {
    public abstract class LetterBlock<PlaceTile, DropItem>: ModItem
	where PlaceTile: ModTile
	where DropItem: ModItem {
		abstract public String _getLetter1();
		abstract public String _getLetter2();

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
			//recipe: create a stack of 69 from one dirt block.
			var resultItem = ModContent.GetInstance<DropItem>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.StoneBlock, 1)
				.Register();
		}
    }

	//so much repetition... fortunately it's generated.
	//still, there really must be a better way... why can we not specify
	//constants in the template parameters?

    public class LetterBlockA: LetterBlock<Blocks.LetterBlockA, Items.Placeable.LetterBlockA> {
        public override String _getLetter1() { return "A"; }
        public override String _getLetter2() { return "a"; }
    }
    public class LetterBlockB: LetterBlock<Blocks.LetterBlockB, Items.Placeable.LetterBlockB> {
        public override String _getLetter1() { return "B"; }
        public override String _getLetter2() { return "b"; }
    }
    public class LetterBlockC: LetterBlock<Blocks.LetterBlockC, Items.Placeable.LetterBlockC> {
        public override String _getLetter1() { return "C"; }
        public override String _getLetter2() { return "c"; }
    }
    public class LetterBlockD: LetterBlock<Blocks.LetterBlockD, Items.Placeable.LetterBlockD> {
        public override String _getLetter1() { return "D"; }
        public override String _getLetter2() { return "d"; }
    }
    public class LetterBlockE: LetterBlock<Blocks.LetterBlockE, Items.Placeable.LetterBlockE> {
        public override String _getLetter1() { return "E"; }
        public override String _getLetter2() { return "e"; }
    }
    public class LetterBlockF: LetterBlock<Blocks.LetterBlockF, Items.Placeable.LetterBlockF> {
        public override String _getLetter1() { return "F"; }
        public override String _getLetter2() { return "f"; }
    }
    public class LetterBlockG: LetterBlock<Blocks.LetterBlockG, Items.Placeable.LetterBlockG> {
        public override String _getLetter1() { return "G"; }
        public override String _getLetter2() { return "g"; }
    }
    public class LetterBlockH: LetterBlock<Blocks.LetterBlockH, Items.Placeable.LetterBlockH> {
        public override String _getLetter1() { return "H"; }
        public override String _getLetter2() { return "h"; }
    }
    public class LetterBlockI: LetterBlock<Blocks.LetterBlockI, Items.Placeable.LetterBlockI> {
        public override String _getLetter1() { return "I"; }
        public override String _getLetter2() { return "i"; }
    }
    public class LetterBlockJ: LetterBlock<Blocks.LetterBlockJ, Items.Placeable.LetterBlockJ> {
        public override String _getLetter1() { return "J"; }
        public override String _getLetter2() { return "j"; }
    }
    public class LetterBlockK: LetterBlock<Blocks.LetterBlockK, Items.Placeable.LetterBlockK> {
        public override String _getLetter1() { return "K"; }
        public override String _getLetter2() { return "k"; }
    }
    public class LetterBlockL: LetterBlock<Blocks.LetterBlockL, Items.Placeable.LetterBlockL> {
        public override String _getLetter1() { return "L"; }
        public override String _getLetter2() { return "l"; }
    }
    public class LetterBlockM: LetterBlock<Blocks.LetterBlockM, Items.Placeable.LetterBlockM> {
        public override String _getLetter1() { return "M"; }
        public override String _getLetter2() { return "m"; }
    }
    public class LetterBlockN: LetterBlock<Blocks.LetterBlockN, Items.Placeable.LetterBlockN> {
        public override String _getLetter1() { return "N"; }
        public override String _getLetter2() { return "n"; }
    }
    public class LetterBlockO: LetterBlock<Blocks.LetterBlockO, Items.Placeable.LetterBlockO> {
        public override String _getLetter1() { return "O"; }
        public override String _getLetter2() { return "o"; }
    }
    public class LetterBlockP: LetterBlock<Blocks.LetterBlockP, Items.Placeable.LetterBlockP> {
        public override String _getLetter1() { return "P"; }
        public override String _getLetter2() { return "p"; }
    }
    public class LetterBlockQ: LetterBlock<Blocks.LetterBlockQ, Items.Placeable.LetterBlockQ> {
        public override String _getLetter1() { return "Q"; }
        public override String _getLetter2() { return "q"; }
    }
    public class LetterBlockR: LetterBlock<Blocks.LetterBlockR, Items.Placeable.LetterBlockR> {
        public override String _getLetter1() { return "R"; }
        public override String _getLetter2() { return "r"; }
    }
    public class LetterBlockS: LetterBlock<Blocks.LetterBlockS, Items.Placeable.LetterBlockS> {
        public override String _getLetter1() { return "S"; }
        public override String _getLetter2() { return "s"; }
    }
    public class LetterBlockT: LetterBlock<Blocks.LetterBlockT, Items.Placeable.LetterBlockT> {
        public override String _getLetter1() { return "T"; }
        public override String _getLetter2() { return "t"; }
    }
    public class LetterBlockU: LetterBlock<Blocks.LetterBlockU, Items.Placeable.LetterBlockU> {
        public override String _getLetter1() { return "U"; }
        public override String _getLetter2() { return "u"; }
    }
    public class LetterBlockV: LetterBlock<Blocks.LetterBlockV, Items.Placeable.LetterBlockV> {
        public override String _getLetter1() { return "V"; }
        public override String _getLetter2() { return "v"; }
    }
    public class LetterBlockW: LetterBlock<Blocks.LetterBlockW, Items.Placeable.LetterBlockW> {
        public override String _getLetter1() { return "W"; }
        public override String _getLetter2() { return "w"; }
    }
    public class LetterBlockX: LetterBlock<Blocks.LetterBlockX, Items.Placeable.LetterBlockX> {
        public override String _getLetter1() { return "X"; }
        public override String _getLetter2() { return "x"; }
    }
    public class LetterBlockY: LetterBlock<Blocks.LetterBlockY, Items.Placeable.LetterBlockY> {
        public override String _getLetter1() { return "Y"; }
        public override String _getLetter2() { return "y"; }
    }
    public class LetterBlockZ: LetterBlock<Blocks.LetterBlockZ, Items.Placeable.LetterBlockZ> {
        public override String _getLetter1() { return "Z"; }
        public override String _getLetter2() { return "z"; }
    }
    public class LetterBlock0: LetterBlock<Blocks.LetterBlock0, Items.Placeable.LetterBlock0> {
        public override String _getLetter1() { return "0"; }
        public override String _getLetter2() { return ")"; }
    }
    public class LetterBlock1: LetterBlock<Blocks.LetterBlock1, Items.Placeable.LetterBlock1> {
        public override String _getLetter1() { return "1"; }
        public override String _getLetter2() { return "!"; }
    }
    public class LetterBlock2: LetterBlock<Blocks.LetterBlock2, Items.Placeable.LetterBlock2> {
        public override String _getLetter1() { return "2"; }
        public override String _getLetter2() { return "@"; }
    }
    public class LetterBlock3: LetterBlock<Blocks.LetterBlock3, Items.Placeable.LetterBlock3> {
        public override String _getLetter1() { return "3"; }
        public override String _getLetter2() { return "#"; }
    }
    public class LetterBlock4: LetterBlock<Blocks.LetterBlock4, Items.Placeable.LetterBlock4> {
        public override String _getLetter1() { return "4"; }
        public override String _getLetter2() { return "$"; }
    }
    public class LetterBlock5: LetterBlock<Blocks.LetterBlock5, Items.Placeable.LetterBlock5> {
        public override String _getLetter1() { return "5"; }
        public override String _getLetter2() { return "%"; }
    }
    public class LetterBlock6: LetterBlock<Blocks.LetterBlock6, Items.Placeable.LetterBlock6> {
        public override String _getLetter1() { return "6"; }
        public override String _getLetter2() { return "^"; }
    }
    public class LetterBlock7: LetterBlock<Blocks.LetterBlock7, Items.Placeable.LetterBlock7> {
        public override String _getLetter1() { return "7"; }
        public override String _getLetter2() { return "&"; }
    }
    public class LetterBlock8: LetterBlock<Blocks.LetterBlock8, Items.Placeable.LetterBlock8> {
        public override String _getLetter1() { return "8"; }
        public override String _getLetter2() { return "*"; }
    }
    public class LetterBlock9: LetterBlock<Blocks.LetterBlock9, Items.Placeable.LetterBlock9> {
        public override String _getLetter1() { return "9"; }
        public override String _getLetter2() { return "("; }
    }
    public class LetterBlockZGrave: LetterBlock<Blocks.LetterBlockZGrave, Items.Placeable.LetterBlockZGrave> {
        public override String _getLetter1() { return "`"; }
        public override String _getLetter2() { return "~"; }
    }
    public class LetterBlockZHyphen: LetterBlock<Blocks.LetterBlockZHyphen, Items.Placeable.LetterBlockZHyphen> {
        public override String _getLetter1() { return "-"; }
        public override String _getLetter2() { return "_"; }
    }
    public class LetterBlockZEqual: LetterBlock<Blocks.LetterBlockZEqual, Items.Placeable.LetterBlockZEqual> {
        public override String _getLetter1() { return "="; }
        public override String _getLetter2() { return "+"; }
    }
    public class LetterBlockZLeftBracket: LetterBlock<Blocks.LetterBlockZLeftBracket, Items.Placeable.LetterBlockZLeftBracket> {
        public override String _getLetter1() { return "["; }
        public override String _getLetter2() { return "{"; }
    }
    public class LetterBlockZRightBracket: LetterBlock<Blocks.LetterBlockZRightBracket, Items.Placeable.LetterBlockZRightBracket> {
        public override String _getLetter1() { return "]"; }
        public override String _getLetter2() { return "}"; }
    }
    public class LetterBlockZBackslash: LetterBlock<Blocks.LetterBlockZBackslash, Items.Placeable.LetterBlockZBackslash> {
        public override String _getLetter1() { return "\\"; }
        public override String _getLetter2() { return "|"; }
    }
    public class LetterBlockZSemicolon: LetterBlock<Blocks.LetterBlockZSemicolon, Items.Placeable.LetterBlockZSemicolon> {
        public override String _getLetter1() { return ";"; }
        public override String _getLetter2() { return ":"; }
    }
    public class LetterBlockZApostrophe: LetterBlock<Blocks.LetterBlockZApostrophe, Items.Placeable.LetterBlockZApostrophe> {
        public override String _getLetter1() { return "'"; }
        public override String _getLetter2() { return "\""; }
    }
    public class LetterBlockZComma: LetterBlock<Blocks.LetterBlockZComma, Items.Placeable.LetterBlockZComma> {
        public override String _getLetter1() { return ","; }
        public override String _getLetter2() { return "<"; }
    }
    public class LetterBlockZPeriod: LetterBlock<Blocks.LetterBlockZPeriod, Items.Placeable.LetterBlockZPeriod> {
        public override String _getLetter1() { return "."; }
        public override String _getLetter2() { return ">"; }
    }
    public class LetterBlockZSlash: LetterBlock<Blocks.LetterBlockZSlash, Items.Placeable.LetterBlockZSlash> {
        public override String _getLetter1() { return "/"; }
        public override String _getLetter2() { return "?"; }
    }
}
