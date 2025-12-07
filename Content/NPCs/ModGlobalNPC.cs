using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PiratePanic.Content.Items;
using PiratePanic.Content.Items.Misc;
using System.Runtime.CompilerServices;
using Terraria.Utilities;
using Terraria.Chat;
using System.Configuration;
using Terraria.Localization;
using PiratePanic.Common.Systems;

namespace PiratePanic.Content.NPCs
{
    public class ExampleGlobalNPC : GlobalNPC
    {
        public override void ModifyShop (NPCShop shop) 	
        {//
            if (shop.NpcType == NPCID.Pirate)
            {
                shop.Add<LookingSpyGlass>();
            }
        }

        public override void GetChat(NPC npc, ref string chat)
        {
            if(npc.type == NPCID.Pirate && !DownedBossSystem.downedDaveEJonesBoss)
            {
            WeightedRandom<string> overrideChat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			overrideChat.Add(Language.GetTextValue("Ye best be clear o’ Jones’ island, lest ye lookin fer a fight."));
			overrideChat.Add(Language.GetTextValue("Cursed pirates be landlubbers now. Doomed ne’er to sail again."));

			string newChat = overrideChat; // chat is implicitly cast to a string. This is where the random choice is made.

            chat = overrideChat;

            }
        }
    }
}