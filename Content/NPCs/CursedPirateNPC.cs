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

namespace PiratePanic.Content.NPCs
{
	[AutoloadHead]
    public class CursedPirate : ModNPC
    {

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			NPCID.Sets.NoTownNPCHappiness[NPC.type] = true;
			NPCID.Sets.SpawnsWithCustomName[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
				// Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
				// If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			ContentSamples.NpcBestiaryRarityStars[Type] = 3; // We can override the default bestiary star count calculation by setting this.
		}

		public override void SetDefaults() {
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;

			this.AIType = 7;
		}

		public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("Spongebob"));
			chat.Add(Language.GetTextValue("Patrick"));
			chat.Add(Language.GetTextValue("Squidward"));
			chat.Add(Language.GetTextValue("Mr. Krabs"));
			chat.Add(Language.GetTextValue("Uhhhh...."), 5.0);
			chat.Add(Language.GetTextValue("YO SOMEBODY GET THE DOOR!!!!"), 0.1);

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

		public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Summon";
		}

		public override void OnChatButtonClicked(bool firstButton, ref string shopName)
		{
			if(firstButton)
			{
				SpawnDaveEJones(Main.myPlayer);
			}
		}


		public override bool CanGoToStatue(bool toKingStatue) => true;

		public static void SpawnDaveEJones(int onWho)
		{
			bool flag = true;
			bool flag2 = false;
			Vector2 zero = Vector2.Zero;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 35) // change this to Dave E Jones ID later
				{
					flag = false;
					break;
				}
			}
			for (int j = 0; j < 200; j++)
			{
				if (!Main.npc[j].active)
				{
					continue;
				}
				if (Main.npc[j].type == ModContent.NPCType<CursedPirate>())
				{
					flag2 = true;
					Main.npc[j].ai[3] = 1f;
					zero = Main.npc[j].position;
					num = Main.npc[j].width;
					num2 = Main.npc[j].height;
					if (Main.netMode == 2)
					{
						NetMessage.SendData(23, -1, -1, null, j);
					}
				}
			}
			if (flag && flag2)
			{
				int num3 = NPC.NewNPC(NPC.GetBossSpawnSource(onWho), (int)zero.X + num / 2, (int)zero.Y + num2 / 2, 35); // change the 35 to Dave E Jones's ID
				Main.npc[num3].netUpdate = true;
				string nPCNameValue = Lang.GetNPCNameValue(ModContent.NPCType<CursedPirate>()); // Change this to Dave E Jones
				if (Main.netMode == 0)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", nPCNameValue), 175, 75);
				}
				else if (Main.netMode == 2)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Lang.GetNPCName(35).ToNetworkText()), new Color(175, 75, 255)); // Change this to Dave E Jones
				}
			}
		}
    }
}