using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.UI;
using Microsoft.Xna.Framework;
using REBEL.UI;

namespace REBEL.Hooks {
    public class WorldHooks: ModSystem {
        public override void OnModLoad() {
            Mod.Logger.Info("Mod loaded OK!");
        }

        public override void Load() {
            REBEL mod = Mod as REBEL;
            if(!Main.dedServ) { //no UI for dedicated server
                mod.ui = new RebelUI(mod);
            }
        }

        public override void UpdateUI(GameTime gameTime) {
            (Mod as REBEL).ui?.UpdateUI(gameTime);
        }

		public override void ModifyInterfaceLayers(
		List<GameInterfaceLayer> layers) {
            //voodoo. nobody really knows how this function works.
            int mouseTextIndex = layers.FindIndex(layer =>
				layer.Name.Equals("Vanilla: Mouse Text"));
            if(mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "REBEL: A Description", //XXX what goes here?
                    delegate {
                        (Mod as REBEL).ui?.Draw();
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
		}

        public override void PostUpdateNPCs() {
            //do this here so it applies even when player is dead.
            REBEL mod = Mod as REBEL;
            foreach(var npc in Main.npc) {
                try {
                    mod.checkTouchedBlocks(npc);
                }
                catch(System.IndexOutOfRangeException ex) {
                    //this can sometimes happen when testing
                    //if an NPC is a player.
                    //I assume it's if the NPC list changes?
                    //Mod.Logger.Debug("IndexOutOfRangeException in PostUpdateNPCs");
                }
            } //foreach

            //may as well do this here. (XXX probably a better place)
            (Mod as REBEL).ui?.update();

        } //PostUpdateNPCs
    } //class
} //namespace
