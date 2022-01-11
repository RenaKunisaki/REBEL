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
            var player = Main.LocalPlayer;
            if(player.controlUp) {
                //press Up to stop forcing inversion, in case
                //you accidentally fall into space.
                mod.forceUpsideDown = false;
            }

            if(mod.forceUpsideDown) {
                //var here = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //Main.NewText($"{here} grav={player.gravity} {player.gravDir}");    
            }
        }

        public override void PreUpdateMovement() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            
            if(mod.forceUpsideDown) {
                //var here = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //Main.NewText($"{here} grav={player.gravity} {player.gravDir}");

                //this has to be done here...
                player.gravDir = -1f; //flips screen
            }
        }

        public override void PostUpdateRunSpeeds() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            if(mod.forceUpsideDown) {
                //var here = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //Main.NewText($"{here} grav={player.gravity} {player.gravDir}");
                if(!mod.wasForceUpsideDown) {
                    Main.NewText("Going upside down", 0xFF, 0xFF, 0x00);
                    //Mod.Logger.Info("Going upside down");
                }
                mod.wasForceUpsideDown = true;

                //...but this has to be done here.
                if(player.gravity > 0) {
                    player.gravity *= -1f; //flips gravity
                }
            }

            if(player.jump != 0) {
                Main.NewText($"Jump = {player.justJumped} {player.jump} boost {player.jumpBoost} speed {player.jumpSpeedBoost} vel {player.velocity}");
            }
        }

        public override void PostUpdate() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            if(mod.forceUpsideDown) {
                //var here = System.Reflection.MethodBase.GetCurrentMethod().Name;
                //Main.NewText($"{here} grav={player.gravity} {player.gravDir}");
            }

            //undo gravity reversal if it's now turned off
            if(mod.wasForceUpsideDown && !mod.forceUpsideDown) {
                Main.NewText("Going upside up", 0xFF, 0xFF, 0x00);
                //Mod.Logger.Info("Going upside up");
                mod.wasForceUpsideDown = false;
                player.gravDir = 1f;
                if(player.gravity < 0) {
                    player.gravity *= -1f;
                }
            }

            mod.checkTouchedBlocks(player);
        }

        public override void OnRespawn(Player player) {
            if(player == Main.LocalPlayer) {
                //reset gravity on respawn
                REBEL mod = Mod as REBEL;
                mod.forceUpsideDown = false;
            }
        }
    }
}
