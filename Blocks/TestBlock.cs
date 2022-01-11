using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Blocks {
    public class TestBlock:
    Base.ItemDropBlock<Items.Placeable.TestBlock>,
    IReactsToTouch {
        public override void PostSetDefaults() {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
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

        public override bool Slope(int i, int j) {
            /** Called when hit by a hammer.
             */
			Main.NewText("Ow!", 0xFF, 0x00, 0x00);
            return false;
		}

        public override bool RightClick(int x, int y) {
            int biome = Main.LocalPlayer.GetPrimaryBiome();
            Main.NewText($"You are in biome {biome}, clicked {x} {y}",
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

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            String name = $"{whom}";
            if(whom is Player p) name = p.name;
            else if(whom is NPC n) name = n.FullName;
            Main.NewText($"I was touched from the {direction} at {location} by {name}");
        }
    }
}
