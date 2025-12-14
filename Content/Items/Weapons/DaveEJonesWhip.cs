using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using PiratePanic.Content.Items.Projectiles;

namespace PiratePanic.Content.Items.Weapons
{
    internal class DaveEJonesWhip : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.BoneWhip);

            Item.DamageType = DamageClass.SummonMeleeSpeed;
            Item.damage = 20;
            Item.knockBack = 2;
            Item.rare = ItemRarityID.Green;

            Item.shoot = ModContent.ProjectileType<DaveEJonesWhipProjectile>();
            Item.shootSpeed = 4;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.UseSound = SoundID.Item152;
            Item.channel = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
        }
    }
}
