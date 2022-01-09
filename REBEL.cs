using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace REBEL
{
	public class REBEL : Mod
	{
		public override void Load() {
			Logger.InfoFormat("Hello world!");
		}

		public override void Unload() {
			Logger.InfoFormat("Goodbye cruel world!");
		}

		public override void PostSetupContent() {
			Logger.InfoFormat("PostSetupContent");
		}

		public override void AddRecipeGroups() {
			Logger.InfoFormat("AddRecipeGroups");
		}

		public override void AddRecipes() {
			Logger.InfoFormat("AddRecipes");
		}
	}
}
