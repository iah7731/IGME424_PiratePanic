using PiratePanic.Content.Items.Projectiles;
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
    internal class DaveEJonesRanged : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.SetNameOverride("Yer Lass' End");
            Item.useTime = 25;
            Item.useAnimation = 25;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 57;
            Item.value = 12000;
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;
            Item.noMelee = false;
            Item.noUseGraphic = false;
        }
    }
}
