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
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using REBEL.Blocks;

//XXX Escape should close the box; need scrollbars
namespace REBEL.UI {
    public class TileConfigUI<TileEntityType>: RebelUIPanel
    where TileEntityType: RebelModTileEntityBase {
        /** Displays configurable items for a tile.
         */
        private TileEntityType tileEntity;
        private Point tileCoords;
        private int nRows;
        private REBEL Mod;
        private const float ROW_HEIGHT = 60f; //XXX calculate as needed.
        private const float TITLE_HEIGHT = 30f;
        private const float ROW_SPACING = 10f;

        public TileConfigUI(int i, int j, TileEntityType entity): base() {
            Mod = ModContent.GetInstance<REBEL>();
            tileEntity = entity;
            tileCoords = new Point(i, j);
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
                    case TileEnumAttribute E:
                        _makeEnumField(E, field);
                        break;
                    case TileFloatAttribute F:
                        _makeFloatField(F, field);
                        break;
                    case TileIntAttribute I:
                        _makeIntField(I, field);
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
        double minValue, double maxValue, double step, double bigStep,
        string format) {
            var entry = new UINumericEntry(value: val,
                minValue: minValue, maxValue: maxValue, step:step,
                bigStep:bigStep, format:format);
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
                (double)attr.minValue, (double)attr.maxValue,
                (double)attr.step,     (double)attr.bigStep,
                attr.format);
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
                (double)attr.minValue, (double)attr.maxValue,
                (double)attr.step,     (double)attr.bigStep,
                attr.format);
            entry.OnValueChanged += (val) => {
				tileEntity._setField<float>(field, (float)val);
			};
            subPanel.Append(entry);
            _addDescriptionRow(attr.description, subPanel);
            panel.Append(subPanel);
        }

        private UIPanel _newRowForEnum(float y) {
            UIPanel pRow = new UIPanel();
            pRow.SetPadding(4);
            pRow.Left  .Set(0f, 0f);
            pRow.Top   .Set(y,  0f);
            pRow.Width .Set(panel.Width.Pixels, 0f);
            pRow.Height.Set(36f, 0f);
            pRow.BackgroundColor = new Color(0x00, 0x00, 0x00, 0x00);
            return pRow;
        }

        private UIPanel _makeButtonForEnum(String text, Vector2 size,
        MouseEvent click, float x) {
            UIText label = new UIText(text);
            label.HAlign = 0.5f;
            label.VAlign = 0.0f;
            label.OnClick += click;

            UIPanel pItem = new UIPanel();
            pItem.SetPadding(4);
            pItem.Left  .Set(x,           0f);
            pItem.Top   .Set(0f,          0f);
            pItem.Width .Set(size.X + 24, 0f);
            pItem.Height.Set(size.Y + 16, 0f);
            pItem.BackgroundColor = new Color(0x00, 0x5D, 0xB3, 192);
            pItem.Append(label);
            pItem.OnClick += click;
            return pItem;
        }

        private void _makeEnumField(TileEnumAttribute attr, MemberInfo field) {
            UIPanel subPanel = _makeNewRow(attr.name);

            //get the sorted list of possible values
            var sort = attr.sort;
            if(sort == null) sort = new List<int>();
            if(!sort.Any()) { //sort however
                foreach(var entry in attr.values) {
                    sort.Add(entry.Key);
                }
            }

            //build the UI for each value
            float x=0, y=TITLE_HEIGHT;
            var font = FontAssets.MouseText.Value;
            UIPanel pRow = null;
            Vector2 meas = new Vector2();
            foreach(var key in sort) {
                if(key == -1) x = 99999999f; //line break

                if(x >= 570f) {
                    //move to next cell (XXX remove hardcoded values)
                    x  = 0;
                    y += 36f;
                    if(pRow != null) {
                        subPanel.Append(pRow);
                        pRow = null; //start a new row
                    }
                }
                if(key == -1) continue;
                if(!attr.values.ContainsKey(key)) { //sanity check
                    Mod.Logger.Error($"Sort ID {key} not in attribute {attr.name} values");
                    continue;
                }
                var value = attr.values[key];

                //make click handler
                var click = new MouseEvent(
                (UIMouseEvent evt, UIElement listeningElement) => {
                    Mod.Logger.Info($"Clicked {value} ({key}) for tile {tileCoords}");
                    tileEntity._setField<int>(field, key);
                    tileEntity.refresh(tileCoords.X, tileCoords.Y);
                });

                //make button
                meas = font.MeasureString(value);
                UIPanel pItem = _makeButtonForEnum(value, meas, click, x);

                //append button to row panel
                if(pRow == null) pRow = _newRowForEnum(y);
                pRow.Append(pItem);
                x += meas.X + 24f;
            }
            y += meas.Y + 16f; //for computing parent panel height

            if(pRow != null) subPanel.Append(pRow);
            //_addDescriptionRow(attr.description, subPanel);

            //XXX this is necessary because if our buttons aren't actually
            //contained within the bounds of the parent panels, they'll appear
            //but not be clickable. TODO compute these properly and allow them
            //to scroll.
            var h = panel.Height.Pixels - subPanel.Height.Pixels;
            subPanel.Height.Set(y, 0f);
            panel   .Height.Set(y, 0f);
            panel.Append(subPanel);
            sort = null;
        }

        public override void update() {
            //...
        } //update
    } //class
} //namespace
