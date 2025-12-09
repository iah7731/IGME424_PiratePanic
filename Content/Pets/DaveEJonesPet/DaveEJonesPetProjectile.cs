using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Pets.DaveEJonesPet
{
	public class DaveEJonesPetProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projPet[Type] = true;
		}

		public override void SetDefaults() {
			Projectile.CloneDefaults(ProjectileID.BabySkeletronHead);
			AIType = ProjectileID.BabySkeletronHead;
		}

		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];

			player.skeletron = false;

			return true;
		}

		public override void AI() {
			Player player = Main.player[Projectile.owner];

			if (!player.dead && player.HasBuff(ModContent.BuffType<DaveEJonesPetBuff>())) {
				Projectile.timeLeft = 2;
			}
		}
	}
}