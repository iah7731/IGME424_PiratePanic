using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Items.Projectiles
{
    internal class TellNoTalesProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Wasp);
            AIType = ProjectileID.Wasp;
            Projectile.ignoreWater = true;


            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            //Projectile.velocity *= 1.05f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }

    }
}
