using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;

using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace REBEL.UI {
    public class DebugUI: RebelUIPanel {
        private UIText uiText;

        Dictionary<int, String> BiomeNames = new Dictionary<int, String>() {
            {PrimaryBiomeID.Forest,            "Forest"},
            {PrimaryBiomeID.NormalUnderground, "Underground"},
            {PrimaryBiomeID.Snow,              "Snow"},
            {PrimaryBiomeID.Desert,            "Desert"},
            {PrimaryBiomeID.Jungle,            "Jungle"},
            {PrimaryBiomeID.Ocean,             "Ocean"},
            {PrimaryBiomeID.Hallow,            "Hallow"},
            {PrimaryBiomeID.Mushroom,          "Mushroom"},
            {PrimaryBiomeID.Dungeon,           "Dungeon"},
            {PrimaryBiomeID.Corruption,        "Corruption"},
            {PrimaryBiomeID.Crimson,           "Crimson"},
        };

        enum Mode {
            PlayerInfo,
            EnvInfo,
            WorldInfo,
            CursorInfo,
            Count
        };
        private int mode;

        public override void Setup() {
            mode = (int)Mode.PlayerInfo;

            //make page buttons
            panel.Append(makeButton("Images/UI/ButtonPlay",
                "Next Page", //XXX localize
                new Rectangle((int)panel.Width.Pixels - 80, 10, 22, 22),
                new MouseEvent(btnNextClicked)));

            //make text
            uiText = new UIText("HOWDY DOODY");
            panel.Append(uiText);
        } //Setup

        private void btnNextClicked(UIMouseEvent evt,
        UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuTick);
			mode++;
            if(mode >= (int)Mode.Count) mode = 0;
		}

        private String _makeText_PlayerInfo() {
            String c = "c/CCCCCC:";
            var player = Main.LocalPlayer;

            int tx = (int)(player.position.X / 16);
            int ty = (int)(player.position.Y / 16);
            return $"[Player Info: [{c}{player.name}]]\n"+
                $"Pos: [{c}{player.position}] (T: [{c}{tx}], [{c}{ty}])\n"+
                $"Vel: [{c}{player.velocity}]\n"+
                $"Breath: [{c}{player.breath}]/[{c}{player.breathMax}]\n"+

                $"Def: [{c}{player.statDefense}] "+
                $"Armor penetration: [{c}{player.armorPenetration}]\n"+

                $"Dash: [{c}{player.dash}] "+
                $"Time: [{c}{player.dashTime}] "+
                $"Delay: [{c}{player.dashDelay}] "+
                $"Immune: [{c}{player.immuneTime}] "+
                $"EnvBuffImmune: [{c}{player.environmentBuffImmunityTimer}] "+
                $"Fish: [{c}{player.fishingSkill}]\n"+

                $"Jump: [{c}{player.jump}] "+
                $"Boost: [{c}{player.jumpSpeedBoost}] "+
                $"ItemTime: [{c}{player.itemTime}]/[{c}{player.itemTimeMax}] "+
                $"Luck: [{c}{player.luck}] "+
                $"Minions: [{c}{player.numMinions}]/[{c}{player.maxMinions}]\n"+

                $"Speeds: Move: [{c}{player.moveSpeed}] "+
                $"Melee: [{c}{player.meleeSpeed}] "+
                $"Fall: [{c}{player.maxFallSpeed}] "+
                $"Pick: [{c}{player.pickSpeed}] "+
                $"Tile: [{c}{player.tileSpeed}] "+
                $"Wall: [{c}{player.wallSpeed}]";
        }

        private String _makeText_EnvInfo() {
            String c = "c/CCCCCC:";
            var player = Main.LocalPlayer;
            int biome = player.GetPrimaryBiome();
            String biomeName = $"#{biome}";
            //if(BiomeNames.ContainsKey(biome)) biomeName = BiomeNames[biome];
            biomeName = ShopHelper.BiomeNameByKey(biome);
            String bloodMoon = Main.bloodMoon ? "[c/FF0000:Blood], " : "";
            String pumpkinMoon = Main.pumpkinMoon ? "[c/FF0000:Pumpkin], " : "";
            String snowMoon = Main.snowMoon ? "[c/FF0000:Snow], " : "";
            String eclipse = Main.eclipse ? "[c/FF0000:Yes]" : "[c/00FF00:No]";
            String rain = Main.raining ? $"[{c}{(int)(Main.rainTime/60)}]" : "[c/00FF00:-]";
            String slimeRain = Main.slimeRain ? $"[c/FF0000:Slime {Main.slimeRainTime}], " : "";

            //what the fuck
            double time = Main.time;
			if (!Main.dayTime) time += 54000.0;
            time = (time + (4.5 * 3600.0)) % 86400.0;
            int hour   = (int)( time / 3600.0);
            int minute = (int)((time / 60.0) % 60.0);
            int second = (int)( time % 60.0);
            String day = (Main.dayTime ? "[c/00FFFF:Day]" : "[c/888888:Night]");

            return "[Environment Info]\n"+
                $"Biome: [{c}{biomeName}]; "+
                $"Moon: {bloodMoon}{pumpkinMoon}{snowMoon}"+
                $"phase [{c}{Main.moonPhase}] "+
                $"type [{c}{Main.moonType}];\n"+

                $"Eclipse: {eclipse}; "+
                $"Rain: {slimeRain}{rain}; "+
                $"Wind: {Main.windSpeedCurrent}\n"+

                $"Graveyard: [{c}{Main.GraveyardVisualIntensity}]; "+
                $"Invasion: [{c}{Main.invasionType}]\n"+

                $"TimeRate: [{c}{Main.dayRate}]; "+
                $"Time: {day}, [{c}{hour:00}:{minute:00}:{second:00}] ";
        }

        private String _makeText_WorldInfo() {
            String c = "c/CCCCCC:";
            List<String> styles = new List<String>();
            if(Main.hardMode) styles.Add("[c/FF0000:HardMode]");
            if(Main.expertMode) styles.Add("[c/FF0000:ExpertMode]");
            if(Main.masterMode) styles.Add("[c/FF0000:MasterMode]");
            if(Main.dontStarveWorld) styles.Add("[c/0088FF:DontStarve]");
            if(Main.drunkWorld) styles.Add("[c/FFFF88:Drunk]");
            if(Main.getGoodWorld) styles.Add("[c/FF88FF:GetGood]");
            if(Main.notTheBeesWorld) styles.Add("[c/FFFF00:NotTheBees]");
            if(Main.tenthAnniversaryWorld) styles.Add("[c/88FF88:10thAnniversary]");
            if(Main.halloween) styles.Add("[c/FF8000:Halloween]");
            if(Main.xMas) styles.Add("[c/00FF00:Xmas]");
            if(!styles.Any()) styles.Add("[c/009DF3:Normal]");
            String style = String.Join(", ", styles);

            Rectangle rWorld = new Rectangle((int)Main.leftWorld,
                (int)Main.topWorld, (int)Main.rightWorld,
                (int)Main.bottomWorld);
            return $"[World Info: [{c}{Main.worldName}]]\n"+
                $"Top: [{c}{rWorld.Top}] (T:[{c}{rWorld.Top / 16}]) "+
                $"Bottom: [{c}{rWorld.Bottom}] (T:[{c}{rWorld.Bottom / 16}])\n"+
                $"Left: [{c}{rWorld.Left}] (T:[{c}{rWorld.Left / 16}]) "+
                $"Right: [{c}{rWorld.Right}] (T:[{c}{rWorld.Right / 16}])\n"+
                $"Style: {style}";
        }

        private String _makeText_CursorInfo() {
            String c = "c/CCCCCC:";
            var player = Main.LocalPlayer;

            Vector2 mouse = Main.MouseWorld;
            int tid = -1;
            int tFrame = -1;
            String wires = "";
            int mx  = (int)(mouse.X/16);
            int my  = (int)(mouse.Y/16);
            var tile = Framing.GetTileSafely(mx, my);
            tid = (int)tile.type;
            tFrame = (int)tile.FrameNumber;
            if(tile.RedWire) wires += "[c/FF0000:R]";
            if(tile.GreenWire) wires += "[c/00FF00:G]";
            if(tile.YellowWire) wires += "[c/FFFF00:Y]";
            if(tile.BlueWire) wires += "[c/00FFFF:B]";
            if(tile.HasActuator) {
                wires += "[c/FFFFFF:" + (tile.IsActuated ? "a" : "A") + "]";
            }
            if(wires == "") wires = $"[{c}None]";

            return "[Cursor Info]\n"+
                $"Screen: [{c}{Main.mouseX}], [{c}{Main.mouseY}]\n"+

                $"World: [{c}{mouse.X}], [{c}{mouse.Y}]\n"+

                $"Tile: [{c}{mx}], [{c}{my}]: #[{c}{tid}], "+
                $"Frame [{c}{tFrame}], wires: {wires}\n"+

                $"Item: #[{c}{player.cursorItemIconID}]\n";
        }

        public override void update() {
            String text = "";
            switch((Mode)mode) {
                case Mode.PlayerInfo: text = _makeText_PlayerInfo(); break;
                case Mode.EnvInfo:    text = _makeText_EnvInfo();    break;
                case Mode.WorldInfo:  text = _makeText_WorldInfo();  break;
                case Mode.CursorInfo: text = _makeText_CursorInfo(); break;
                default: break;
            }
            uiText.SetText(text);
        }
    } //class
} //namespace
