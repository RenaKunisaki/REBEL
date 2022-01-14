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
    public class CollisionTest:
    ItemDropBlock<Items.Placeable.CollisionTest> {
        /** A block that changes color depending what part
         *  of the player touches it.
         */
        enum Direction {
            None,
            Top, Right, Bottom, Left,
            TopRight, BottomRight, BottomLeft, TopLeft,
            Inside
        }
        public override String Texture {
            get => "REBEL/Blocks/Debug/CollisionTest/Block";
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

        public override void NearbyEffects(int i, int j, bool closer) {
            var hit = Main.LocalPlayer.Hitbox;
            int left = (int)(hit.Left / 16);
            int right = (int)(hit.Right / 16);
            int top = (int)(hit.Top / 16);
            int bottom = (int)(hit.Bottom / 16);

            Direction dir = Direction.None;
            if(left == i) {
                if(top == j) dir = Direction.TopLeft;
                else if(bottom == j) dir = Direction.BottomLeft;
                else dir = Direction.Left;
            }
            else if(right == i) {
                if(top == j) dir = Direction.TopRight;
                else if(bottom == j) dir = Direction.BottomRight;
                else dir = Direction.Right;
            }
            else if(top == j) dir = Direction.Top;
            else if(bottom == j) dir = Direction.Bottom;

            setFrame(i, j, 0, (int)dir);
        }
    }
}

namespace REBEL.Items.Placeable {
    public class CollisionTest: TilePlaceItem<Blocks.CollisionTest, CollisionTest> {
		public override String Texture {
            get => "REBEL/Blocks/Debug/CollisionTest/Item";
        }
        public override String _getName() => "Collision Test";
        public override String _getDescription() => "Lights up to show hitbox regions.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;
        public override void AddRecipes() {
			//do nothing
		}
    }
}
