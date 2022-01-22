using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.Blocks;
using REBEL.Blocks.Base;

namespace REBEL.Blocks {
    public class RainbowLamp:
    ItemDropBlock<Items.Placeable.RainbowLamp> {
        /** Emits light that cycles through colours.
         */
        //XXX refer to ExampleTorch for better animation
        public override String Texture {
            get => "REBEL/Blocks/Decorative/RainbowLamp/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.newTile.HookPostPlaceMyPlayer =
                new PlacementHook(ModContent.GetInstance<RainbowLampEntity>()
                    .Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
        }

        public override void AnimateIndividualTile(int type, int i, int j,
        ref int frameXOffset, ref int frameYOffset) {
            if(getFrameBlock(i, j).X != 0) return; //turned off

            float intensity = 0.5f;
            int speed = 2048;
            int index = ModContent.GetInstance<RainbowLampEntity>().Find(i, j);
            if(index < 0) {
                Mod.Logger.Info($"No RainbowLampEntity for {i}, {j}");
            }
            else {
                var entity = (RainbowLampEntity)TileEntity.ByID[index];
                intensity = entity.lightIntensity;
                speed = entity.animSpeed;
            }
            if(speed == 0) speed = 2048;

            float hue = (float)Main.tileFrame[Type] / (float)speed;
            Color color = Main.hslToRgb(hue, 1f, 0.5f);
            Lighting.AddLight(new Vector2(i, j).ToWorldCoordinates(),
                color.ToVector3() * intensity);
            //the vector can be scaled for more light.
            //at 255 it's basically a portable sun.
        }

        public override void AnimateTile(ref int frame, ref int frameCounter) {
            frame = (int)((((uint)frame)+1) & 0x7FFFFFFF);
        }

        public override void HitWire(int i, int j) {
            Point p = getFrameBlock(i, j);
            p.X ^= 1; //turn the lamp
            setFrame(i, j, p.X, p.Y);
        }

        public override float GetTorchLuck(Player player) {
            //this torch is slightly lucky.
            return 0.1f;
        }

        public override void KillTile(int i, int j, ref bool fail,
        ref bool effectOnly, ref bool noItem) {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
            if(!(fail || effectOnly)) {
                ModContent.GetInstance<RainbowLampEntity>().Kill(i, j);
            }
        }

        public override bool RightClick(int x, int y) {
            (Mod as REBEL).showUIForTile<RainbowLampEntity>(x, y);
            return true;
        }
    } //class


    public class RainbowLampEntity: RebelModTileEntity<RainbowLamp> {
        //Stores parameters for individual RainbowLamp tiles.
        public override String displayName {
            get => "Rainbow Lamp";
        }

        public float _lightIntensity = 5f;

        //this doesn't need to exist (just expose the underlying value
        //directly) but I'm leaving it for now for testing
        //property attributes.
        //now that I think about it, did we need attributes at all?
        //could they not just be TileFloatAttribute(...) lightIntensity
        //and lightIntensity.value?
        //might not really simplify much, though... they still can be
        //both fields and properties so we'd still duplicate stuff.
        [TileFloatAttribute("Intensity", "How bright the light is",
            defaultValue: 5f)]
        public float lightIntensity {
            get => _lightIntensity;
            set { _lightIntensity = value; }
        }

        //XXX any way to get the default value from what animSpeed is
        //initialized to? ie write "int animSpeed = 2048" and don't
        //repeat the default here
        [TileIntAttribute("Speed", "How long the cycle takes, in frames",
            defaultValue: 2048)]
        public int animSpeed; //frame count
    } //class
} //namespace

namespace REBEL.Items.Placeable {
    public class RainbowLamp: TilePlaceItem<Blocks.RainbowLamp, RainbowLamp> {
		public override String Texture {
            get => "REBEL/Blocks/Decorative/RainbowLamp/Item";
        }
        public override String _getName() => "Rainbow Lamp";
        public override String _getDescription() => "Cycles through colors.";
        public override int _getResearchNeeded() => 3;
        public override int _getValue() => 100;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.RainbowLamp>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.RedTorch, 1)
				.AddIngredient(ItemID.GreenTorch, 1)
				.AddIngredient(ItemID.BlueTorch, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}
    }
}
