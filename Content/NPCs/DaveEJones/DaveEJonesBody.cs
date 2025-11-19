using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using PiratePanic.Content.Items.Weapons;

namespace PiratePanic.Content.NPCs.DaveEJones
{
	// The main part of the boss, usually referred to as "body"
	[AutoloadBossHead] // This attribute looks for a texture called "ClassName_Head_Boss" and automatically registers it as the NPC boss head icon
	public class DaveEJonesBody : ModNPC
	{
		public override void SetStaticDefaults() {
			Main.npcFrameCount[Type] = 6;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			// Specify the debuffs it is immune to. Most NPCs are immune to Confused.
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
			// This boss also becomes immune to OnFire and all buffs that inherit OnFire immunity during the second half of the fight. See the ApplySecondStageBuffImmunities method.

			// // Influences how the NPC looks in the Bestiary
			// NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
			// 	CustomTexturePath = "ExampleMod/Assets/Textures/Bestiary/MinionBoss_Preview",
			// 	PortraitScale = 0.6f, // Portrait refers to the full picture when clicking on the icon in the bestiary
			// 	PortraitPositionYOverride = 0f,
			// };
			// NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults() {

            NPC.width = 80;
			NPC.height = 102;
			NPC.aiStyle = NPCAIStyleID.SkeletronPrimeHead;
			NPC.damage = 40;
			NPC.defense = 20;
			NPC.lifeMax = 20000;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.value = 120000f;
			NPC.knockBackResist = 0f;
			NPC.boss = true;
			NPC.npcSlots = 10f;

			// Default buff immunities should be set in SetStaticDefaults through the NPCID.Sets.ImmuneTo{X} arrays.
			// To dynamically adjust immunities of an active NPC, NPC.buffImmune[] can be changed in AI: NPC.buffImmune[BuffID.OnFire] = true;
			// This approach, however, will not preserve buff immunities. To preserve buff immunities, use the NPC.BecomeImmuneTo and NPC.ClearImmuneToBuffs methods instead, as shown in the ApplySecondStageBuffImmunities method below.


			// The following code assigns a music track to the boss in a simple way.
			if (!Main.dedServ) {
				//Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/Ropocalypse2");
			}
		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			// Sets the description of this NPC that is listed in the bestiary
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				new MoonLordPortraitBackgroundProviderBestiaryInfoElement(), // Plain black background
				new FlavorTextBestiaryInfoElement("Dave E Jones")
			});
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

			// The order in which you add loot will appear as such in the Bestiary. To mirror vanilla boss order:
			// 1. Trophy
			// 2. Classic Mode ("not expert")
			// 3. Expert Mode (usually just the treasure bag)
			// 4. Master Mode (relic first, pet last, everything else in between)

			// Trophies are spawned with 1/10 chance
			//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.MinionBossTrophy>(), 10));

			// All the Classic Mode drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

			// Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TellNoTales>(), 2));
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DaveEJonesMelee>(), 2));


			// Finally add the leading rule
			npcLoot.Add(notExpertRule);

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			//npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<MinionBossBag>()));

			// ItemDropRule.MasterModeCommonDrop for the relic
			//npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.MinionBossRelic>()));

			// ItemDropRule.MasterModeDropOnAllPlayers for the pet
			//npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MinionBossPetItem>(), 4));
		}


		public override void BossLoot(ref int potionType) 
        {
			potionType = ItemID.HealingPotion;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot) {
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"
				int backGoreType = Mod.Find<ModGore>("DaveEJonesBossBody_Top").Type;
				int frontGoreType = Mod.Find<ModGore>("DaveEJonesBossBody_Bottom").Type;

				var entitySource = NPC.GetSource_Death();

				for (int i = 0; i < 2; i++) {
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
					Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
				}

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}
	}
		// Base game skeletron prime arm stats
		// 	else if (this.type == 128)
		// {
		// 	base.width = 52;
		// 	base.height = 52;
		// 	this.aiStyle = 35;
		// 	this.damage = 30;
		// 	this.defense = 23;
		// 	this.lifeMax = 7000;
		// 	this.HitSound = SoundID.NPCHit4;
		// 	this.DeathSound = SoundID.NPCDeath14;
		// 	this.noGravity = true;
		// 	this.noTileCollide = true;
		// 	this.knockBackResist = 0f;
		// 	this.netAlways = true;
		// }
		// else if (this.type == 129)
		// {
		// 	base.width = 52;
		// 	base.height = 52;
		// 	this.aiStyle = 33;
		// 	this.damage = 56;
		// 	this.defense = 38;
		// 	this.lifeMax = 9000;
		// 	this.HitSound = SoundID.NPCHit4;
		// 	this.DeathSound = SoundID.NPCDeath14;
		// 	this.noGravity = true;
		// 	this.noTileCollide = true;
		// 	this.knockBackResist = 0f;
		// 	this.netAlways = true;
		// }
		// else if (this.type == 130)
		// {
		// 	base.width = 52;
		// 	base.height = 52;
		// 	this.aiStyle = 34;
		// 	this.damage = 52;
		// 	this.defense = 34;
		// 	this.lifeMax = 9000;
		// 	this.HitSound = SoundID.NPCHit4;
		// 	this.DeathSound = SoundID.NPCDeath14;
		// 	this.noGravity = true;
		// 	this.noTileCollide = true;
		// 	this.knockBackResist = 0f;
		// 	this.netAlways = true;
		// }
		// else if (this.type == 131)
		// {
		// 	base.width = 52;
		// 	base.height = 52;
		// 	this.aiStyle = 36;
		// 	this.damage = 29;
		// 	this.defense = 20;
		// 	this.lifeMax = 6000;
		// 	this.HitSound = SoundID.NPCHit4;
		// 	this.DeathSound = SoundID.NPCDeath14;
		// 	this.noGravity = true;
		// 	this.noTileCollide = true;
		// 	this.knockBackResist = 0f;
		// 	this.netAlways = true;
		// }
}