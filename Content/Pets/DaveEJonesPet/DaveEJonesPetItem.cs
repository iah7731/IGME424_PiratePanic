using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Pets.DaveEJonesPet
{
	public class DaveEJonesPetItem : ModItem
	{
		public override void SetDefaults() {
			Item.DefaultToVanitypet(ModContent.ProjectileType<DaveEJonesPetProjectile>(), ModContent.BuffType<DaveEJonesPetBuff>());
			Item.width = 30;
			Item.height = 24;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.sellPrice(0, 5);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			player.AddBuff(Item.buffType, 2); 

			return false;
		}
	}
}