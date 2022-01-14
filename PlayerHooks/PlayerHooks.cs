using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class PlayerHooks: ModPlayer {
        protected void updateGravityControl() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;

            if(player.controlUp) {
                //press Up to stop forcing inversion, in case
                //you accidentally fall into space.
                mod.forceUpsideDown = false;
            }

            if(mod.forceUpsideDown) {
                if(!mod.wasForceUpsideDown) {
                    //Main.NewText("Going upside down", 0xFF, 0xFF, 0x00);
                    //Mod.Logger.Info("Going upside down");
                }
                mod.wasForceUpsideDown = true;
                player.gravDir  = -1f; //flips screen
                player.gravity *= -1f; //flips gravity
                //manually setting these causes issues with jumping
                //so just give the buff instead.
                player.AddBuff(BuffID.Gravitation, 1);
            }
            else if(mod.wasForceUpsideDown) {
                mod.wasForceUpsideDown = false;
                //Main.NewText("Going upside up", 0xFF, 0xFF, 0x00);
                //Mod.Logger.Info("Going upside up");
                player.ClearBuff(BuffID.Gravitation);
            }
        }

        /* public override void PreUpdate() {
        }

        public override void PreUpdateMovement() {
        }

        public override void PostUpdateBuffs() {
        }

        public override void PostUpdateRunSpeeds() {
        } */

        public override void PostUpdate() {
            REBEL mod = Mod as REBEL;
            var player = Main.LocalPlayer;
            updateGravityControl();
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
