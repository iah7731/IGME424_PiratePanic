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


            Projectile.tileCollide = false;
        }
    }
}
