using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using REBEL.Blocks;

namespace REBEL.UI {
    public class TileConfigUI<TileEntityType>: RebelUIPanel
    where TileEntityType: RebelModTileEntityBase {
        /** Displays configurable items for a tile.
         */
        private TileEntityType tileEntity;
        private int nRows;
        private REBEL Mod;
        private const float ROW_HEIGHT = 30f;

        public TileConfigUI(int i, int j, TileEntityType entity): base() {
            Mod = ModContent.GetInstance<REBEL>();
            tileEntity = entity;
            nRows = 1; //don't overlap close button
        }

        public override void Setup() {
            UIText title = new UIText($"Configure {tileEntity.displayName}");
            title.Top.Set(4f, 0f); //lol what's padding?
            title.Left.Set(4f, 0f);
            panel.Append(title);

            foreach(MemberInfo field in tileEntity.getAttrs()) {
                var attr = field.GetCustomAttribute<TileAttributeBase>(false);
                switch(attr) {
                    case null: continue;
                    case TileIntAttribute I:
                        _makeIntField(I, field);
                        break;
                    case TileFloatAttribute F:
                        _makeFloatField(F, field);
                        break;
                    default:
                        Mod.Logger.Error($"BUG: No entry in Setup for type {attr}");
                        break;
                }
            }

            /* //make page buttons
            panel.Append(makeButton("Images/UI/ButtonPlay",
                "Next Page", //XXX localize
                new Rectangle((int)panel.Width.Pixels - 80, 10, 22, 22),
                new MouseEvent(btnNextClicked)));

            //make text
            uiText = new UIText("HOWDY DOODY");
            panel.Append(uiText); */
        } //Setup

        private UIPanel _makeNewRow(String label) {
            UIPanel subPanel = new UIPanel();
            subPanel.SetPadding(4);
            subPanel.Left  .Set(0f,   0f);
            subPanel.Top   .Set((float)nRows * ROW_HEIGHT, 0f);
            subPanel.Width .Set(panel.Width.Pixels,  0f);
            subPanel.Height.Set(ROW_HEIGHT, 0f);
            subPanel.BackgroundColor = new Color(0x00, 0x5D, 0xB3, 192);

            UIText tLabel = new UIText(label);
            tLabel.Width.Set(panel.Width.Pixels / 2f, 0f);
            tLabel.HAlign = 0f; //does fuck all
            subPanel.Append(tLabel);

            nRows++;
            return subPanel;
        }

        private void _makeIntField(TileIntAttribute attr, MemberInfo field) {
            UIPanel subPanel = _makeNewRow(attr.name);

            //XXX entry field
            int v = tileEntity._getField<int>(field);
            UIText text = new UIText($"{v}");
            text.Width.Set(panel.Width.Pixels / 2f, 0f);
            text.Left.Set(panel.Width.Pixels / 2f, 0f);
            text.HAlign = 0f;
            subPanel.Append(text);
            panel.Append(subPanel);

        }
        private void _makeFloatField(TileFloatAttribute attr, MemberInfo field) {
            UIPanel subPanel = _makeNewRow(attr.name);

            //XXX entry field
            float v = tileEntity._getField<float>(field);
            UIText text = new UIText($"{v}");
            text.Width.Set(panel.Width.Pixels / 2f, 0f);
            text.Left.Set(panel.Width.Pixels / 2f, 0f);
            text.HAlign = 0f;
            subPanel.Append(text);
            panel.Append(subPanel);
        }

        public override void update() {
            //...
        } //update
    } //class
} //namespace
