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
    public abstract class NumericDisplayBase<TDropItem>:
    ItemDropBlock<TDropItem>
    where TDropItem: ModItem {
        /** Abstract base class for numeric display blocks.
         */
        public override String Texture {
            //reuse this
            get => "REBEL/Blocks/Decorative/LetterBlock/Block";
        }
        abstract public int _getFrameX();
        abstract public int _getFrameY();

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
            setFrame(i, j, this._getFrameX(), this._getFrameY());
        }

        public override bool RightClick(int x, int y) {
            HitWire(x, y); //same behaviour
            return true;
        }

        public bool isANumericTile(int i, int j) {
            //Check if the tile at this location is any NumericDisplayBase tile.
            Tile tile = Main.tile[i, j];
            if(tile.type == ModContent.GetInstance<NumericDisplayDigit>().Type
            || tile.type == ModContent.GetInstance<NumericDisplayClear>().Type
            || tile.type == ModContent.GetInstance<NumericDisplayInc>().Type
            || tile.type == ModContent.GetInstance<NumericDisplayDec>().Type) {
                return true;
            }
            return false;
        }

        public bool isADigitTile(int i, int j) {
            //Check if the tile at this location is a digit tile.
            Tile tile = Main.tile[i, j];
            if(tile.type == ModContent.GetInstance<NumericDisplayDigit>().Type) {
                return true;
            }
            return false;
        }

        public void noDigit() {
            Main.NewText("No digit block near this button!", 0xFF, 0x80, 0x00);
        }

        public bool findDigit(int i, int j, ref int outI, ref int outJ) {
            //find a nearby numeric display block
            int nSpc = 0;
            int bi = i-1;

            //look left
            //it's important that we look left first, since the inc/dec buttons
            //rely on this to handle rollover.
            while(bi > Main.leftWorld && (i-bi) < 16) {
                if(isADigitTile(bi, j)) { outI = bi; outJ = j; return true; }
                else if(isANumericTile(bi, j)) bi--; //skip
                else if(nSpc == 0) { nSpc++; bi--; } //skip
                else break;
            }

            //look right
            nSpc = 0;
            bi = i+1;
            while(bi < Main.rightWorld && (bi-i) < 16) {
                if(isADigitTile(bi, j)) { outI = bi; outJ = j; return true; }
                else if(isANumericTile(bi, j)) bi++; //skip
                else if(nSpc == 0) { nSpc++; bi++; } //skip
                else break;
            }

            //look up
            nSpc = 0;
            int bj = j-1;
            while(bj > Main.topWorld && (j-bj) < 16) {
                if(isADigitTile(i, bj)) { outI = i; outJ = bj; return true; }
                else if(isANumericTile(i, bj)) bj--; //skip
                else if(nSpc == 0) { nSpc++; bj--; } //skip
                else break;
            }

            //look down
            nSpc = 0;
            bj = j+1;
            while(bj < Main.bottomWorld && (bj-j) < 16) {
                if(isADigitTile(i, bj)) { outI = i; outJ = bj; return true; }
                else if(isANumericTile(i, bj)) bj++; //skip
                else if(nSpc == 0) { nSpc++; bj++; } //skip
                else break;
            }

            //none found
            return false;
        }
    }

    public class NumericDisplayDigit:
    NumericDisplayBase<Items.Placeable.NumericDisplayDigit> {
        /** A digit in a numeric display.
         */
        public override int _getFrameX() { return  0; }
        public override int _getFrameY() { return 26; }

        public void setDigit(int i, int j, int digit) {
            //set which digit is displayed.
            //the digit 0 is the 26th tile in the texture
            setFrame(i, j, 0, digit+26);
        }

        public int getDigit(int i, int j) {
            //check which digit is displayed.
            Tile tile = Main.tile[i, j];
            return (int)(tile.frameY / getFrameHeight()) - 26;
        }

        public bool increment(int i, int j) {
            //add 1 to the displayed digit. return true if overflow.
            int d = getDigit(i, j);
            if(d == 9) {
                //Mod.Logger.Info($"Overflow {i},{j}");
                setDigit(i, j, 0);
                if(j > Main.topWorld) (Mod as REBEL).tripWire(i, j-1);
                return true; //overflow
            }
            //Mod.Logger.Info($"Increment {i},{j} to {d+1}");
            setDigit(i, j, d+1);
            return false;
        }

        public bool decrement(int i, int j) {
            //subtract 1 from the displayed digit. return true if underflow.
            int d = getDigit(i, j);
            if(d == 0) {
                //Mod.Logger.Info($"Underflow {i},{j}");
                setDigit(i, j, 9);
                if(j < Main.bottomWorld) (Mod as REBEL).tripWire(i, j+1);
                return true; //underflow
            }
            //Mod.Logger.Info($"Decrement {i},{j} to {d-1}");
            setDigit(i, j, d-1);
            return false;
        }

        public override void PlaceInWorld(int i, int j, Item item) {
            setDigit(i, j, 0);
        }

        public override void HitWire(int i, int j) {
            if((Mod as REBEL).wireAlreadyHit(i, j)) return;
            increment(i, j);
        }
    }

    public class NumericDisplayClear:
    NumericDisplayBase<Items.Placeable.NumericDisplayClear> {
        /** A clear button for a numeric display.
         */
        public override int _getFrameX() { return  0; }
        public override int _getFrameY() { return 48; }

        public override void HitWire(int i, int j) {
            if((Mod as REBEL).wireAlreadyHit(i, j)) return;

            //reset all blocks to our left
            int nSpc = 0, nDigit = 0;
            while(i > Main.leftWorld) {
                if(isADigitTile(i-1, j)) {
                    //Mod.Logger.Info($"Reset {i-1},{j}");
                    setFrame(i-1, j, 0, 26); //reset to 0
                    i--;
                    nSpc = 0;
                    nDigit++;
                }
                else if(isANumericTile(i-1, j)) {
                    //Mod.Logger.Info($"Skip button {i-1},{j}");
                    i--; //skip other buttons
                }
                else if(nSpc == 0) {
                    //Mod.Logger.Info($"Skip other {i-1},{j}");
                    //skip one non-digit tile to allow for separators
                    nSpc++;
                    i--;
                }
                else break;
            }
            //Mod.Logger.Info($"Reset {nDigit} digits");
            if(nDigit == 0) {
                Main.NewText("No digit to the left of this button!",
                    0xFF, 0x80, 0x00);
            }
        }
    }

    public class NumericDisplayInc:
    NumericDisplayBase<Items.Placeable.NumericDisplayInc> {
        /** An increment button for a numeric display.
         */
        public override int _getFrameX() { return  0; }
        public override int _getFrameY() { return 47; }

        public override void HitWire(int i, int j) {
            if((Mod as REBEL).wireAlreadyHit(i, j)) return;

            int x=0, y=0;
            if(!findDigit(i, j, ref x, ref y)) {
                noDigit();
                return;
            }

            int nDigit = 0;
            //Mod.Logger.Info("Increment start");
            while(x > Main.leftWorld) {
                nDigit++;
                //Mod.Logger.Info($"At {i},{j} inc {x},{y} ({nDigit})");
                var dTile = ModContent.GetInstance<NumericDisplayDigit>();
                if(!dTile.increment(x, y)) break;
                x--; //if overflow, loop to next digit
            }
            //Mod.Logger.Info($"Incremented {nDigit} digits");
        }
    }

    public class NumericDisplayDec:
    NumericDisplayBase<Items.Placeable.NumericDisplayDec> {
        /** A decrement button for a numeric display.
         */
        public override int _getFrameX() { return  1; }
        public override int _getFrameY() { return 47; }

        public override void HitWire(int i, int j) {
            if((Mod as REBEL).wireAlreadyHit(i, j)) return;

            int x=0, y=0;
            if(!findDigit(i, j, ref x, ref y)) {
                noDigit();
                return;
            }

            int nDigit = 0;
            //Mod.Logger.Info("Decrement start");
            while(x > Main.leftWorld) {
                nDigit++;
                //Mod.Logger.Info($"At {i},{j} dec {x},{y} ({nDigit})");
                var dTile = ModContent.GetInstance<NumericDisplayDigit>();
                if(!dTile.decrement(x, y)) break;
                x--; //if underflow, loop to next digit
            }
            //Mod.Logger.Info($"Decremented {nDigit} digits");
        }
    }
}

namespace REBEL.Items.Placeable {
    public class NumericDisplayDigit : TilePlaceItem<
    Blocks.NumericDisplayDigit, NumericDisplayDigit> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/0";
        }
        public override String _getName() => "Numeric Display Digit";
        public override String _getDescription() => "A digit that can be adjusted.";
        public override bool _showsWires() => true;
    }

    public class NumericDisplayInc : TilePlaceItem<
    Blocks.NumericDisplayInc, NumericDisplayInc> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Inc";
        }
        public override String _getName() => "Numeric Display Increment";
        public override String _getDescription() => "Increments nearby digits when activated.";
        public override bool _showsWires() => true;
    }

    public class NumericDisplayDec : TilePlaceItem<
    Blocks.NumericDisplayDec, NumericDisplayDec> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Dec";
        }
        public override String _getName() => "Numeric Display Decrement";
        public override String _getDescription() => "Decrements nearby digits when activated.";
        public override bool _showsWires() => true;
    }

    public class NumericDisplayClear : TilePlaceItem<
    Blocks.NumericDisplayClear, NumericDisplayClear> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/LetterBlock/ItemTextures/Clear";
        }
        public override String _getName() => "Numeric Display Clear";
        public override String _getDescription() => "Resets nearby digits when activated.";
        public override bool _showsWires() => true;
    }
}
