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
        internal DebugUI debugUI;
		private UserInterface _debugUI;

        public override void OnModLoad() {
            Mod.Logger.Info("Mod loaded OK!");
        }

        public override void Load() {
            //UI voodoo
			if(!Main.dedServ) {
				debugUI = new DebugUI();
				debugUI.Activate();
				_debugUI = new UserInterface();
			}
        }

        public void showDebugUI(bool show) {
            //use Mod.showDebugUI() instead of calling this.
            _debugUI.SetState(show ? debugUI : null);
        }

        public override void UpdateUI(GameTime gameTime) {
            _debugUI?.Update(gameTime);
        }

		public override void ModifyInterfaceLayers(
		List<GameInterfaceLayer> layers) {
            int mouseTextIndex = layers.FindIndex(layer =>
				layer.Name.Equals("Vanilla: Mouse Text"));
            if(mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "REBEL: A Description",
                    delegate {
						_debugUI.Draw(Main.spriteBatch, new GameTime());
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
                    Mod.Logger.Debug("IndexOutOfRangeException in PostUpdateNPCs");
                }
            } //foreach

            //may as well do this here.
            debugUI?.update();

        } //PostUpdateNPCs
    } //class
} //namespace
