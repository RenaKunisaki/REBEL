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

namespace REBEL {
	public class REBEL: Mod {
		Dictionary<Blocks.TouchDirection, Point> TouchOffsets;
        Dictionary<ushort, Action<Entity, Point, Blocks.TouchDirection>> TouchHandlers;

        public override void Load() {
			Logger.InfoFormat("Hello world!");

            //Methods to handle whom touching each type.
			//XXX can this be automated? somehow test if the tile is of
			//a type that has an OnTouched method?
            var test   = ModContent.GetInstance<Blocks.TestBlock>();
            var bounce = ModContent.GetInstance<Blocks.BounceBlock>();
            var boost  = ModContent.GetInstance<Blocks.BoostBlock>();
            var heal   = ModContent.GetInstance<Blocks.HealHurtBlock>();
            TouchHandlers = new Dictionary<ushort, Action<Entity, Point, Blocks.TouchDirection>>() {
                {test  .Type, (p,l,d) => test  .OnTouched(p,l,d)},
                {bounce.Type, (p,l,d) => bounce.OnTouched(p,l,d)},
                {boost .Type, (p,l,d) => boost .OnTouched(p,l,d)},
                {heal  .Type, (p,l,d) => heal  .OnTouched(p,l,d)},
            };
		}

		public override void Unload() {
			Logger.InfoFormat("Goodbye cruel world!");
		}

		public void checkTouchedBlocks(Entity whom) {
			/** Check if the given Entity is touching any of our blocks.
			 */
			//ignore the dead.
            if(whom is Player p && p.statLife <= 0) return;
            if(whom is NPC n && n.life <= 0) return;

			//shorthand to make code less ugly maybe
			Blocks.TouchDirection D_TL = Blocks.TouchDirection.TopLeft;
			Blocks.TouchDirection D_T  = Blocks.TouchDirection.Top;
			Blocks.TouchDirection D_TR = Blocks.TouchDirection.TopRight;
			Blocks.TouchDirection D_L  = Blocks.TouchDirection.Left;
			Blocks.TouchDirection D_I  = Blocks.TouchDirection.Inside;
			Blocks.TouchDirection D_R  = Blocks.TouchDirection.Right;
			Blocks.TouchDirection D_BL = Blocks.TouchDirection.BottomLeft;
			Blocks.TouchDirection D_B  = Blocks.TouchDirection.Bottom;
			Blocks.TouchDirection D_BR = Blocks.TouchDirection.BottomRight;

			Point TopLeft = whom.TopLeft.ToTileCoordinates();
			//get the blocks above us, so we know when we bonk our head.
			if(whom.velocity.Y < 0) TopLeft.Y -= 1;
			Point BottomRight = whom.BottomRight.ToTileCoordinates();

			for(int y=TopLeft.Y; y<=BottomRight.Y; y++) {
				for(int x=TopLeft.X; x<=BottomRight.X; x++) {
					var tile = Main.tile[x, y];
					if(tile.IsActive && TouchHandlers.ContainsKey(tile.type)) {
						Blocks.TouchDirection dir = D_I;
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
