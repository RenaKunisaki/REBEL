using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;

namespace REBEL.Hooks {
    public class PlayerHooks: ModPlayer {
        public override void PostUpdate() {
            REBEL mod = Mod as REBEL;
            mod.checkTouchedBlocks(Main.LocalPlayer);
        }
    }
}
