using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class PlayerHooks: ModPlayer {
        
        public override void PreUpdate() {
            REBEL mod = Mod as REBEL;
            if(mod.forceUpsideDown) {
                //must be done here to have any effect
                if(Main.LocalPlayer.gravity > 0) {
                    Main.LocalPlayer.gravity *= -1f; //flips gravity
                }
                mod.wasForceUpsideDown = true;
            }
            else if(mod.wasForceUpsideDown) { //restore gravity
                if(Main.LocalPlayer.gravity < 0) {
                    Main.LocalPlayer.gravity *= -1f;
                }
            }
        }

        public override void PostUpdate() {
            REBEL mod = Mod as REBEL;
            if(mod.forceUpsideDown) {
                //must be done here to have any effect
                Main.LocalPlayer.gravDir = -1f; //flips screen
            }
            mod.checkTouchedBlocks(Main.LocalPlayer);

            if(mod.wasForceUpsideDown && !mod.forceUpsideDown) {
                mod.wasForceUpsideDown = false;
            }
        }
    }
}
