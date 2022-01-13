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
    public class GravityBlock:
    ItemDropBlock<Items.Placeable.GravityBlock>,
    IReactsToTouch {
        /** A block that flips gravity.
         */
        public override String Texture {
            get => "REBEL/Blocks/Physics/GravityBlock/Block";
        }

        Dictionary<int, uint> Cooldown;

        public override void SetStaticDefaults() {
            (Mod as REBEL).registerTouchHandler(Type, OnTouched);
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            Cooldown = new Dictionary<int, uint>();
        }

        public void OnTouched(Entity whom, Point location,
        TouchDirection direction) {
            var tile = Main.tile[location.X, location.Y];
            if(tile.IsActuated) return; //don't react when turned off.

            //sadly, NPCs don't have gravity vars, so we can't flip them.
            if(!(whom is Player p)) return;
            //Main.NewText($"Grav touch {direction} at {location} by {p.name}");
            if(p.gravControl) return; //already have a gravity potion

            if(Cooldown.ContainsKey(whom.whoAmI)) {
                uint diff = Main.GameUpdateCount - Cooldown[whom.whoAmI];
                if(diff < 60) return; //don't instantly re-flip
                else Cooldown[whom.whoAmI] = Main.GameUpdateCount;
            }
            else Cooldown[whom.whoAmI] = Main.GameUpdateCount;

            REBEL mod = Mod as REBEL;
            mod.forceUpsideDown = !mod.forceUpsideDown;
        }

        protected void _pruneCooldownList() {
            if(Cooldown is null) return;

            var remove = new List<int>();
            foreach(var item in Cooldown) {
                if(item.Key != Main.LocalPlayer.whoAmI //no sense removing that
                && (Main.GameUpdateCount - item.Value) > 600) {
                    remove.Add(item.Key);
                }
            }
            foreach(var k in remove) {
                Cooldown.Remove(k);
            }
        }

        //repeat the middle frame to avoid duplicating it in memory.
        static int[] animFrames = {0, 1, 2, 1};
        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            frameYOffset = (animFrames[Main.tileFrame[Type] & 3]) * getFrameHeight();
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            if(++frameCounter >= 16) {
                frameCounter = 0;
                frame = (frame+1) & 3;
            }

            //now's a good time to clean this up.
            _pruneCooldownList();
        }
    }
}

namespace REBEL.Items.Placeable {
    public class GravityBlock: TilePlaceItem<Blocks.GravityBlock, GravityBlock> {
		public override String Texture {
            get => "REBEL/Blocks/Physics/GravityBlock/Item";
        }
        public override String _getName() => "Gravity Block";
        public override String _getDescription() =>
            "A block that flips gravity when touched.";
        public override int _getResearchNeeded() => 100;
        public override int _getValue() => 500;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.GravityBlock>();
			resultItem.CreateRecipe(69)
				.AddIngredient(ItemID.DirtBlock, 1)
				.Register();
		}
    }
}
