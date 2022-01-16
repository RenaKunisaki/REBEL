using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class WorldHooks: ModSystem {
        public override void OnModLoad() {
            Mod.Logger.Info("Mod loaded OK!");
        }

        public override void PostUpdateNPCs() {
            //do this here so it applies even when player is dead.
            REBEL mod = Mod as REBEL;
            foreach(var npc in Main.npc) {
                try {
                    mod.checkTouchedBlocks(npc);
                }
                catch(System.IndexOutOfRangeException) {
                    //this can sometimes happen when testing
                    //if an NPC is a player.
                    //I assume it's if the NPC list changes?
                }
            }
        }
    }
}
