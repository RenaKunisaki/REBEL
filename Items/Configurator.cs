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
using REBEL.Blocks;

namespace REBEL.Items {
    public class Configurator: RebelItem {
        /** Use it on one of our blocks to configure them.
         */
        public override String Texture {
            get => "REBEL/Items/Configurator";
        }
        public override String _getName() => "REBEL Configurator";
        public override String _getDescription() => "Click a block to configure it";
        public override int _getResearchNeeded() => 1;
        public override int _getValue() => 0;
        public override int _getMaxStack() => 1;
        public override bool _showsWires() => true;

        // UseStyle is called each frame that the item is being actively used.
		public override void UseStyle(Player player, Rectangle heldItemFrame) {
            Vector2 mouse = Main.MouseWorld;
            int tid = -1;
            int tFrame = -1;
            int mx  = (int)(mouse.X/16);
            int my  = (int)(mouse.Y/16);
            if(mouse.X < (Main.leftWorld/16) || mouse.X >= (Main.rightWorld /16)
            || mouse.Y < (Main.topWorld /16) || mouse.Y >= (Main.bottomWorld/16)) {
                SoundEngine.PlaySound(2, -1, -1, 16); //fart
                return;
            }
            var tile = Framing.GetTileSafely(mx, my);
            if(tile.TileType == ModContent.GetInstance<RainbowLamp>().Type) {
                Main.NewText("poo");
            }
        }
    }
}
