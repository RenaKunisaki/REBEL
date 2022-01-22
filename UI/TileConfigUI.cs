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

//XXX Escape should close the box
namespace REBEL.UI {
    public class TileConfigUI<TileEntityType>: RebelUIPanel
    where TileEntityType: RebelModTileEntityBase {
        /** Displays configurable items for a tile.
         */
        private TileEntityType tileEntity;
        private int nRows;
        private REBEL Mod;
        private const float ROW_HEIGHT = 60f; //XXX calculate as needed.
        private const float TITLE_HEIGHT = 30f;
        private const float ROW_SPACING = 10f;

        public TileConfigUI(int i, int j, TileEntityType entity): base() {
            Mod = ModContent.GetInstance<REBEL>();
            tileEntity = entity;
            nRows = 0;
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

            //should have big "save/cancel/set as defaults/reset to defaults"
            //buttons at the bottom.
        } //Setup

        private UIPanel _makeNewRow(String label) {
            UIPanel subPanel = new UIPanel();
            float height = (ROW_HEIGHT+ROW_SPACING);
            subPanel.SetPadding(4);
            subPanel.Left  .Set(0f,   0f);
            subPanel.Top   .Set(((float)nRows * height)+TITLE_HEIGHT, 0f);
            subPanel.Width .Set(panel.Width.Pixels,  0f);
            subPanel.Height.Set(ROW_HEIGHT, 0f);
            subPanel.BackgroundColor = new Color(0x00, 0x5D, 0xB3, 192);

            UIText tLabel = new UIText(label);
            tLabel.HAlign = 0f;
            //this cancels out HAlign and is entirely unnecessary
            //tLabel.Width.Set(panel.Width.Pixels / 2f, 0f);
            subPanel.Append(tLabel);

            nRows++;
            return subPanel;
        }

        private UIText _addDescriptionRow(String description, UIPanel subPanel) {
            UIText text = new UIText(description);
            text.HAlign = 0f;
            text.Top.Set(30f, 0f);
            subPanel.Append(text);
            return text;
        }

        private UINumericEntry _makeNumericEntry(double val,
        double minVal, double maxVal) {
            var entry = new UINumericEntry(value: val,
                minVal: minVal, maxVal: maxVal);
            entry.Width.Set((panel.Width.Pixels / 2f) - 30f, 0f);
            entry.Left.Set(panel.Width.Pixels / 2f, 0f);
            //entry.HAlign = 0f;
            return entry;
        }

        //XXX there must be some way to template this.
        private void _makeIntField(TileIntAttribute attr, MemberInfo field) {
            UIPanel subPanel = _makeNewRow(attr.name);
            UINumericEntry entry = _makeNumericEntry(
                (double)tileEntity._getField<int>(field),
                (double)attr.minValue,
                (double)attr.maxValue);
            entry.OnValueChanged += (val) => {
				tileEntity._setField<int>(field, (int)val);
			};
            subPanel.Append(entry);
            _addDescriptionRow(attr.description, subPanel);
            panel.Append(subPanel);
        }

        private void _makeFloatField(TileFloatAttribute attr, MemberInfo field) {
            UIPanel subPanel = _makeNewRow(attr.name);
            UINumericEntry entry = _makeNumericEntry(
                (double)tileEntity._getField<float>(field),
                (double)attr.minValue,
                (double)attr.maxValue);
            entry.OnValueChanged += (val) => {
				tileEntity._setField<float>(field, (float)val);
			};
            subPanel.Append(entry);
            _addDescriptionRow(attr.description, subPanel);
            panel.Append(subPanel);
        }

        public override void update() {
            //...
        } //update
    } //class
} //namespace
