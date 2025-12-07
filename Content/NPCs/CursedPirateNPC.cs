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

namespace PiratePanic.Content.NPCs
{
	[AutoloadHead]
    public class CursedPirate : ModNPC
	{
		int hayBlockX = 0;
		int hayBlockY = 0;

		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 26; // The total amount of frames the NPC has
			NPCID.Sets.ShimmerTownTransform[NPC.type] = false; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

			NPCID.Sets.ShimmerTownTransform[Type] = false; // Allows for this NPC to have a different texture after touching the Shimmer liquid.

			NPCID.Sets.NoTownNPCHappiness[NPC.type] = true;
			NPCID.Sets.SpawnsWithCustomName[Type] = true;

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, 
				Direction = 1 
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			ContentSamples.NpcBestiaryRarityStars[Type] = 3; // We can override the default bestiary star count calculation by setting this.
			NPCID.Sets.ActsLikeTownNPC[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = NPCAIStyleID.Passive;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;
			AnimationType = NPCID.Guide;
			NPC.dontTakeDamage = true;
			SpawnModBiomes = [ModContent.GetInstance<PirateIsland>().Type];
		}

		public override string GetChat() {
			Player closestPlayer = Main.player[Player.FindClosest(NPC.position, NPC.width, NPC.height)];
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add(Language.GetTextValue("*Harrdarr Yarr Blarr*"));
			chat.Add(Language.GetTextValue("Ye seen me crew? Told ‘em to fetch me a bottle ages ago!"));

			if(closestPlayer.statLifeMax2 >= 400 && closestPlayer.statDefense >= 25)
            {
                chat.Add(Language.GetTextValue("Ye be here to face Jones? Aye I bet a bottle in yer favor."), 1.0);
				chat.Add(Language.GetTextValue("Jones be damned again with ye here."), 1.0);
            }
			else
            {
              chat.Add(Language.GetTextValue("Ye best turn back, lest ye lose yer life."), 3.0);
			  chat.Add(Language.GetTextValue("Jones be waitin’ fer ye on the other side."), 3.0);  
            }

			if(!closestPlayer.Male)
            {
                chat.Add(Language.GetTextValue("A lass could ne’er best Jones. But as strong as ye are…"), 1.0);
            }

			if(Main.IsItStorming)
            {
                chat.Add(Language.GetTextValue("Ye been foolish to sail the wild sea."), 0.5);
            }

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

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.InModBiome(ModContent.GetInstance<PirateIsland>())
			&& !NPC.AnyNPCs(Type)
			&& spawnInfo.SpawnTileType == TileID.HayBlock)
			{
				return 1f;
			}

			return 0f;
		}

		public override void HitEffect(NPC.HitInfo hit) {

			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				// Retrieve the gore types. This NPC only has shimmer variants. (6 total gores)
				string variant = "";
				int headGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Head").Type;
				int armGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Arm").Type;
				int legGore = Mod.Find<ModGore>($"{Name}_Gore{variant}_Leg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}

		public static void SpawnDaveEJones(int onWho)
		{
			bool flag = true;
			bool flag2 = false;
			Vector2 zero = Vector2.Zero;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<DaveEJonesBody>())
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
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, j);
					}
				}
			}
			if (flag && flag2)
			{
				int num3 = NPC.NewNPC(NPC.GetBossSpawnSource(onWho), (int)zero.X + num / 2, (int)zero.Y + num2 / 2, ModContent.NPCType<DaveEJonesBody>()); // change the 35 to Dave E Jones's ID
				Main.npc[num3].netUpdate = true;
				string nPCNameValue = Lang.GetNPCNameValue(ModContent.NPCType<DaveEJonesBody>()); // Change this to Dave E Jones
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(Language.GetTextValue("Announcement.HasAwoken", nPCNameValue), 175, 75);
				}
				else if (Main.netMode == NetmodeID.Server)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", Lang.GetNPCName(ModContent.NPCType<DaveEJonesBody>()).ToNetworkText()), new Color(175, 75, 255)); // Change this to Dave E Jones
				}
			}
		}

    }
}