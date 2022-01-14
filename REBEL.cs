using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using REBEL.Blocks.Base;

namespace REBEL {
	public class REBEL: Mod {
		Dictionary<ushort, Action<Entity, Point, TouchDirection>> TouchHandlers;
		Dictionary<Point, uint> lastWireHit;
		public bool forceUpsideDown; //force reverse gravity + screen flip
		public bool wasForceUpsideDown;

        public override void Load() {
			//Logger.InfoFormat("Hello world!");
			TouchHandlers = new Dictionary<ushort,
				Action<Entity, Point, TouchDirection>>();
			lastWireHit = new Dictionary<Point, uint>();
			forceUpsideDown = false;
		}

		public override void Unload() {
			//Logger.InfoFormat("Goodbye cruel world!");
			if(!(TouchHandlers is null)) TouchHandlers.Clear();
			TouchHandlers = null;
			if(!(lastWireHit is null)) lastWireHit.Clear();
			lastWireHit = null;
		}

		public void tripWire(int i, int j, int w=1, int h=1) {
			for(int y=0; y<h; y++) {
				for(int x=0; x<w; x++) {
					//avoid tile triggering itself
					if(!wireAlreadyHit(i+x, j+y)) {
						try {
							Wiring.TripWire(i+x, j+y, 1, 1);
						}
						catch(System.ArgumentException) {
							//ignore. this happens if the wire is already tripped.
							//Logger.Info("Got ArgumentException in TripWire");
						}
					}
				}
			}
		}
		public bool wireAlreadyHit(int i, int j) {
			/** For whatever reason, a tile that trips a wire somewhere
			 *  other than itself also gets tripped, even if there's not
			 *  a direct connection back to the tile.
			 *  This method ensures a tile doesn't trip multiple times
			 *  in one tick and get stuck in a loop.
			 */
			uint tick = Main.GameUpdateCount;
            Point p = new Point(i, j);
            if(!lastWireHit.ContainsKey(p)) lastWireHit[p] = 0;
            if(lastWireHit[p] == tick) return true;
            lastWireHit[p] = tick;
			return false;
		}
		public void deleteWire(int i, int j) {
			/** Remove point from lastWireHit to avoid leaking memory.
			 */
			Point p = new Point(i, j);
			if(lastWireHit.ContainsKey(p)) lastWireHit.Remove(p);
		}

		public void registerTouchHandler(ushort type,
		Action<Entity, Point, TouchDirection> handler) {
			/** Register a function to call when this type of block
			 *  is touched by the player, an NPC, or other entity.
			 */
			TouchHandlers[type] = handler;
		}

		public void checkTouchedBlocks(Entity whom) {
			/** Check if the given Entity is touching any of our blocks.
			 */
			//ignore the dead.
			//note that a dead NPC is still there, invisible, at the
			//position it died at. I assume it goes away eventually,
			//like when its entry in the array is overwritten?
            if(whom is Player p && p.statLife <= 0) return;
            if(whom is NPC n && n.life <= 0) return;

			//shorthand to make code less ugly maybe
			TouchDirection D_TL = TouchDirection.TopLeft;
			TouchDirection D_T  = TouchDirection.Top;
			TouchDirection D_TR = TouchDirection.TopRight;
			TouchDirection D_L  = TouchDirection.Left;
			TouchDirection D_I  = TouchDirection.Inside;
			TouchDirection D_R  = TouchDirection.Right;
			TouchDirection D_BL = TouchDirection.BottomLeft;
			TouchDirection D_B  = TouchDirection.Bottom;
			TouchDirection D_BR = TouchDirection.BottomRight;

			//XXX do a line intersect test using oldPosition to prevent
			//clipping at high speeds.

			Point TopLeft = new Vector2(
				whom.Hitbox.Left, whom.Hitbox.Top).ToTileCoordinates();
			//get the blocks above us, so we know when we bonk our head.
			if(whom.velocity.Y < 0) TopLeft.Y -= 1;
			//also need to do this to check for left contact.
			if(whom.velocity.X < 0) TopLeft.X -= 1;
			Point BottomRight = new Vector2(
				whom.Hitbox.Right, whom.Hitbox.Bottom).ToTileCoordinates();

			//check each tile the entity occupies.
			for(int y=TopLeft.Y; y<=BottomRight.Y; y++) {
				for(int x=TopLeft.X; x<=BottomRight.X; x++) {
					var tile = Main.tile[x, y];
					if(tile.IsActive && TouchHandlers.ContainsKey(tile.type)) {
						//work out the direction we touched from.
						//this is the opposite of the direction we are
						//from the tile, so if the contact is our bottom
						//left point, it's the tile's top right.
						TouchDirection dir = D_I;
						if(x == TopLeft.X) {
							dir = D_R;
							if     (y == TopLeft.Y)     dir = D_BR;
							else if(y == BottomRight.Y) dir = D_TR;
						}
						else if(x == BottomRight.X) {
							dir = D_L;
							if     (y == TopLeft.Y)     dir = D_BL;
							else if(y == BottomRight.Y) dir = D_TL;
						}
						else {
							if     (y == TopLeft.Y)     dir = D_B;
							else if(y == BottomRight.Y) dir = D_T;
						}
						TouchHandlers[tile.type](whom, new Point(x, y), dir);
					}
				}
			}
		}
	}
}
