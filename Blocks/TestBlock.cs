using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class TestBlock : ModTile {
        public override void PostSetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            //TileObjectData.newTile.Width = 1; //tile coords
            //TileObjectData.newTile.Height = 1;
            ////which tile is under the cursor when placing
            //TileObjectData.newTile.Origin = new Point16(0, 0);

            //TileObjectData.newTile.CoordinateHeights = new int[] { 16 };
            //TileObjectData.newTile.CoordinateWidth = 16;
            //TileObjectData.newTile.CoordinatePadding = 2;

            //TileObjectData.newTile.HookCheck = new PlacementHook(CanPlace, -1, 0, true);
            //TileObjectData.newTile.UsesCustomCanPlace = true;
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Hook_AfterPlacement, -1, 0, false);
            TileObjectData.addTile(Type);

            //add entry to minimap
            //why can't we chain here? :(
            //anyway we don't usually need this.
            //ModTranslation name = CreateMapEntryName();
            //name.SetDefault("Test Block");
            //AddMapEntry(new Color(0x00, 0x9D, 0xF3), name);
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            //Called when this tile is hit by a pickaxe.
            //i, j: this tile's world tile-coordinates.
            //fail: whether we didn't hit hard enough to destroy it.
            //effectOnly: only create dust
            //noItem: don't drop anything.
            if(fail) return;

            Main.NewText("YOU'VE KILLED ME!", 0x00, 0x9D, 0xF3);
            if(!noItem) {
                Item.NewItem(i * 16, j * 16, 16, 32,
                    ModContent.ItemType<Items.Placeable.BoostBlock>());
            }
		}

        //public override void KillMultiTile(int i, int j, int frameX, int frameY) {
        //    //used for multi-tile objects.
        //    Main.NewText(
        //        String.Format("YOU'VE KILLED ME! {0} {1}, frame {2} {3}",
        //            i, j, frameX, frameY),
        //        0x00, 0x9D, 0xF3);
		//	Item.NewItem(i * 16, j * 16, 16, 32,
        //        ModContent.ItemType<Items.Placeable.BoostBlock>());
		//}

        public override bool RightClick(int x, int y) {
            int biome = Main.LocalPlayer.GetPrimaryBiome();
            Main.NewText(
                String.Format("You are in biome {0}, clicked {1} {2}",
                    biome, x, y),
                0x00, 0x9D, 0xF3);
            //Mod.Logger.InfoFormat("You are in biome {0}.", biome);
            return true; //we did something, don't do default right click
        }

        //public override void NearbyEffects(int i, int j, bool closer) {
            //called when player is nearby.
            //i, j are world tile-coordinates of this tile.
            //closer is if the player is close enough for things like clocks
            //to do their automatic effect.
            //Main.NewText(
            //    String.Format("NearbyEffects {0}, {1}, {2}", i, j, closer),
            //    0x00, 0x9D, 0xF3);
        //}

        public override void FloorVisuals(Player player) {
            //called when player stands on this tile
            player.velocity.Y = -10;
            //Main.NewText("BOING!", 0x00, 0x9D, 0xF3);
        }
    }
}
