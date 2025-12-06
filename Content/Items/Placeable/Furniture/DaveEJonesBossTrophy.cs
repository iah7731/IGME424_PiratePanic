using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Items.Placeable.Furniture
{
	public class DaveEJonesBossTrophy : ModItem
	{
		public override void SetDefaults() {
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle as well as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.DaveEJonesTrophy>(), 0);
			Item.width = 30;
			Item.height = 40;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 5);
		}
	}
}