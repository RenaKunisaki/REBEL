using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework;
using REBEL.UI;
using REBEL.Blocks;

namespace REBEL.UI {
    public enum UIPanelId {
        None,  //don't show any UI
        Debug, //debug info
        Tile,  //tile config
        Count  //number of UI panels
    };

    public class RebelUI {
        internal RebelUIPanel _curPanel;
        public RebelUIPanel curPanel {
            get => _curPanel;
        }

        internal UIPanelId _curPanelId;
        public UIPanelId curPanelId {
            get => _curPanelId;
        }

        internal REBEL Mod;
		internal UserInterface ui;
        internal DebugUI debugUI;
        internal Dictionary<UIPanelId, RebelUIPanel> uiPanels;

        public RebelUI(REBEL mod) {
            Mod = mod;
            _curPanelId = UIPanelId.None;
            _curPanel = null;

            //set up the UI panels
            ui = new UserInterface();
            debugUI = new DebugUI();
            debugUI.Activate();

            //make the panel map
            uiPanels = new Dictionary<UIPanelId, RebelUIPanel>() {
                {UIPanelId.None,  null},
                {UIPanelId.Debug, debugUI},
                //tile config is generated on the fly
            };
        } //constructor

        ~RebelUI() {
            debugUI = null;
            uiPanels = null;
            _curPanel = null;
        } //finalizer (destructor)

        public void _showPanel(UIPanelId id) {
            //use Mod.showUI() instead of calling this.
            if(id == _curPanelId) return;
            if(!uiPanels.ContainsKey(id)) {
                Mod.Logger.Error($"Unknown UIPanelId {id}");
                return;
            }
            ui?.SetState(uiPanels[id]);
            _curPanelId = id;
            _curPanel = uiPanels[id];
        }

        public void _showUIForTile<TileEntityType>(int i, int j)
        where TileEntityType: RebelModTileEntityBase {
            int index = ModContent.GetInstance<TileEntityType>().Find(i, j);
            if(index < 0) {
                Main.NewText("This tile has no settings.");
                return;
            }
            TileEntityType entity = (TileEntityType)TileEntity.ByID[index];
            _curPanel = new TileConfigUI<TileEntityType>(i, j, entity);
            _curPanel.Activate();
            _curPanelId = UIPanelId.Tile;
            ui?.SetState(_curPanel);
		}

        public void UpdateUI(GameTime gameTime) {
            //periodic callback from ModSystem
            ui?.Update(gameTime);
        }

        public void Draw() {
            //periodic callback from ModSystem
            ui?.Draw(Main.spriteBatch, new GameTime());
        }

        public void update() {
            //callback each game tick to actually update the contents
            _curPanel?.update();
        }
    } //class
} //namespace
