using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace PiratePanic.Content.Items.Weapons
{
    internal class DaveEJonesMelee : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.TerraBlade);
            Item.SetNameOverride("Dave's Swashbuckler");
            Item.shoot = ModContent.ProjectileType<Projectiles.DaveEJonesMeleeProjectile>();
            Item.shootSpeed = 12f;
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.damage = 57;
            Item.value = 12000;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
            Item.noMelee = false;                
            Item.noUseGraphic = false;           
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.CursedTorch);
            }
        }
    }
}
