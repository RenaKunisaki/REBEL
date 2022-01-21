using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
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
            p.X ^= 1; //turn the lamp on/off
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
    } //class


    public class RainbowLampEntity: ModTileEntity {
        //Stores parameters for individual RainbowLamp tiles.
        internal float lightIntensity = 5f;
        internal int animSpeed = 2048; //frame count

        public override void Update() {
            // Sending 86 aka, TileEntitySharing, triggers NetSend.
            //Think of it like manually calling sync.
            NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null,
                ID, Position.X, Position.Y);
		}

		public override void NetReceive(BinaryReader reader) {
			lightIntensity = reader.ReadSingle(); //ReadFloat would be too obvious
			animSpeed      = reader.ReadInt32();
		}
		public override void NetSend(BinaryWriter writer) {
			writer.Write(lightIntensity);
			writer.Write(animSpeed);
		}

		public override void SaveData(TagCompound tag) {
			tag["lightIntensity"] = lightIntensity;
			tag["animSpeed"] = animSpeed;
		}
		public override void LoadData(TagCompound tag) {
            lightIntensity = tag.Get<float>("lightIntensity");
            animSpeed = tag.Get<int>("animSpeed");
		}

		public override bool IsTileValidForEntity(int i, int j) {
			Tile tile = Main.tile[i, j];
			return tile.IsActive
                && tile.type == ModContent.TileType<RainbowLamp>();
                //&& tile.frameX == 0 && tile.frameY == 0;
		}

		public override int Hook_AfterPlacement(int i, int j, int type,
        int style, int direction, int alternate) {
			if(Main.netMode == NetmodeID.MultiplayerClient) {
				NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1,
                    null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i, j);
		}
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
