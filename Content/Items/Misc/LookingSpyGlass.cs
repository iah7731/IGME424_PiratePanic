using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SubworldLibrary;
using PiratePanic.Content.Subworlds;


namespace PiratePanic.Content.Items.Misc
{
    internal class LookingSpyGlass : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item6;
            Item.consumable = false;
            Item.value = 12000;
            Item.sellPrice(9284);
        }

        public override bool? UseItem(Player player)
        {
            // Only let it work on the local player
            if (Main.myPlayer == player.whoAmI)
            {
                if (SubworldSystem.IsActive<DaveEJonesIsland>())
                {
                    Main.NewText("The Island be waiting");
                    SubworldSystem.Exit();
                }
                else
                {
                    Main.NewText("Welcome to this cursed place");
                    SubworldSystem.Enter<DaveEJonesIsland>();
                }
            }

            return true;
        }

        // Saving this for the boss summon
        //public override void AddRecipes()
        //{
        //    Recipe recipe = CreateRecipe();
        //    recipe.AddIngredient(ItemID.Skull, 1);
        //    recipe.AddIngredient(ItemID.Bone, 20);
        //    recipe.AddIngredient(ItemID.MagicMirror, 1);
        //    recipe.AddTile(TileID.MythrilAnvil);
        //    recipe.Register();
        //}

    }
}
