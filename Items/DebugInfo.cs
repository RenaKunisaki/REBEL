using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using REBEL.UI;

namespace REBEL.Items {
    public class DebugInfoItem: RebelItem {
        /** When used, displays some info in a popup window.
         */
        public override String Texture {
            get => "REBEL/Items/DebugInfoItem";
        }
        public override String _getName() => "Debug Info";
        public override String _getDescription() => "Shows some info";
        public override int _getResearchNeeded() => 1;
        public override int _getValue() => 0;
        public override bool _showsWires() => true;

        // UseStyle is called each frame that the item is being actively used.
		public override void UseStyle(Player player, Rectangle heldItemFrame) {
            (Mod as REBEL).showDebugUI(true);
        }
    }
}
