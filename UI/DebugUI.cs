using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using ReLogic.Content;

namespace REBEL.UI {
    public class DebugUI: UIState {
        /** The UI, which covers the entire screen, even if we don't
         *  put anything there.
         */
        public static bool visible;
        private DragableUIPanel panel;

        private static UIHoverImageButton makeButton(String texture,
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

        public override void OnInitialize() {
            //make the actual panel
            Mod.Logger.Info("UI init");
            panel = new DragableUIPanel();
            panel.SetPadding(0);
            panel.Left  .Set(400f, 0f); //set position relative to screen
            panel.Top   .Set(100f, 0f);
            panel.Width .Set(170f, 0f);
            panel.Height.Set( 70f, 0f);
            panel.BackgroundColor = new Color(73, 94, 171);

            //make close button
            var btnClose = makeButton("Terraria/UI/ButtonDelete",
                Language.GetTextValue("LegacyInterface.52"), //"Close"
                new Rectangle(140, 10, 22, 22),
                new MouseEvent(btnCloseClicked));
            panel.Append(btnClose);

            //display the panel
            Append(panel);
            Recalculate();
        } //OnInitialize

        private void btnCloseClicked(UIMouseEvent evt,
        UIElement listeningElement) {
			SoundEngine.PlaySound(SoundID.MenuClose);
			visible = false;
		}

        //public void update() {
        //    //...
        //}
    } //class
} //namespace
