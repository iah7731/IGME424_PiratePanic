using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;
using Terraria.Chat;
using PiratePanic.Content.Biomes;
using PiratePanic.Content.NPCs.DaveEJones;

namespace PiratePanic.Content.Items.Misc
{
    internal class DaveEJonesSummon : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 45;
            Item.useTime = 45;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Roar;
            Item.consumable = true;
            Item.value = 0;
            Item.sellPrice(0);
        }

        public override bool? UseItem(Player player)
        {
            if (!NPC.AnyNPCs(ModContent.NPCType<DaveEJonesBody>()))
            {
                Vector2 zero = player.position;
                int num = player.width;
                int num2 = player.height;
                int num3 = NPC.NewNPC(NPC.GetBossSpawnSource(Main.myPlayer), (int)zero.X + num / 2, (int)zero.Y + num2 / 2, ModContent.NPCType<DaveEJonesBody>()); // change the 35 to Dave E Jones's ID
                Main.npc[num3].netUpdate = true;
                string nPCNameValue = Lang.GetNPCNameValue(ModContent.NPCType<DaveEJonesBody>()); // Change this to Dave E Jones
                if (Main.netMode == 0)
                {
                    Main.NewText(Language.GetTextValue("Announcement.HasAwoken", nPCNameValue), 175, 75);
                }
                else if (Main.netMode == 2)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Lang.GetNPCName(35).ToNetworkText()), new Color(175, 75, 255)); // Change this to Dave E Jones
                }

                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Bone, 20);
            recipe.AddIngredient(ItemID.Skull, 1);
            recipe.AddTile(TileID.DemonAltar);
            recipe.Register();
        }
    }
}
