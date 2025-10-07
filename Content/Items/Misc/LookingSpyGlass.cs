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
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item6;
            Item.consumable = false;
        }

        public override bool? UseItem(Player player)
        {
            // Only let it work on the local player
            if (Main.myPlayer == player.whoAmI)
            {
                if (SubworldSystem.IsActive<DaveEJonesIsland>())
                {
                    Main.NewText("Returning to the overworld...");
                    SubworldSystem.Exit();
                }
                else
                {
                    Main.NewText("Entering the subworld...");
                    SubworldSystem.Enter<DaveEJonesIsland>(); // Enters subworld
                }
            }

            return true;
        }

    }
}
