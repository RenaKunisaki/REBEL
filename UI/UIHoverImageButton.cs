using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;

namespace REBEL.UI {
	public class UIHoverImageButton: UIImageButton {
		internal string HoverText;

		public UIHoverImageButton(Asset<Texture2D> texture, string hoverText):
        base(texture) {
			HoverText = hoverText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
			if (IsMouseHovering) {
				Main.hoverItemName = HoverText;
			}
		}
	}
}
