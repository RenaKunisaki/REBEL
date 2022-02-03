using System;
using Terraria;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using ReLogic.Content;
using ReLogic.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace REBEL.UI {
    public class UINumericEntry: UITextEntry {
        //you might think you could make this a template that can
        //accept both int and float, but, lolnope.
        //XXX add +/- buttons
        public event Action<double> OnValueChanged;
        public String format { get; private set; }
        public double step, bigStep;
        public double minValue, maxValue;
        private double _value;
        public double value {
            get => _value;
            set {
                _value = Math.Min(Math.Max(minValue, value), value);
                OnValueChanged?.Invoke(_value);
                _updateText();
            }
        }

        public UINumericEntry(double value,
        double minValue=Double.NegativeInfinity,
        double maxValue=Double.PositiveInfinity,
        double step=1, double bigStep=10,
        string format="G", float textScale=1, bool large=false):
        base(value.ToString(format), textScale, large) {
            this.format   = format;   //format string for display
            this.step     = step;     //how much to adjust with Up/Down keys
            this.bigStep  = bigStep;  //how much to adjust with Page keys
            this.minValue = minValue; //minimum acceptable value
            this.maxValue = maxValue; //maximum acceptable value
            this._value   = value;    //current value

            this.isValid = str => {
                if(Double.TryParse(str, out double val)) {
                    return val >= this.minValue && val <= this.maxValue;
                }
                else return false;
            };
            this.OnTextChanged += (text) => {
                if(Double.TryParse(text, out double val)) {
                    bool changed = val != this._value;
                    this._value = val;
                    if(changed) OnValueChanged?.Invoke(val);
                }
            };
            this.OnUpPressed   += () => { this.value += this.step; };
            this.OnDownPressed += () => { this.value -= this.step; };
        }

        protected override void handleKeys() {
            if(!focused) return;
            base.handleKeys();
            if(JustPressed(Keys.PageUp))   this.value += this.bigStep;
            if(JustPressed(Keys.PageDown)) this.value -= this.bigStep;
        }

        private void _updateText() {
            string text = this.value.ToString(format);
            if(text != base.Text) base.SetText(text);
            //don't call OnTextChanged/OnValueChanged since
            //this is usually called when we already did that.
        }
    }
}
