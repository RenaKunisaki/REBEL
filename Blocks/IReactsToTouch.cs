using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public enum TouchDirection {
        Top,     //player standing on top of this block
        Bottom,  //player hit block from below
        Left,    //player touched left side
        Right,   //player touched right side
        TopLeft, //player standing on top left corner
        TopRight,
        BottomLeft,
        BottomRight,
        Inside //player is overlapping this block
    }

    public interface IReactsToTouch {
        /** A block that does something when touched.
         */
        public void OnTouched(Player player, Point location, TouchDirection direction);
    }
}
