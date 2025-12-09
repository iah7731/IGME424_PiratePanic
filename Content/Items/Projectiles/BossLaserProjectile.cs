using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Items.Projectiles
{
    internal class BossLaserProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DeathLaser);
            AIType = ProjectileID.DeathLaser;
            Projectile.damage = 19;
        }
    }
}
