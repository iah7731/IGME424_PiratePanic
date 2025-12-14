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
    internal class DaveEJonesMeleeProjectile : ModProjectile
    {
        public override void SetDefaults() 
        { 
            AIType = ProjectileID.TerraBeam; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.damage = 23; 
            Projectile.light = 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }
    }
}
