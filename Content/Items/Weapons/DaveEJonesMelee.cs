using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

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
            Item.width = 26;
            Item.height = 42;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 50;
            Item.knockBack = 6;
            Item.crit = 6;

            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Projectiles.DaveEJonesMeleeProjectile>();
            Item.shootSpeed = 8f;
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
