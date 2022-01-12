using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public abstract class LetterBlock<DropItemType>:
    ItemDropBlock<DropItemType>
    where DropItemType: ModItem {
        /** A block that's a letter.
         */
        public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/LetterBlock";
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
            Tile tile = Main.tile[i, j];
            tile.frameY = (short)(_getFrame() * 18);
            if(Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(-1, Player.tileTargetX,
                    Player.tileTargetY, 1, TileChangeType.None);
			}
        }

        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Tile tile = Main.tile[i, j];
            tile.frameX ^= 18;
            tile.frameY = (short)(_getFrame() * 18);
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
    public class LetterBlockZGrave: LetterBlock<Items.Placeable.LetterBlockZGrave> {
        public override int _getFrame() { return 36; }
    }
    public class LetterBlockZHyphen: LetterBlock<Items.Placeable.LetterBlockZHyphen> {
        public override int _getFrame() { return 37; }
    }
    public class LetterBlockZEqual: LetterBlock<Items.Placeable.LetterBlockZEqual> {
        public override int _getFrame() { return 38; }
    }
    public class LetterBlockZLeftBracket: LetterBlock<Items.Placeable.LetterBlockZLeftBracket> {
        public override int _getFrame() { return 39; }
    }
    public class LetterBlockZRightBracket: LetterBlock<Items.Placeable.LetterBlockZRightBracket> {
        public override int _getFrame() { return 40; }
    }
    public class LetterBlockZBackslash: LetterBlock<Items.Placeable.LetterBlockZBackslash> {
        public override int _getFrame() { return 41; }
    }
    public class LetterBlockZSemicolon: LetterBlock<Items.Placeable.LetterBlockZSemicolon> {
        public override int _getFrame() { return 42; }
    }
    public class LetterBlockZApostrophe: LetterBlock<Items.Placeable.LetterBlockZApostrophe> {
        public override int _getFrame() { return 43; }
    }
    public class LetterBlockZComma: LetterBlock<Items.Placeable.LetterBlockZComma> {
        public override int _getFrame() { return 44; }
    }
    public class LetterBlockZPeriod: LetterBlock<Items.Placeable.LetterBlockZPeriod> {
        public override int _getFrame() { return 45; }
    }
    public class LetterBlockZSlash: LetterBlock<Items.Placeable.LetterBlockZSlash> {
        public override int _getFrame() { return 46; }
    }
}
