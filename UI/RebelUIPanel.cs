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
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace REBEL.UI {
    public abstract class RebelUIPanel: UIState {
        /** Base for UI panels.
         */
        public DragableUIPanel panel;
        public Rectangle defaultPosition {
            get => new Rectangle(400, 100, 600, 220);
        }

        public static UIHoverImageButton makeButton(String texture,
            String text, Rectangle pos, MouseEvent onClick) {
            Asset<Texture2D> tex = Main.Assets.Request<Texture2D>(texture);
			UIHoverImageButton btn = new UIHoverImageButton(tex, text);
			btn.Left  .Set(pos.Left,   0f);
			btn.Top   .Set(pos.Top,    0f);
			btn.Width .Set(pos.Width,  0f);
			btn.Height.Set(pos.Height, 0f);
			btn.OnClick += new MouseEvent(onClick);
            return btn;
        }

        public static DragableUIPanel makePanel(Rectangle rect) {
            var panel = new DragableUIPanel();
            panel.SetPadding(4);
            panel.Left  .Set(rect.Left,   0f); //default position relative to screen
            panel.Top   .Set(rect.Top,    0f);
            panel.Width .Set(rect.Width,  0f);
            panel.Height.Set(rect.Height, 0f);
            panel.BackgroundColor = new Color(0x00, 0x5D, 0xB3, 192);
            return panel;
        }

        public override void OnInitialize() {
            //make the actual panel
            panel = makePanel(defaultPosition);

            //make close button
            panel.Append(makeButton("Images/UI/ButtonDelete",
                Language.GetTextValue("LegacyInterface.52"), //"Close"
                new Rectangle((int)panel.Width.Pixels - 40, 5, 22, 22),
                new MouseEvent(btnCloseClicked)));

            Setup();

            //display the panel
            Append(panel);
        } //OnInitialize

        //called to add the UI elements when created
        public abstract void Setup();

        private void btnCloseClicked(UIMouseEvent evt,
        UIElement listeningElement) {
            onClose();
        }

        public virtual void onClose() {
			SoundEngine.PlaySound(SoundID.MenuClose);
			ModContent.GetInstance<REBEL>().showUI(UIPanelId.None);
		}

        public virtual void update() {
            //default: do nothing
        }
    } //class
} //namespace
