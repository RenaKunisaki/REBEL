using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class PlayerHooks: ModPlayer {
        Dictionary<Blocks.TouchDirection, Point> TouchOffsets;
        Dictionary<ushort, Action<Player, Point, Blocks.TouchDirection>> TouchHandlers;

        PlayerHooks() {
            //Offsets to add to position to detect touched tiles
            //in each direction. TouchDirection here refers to the part
            //of the tile being touched, so it's the opposite of the
            //player's direction.
            TouchOffsets = new Dictionary<Blocks.TouchDirection, Point>() {
                {Blocks.TouchDirection.Inside,      new Point( 0,  0)},
                {Blocks.TouchDirection.Top,         new Point( 0,  1)},
                {Blocks.TouchDirection.Bottom,      new Point( 0, -1)},
                {Blocks.TouchDirection.Left,        new Point( 1,  0)},
                {Blocks.TouchDirection.Right,       new Point(-1,  0)},
                {Blocks.TouchDirection.TopLeft,     new Point( 1,  1)},
                {Blocks.TouchDirection.TopRight,    new Point(-1,  1)},
                {Blocks.TouchDirection.BottomLeft,  new Point( 1, -1)},
                {Blocks.TouchDirection.BottomRight, new Point(-1, -1)},
            };

            //Methods to handle player touching each type.
            var bounce = ModContent.GetInstance<Blocks.BounceBlock>();
            var boost  = ModContent.GetInstance<Blocks.BoostBlock>();
            TouchHandlers = new Dictionary<ushort, Action<Player, Point, Blocks.TouchDirection>>() {
                {bounce.Type, (p,l,d) => bounce.OnTouched(p,l,d)},
                {boost .Type, (p,l,d) => boost .OnTouched(p,l,d)},
            };
        }

        public override void PostUpdate() {
            _checkPlayerTouchedBlocks(Main.LocalPlayer);
        }

        protected void _checkPlayerTouchedBlocks(Player player) {
            //Check for touched tiles.
            //touch direction is opposite of coordinate direction
            var coords = new Dictionary<Blocks.TouchDirection, Vector2>() {
                {Blocks.TouchDirection.Inside,      player.Center},
                {Blocks.TouchDirection.Top,         player.Bottom},
                {Blocks.TouchDirection.Bottom,      player.Top},
                {Blocks.TouchDirection.Left,        player.Right},
                {Blocks.TouchDirection.Right,       player.Left},
                {Blocks.TouchDirection.TopLeft,     player.BottomRight},
                {Blocks.TouchDirection.TopRight,    player.BottomLeft},
                {Blocks.TouchDirection.BottomLeft,  player.TopRight},
                {Blocks.TouchDirection.BottomRight, player.TopLeft},
            };

            foreach(var entry in coords) {
                var dir   = entry.Key;
                var coord = entry.Value;
                var offset = TouchOffsets[dir];
                //we want to look halfway to the next tile, since
                //converting to tile coords is integer truncation,
                //so if we don't do that, it won't work for top/left.
                int tx    = (int)((coord.X + ((float)offset.X * 8.0)) / 16.0);
                int ty    = (int)((coord.Y + ((float)offset.Y * 8.0)) / 16.0);
                var loc   = new Point(tx, ty);
                var tile  = Main.tile[tx, ty];
                if(tile.IsActive && TouchHandlers.ContainsKey(tile.type)) {
                    TouchHandlers[tile.type](player, loc, dir);
                }
            }
        }
    }
}
