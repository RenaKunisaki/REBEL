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
    public class PurityBlock:
    ItemDropBlock<Items.Placeable.PurityBlock> {
        /** A block that purifies blocks within a certain radius.
         */
        Dictionary<ushort, ushort> PureTiles;
        Dictionary<int, int> PureNPCs;
        public override String Texture {
            get => "REBEL/Blocks/Misc/PurityBlock/Block";
        }

        public override void SetStaticDefaults() {
            Main.tileSolid[Type] = false;
            Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
            Main.tileFrameImportant[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleSwitch);
            TileObjectData.addTile(Type);

            //corrupt tile => pure version
            //XXX lots more that aren't their own tile type; eg Corrupt Torch
            PureTiles = new Dictionary<ushort, ushort>() {
                {TileID.CorruptGrass,        TileID.Grass},
                {TileID.CorruptPlants,       TileID.Plants},
                {TileID.CorruptThorns,       TileID.JungleThorns},
                {TileID.CorruptIce,          TileID.IceBlock},
                {TileID.CorruptHardenedSand, TileID.HardenedSand},
                {TileID.CorruptSandstone,    TileID.Sandstone},
                {TileID.CrimsonVines,        TileID.JungleVines},
                {TileID.CrimsonHardenedSand, TileID.HardenedSand},
                {TileID.CrimsonSandstone,    TileID.Sandstone},
                {TileID.HallowedGrass,       TileID.Grass},
                {TileID.HallowedPlants,      TileID.Plants},
                {TileID.HallowedPlants2,     TileID.Plants2},
                {TileID.HallowedVines,       TileID.JungleVines},
                {TileID.HallowedIce,         TileID.IceBlock},
                {TileID.HallowHardenedSand,  TileID.HardenedSand},
                {TileID.HallowSandstone,     TileID.Sandstone},
                //XXX are these needed?
                {TileID.Ebonstone,           TileID.Stone},
                {TileID.EbonstoneBrick,      TileID.SandstoneBrick},
                {TileID.Ebonsand,            TileID.Sand},
                {TileID.Ebonwood,            TileID.WoodBlock},
                {TileID.Pearlstone,          TileID.Stone},
                {TileID.PearlstoneBrick,     TileID.SandstoneBrick},
                {TileID.Pearlsand,           TileID.Sand},
                {TileID.Pearlwood,           TileID.WoodBlock},
                {TileID.Crimstone,           TileID.Stone},
                {TileID.Crimsand,            TileID.Sand},
            };
            PureNPCs = new Dictionary<int, int>() {
                {NPCID.CorruptBunny,          NPCID.Bunny},
                {NPCID.CrimsonBunny,          NPCID.Bunny},
                {NPCID.CorruptGoldfish,       NPCID.Goldfish},
                {NPCID.CrimsonGoldfish,       NPCID.Goldfish},
                {NPCID.CorruptSlime,          NPCID.GreenSlime},
                {NPCID.CorruptPenguin,        NPCID.Penguin},
                {NPCID.CrimsonPenguin,        NPCID.Penguin},
                {NPCID.PigronCorruption,      NPCID.Bunny},
                {NPCID.PigronHallow,          NPCID.Bunny},
                {NPCID.BigMimicCorruption,    NPCID.Mimic},
                {NPCID.BigMimicCrimson,       NPCID.Mimic},
                {NPCID.BigMimicHallow,        NPCID.Mimic},
                {NPCID.DesertGhoulCorruption, NPCID.DesertGhoul},
                {NPCID.DesertGhoulCrimson,    NPCID.DesertGhoul},
                {NPCID.DesertGhoulHallow,     NPCID.DesertGhoul},
                {NPCID.SandsharkCorrupt,      NPCID.SandShark}, //lolconsistency
                {NPCID.SandsharkCrimson,      NPCID.SandShark},
                {NPCID.SandsharkHallow,       NPCID.SandShark},
            };
        }

        public override void NearbyEffects(int i, int j, bool closer) {
            int x1 = Math.Max(i-8, (int)(Main.leftWorld / 16));
            int x2 = Math.Min(i+8, (int)(Main.rightWorld / 16)-1);
            int y1 = Math.Max(j-8, (int)(Main.topWorld / 16));
            int y2 = Math.Min(j+8, (int)(Main.bottomWorld / 16)-1);
            for(int y=y1; y<=y2; y++) {
                for(int x=x1; x<=x2; x++) {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if(PureTiles.ContainsKey(tile.TileType)) {
                        tile.TileType = PureTiles[tile.TileType];
                    }
                }
            }
            Vector2 vBlock = new Vector2(i*16, j*16);
            for(int n=0; n<Main.npc.Length; n++) {
                var npc = Main.npc[n];
                if(npc.active && PureNPCs.ContainsKey(npc.type)
                && vBlock.Distance(npc.position) < 8*16) {
                    //XXX this doesn't work; it changes the appearance,
                    //but they're still the original type (have the same
                    //name, will attack/be attacked by town NPCs, etc)
                    //npc.type = PureNPCs[npc.type];
                    //npc.aiStyle = -1;
                    //instead, replace it.
                    int n2 = (Mod as REBEL).replaceNPC(n, PureNPCs[npc.type]);
                    //Mod.Logger.Info($"Replace NPC {n} {npc.FullName} => {n2} {Main.npc[n2].FullName}");
                }
            }
        }
    }
}

namespace REBEL.Items.Placeable {
    public class PurityBlock: TilePlaceItem<Blocks.PurityBlock, PurityBlock> {
		public override String Texture {
            //reuse this
            get => "REBEL/Blocks/Misc/PurityBlock/Block";
        }
        public override String _getName() => "Purity Shield";
        public override String _getDescription() => "Prevents corruption/crimson/hallow from spreading nearby.";
        public override int _getResearchNeeded() => 3;
        public override int _getValue() => 2000;

        public override void AddRecipes() {
			var resultItem = ModContent.GetInstance<Items.Placeable.PurityBlock>();
			resultItem.CreateRecipe(1)
				.AddIngredient(ItemID.LifeCrystal, 1)
				.AddIngredient(ItemID.Sunflower, 5)
                .AddTile(TileID.CookingPots)
				.Register();
		}
    }
}
