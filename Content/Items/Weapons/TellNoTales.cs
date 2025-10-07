using Terraria;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using PiratePanic.Content.Items.Projectiles;

namespace PiratePanic.Content.Items.Weapons
{
    public class TellNoTales : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.WaspGun);
            Item.damage = 60;
            Item.value = 12000;
            Item.rare = ItemRarityID.Green;
            Item.shoot = ModContent.ProjectileType<TellNoTalesProjectile>();
        }
    }
}
