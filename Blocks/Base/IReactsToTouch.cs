using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks.Base {
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

    public interface IReactsToTouch: IModType {
        /** Interface for a block that does something when touched.
         */
        
        /** Called when an entity touches the block.
         *  @param whom What touched it. Can be Player, NPC, or other Entity.
         *  @param location The tile coordinates of the block.
         *  @param direction Which part of the block was touched.
         *  @note Not called if the entity is dead.
         */
        abstract public void OnTouched(Entity whom, Point location,
        TouchDirection direction);
    }
}
