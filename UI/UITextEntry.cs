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
    //largely copied from Modder's Toolkit.
	//XXX left/right should move cursor.
    public class UITextEntry: UITextPanel<string> {
        internal bool focused = false;
		private int _cursor;
		private int _frameCount;
		private int _maxLength = 60;
		public string hintText="";
        public event Action OnFocus;
		public event Action OnUnfocus;
		public event Action<string> OnTextChanged;
		public event Action OnTabPressed;
		public event Action OnEnterPressed;
		public event Action OnUpPressed;
		public event Action OnDownPressed;
		public Func<string, bool> isValid; //why is result last!?
		public Color bgColor, bgColorFocus;
		public Color bgColorInvalid, bgColorInvalidFocus;

        public UITextEntry(string text="", float textScale=1, bool large=false):
        base(text, textScale, large) {
            SetPadding(4);
			bgColor             = new Color(0x00, 0x00, 0x00, 0x80);
			bgColorFocus        = new Color(0x00, 0x00, 0x00, 0xC0);
			bgColorInvalid      = new Color(0x40, 0x00, 0x00, 0x80);
			bgColorInvalidFocus = new Color(0x40, 0x00, 0x00, 0xC0);
			isValid = (str) => true;
        }

        public override void Click(UIMouseEvent evt) {
			Focus();
			base.Click(evt);
		}

        public void Unfocus() {
			if(focused) {
				focused = false;
				Main.blockInput = false;
				OnUnfocus?.Invoke();
			}
		}

        public void Focus() {
			if(!focused) {
				Main.clrInput();
				focused = true;
				Main.blockInput = true;
				OnFocus?.Invoke();
			}
		}

        public override void Update(GameTime gameTime) {
			Vector2 MousePosition = new Vector2(
                (float)Main.mouseX, (float)Main.mouseY);
			if (!ContainsPoint(MousePosition) && Main.mouseLeft) {
				//TODO, figure out how to refocus without triggering
				//unfocus while clicking enable button.
				Unfocus();
			}
			base.Update(gameTime);
		}

        public void Write(string text) {
			bool changed = text != Text;
			base.SetText(base.Text.Insert(this._cursor, text));
			this._cursor += text.Length;
			_cursor = Math.Min(Text.Length, _cursor);
			Recalculate();
			if(changed) OnTextChanged?.Invoke(text);
		}

		public void WriteAll(string text) {
			bool changed = text != Text;
			base.SetText(text);
			this._cursor = text.Length;
			//_cursor = Math.Min(Text.Length, _cursor);
			Recalculate();
			if(changed) OnTextChanged?.Invoke(text);
		}

        public override void SetText(string text, float textScale=1f,
        bool large=false) {
			if(text == Text) return;
			if(text.ToString().Length > this._maxLength) {
				text = text.ToString().Substring(0, this._maxLength);
			}
			base.SetText(text, textScale, large);
			//this.MinWidth.Set(120, 0f);
			this._cursor = Math.Min(base.Text.Length, this._cursor);
			OnTextChanged?.Invoke(text);
		}

        public void Backspace() {
			if(this._cursor == 0) return;
			base.SetText(base.Text.Substring(0, base.Text.Length - 1));
			Recalculate();
		}

		public void CursorLeft() {
			if(this._cursor > 0) this._cursor--;
		}

		public void CursorRight() {
			if(this._cursor < base.Text.Length) this._cursor++;
		}

        protected static bool JustPressed(Keys key) {
			return Main.inputText.IsKeyDown(key)
                && !Main.oldInputText.IsKeyDown(key);
		}

		protected virtual void handleKeys() {
			if(!focused) return;
			if(JustPressed(Keys.Tab)) {
                Unfocus();
                OnTabPressed?.Invoke();
            }

            if(JustPressed(Keys.Enter)) {
                //Unfocus();
                OnEnterPressed?.Invoke();
				//XXX prevent chat opening
            }
            if(JustPressed(Keys.Up))   OnUpPressed?.Invoke();
            if(JustPressed(Keys.Down)) OnDownPressed?.Invoke();
		}

        protected void _drawFocused() {
            Terraria.GameInput.PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            // This might work.....assuming chat isn't open
            WriteAll(Main.GetInputText(Text));
        }

		protected void _drawText(SpriteBatch spriteBatch) {
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 pos = innerDimensions.Position();
			pos.Y += (IsLarge ? (10f * TextScale) : 4f) * TextScale;
			pos.X += 4f;
			if (IsLarge) {
				Utils.DrawBorderStringBig(spriteBatch, Text, pos, TextColor,
					TextScale, 0f, 0f, -1);
			}
			else Utils.DrawBorderString(spriteBatch, Text, pos, TextColor,
				TextScale, 0f, 0f, -1);
		}

		protected void _drawHintText(SpriteBatch spriteBatch, ref Vector2 pos,
		DynamicSpriteFont spriteFont) {
			Vector2 hintTextSize = new Vector2(
				spriteFont.MeasureString(hintText.ToString()).X,
				IsLarge ? 32f : 16f) * TextScale;
			pos.Y += (IsLarge ? (10f * TextScale) : 4f) * TextScale;
			pos.X += 4f;
			if(base.IsLarge) {
				Utils.DrawBorderStringBig(spriteBatch, hintText, pos,
					Color.Gray, base.TextScale, 0f, 0f, -1);
			}
			else {
				Utils.DrawBorderString(spriteBatch, hintText, pos,
					Color.Gray, base.TextScale, 0f, 0f, -1);
				pos.X -= 5;
				//pos.X -= (innerDimensions.Width - hintTextSize.X) * 0.5f;
			}
		}

		protected void _drawCursor(SpriteBatch spriteBatch, ref Vector2 pos,
		DynamicSpriteFont spriteFont) {
			if((this._frameCount %= 40) > 20) return;

			Vector2 vector = new Vector2(spriteFont.MeasureString(
				base.Text.Substring(0, this._cursor)).X,
				base.IsLarge ? 32f : 16f) * base.TextScale;

			//pos.X += //(innerDimensions.Width - base.TextSize.X) * 0.5f +
			//	vector.X - (base.IsLarge ? 8f : 4f) * base.TextScale + 6f;
			pos.Y += (IsLarge ? (10f * TextScale) : 6f) * TextScale;
			pos.X += 4f + vector.X;
			if(base.IsLarge) Utils.DrawBorderStringBig(spriteBatch, "|", pos,
				base.TextColor, base.TextScale, 0f, 0f, -1);
			else Utils.DrawBorderString(spriteBatch, "|", pos,
				base.TextColor, base.TextScale, 0f, 0f, -1);
		}

        protected override void DrawSelf(SpriteBatch spriteBatch) {
			//draw background
			Rectangle hitbox = GetDimensions().ToRectangle();
			//hitbox.Inflate(4, 4);
			Color bg;
			if(focused) {
				bg = isValid(Text) ? bgColorFocus : bgColorInvalidFocus;
			}
			else {
				bg = isValid(Text) ? bgColor : bgColorInvalid;
			}
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, hitbox, bg);

			if(focused) _drawFocused();
			handleKeys();
			_drawText(spriteBatch);
			this._frameCount++;

			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Vector2 pos = innerDimensions.Position();
			DynamicSpriteFont spriteFont = base.IsLarge ?
				FontAssets.DeathText.Value :
				FontAssets.MouseText.Value;

			pos.Y -= (base.IsLarge ? 8f : 1f) * base.TextScale;
			if(Text.Length == 0) _drawHintText(spriteBatch, ref pos, spriteFont);
			if(focused) _drawCursor(spriteBatch, ref pos, spriteFont);
		}
    } //class
} //namespace
