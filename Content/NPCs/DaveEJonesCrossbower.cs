using Microsoft.Xna.Framework;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace PiratePanic.Content.NPCs
{
	public class DaveEJonesCrossbower : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.PirateCrossbower];

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f 
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
		}

		public override void SetDefaults() {
			NPC.width = 18;
			NPC.height = 40;
			NPC.damage = 45;
			NPC.defense = 27;
			NPC.lifeMax = 375;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 60f;
			NPC.knockBackResist = 0.3f;
			NPC.aiStyle = NPCAIStyleID.Fighter; 

			AIType = NPCID.PirateCrossbower; // Might end up despawning if there isn't a Pirate Invasion. Needs to be checked.
			AnimationType = NPCID.PirateCrossbower; 
			Banner = Item.NPCtoBanner(NPCID.PirateCrossbower);
			BannerItem = Item.BannerToItem(Banner); 
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.NightTime,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Dave E Jones Crossbower"),
			]);
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

		// public override float SpawnChance(NPCSpawnInfo spawnInfo) {
		// 	// Can only spawn in the ExampleSurfaceBiome and if there are no other ExampleZombieThiefs
		// 	if (spawnInfo.Player.InModBiome(ModContent.GetInstance<ExampleSurfaceBiome>()) && !NPC.AnyNPCs(Type)) {
		// 		return SpawnCondition.OverworldNightMonster.Chance * 0.1f; // Spawn with 1/10th the chance of a regular zombie.
		// 	}

		// 	return 0f;
		// }
	}
}