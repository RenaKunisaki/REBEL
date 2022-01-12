using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class PlayerHooks: ModPlayer {
        protected void fuckingGravityShit1() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;

            if(player.controlUp) {
                //press Up to stop forcing inversion, in case
                //you accidentally fall into space.
                mod.forceUpsideDown = false;
            }

            if(mod.forceUpsideDown) {
                if(!mod.wasForceUpsideDown) {
                    Main.NewText("Going upside down", 0xFF, 0xFF, 0x00);
                    //Mod.Logger.Info("Going upside down");
                }
                mod.wasForceUpsideDown = true;
                player.gravDir = -1f; //flips screen
            }
            else if(mod.wasForceUpsideDown) {
                mod.wasForceUpsideDown = false;
                Main.NewText("Going upside up", 0xFF, 0xFF, 0x00);
                //Mod.Logger.Info("Going upside up");
                player.gravDir = 1f;
                if(player.gravity < 0) {
                    player.gravity *= -1f;
                }
            }

            //if(player.jump != 0) {
            //    Main.NewText($"Jump = {player.justJumped} {player.jump} boost {player.jumpBoost} speed {player.jumpSpeedBoost} vel {player.velocity}");
            //}
        }

        protected void fuckingGravityShit2() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            if(mod.forceUpsideDown) {
                 if(player.gravity > 0) {
                    player.gravity *= -1f; //flips gravity
                }
                //player.gravControl = true; //does fuck all
            }
        }

        //I can have gravity inverted or be able to jump.
        //not both. even though the game does it just fine.
        //if I fuck with it just right I can also make it
        //so you can only jump once.
        public override void PreUpdate() {
            //fuckingGravityShit1();
            //fuckingGravityShit2();
        }

        public override void PreUpdateMovement() {
            //fuckingGravityShit1();
            fuckingGravityShit2();
        }

        public override void PostUpdateBuffs() {
            //fuckingGravityShit1();
            //fuckingGravityShit2();
        }

        public override void PostUpdateRunSpeeds() {
            //fuckingGravityShit1();
            //fuckingGravityShit2();
        }

        public override void PostUpdate() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            fuckingGravityShit1();
            //fuckingGravityShit2();
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
