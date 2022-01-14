using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public class TestBlock:
    ItemDropBlock<Items.Placeable.TestBlock>,
    IReactsToTouch {
        public override String Texture {
            get => "REBEL/Blocks/Misc/TestBlock/Block";
        }

        public override void PostSetDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
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
            //Main.drawBetterDebug = !Main.drawBetterDebug;
            //Main.LocalPlayer.bunny = true;
            return false;
		}

        public override bool RightClick(int x, int y) {
            Player p = Main.LocalPlayer;
            int biome = p.GetPrimaryBiome();
            Main.NewText($"You are in biome {biome}, clicked {x} {y}, grav {p.gravity}",
                0x00, 0x9D, 0xF3);
            //Mod.Logger.InfoFormat("You are in biome {0}.", biome);

            Mod.Logger.Info($"TopLeft={p.TopLeft/18} BottomRight={p.BottomRight/18} pos={p.position/18}");

            //var rebel = Mod as REBEL;
            //rebel.forceUpsideDown = !rebel.forceUpsideDown;
            Wiring.TripWire(x, y, 1, 1); //send a signal
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
            Main.NewText($"[{Main.GameUpdateCount}] I was touched from the {direction} at {location} by {name}");
        }

        public override void HitWire(int i, int j) {
            //called when a signal passes through this tile via wire.
            SoundEngine.PlaySound(2, -1, -1, 16); //fart
            Tile tile = Main.tile[i, j];
            //instead of doing this we could just put an actuator on it.
            tile.IsActuated = !tile.IsActuated;
            //Main.NewText($"HitWire({Wiring._currentWireColor})");
            //1:red 2:blue 3:green 4:yellow
        }

        public override void ModifyLight(int i, int j, ref float r,
        ref float g, ref float b) {
            //Main.NewText($"Light {r} {g} {b}");
            r = 1f;
        }
    }
}

namespace REBEL.Items.Placeable {
    public class TestBlock: TilePlaceItem<Blocks.TestBlock, TestBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Misc/TestBlock/Item";
        }
        public override String _getName() => "Test Block";
        public override String _getDescription() => "A block for debugging.";

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.TestBlock>();
			resultItem.CreateRecipe(7511)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
