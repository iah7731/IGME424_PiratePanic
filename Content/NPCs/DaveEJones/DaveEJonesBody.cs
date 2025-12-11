using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using Terraria.Graphics;
using ReLogic.Content;
using PiratePanic.Content.Biomes;
using PiratePanic.Common.Systems;
using PiratePanic.Content.Pets.DaveEJonesPet;
using PiratePanic.Content.Items.Projectiles;

namespace PiratePanic.Content.NPCs.DaveEJones
{
    // The main part of the boss, usually referred to as "body"
    [AutoloadBossHead] // This attribute looks for a texture called "ClassName_Head_Boss" and automatically registers it as the NPC boss head icon

    public class DaveEJonesBody : ModNPC
    {
       	public static SoundStyle ahoySound; 
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6;

            // Automatically group with other bosses
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Specify the debuffs it is immune to. Most NPCs are immune to Confused.
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
            ContentSamples.NpcBestiaryRarityStars[Type] = 4;
            ahoySound = new SoundStyle("PiratePanic/Assets/Sounds/ahoy") {
					PitchVariance = 0.5f,
				};
        }
        public override void SetDefaults()
        {
            NPC.width = 80;
            NPC.height = 102;
            NPC.aiStyle = -1;
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
            AnimationType = NPCID.SkeletronPrime;
            NPC.value = Item.buyPrice(gold: 15);
            SpawnModBiomes = [ModContent.GetInstance<PirateIsland>().Type];
            // The following code assigns a music track to the boss in a simple way.
            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TerrariaModBossFinalMaybe");
            }
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // Sets the description of this NPC that is listed in the bestiary
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				new FlavorTextBestiaryInfoElement("Cursed by the treasure he sought, Dave E Jones’s soul now resides within another unfortunate captain drawn in by the allure of riches, ready to protect what’s his.")
            });
        }

        public override void AI()
        {
            NPC.reflectsProjectiles = false;
            if (NPC.ai[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.TargetClosest();
                NPC.ai[0] = 1f;
                SoundEngine.PlaySound(ahoySound, new Vector2((int)NPC.position.X, (int)NPC.position.Y));
                int num495 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<DaveEJonesCannon>());
                Main.npc[num495].ai[0] = -1f;
                Main.npc[num495].ai[1] = NPC.whoAmI;
                Main.npc[num495].target = NPC.target;
                Main.npc[num495].netUpdate = true;
                num495 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<DaveEJonesSaw>());
                Main.npc[num495].ai[0] = 1f;
                Main.npc[num495].ai[1] = NPC.whoAmI;
                Main.npc[num495].target = NPC.target;
                Main.npc[num495].netUpdate = true;
                num495 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<DaveEJonesVice>());
                Main.npc[num495].ai[0] = -1f;
                Main.npc[num495].ai[1] = NPC.whoAmI;
                Main.npc[num495].target = NPC.target;
                Main.npc[num495].ai[3] = 150f;
                Main.npc[num495].netUpdate = true;
                num495 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)(NPC.position.X + (float)(NPC.width / 2)), (int)NPC.position.Y + NPC.height / 2, ModContent.NPCType<DaveEJonesLaser>());
                Main.npc[num495].ai[0] = 1f;
                Main.npc[num495].ai[1] = NPC.whoAmI;
                Main.npc[num495].target = NPC.target;
                Main.npc[num495].netUpdate = true;
                Main.npc[num495].ai[3] = 150f;
            }
            if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 6000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 6000f)
            {
                NPC.TargetClosest();
                if (Main.player[NPC.target].dead || Math.Abs(NPC.position.X - Main.player[NPC.target].position.X) > 6000f || Math.Abs(NPC.position.Y - Main.player[NPC.target].position.Y) > 6000f)
                {
                    NPC.ai[1] = 3f;
                }
            }
            if (NPC.ai[1] == 0f)
            {
                NPC.ai[2] += 1f;
                if (NPC.ai[2] >= 600f)
                {
                    NPC.ai[2] = 0f;
                    NPC.ai[1] = 1f;
                    NPC.TargetClosest();
                    NPC.netUpdate = true;
                }
                NPC.rotation = NPC.velocity.X / 15f;
                float num496 = 0.1f;
                float num497 = 2f;
                float num498 = 0.1f;
                float num499 = 8f;
                int num500 = 200;
                int num501 = 500;
                float num502 = 0f;
                int num503 = ((!(Main.player[NPC.target].Center.X < NPC.Center.X)) ? 1 : (-1));
                if (Main.expertMode)
                {
                    num496 = 0.03f;
                    num497 = 4f;
                    num498 = 0.07f;
                    num499 = 9.5f;
                }
                if (NPC.position.Y > Main.player[NPC.target].position.Y - (float)num500)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.98f;
                    }
                    NPC.velocity.Y -= num496;
                    if (NPC.velocity.Y > num497)
                    {
                        NPC.velocity.Y = num497;
                    }
                }
                else if (NPC.position.Y < Main.player[NPC.target].position.Y - (float)num501)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.98f;
                    }
                    NPC.velocity.Y += num496;
                    if (NPC.velocity.Y < 0f - num497)
                    {
                        NPC.velocity.Y = 0f - num497;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) > Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) + 100f + num502)
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.98f;
                    }
                    NPC.velocity.X -= num498;
                    if (NPC.velocity.X > num499)
                    {
                        NPC.velocity.X = num499;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - 100f + num502)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.98f;
                    }
                    NPC.velocity.X += num498;
                    if (NPC.velocity.X < 0f - num499)
                    {
                        NPC.velocity.X = 0f - num499;
                    }
                }
            }
            else if (NPC.ai[1] == 1f)
            {
                NPC.defense *= 2;
                NPC.damage *= 2;
                NPC.ai[2] += 1f;
                if (NPC.ai[2] == 2f)
                {
                    SoundEngine.PlaySound(ahoySound, new Vector2((int)NPC.position.X, (int)NPC.position.Y));
                }
                if (NPC.ai[2] >= 400f)
                {
                    NPC.ai[2] = 0f;
                    NPC.ai[1] = 0f;
                }

                NPC.rotation += (float)NPC.direction * 0.3f;

                Vector2 vector54 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num504 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector54.X;
                float num505 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector54.Y;
                float num506 = (float)Math.Sqrt(num504 * num504 + num505 * num505);
                float num507 = 2f;
                if (Main.expertMode)
                {
                    num507 = 6f;
                    if (num506 > 150f)
                    {
                        num507 *= 1.05f;
                    }
                    if (num506 > 200f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 250f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 300f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 350f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 400f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 450f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 500f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 550f)
                    {
                        num507 *= 1.1f;
                    }
                    if (num506 > 600f)
                    {
                        num507 *= 1.1f;
                    }
                }
                num506 = num507 / num506;
                NPC.velocity.X = num504 * num506;
                NPC.velocity.Y = num505 * num506;
            }

            else
            {
                if (NPC.ai[1] != 3f)
                {
                    return;
                }
                else
                {
                    NPC.EncourageDespawn(500);
                    NPC.velocity.Y += 0.1f;
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.95f;
                    }
                    NPC.velocity.X *= 0.95f;
                }
            }

        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Trophies are spawned with 1/10 chance
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.Furniture.DaveEJonesBossTrophy>(), 10));

            // All the Classic Mode drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
            LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());

            // Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<TellNoTales>(), 3));
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DaveEJonesMelee>(), 3));
            notExpertRule.OnSuccess(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<DaveEJonesBody>()));
            
            // Finally add the leading rule
            npcLoot.Add(notExpertRule);

            // Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<Items.Consumables.DaveEJonesBossBag>()));

            // ItemDropRule.MasterModeCommonDrop for the relic
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.Furniture.DaveEJonesBossRelic>()));

            // ItemDropRule.MasterModeDropOnAllPlayers for the pet
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<DaveEJonesPetItem>(), 4));
        }


        public override void BossLoot(ref int potionType)
        {
            potionType = ItemID.HealingPotion;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("DaveEJonesBossBody_Top").Type;
                int frontGoreType = Mod.Find<ModGore>("DaveEJonesBossBody_Bottom").Type;

                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);

                SoundEngine.PlaySound(ahoySound, NPC.Center);

                // This adds a screen shake (screenshake) similar to Deerclops
                PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
                Main.instance.CameraModifiers.Add(modifier);
            }
        }


        public override void OnKill()
        {
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedDaveEJonesBoss, -1);
        }
    }
    internal class DaveEJonesCannon : ModNPC
    {
		public static Asset<Texture2D> boneArm;

		public override void Load()
        {
            boneArm = ModContent.Request<Texture2D>("PiratePanic/Content/NPCs/DaveEJones/DaveEJonesBody_Arm_Bone_2");
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<DaveEJonesBody>();
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 52;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 22;
            NPC.lifeMax = 5000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * NPC.ai[0], NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++)
            {
                float num21 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector7.X;
                float num22 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector7.Y;
                float num23 = 0f;
                if (k == 0)
                {
                    num21 -= 200f * NPC.ai[0];
                    num22 += 130f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 92f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                else
                {
                    num21 -= 50f * NPC.ai[0];
                    num22 += 80f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 60f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                float rotation7 = (float)Math.Atan2(num22, num21) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(boneArm.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, boneArm.Width(), boneArm.Height()), color7, rotation7, new Vector2((float)boneArm.Width() * 0.5f, (float)boneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0)
                {
                    vector7.X += num21 * num23 / 2f;
                    vector7.Y += num22 * num23 / 2f;
                }
                else if (NPC.active)
                {
                    vector7.X += num21 * num23 - 16f;
                    vector7.Y += num22 * num23 - 6f;
                    int num24 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, 6, num21 * 0.02f, num22 * 0.02f, 0, default(Color), 2.5f);
                    Main.dust[num24].noGravity = true;
                }
            }
			return true;
        }

        public override void AI()
        {
            NPC.spriteDirection = -(int)NPC.ai[0];
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != ModContent.NPCType<DaveEJonesBody>())
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (NPC.ai[2] == 0f)
            {
                if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                {
                    NPC.EncourageDespawn(10);
                }
                if (Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
                {
                    NPC.localAI[0] += 2f;
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.07f;
                        if (NPC.velocity.Y > 6f)
                        {
                            NPC.velocity.Y = 6f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.07f;
                        if (NPC.velocity.Y < -6f)
                        {
                            NPC.velocity.Y = -6f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X -= 0.1f;
                        if (NPC.velocity.X > 8f)
                        {
                            NPC.velocity.X = 8f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X += 0.1f;
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -8f;
                        }
                    }
                }
                else
                {
                    NPC.ai[3] += 1f;
                    if (NPC.ai[3] >= 1100f)
                    {
                        NPC.localAI[0] = 0f;
                        NPC.ai[2] = 1f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y - 150f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.04f;
                        if (NPC.velocity.Y > 3f)
                        {
                            NPC.velocity.Y = 3f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 150f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.04f;
                        if (NPC.velocity.Y < -3f)
                        {
                            NPC.velocity.Y = -3f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + 200f)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X -= 0.2f;
                        if (NPC.velocity.X > 8f)
                        {
                            NPC.velocity.X = 8f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + 160f)
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X += 0.2f;
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -8f;
                        }
                    }
                }
                Vector2 vector66 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num545 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector66.X;
                float num546 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector66.Y;
                float num547 = (float)Math.Sqrt(num545 * num545 + num546 * num546);
                NPC.rotation = (float)Math.Atan2(num546, num545) + 1.57f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] > 140f)
                    {
                        NPC.localAI[0] = 0f;
                        float num548 = 12f;
                        int num549 = 0;
                        int num550 = 102;
                        num547 = num548 / num547;
                        num545 = (0f - num545) * num547;
                        num546 = (0f - num546) * num547;
                        num545 += (float)Main.rand.Next(-40, 41) * 0.01f;
                        num546 += (float)Main.rand.Next(-40, 41) * 0.01f;
                        vector66.X += num545 * 4f;
                        vector66.Y += num546 * 4f;
                        int num551 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector66.X, vector66.Y, num545, num546, num550, num549, 0f, Main.myPlayer);
                    }
                }
            }
            else
            {
                if (NPC.ai[2] != 1f)
                {
                    return;
                }
                NPC.ai[3] += 1f;
                if (NPC.ai[3] >= 300f)
                {
                    NPC.localAI[0] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
                Vector2 vector67 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num552 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector67.X;
                float num553 = Main.npc[(int)NPC.ai[1]].position.Y - vector67.Y;
                num553 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 80f - vector67.Y;
                float num554 = (float)Math.Sqrt(num552 * num552 + num553 * num553);
                num554 = 6f / num554;
                num552 *= num554;
                num553 *= num554;
                if (NPC.velocity.X > num552)
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    NPC.velocity.X -= 0.04f;
                }
                if (NPC.velocity.X < num552)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    NPC.velocity.X += 0.04f;
                }
                if (NPC.velocity.Y > num553)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.9f;
                    }
                    NPC.velocity.Y -= 0.08f;
                }
                if (NPC.velocity.Y < num553)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.9f;
                    }
                    NPC.velocity.Y += 0.08f;
                }
                NPC.TargetClosest();
                vector67 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                num552 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector67.X;
                num553 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector67.Y;
                num554 = (float)Math.Sqrt(num552 * num552 + num553 * num553);
                NPC.rotation = (float)Math.Atan2(num553, num552) - 1.57f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] > 40f)
                    {
                        NPC.localAI[0] = 0f;
                        float num555 = 10f;
                        int num556 = 0;
                        int num557 = 102;
                        num554 = num555 / num554;
                        num552 *= num554;
                        num553 *= num554;
                        num552 += (float)Main.rand.Next(-40, 41) * 0.01f;
                        num553 += (float)Main.rand.Next(-40, 41) * 0.01f;
                        vector67.X += num552 * 4f;
                        vector67.Y += num553 * 4f;
                        int num558 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector67.X, vector67.Y, num552, num553, num557, num556, 0f, Main.myPlayer);
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("DaveEJonesCannon_Top").Type;
                int frontGoreType = Mod.Find<ModGore>("DaveEJonesCannon_Bottom").Type;

                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
            }
        }
    }

    internal class DaveEJonesSaw : ModNPC
    {
        public static Asset<Texture2D> boneArm;

		public override void Load()
        {
            boneArm = ModContent.Request<Texture2D>("PiratePanic/Content/NPCs/DaveEJones/DaveEJonesBody_Arm_Bone_2");
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<DaveEJonesBody>();
            Main.npcFrameCount[Type] = 1;
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 52;
            NPC.aiStyle = -1;
            NPC.damage = 40;
            NPC.defense = 30;
            NPC.lifeMax = 700;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
        }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * NPC.ai[0], NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++)
            {
                float num21 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector7.X;
                float num22 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector7.Y;
                float num23 = 0f;
                if (k == 0)
                {
                    num21 -= 200f * NPC.ai[0];
                    num22 += 130f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 92f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                else
                {
                    num21 -= 50f * NPC.ai[0];
                    num22 += 80f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 60f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                float rotation7 = (float)Math.Atan2(num22, num21) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(boneArm.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, boneArm.Width(), boneArm.Height()), color7, rotation7, new Vector2((float)boneArm.Width() * 0.5f, (float)boneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0)
                {
                    vector7.X += num21 * num23 / 2f;
                    vector7.Y += num22 * num23 / 2f;
                }
                else if (NPC.active)
                {
                    vector7.X += num21 * num23 - 16f;
                    vector7.Y += num22 * num23 - 6f;
                    int num24 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, 6, num21 * 0.02f, num22 * 0.02f, 0, default(Color), 2.5f);
                    Main.dust[num24].noGravity = true;
                }
            }
			return true;
        }

        public override void AI()
        {
            Vector2 vector56 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num515 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector56.X;
            float num516 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector56.Y;
            float num517 = (float)Math.Sqrt(num515 * num515 + num516 * num516);
            if (NPC.ai[2] != 99f)
            {
                if (num517 > 800f)
                {
                    NPC.ai[2] = 99f;
                }
            }
            else if (num517 < 400f)
            {
                NPC.ai[2] = 0f;
            }
            NPC.spriteDirection = -(int)NPC.ai[0];
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != ModContent.NPCType<DaveEJonesBody>())
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (NPC.ai[2] == 99f)
            {
                if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.96f;
                    }
                    NPC.velocity.Y -= 0.1f;
                    if (NPC.velocity.Y > 8f)
                    {
                        NPC.velocity.Y = 8f;
                    }
                }
                else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.96f;
                    }
                    NPC.velocity.Y += 0.1f;
                    if (NPC.velocity.Y < -8f)
                    {
                        NPC.velocity.Y = -8f;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.96f;
                    }
                    NPC.velocity.X -= 0.5f;
                    if (NPC.velocity.X > 12f)
                    {
                        NPC.velocity.X = 12f;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.96f;
                    }
                    NPC.velocity.X += 0.5f;
                    if (NPC.velocity.X < -12f)
                    {
                        NPC.velocity.X = -12f;
                    }
                }
            }
            else if (NPC.ai[2] == 0f || NPC.ai[2] == 3f)
            {
                if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                {
                    NPC.EncourageDespawn(10);
                }
                if (Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
                {
                    NPC.TargetClosest();
                    if (Main.player[NPC.target].dead)
                    {
                        NPC.velocity.Y += 0.1f;
                        if (NPC.velocity.Y > 16f)
                        {
                            NPC.velocity.Y = 16f;
                        }
                    }
                    else
                    {
                        Vector2 vector57 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                        float num518 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector57.X;
                        float num519 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector57.Y;
                        float num520 = (float)Math.Sqrt(num518 * num518 + num519 * num519);
                        num520 = 7f / num520;
                        num518 *= num520;
                        num519 *= num520;
                        NPC.rotation = (float)Math.Atan2(num519, num518) - 1.57f;
                        if (NPC.velocity.X > num518)
                        {
                            if (NPC.velocity.X > 0f)
                            {
                                NPC.velocity.X *= 0.97f;
                            }
                            NPC.velocity.X -= 0.05f;
                        }
                        if (NPC.velocity.X < num518)
                        {
                            if (NPC.velocity.X < 0f)
                            {
                                NPC.velocity.X *= 0.97f;
                            }
                            NPC.velocity.X += 0.05f;
                        }
                        if (NPC.velocity.Y > num519)
                        {
                            if (NPC.velocity.Y > 0f)
                            {
                                NPC.velocity.Y *= 0.97f;
                            }
                            NPC.velocity.Y -= 0.05f;
                        }
                        if (NPC.velocity.Y < num519)
                        {
                            if (NPC.velocity.Y < 0f)
                            {
                                NPC.velocity.Y *= 0.97f;
                            }
                            NPC.velocity.Y += 0.05f;
                        }
                    }
                    NPC.ai[3] += 1f;
                    if (NPC.ai[3] >= 600f)
                    {
                        NPC.ai[2] = 0f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                }
                else
                {
                    NPC.ai[3] += 1f;
                    if (NPC.ai[3] >= 300f)
                    {
                        NPC.ai[2] += 1f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + 320f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.04f;
                        if (NPC.velocity.Y > 3f)
                        {
                            NPC.velocity.Y = 3f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + 260f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.04f;
                        if (NPC.velocity.Y < -3f)
                        {
                            NPC.velocity.Y = -3f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X -= 0.3f;
                        if (NPC.velocity.X > 12f)
                        {
                            NPC.velocity.X = 12f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 250f)
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X += 0.3f;
                        if (NPC.velocity.X < -12f)
                        {
                            NPC.velocity.X = -12f;
                        }
                    }
                }
                Vector2 vector58 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num521 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector58.X;
                float num522 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector58.Y;
                float num523 = (float)Math.Sqrt(num521 * num521 + num522 * num522);
                NPC.rotation = (float)Math.Atan2(num522, num521) + 1.57f;
            }
            else if (NPC.ai[2] == 1f)
            {
                Vector2 vector59 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num524 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector59.X;
                float num525 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector59.Y;
                float num526 = (float)Math.Sqrt(num524 * num524 + num525 * num525);
                NPC.rotation = (float)Math.Atan2(num525, num524) + 1.57f;
                NPC.velocity.X *= 0.95f;
                NPC.velocity.Y -= 0.1f;
                if (NPC.velocity.Y < -8f)
                {
                    NPC.velocity.Y = -8f;
                }
                if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 200f)
                {
                    NPC.TargetClosest();
                    NPC.ai[2] = 2f;
                    vector59 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    num524 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector59.X;
                    num525 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector59.Y;
                    num526 = (float)Math.Sqrt(num524 * num524 + num525 * num525);
                    num526 = 22f / num526;
                    NPC.velocity.X = num524 * num526;
                    NPC.velocity.Y = num525 * num526;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[2] == 2f)
            {
                if (NPC.position.Y > Main.player[NPC.target].position.Y || NPC.velocity.Y < 0f)
                {
                    NPC.ai[2] = 3f;
                }
            }
            else if (NPC.ai[2] == 4f)
            {
                NPC.TargetClosest();
                Vector2 vector60 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num527 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector60.X;
                float num528 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector60.Y;
                float num529 = (float)Math.Sqrt(num527 * num527 + num528 * num528);
                num529 = 7f / num529;
                num527 *= num529;
                num528 *= num529;
                if (NPC.velocity.X > num527)
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.97f;
                    }
                    NPC.velocity.X -= 0.05f;
                }
                if (NPC.velocity.X < num527)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.97f;
                    }
                    NPC.velocity.X += 0.05f;
                }
                if (NPC.velocity.Y > num528)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.97f;
                    }
                    NPC.velocity.Y -= 0.05f;
                }
                if (NPC.velocity.Y < num528)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.97f;
                    }
                    NPC.velocity.Y += 0.05f;
                }
                NPC.ai[3] += 1f;
                if (NPC.ai[3] >= 600f)
                {
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
                vector60 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                num527 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector60.X;
                num528 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector60.Y;
                num529 = (float)Math.Sqrt(num527 * num527 + num528 * num528);
                NPC.rotation = (float)Math.Atan2(num528, num527) + 1.57f;
            }
            else if (NPC.ai[2] == 5f && ((NPC.velocity.X > 0f && NPC.position.X + (float)(NPC.width / 2) > Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2)) || (NPC.velocity.X < 0f && NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2))))
            {
                NPC.ai[2] = 0f;
            }

        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("DaveEJonesSaw_Top").Type;
                int frontGoreType = Mod.Find<ModGore>("DaveEJonesSaw_Bottom").Type;

                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
            }
        }

    }

    internal class DaveEJonesVice : ModNPC
    {
        public static Asset<Texture2D> boneArm;

		public override void Load()
        {
            boneArm = ModContent.Request<Texture2D>("PiratePanic/Content/NPCs/DaveEJones/DaveEJonesBody_Arm_Bone_2");
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<DaveEJonesBody>();
            Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 52;
            NPC.aiStyle = -1;
            NPC.damage = 45;
            NPC.defense = 31;
            NPC.lifeMax = 6500;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
            AnimationType = NPCID.PrimeVice;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * NPC.ai[0], NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++)
            {
                float num21 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector7.X;
                float num22 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector7.Y;
                float num23 = 0f;
                if (k == 0)
                {
                    num21 -= 200f * NPC.ai[0];
                    num22 += 130f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 92f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                else
                {
                    num21 -= 50f * NPC.ai[0];
                    num22 += 80f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 60f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                float rotation7 = (float)Math.Atan2(num22, num21) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(boneArm.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, boneArm.Width(), boneArm.Height()), color7, rotation7, new Vector2((float)boneArm.Width() * 0.5f, (float)boneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0)
                {
                    vector7.X += num21 * num23 / 2f;
                    vector7.Y += num22 * num23 / 2f;
                }
                else if (NPC.active)
                {
                    vector7.X += num21 * num23 - 16f;
                    vector7.Y += num22 * num23 - 6f;
                    int num24 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, 6, num21 * 0.02f, num22 * 0.02f, 0, default(Color), 2.5f);
                    Main.dust[num24].noGravity = true;
                }
            }
			return true;
        }

        public override void AI()
        {
            NPC.spriteDirection = -(int)NPC.ai[0];
            Vector2 vector61 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
            float num530 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector61.X;
            float num531 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector61.Y;
            float num532 = (float)Math.Sqrt(num530 * num530 + num531 * num531);
            if (NPC.ai[2] != 99f)
            {
                if (num532 > 800f)
                {
                    NPC.ai[2] = 99f;
                }
            }
            else if (num532 < 400f)
            {
                NPC.ai[2] = 0f;
            }
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != ModContent.NPCType<DaveEJonesBody>())
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (NPC.ai[2] == 99f)
            {
                if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.96f;
                    }
                    NPC.velocity.Y -= 0.1f;
                    if (NPC.velocity.Y > 8f)
                    {
                        NPC.velocity.Y = 8f;
                    }
                }
                else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.96f;
                    }
                    NPC.velocity.Y += 0.1f;
                    if (NPC.velocity.Y < -8f)
                    {
                        NPC.velocity.Y = -8f;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.96f;
                    }
                    NPC.velocity.X -= 0.5f;
                    if (NPC.velocity.X > 12f)
                    {
                        NPC.velocity.X = 12f;
                    }
                }
                if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.96f;
                    }
                    NPC.velocity.X += 0.5f;
                    if (NPC.velocity.X < -12f)
                    {
                        NPC.velocity.X = -12f;
                    }
                }
            }
            else if (NPC.ai[2] == 0f || NPC.ai[2] == 3f)
            {
                if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                {
                    NPC.EncourageDespawn(10);
                }
                if (Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
                {
                    NPC.TargetClosest();
                    NPC.TargetClosest();
                    if (Main.player[NPC.target].dead)
                    {
                        NPC.velocity.Y += 0.1f;
                        if (NPC.velocity.Y > 16f)
                        {
                            NPC.velocity.Y = 16f;
                        }
                    }
                    else
                    {
                        Vector2 vector62 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                        float num533 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector62.X;
                        float num534 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector62.Y;
                        float num535 = (float)Math.Sqrt(num533 * num533 + num534 * num534);
                        num535 = 12f / num535;
                        num533 *= num535;
                        num534 *= num535;
                        NPC.rotation = (float)Math.Atan2(num534, num533) - 1.57f;
                        if (Math.Abs(NPC.velocity.X) + Math.Abs(NPC.velocity.Y) < 2f)
                        {
                            NPC.rotation = (float)Math.Atan2(num534, num533) - 1.57f;
                            NPC.velocity.X = num533;
                            NPC.velocity.Y = num534;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            NPC.velocity *= 0.97f;
                        }
                        NPC.ai[3] += 1f;
                        if (NPC.ai[3] >= 600f)
                        {
                            NPC.ai[2] = 0f;
                            NPC.ai[3] = 0f;
                            NPC.netUpdate = true;
                        }
                    }
                }
                else
                {
                    NPC.ai[3] += 1f;
                    if (NPC.ai[3] >= 600f)
                    {
                        NPC.ai[2] += 1f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y + 300f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.1f;
                        if (NPC.velocity.Y > 3f)
                        {
                            NPC.velocity.Y = 3f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y + 230f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.1f;
                        if (NPC.velocity.Y < -3f)
                        {
                            NPC.velocity.Y = -3f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + 250f)
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.94f;
                        }
                        NPC.velocity.X -= 0.3f;
                        if (NPC.velocity.X > 9f)
                        {
                            NPC.velocity.X = 9f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2))
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.94f;
                        }
                        NPC.velocity.X += 0.2f;
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -8f;
                        }
                    }
                }
                Vector2 vector63 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num536 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector63.X;
                float num537 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector63.Y;
                float num538 = (float)Math.Sqrt(num536 * num536 + num537 * num537);
                NPC.rotation = (float)Math.Atan2(num537, num536) + 1.57f;
            }
            else if (NPC.ai[2] == 1f)
            {
                if (NPC.velocity.Y > 0f)
                {
                    NPC.velocity.Y *= 0.9f;
                }
                Vector2 vector64 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num539 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 280f * NPC.ai[0] - vector64.X;
                float num540 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector64.Y;
                float num541 = (float)Math.Sqrt(num539 * num539 + num540 * num540);
                NPC.rotation = (float)Math.Atan2(num540, num539) + 1.57f;
                NPC.velocity.X = (NPC.velocity.X * 5f + Main.npc[(int)NPC.ai[1]].velocity.X) / 6f;
                NPC.velocity.X += 0.5f;
                NPC.velocity.Y -= 0.5f;
                if (NPC.velocity.Y < -9f)
                {
                    NPC.velocity.Y = -9f;
                }
                if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 280f)
                {
                    NPC.TargetClosest();
                    NPC.ai[2] = 2f;
                    vector64 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    num539 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector64.X;
                    num540 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector64.Y;
                    num541 = (float)Math.Sqrt(num539 * num539 + num540 * num540);
                    num541 = 20f / num541;
                    NPC.velocity.X = num539 * num541;
                    NPC.velocity.Y = num540 * num541;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[2] == 2f)
            {
                if (NPC.position.Y > Main.player[NPC.target].position.Y || NPC.velocity.Y < 0f)
                {
                    if (NPC.ai[3] >= 4f)
                    {
                        NPC.ai[2] = 3f;
                        NPC.ai[3] = 0f;
                    }
                    else
                    {
                        NPC.ai[2] = 1f;
                        NPC.ai[3] += 1f;
                    }
                }
            }
            else if (NPC.ai[2] == 4f)
            {
                Vector2 vector65 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num542 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 200f * NPC.ai[0] - vector65.X;
                float num543 = Main.npc[(int)NPC.ai[1]].position.Y + 230f - vector65.Y;
                float num544 = (float)Math.Sqrt(num542 * num542 + num543 * num543);
                NPC.rotation = (float)Math.Atan2(num543, num542) + 1.57f;
                NPC.velocity.Y = (NPC.velocity.Y * 5f + Main.npc[(int)NPC.ai[1]].velocity.Y) / 6f;
                NPC.velocity.X += 0.5f;
                if (NPC.velocity.X > 12f)
                {
                    NPC.velocity.X = 12f;
                }
                if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 500f || NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) + 500f)
                {
                    NPC.TargetClosest();
                    NPC.ai[2] = 5f;
                    vector65 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                    num542 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector65.X;
                    num543 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector65.Y;
                    num544 = (float)Math.Sqrt(num542 * num542 + num543 * num543);
                    num544 = 17f / num544;
                    NPC.velocity.X = num542 * num544;
                    NPC.velocity.Y = num543 * num544;
                    NPC.netUpdate = true;
                }
            }
            else if (NPC.ai[2] == 5f && NPC.position.X + (float)(NPC.width / 2) < Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - 100f)
            {
                if (NPC.ai[3] >= 4f)
                {
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                }
                else
                {
                    NPC.ai[2] = 4f;
                    NPC.ai[3] += 1f;
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("DaveEJonesVice").Type;

                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
            }
        }
    }

    internal class DaveEJonesLaser : ModNPC
    {
        public static Asset<Texture2D> boneArm;

		public override void Load()
        {
            boneArm = ModContent.Request<Texture2D>("PiratePanic/Content/NPCs/DaveEJones/DaveEJonesBody_Arm_Bone_2");
        }
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<DaveEJonesBody>();
        }

        public override void SetDefaults()
        {
            NPC.width = 52;
            NPC.height = 52;
            NPC.aiStyle = -1;
            NPC.damage = 25;
            NPC.defense = 20;
            NPC.lifeMax = 4000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.knockBackResist = 0f;
            NPC.netAlways = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
             Vector2 vector7 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f - 5f * NPC.ai[0], NPC.position.Y + 20f);
            for (int k = 0; k < 2; k++)
            {
                float num21 = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - vector7.X;
                float num22 = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - vector7.Y;
                float num23 = 0f;
                if (k == 0)
                {
                    num21 -= 200f * NPC.ai[0];
                    num22 += 130f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 92f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                else
                {
                    num21 -= 50f * NPC.ai[0];
                    num22 += 80f;
                    num23 = (float)Math.Sqrt(num21 * num21 + num22 * num22);
                    num23 = 60f / num23;
                    vector7.X += num21 * num23;
                    vector7.Y += num22 * num23;
                }
                float rotation7 = (float)Math.Atan2(num22, num21) - 1.57f;
                Color color7 = Lighting.GetColor((int)vector7.X / 16, (int)(vector7.Y / 16f));
                spriteBatch.Draw(boneArm.Value, new Vector2(vector7.X - screenPos.X, vector7.Y - screenPos.Y), new Rectangle(0, 0, boneArm.Width(), boneArm.Height()), color7, rotation7, new Vector2((float)boneArm.Width() * 0.5f, (float)boneArm.Height() * 0.5f), 1f, SpriteEffects.None, 0f);
                if (k == 0)
                {
                    vector7.X += num21 * num23 / 2f;
                    vector7.Y += num22 * num23 / 2f;
                }
                else if (NPC.active)
                {
                    vector7.X += num21 * num23 - 16f;
                    vector7.Y += num22 * num23 - 6f;
                    int num24 = Dust.NewDust(new Vector2(vector7.X, vector7.Y), 30, 10, 6, num21 * 0.02f, num22 * 0.02f, 0, default(Color), 2.5f);
                    Main.dust[num24].noGravity = true;
                }
            }
			return true;
        }

        public override void AI()
        {
            NPC.spriteDirection = -(int)NPC.ai[0];
            if (!Main.npc[(int)NPC.ai[1]].active || Main.npc[(int)NPC.ai[1]].type != ModContent.NPCType<DaveEJonesBody>())
            {
                NPC.ai[2] += 10f;
                if (NPC.ai[2] > 50f || Main.netMode != NetmodeID.Server)
                {
                    NPC.life = -1;
                    NPC.HitEffect();
                    NPC.active = false;
                }
            }
            if (NPC.ai[2] == 0f || NPC.ai[2] == 3f)
            {
                if (Main.npc[(int)NPC.ai[1]].ai[1] == 3f)
                {
                    NPC.EncourageDespawn(10);
                }
                if (Main.npc[(int)NPC.ai[1]].ai[1] != 0f)
                {
                    NPC.localAI[0] += 3f;
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.07f;
                        if (NPC.velocity.Y > 6f)
                        {
                            NPC.velocity.Y = 6f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.07f;
                        if (NPC.velocity.Y < -6f)
                        {
                            NPC.velocity.Y = -6f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X -= 0.1f;
                        if (NPC.velocity.X > 8f)
                        {
                            NPC.velocity.X = 8f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 120f * NPC.ai[0])
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X += 0.1f;
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -8f;
                        }
                    }
                }
                else
                {
                    NPC.ai[3] += 1f;
                    if (NPC.ai[3] >= 800f)
                    {
                        NPC.ai[2] += 1f;
                        NPC.ai[3] = 0f;
                        NPC.netUpdate = true;
                    }
                    if (NPC.position.Y > Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y > 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y -= 0.1f;
                        if (NPC.velocity.Y > 3f)
                        {
                            NPC.velocity.Y = 3f;
                        }
                    }
                    else if (NPC.position.Y < Main.npc[(int)NPC.ai[1]].position.Y - 100f)
                    {
                        if (NPC.velocity.Y < 0f)
                        {
                            NPC.velocity.Y *= 0.96f;
                        }
                        NPC.velocity.Y += 0.1f;
                        if (NPC.velocity.Y < -3f)
                        {
                            NPC.velocity.Y = -3f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) > Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 180f * NPC.ai[0])
                    {
                        if (NPC.velocity.X > 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X -= 0.14f;
                        if (NPC.velocity.X > 8f)
                        {
                            NPC.velocity.X = 8f;
                        }
                    }
                    if (NPC.position.X + (float)(NPC.width / 2) < Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - 180f * NPC.ai[0])
                    {
                        if (NPC.velocity.X < 0f)
                        {
                            NPC.velocity.X *= 0.96f;
                        }
                        NPC.velocity.X += 0.14f;
                        if (NPC.velocity.X < -8f)
                        {
                            NPC.velocity.X = -8f;
                        }
                    }
                }
                NPC.TargetClosest();
                Vector2 vector68 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num559 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector68.X;
                float num560 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector68.Y;
                float num561 = (float)Math.Sqrt(num559 * num559 + num560 * num560);
                NPC.rotation = (float)Math.Atan2(num560, num559) - 1.57f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] > 200f)
                    {
                        NPC.localAI[0] = 0f;
                        float num562 = 8f;
                        int num563 = 19;
                        int num564 = ModContent.GetInstance<BossLaserProjectile>().Type;
                        num561 = num562 / num561;
                        num559 *= num561;
                        num560 *= num561;
                        num559 += (float)Main.rand.Next(-40, 41) * 0.05f;
                        num560 += (float)Main.rand.Next(-40, 41) * 0.05f;
                        vector68.X += num559 * 8f;
                        vector68.Y += num560 * 8f;
                        int num565 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector68.X, vector68.Y, num559, num560, num564, num563, 0f, Main.myPlayer);
                    }
                }
            }
            else
            {
                if (NPC.ai[2] != 1f)
                {
                    return;
                }
                NPC.ai[3] += 1f;
                if (NPC.ai[3] >= 200f)
                {
                    NPC.localAI[0] = 0f;
                    NPC.ai[2] = 0f;
                    NPC.ai[3] = 0f;
                    NPC.netUpdate = true;
                }
                Vector2 vector69 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float num566 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - 350f - vector69.X;
                float num567 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - 20f - vector69.Y;
                float num568 = (float)Math.Sqrt(num566 * num566 + num567 * num567);
                num568 = 7f / num568;
                num566 *= num568;
                num567 *= num568;
                if (NPC.velocity.X > num566)
                {
                    if (NPC.velocity.X > 0f)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    NPC.velocity.X -= 0.1f;
                }
                if (NPC.velocity.X < num566)
                {
                    if (NPC.velocity.X < 0f)
                    {
                        NPC.velocity.X *= 0.9f;
                    }
                    NPC.velocity.X += 0.1f;
                }
                if (NPC.velocity.Y > num567)
                {
                    if (NPC.velocity.Y > 0f)
                    {
                        NPC.velocity.Y *= 0.9f;
                    }
                    NPC.velocity.Y -= 0.03f;
                }
                if (NPC.velocity.Y < num567)
                {
                    if (NPC.velocity.Y < 0f)
                    {
                        NPC.velocity.Y *= 0.9f;
                    }
                    NPC.velocity.Y += 0.03f;
                }
                NPC.TargetClosest();
                vector69 = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                num566 = Main.player[NPC.target].position.X + (float)(Main.player[NPC.target].width / 2) - vector69.X;
                num567 = Main.player[NPC.target].position.Y + (float)(Main.player[NPC.target].height / 2) - vector69.Y;
                num568 = (float)Math.Sqrt(num566 * num566 + num567 * num567);
                NPC.rotation = (float)Math.Atan2(num567, num566) - 1.57f;
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.localAI[0] += 1f;
                    if (NPC.localAI[0] > 80f)
                    {
                        NPC.localAI[0] = 0f;
                        float num569 = 10f;
                        int num570 = 25;
                        int num571 = 100;
                        num568 = num569 / num568;
                        num566 *= num568;
                        num567 *= num568;
                        num566 += (float)Main.rand.Next(-40, 41) * 0.05f;
                        num567 += (float)Main.rand.Next(-40, 41) * 0.05f;
                        vector69.X += num566 * 8f;
                        vector69.Y += num567 * 8f;
                        int num572 = Projectile.NewProjectile(NPC.GetSource_FromAI(), vector69.X, vector69.Y, num566, num567, num571, num570, 0f, Main.myPlayer);
                    }
                }
            }
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // If the NPC dies, spawn gore and play a sound
            if (Main.netMode == NetmodeID.Server)
            {
                // We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
                return;
            }

            if (NPC.life <= 0)
            {
                // These gores work by simply existing as a texture inside any folder which path contains "Gores/"
                int backGoreType = Mod.Find<ModGore>("DaveEJonesLaser_Top").Type;
                int frontGoreType = Mod.Find<ModGore>("DaveEJonesLaser_Bottom").Type;

                var entitySource = NPC.GetSource_Death();

                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), backGoreType);
                Gore.NewGore(entitySource, NPC.position, new Vector2(Main.rand.Next(-6, 7), Main.rand.Next(-6, 7)), frontGoreType);
            }
        }
    }
}