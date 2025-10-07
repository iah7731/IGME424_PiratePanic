using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PiratePanic.Content.Items;

namespace PiratePanic.Content.NPCs
{
    public class ExampleGlobalNPC : GlobalNPC
    {
        public override void ModifyShop (NPCShop shop) 	
        {//
            if (shop.NpcType == NPCID.Pirate)
            {
                shop.Add<TutorialSword>();
            }
        }
    }
}