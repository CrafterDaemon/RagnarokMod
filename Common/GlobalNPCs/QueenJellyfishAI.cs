using Microsoft.Xna.Framework;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using ThoriumMod;
using ThoriumMod.NPCs;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.Projectiles;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.Sounds;
using ThoriumMod.NPCs.BossQueenJellyfish;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class QueenJellyfishAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) 
		{
			return npc.type == thorium.Find<ModNPC>("QueenJellyfish").Type;
		}
		public static int whirlPoolTimer;
		public static int bubbleTimer;
		public static int spawnType = 1;
		public static int spawnFrequency;
		public static float speed;
		public static int attackFrequency;
		public static int diverOffset;
		public static int counting;
		public static int counter;
		
		public override bool PreAI(NPC npc) 
		{
			if(CalamityGamemodeCheck.isBossrush) 
			{	
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().jelly == ThoriumBossRework_selection_mode.Ragnarok)) 
				{
								return true;
				}
				NPCHelper.BatAI(npc, thorium.Find<ModNPC>("QueenJellyfish").AIType, 3.5f);
						Player player = Main.player[npc.target];
						if (!player.active || player.dead)
						{
							whirlPoolTimer = 0;
							NPC npc1 = npc;
							npc1.velocity.Y = npc1.velocity.Y + 0.1f;
							if (npc.timeLeft > 20)
							{
								npc.timeLeft = 20;
							}
							return false;
						}
						if (player.position.Y < npc.position.Y + 300f) // Increases distance to player a bit
						{
							NPC npc2 = npc;
							npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1f : 0.1f);
						}
						if (player.position.Y > npc.position.Y + 300f)
						{
							NPC npc3 = npc;
							npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1f : 0.1f);
						}
						npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
						if (whirlPoolTimer <= 0)
						{
							if (npc.rotation <= 0.35f && npc.rotation >= -0.35f)
							{
								npc.rotation += 0.01f * (float)npc.direction;
							}
							if (npc.rotation > 0.35f)
							{
								npc.rotation = 0.34f;
							}
							if (npc.rotation < -0.35f)
							{
								npc.rotation = -0.34f;
							}
						}
						else
						{
							npc.rotation = 0f;
						}
						npc.ai[0] += 1f;
						npc.ai[1] += 1f;
						npc.ai[2] += 1f;
						npc.ai[3] += 1f;
						bubbleTimer++;
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						if (whirlPoolTimer < 90) // take longer to track target
						{
							if (spawnType > 0)
							{
								if (npc.ai[0] >= 0f)
								{
									int num = 0;
									for (int i = 0; i < Main.maxNPCs; i++)
									{
										NPC npc4 = Main.npc[i];
										if (npc4.active && npc4.type == ModContent.NPCType<ZealousJellyfish>())
										{
											num++;
										}
									}
									if (num < 3 && Main.netMode != 1)
									{
										int num2 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<ZealousJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
										if (num2 < Main.maxNPCs && Main.netMode == 2)
										{
											NetMessage.SendData(23, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
										}
									}
									npc.ai[0] = (float)(-300 + spawnFrequency);
								}
								if (spawnType > 1)
								{
									if (npc.ai[1] >= 0f)
									{
										int num3 = 0;
										for (int j = 0; j < Main.maxNPCs; j++)
										{
											NPC npc5 = Main.npc[j];
											if (npc5.active && npc5.type == ModContent.NPCType<SpittingJellyfish>())
											{
												num3++;
											}
										}
										if (num3 < 3 && Main.netMode != 1)
										{
											int num4 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<SpittingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num4 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num4, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[1] = (float)(-600 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
									if (spawnType > 2 && npc.ai[2] >= 0f)
									{
										int num5 = 0;
										for (int k = 0; k < Main.maxNPCs; k++)
										{
											NPC npc6 = Main.npc[k];
											if (npc6.active && npc6.type == ModContent.NPCType<DistractingJellyfish>())
											{
												num5++;
											}
										}
										if (num5 < 3 && Main.netMode != 1)
										{
											int num6 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<DistractingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num6 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num6, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[2] = (float)(-180 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
								}
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.8)
							{
								speed = 18f; // speed up to 18
								spawnType = 1;
								attackFrequency = 0; 
								spawnFrequency = 20;  // boosted to 20
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.6 && (double)npc.life < (double)npc.lifeMax * 0.8)
							{
								speed = 20f; // speed up to 20
								spawnType = 2;
								attackFrequency = 140; // boosted to 140
 								spawnFrequency = 40;  // boosted to 40
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.4 && (double)npc.life < (double)npc.lifeMax * 0.6)
							{
								speed = 22f;   //speed up to 22
								spawnType = 3;    
								attackFrequency = 180; // boosted to 180
								spawnFrequency = 60;   // boosted to 60
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.4)
							{
								speed = 25f; // speed up to 25
								spawnType = 3;
								attackFrequency = 240; // Boosted to 240
								spawnFrequency = 70; // Boosted to 70
							}  // Does not care about the player standing on the ground anymore
							if ((double)npc.life < (double)npc.lifeMax * 0.6 && !player.dead && bubbleTimer >= 0 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
							{
								if (Main.netMode != 1)
								{
									Vector2 center = npc.Center;
									Vector2 vector = player.DirectionFrom(center);
									Projectile.NewProjectile(source_FromAI, center, vector * speed, ModContent.ProjectileType<BubbleBomb>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
								bubbleTimer = -300 + attackFrequency;
							}
						}
						else
						{
							bubbleTimer = -300 + attackFrequency;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4)
						{
							whirlPoolTimer++;
							if (whirlPoolTimer > 0 && whirlPoolTimer < 89)
							{
								if (npc.velocity.X < 40f && npc.velocity.X > -40f) 
								{
									if (player.position.X > npc.position.X + 50 || player.position.X < npc.position.X -50) 
									{
										npc.velocity.X *= 1.05f;
									}
									else 
									{
										npc.velocity.X = player.velocity.X * 1.1f; // Just is slightly faster if near the player
									}
								}
								for (int l = 0; l < 5; l++)
								{
									int num7 = Dust.NewDust(npc.position, npc.width, npc.height + 550, 29, 0f, -8f, 100, default(Color), 3f);
									Main.dust[num7].noGravity = true;
								}
							}
							if (whirlPoolTimer > 90)
							{
								npc.defense = 7 * npc.defDefense; // boostes defense by alot
								npc.velocity.X = 0f; 
								npc.velocity.Y = 0f;
							}
							else
							{
								npc.defense = npc.defDefense;
							}
							if (Main.netMode != 1)
							{
								int num8 = 111;
								int num9 = 12;
								if (whirlPoolTimer <= num8 && whirlPoolTimer > num8 - 2 * num9)
								{
									int num10 = ModContent.ProjectileType<QueenTorrent>();
									for (int m = 0; m < num9; m++)
									{
										if (whirlPoolTimer == num8 - 2 * m)
										{
											int num11 = 100 + 40 * m;
											float num12;
											if (m >= 9)
											{
												num12 = 3f;
											}
											else if (m >= 7)
											{
												num12 = 2f;
											}
											else if (m >= 5)
											{
												num12 = 1f;
											}
											else
											{
												num12 = 0f;
											}
											Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + (float)num11, 0f, 0f, num10, 25, 0f, Main.myPlayer, num12, 0f, 0f);
										}
									}
								}
							}
							if (whirlPoolTimer == 540 && Main.netMode != 2)
							{
								Vector2 vector2 = npc.position + new Vector2((float)Main.rand.Next(npc.width - 8), (float)Main.rand.Next(npc.height / 2));
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore1").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore2").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore3").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore4").Type, 1f);
							}
							if (whirlPoolTimer > 600)
							{
								whirlPoolTimer = -600;
							}
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4 && Main.expertMode && whirlPoolTimer > 60)
						{
							if (npc.localAI[0] == 0f)
							{
								if (Main.netMode != 1)
								{
									for (int n = 0; n < 6; n++)
									{
										if (Projectile.NewProjectile(new TentacleEntitySource(npc, n, null), npc.Center, Vector2.Zero, ModContent.ProjectileType<QueenJellyfishArm>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f) == 1000)
										{
											npc.active = false;
											return false;
										}
									}
								}
								npc.localAI[0] = 1f;
							}
							return true;
						}
						npc.rotation /= 5f;
						return false;
			}
			else if (CalamityGamemodeCheck.isDeath) 
			{
				// Checks if this boss ai should be reworked
						if(!(ModContent.GetInstance<BossConfig>().jelly == ThoriumBossRework_selection_mode.Ragnarok)) 
						{
								return true;
						}
						
						NPCHelper.BatAI(npc, thorium.Find<ModNPC>("QueenJellyfish").AIType, 1.6f); // Significant speed up
						Player player = Main.player[npc.target];
						if (!player.active || player.dead)
						{
							whirlPoolTimer = 0;
							NPC npc1 = npc;
							npc1.velocity.Y = npc1.velocity.Y + 0.1f;
							if (npc.timeLeft > 20)
							{
								npc.timeLeft = 20;
							}
							return false;
						}
						if (player.position.Y < npc.position.Y + 225f)
						{
							NPC npc2 = npc;
							npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.6f : 0.16f);
						}
						if (player.position.Y > npc.position.Y + 225f)
						{
							NPC npc3 = npc;
							npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.6f : 0.16f);
						}
						npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
						if (whirlPoolTimer <= 0)
						{
							if (npc.rotation <= 0.35f && npc.rotation >= -0.35f)
							{
								npc.rotation += 0.01f * (float)npc.direction;
							}
							if (npc.rotation > 0.35f)
							{
								npc.rotation = 0.34f;
							}
							if (npc.rotation < -0.35f)
							{
								npc.rotation = -0.34f;
							}
						}
						else
						{
							npc.rotation = 0f;
						}
						npc.ai[0] += 1f;
						npc.ai[1] += 1f;
						npc.ai[2] += 1f;
						npc.ai[3] += 1f;
						bubbleTimer++;
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						if (whirlPoolTimer < 60)
						{
							if (spawnType > 0)
							{
								if (npc.ai[0] >= 0f)
								{
									int num = 0;
									for (int i = 0; i < Main.maxNPCs; i++)
									{
										NPC npc4 = Main.npc[i];
										if (npc4.active && npc4.type == ModContent.NPCType<ZealousJellyfish>())
										{
											num++;
										}
									}
									if (num < 4 && Main.netMode != 1) // Can spawn up to 4 instead
									{
										int num2 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<ZealousJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
										if (num2 < Main.maxNPCs && Main.netMode == 2)
										{
											NetMessage.SendData(23, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
										}
									}
									npc.ai[0] = (float)(-300 + spawnFrequency);
								}
								if (spawnType > 1)
								{
									if (npc.ai[1] >= 0f)
									{
										int num3 = 0;
										for (int j = 0; j < Main.maxNPCs; j++)
										{
											NPC npc5 = Main.npc[j];
											if (npc5.active && npc5.type == ModContent.NPCType<SpittingJellyfish>())
											{
												num3++;
											}
										}
										if (num3 < 4 && Main.netMode != 1) //Can spawn up to 4 instead
										{
											int num4 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<SpittingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num4 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num4, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[1] = (float)(-600 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
									if (spawnType > 2 && npc.ai[2] >= 0f)
									{
										int num5 = 0;
										for (int k = 0; k < Main.maxNPCs; k++)
										{
											NPC npc6 = Main.npc[k];
											if (npc6.active && npc6.type == ModContent.NPCType<DistractingJellyfish>())
											{
												num5++;
											}
										}
										if (num5 < 4 && Main.netMode != 1)    //Can spawn up to 4 instead
										{
											int num6 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<DistractingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num6 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num6, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[2] = (float)(-180 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
								}
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.8) // Speeds up bubble bombs
							{
								speed = 7f;
								spawnType = 1;
								attackFrequency = 0;
								spawnFrequency = 0;
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.6 && (double)npc.life < (double)npc.lifeMax * 0.8) // Spawns Spitting Jellyfish 
							{
								speed = 8f;
								spawnType = 2;
								attackFrequency = 100;  // boosts attack frequency to 100
								spawnFrequency = 30;
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.4 && (double)npc.life < (double)npc.lifeMax * 0.6)
							{
								speed = 9f;
								spawnType = 3;
								attackFrequency = 170;  // Boosts attack frequency to 170
								spawnFrequency = 45;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.4)
							{
								speed = 11f;
								spawnType = 3;
								attackFrequency = 225; // Boosts attack frequency to 225
								spawnFrequency = 60;
							}  
							if ((double)npc.life < (double)npc.lifeMax * 0.6 && !player.dead && bubbleTimer >= 0 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
							{
								if (Main.netMode != 1)
								{
									Vector2 center = npc.Center;
									Vector2 vector = player.DirectionFrom(center);
									Projectile.NewProjectile(source_FromAI, center, vector * speed, ModContent.ProjectileType<BubbleBomb>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
								bubbleTimer = -300 + attackFrequency;
							}
						}
						else
						{
							bubbleTimer = -300 + attackFrequency;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4)
						{
							whirlPoolTimer++;
							if (whirlPoolTimer > 0 && whirlPoolTimer < 59)
							{
								for (int l = 0; l < 5; l++)
								{
									int num7 = Dust.NewDust(npc.position, npc.width, npc.height + 550, 29, 0f, -8f, 100, default(Color), 3f);
									Main.dust[num7].noGravity = true;
								}
							}
							if (whirlPoolTimer > 60) // Increased defense 
							{
								npc.defense = 6 * npc.defDefense;
								npc.velocity.X = 0f;
								npc.velocity.Y = 0f;
							}
							else
							{
								npc.defense = npc.defDefense;
							}
							if (Main.netMode != 1)
							{
								int num8 = 81;
								int num9 = 12;
								if (whirlPoolTimer <= num8 && whirlPoolTimer > num8 - 2 * num9)
								{
									int num10 = ModContent.ProjectileType<QueenTorrent>();
									for (int m = 0; m < num9; m++)
									{
										if (whirlPoolTimer == num8 - 2 * m)
										{
											int num11 = 100 + 40 * m;
											float num12;
											if (m >= 9)
											{
												num12 = 3f;
											}
											else if (m >= 7)
											{
												num12 = 2f;
											}
											else if (m >= 5)
											{
												num12 = 1f;
											}
											else
											{
												num12 = 0f;
											}
											Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + (float)num11, 0f, 0f, num10, 25, 0f, Main.myPlayer, num12, 0f, 0f);
										}
									}
								}
							}
							if (whirlPoolTimer == 540 && Main.netMode != 2)
							{
								Vector2 vector2 = npc.position + new Vector2((float)Main.rand.Next(npc.width - 8), (float)Main.rand.Next(npc.height / 2));
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore1").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore2").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore3").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore4").Type, 1f);
							}
							if (whirlPoolTimer > 600)
							{
								whirlPoolTimer = -600;
							}
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4 && Main.expertMode && whirlPoolTimer > 60)
						{
							if (npc.localAI[0] == 0f)
							{
								if (Main.netMode != 1)
								{
									for (int n = 0; n < 6; n++)
									{
										if (Projectile.NewProjectile(new TentacleEntitySource(npc, n, null), npc.Center, Vector2.Zero, ModContent.ProjectileType<QueenJellyfishArm>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f) == 1000)
										{
											npc.active = false;
											return false;
										}
									}
								}
								npc.localAI[0] = 1f;
							}
							return true;
						}
						npc.rotation /= 5f;
						return false;
			}
			else if(CalamityGamemodeCheck.isRevengeance) 
			{
				// Checks if this boss ai should be reworked
						if(!(ModContent.GetInstance<BossConfig>().jelly == ThoriumBossRework_selection_mode.Ragnarok)) 
						{
								return true;
						}
						
						NPCHelper.BatAI(npc, thorium.Find<ModNPC>("QueenJellyfish").AIType, 1.2f); // Slightly speed up
						Player player = Main.player[npc.target];
						if (!player.active || player.dead)
						{
							whirlPoolTimer = 0;
							NPC npc1 = npc;
							npc1.velocity.Y = npc1.velocity.Y + 0.1f;
							if (npc.timeLeft > 20)
							{
								npc.timeLeft = 20;
							}
							return false;
						}
						if (player.position.Y < npc.position.Y + 225f)
						{
							NPC npc2 = npc;
							npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.2f : 0.12f);
						}
						if (player.position.Y > npc.position.Y + 225f)
						{
							NPC npc3 = npc;
							npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.2f : 0.12f);
						}
						npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -15f, 15f);
						if (whirlPoolTimer <= 0)
						{
							if (npc.rotation <= 0.35f && npc.rotation >= -0.35f)
							{
								npc.rotation += 0.01f * (float)npc.direction;
							}
							if (npc.rotation > 0.35f)
							{
								npc.rotation = 0.34f;
							}
							if (npc.rotation < -0.35f)
							{
								npc.rotation = -0.34f;
							}
						}
						else
						{
							npc.rotation = 0f;
						}
						npc.ai[0] += 1f;
						npc.ai[1] += 1f;
						npc.ai[2] += 1f;
						npc.ai[3] += 1f;
						bubbleTimer++;
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						if (whirlPoolTimer < 60)
						{
							if (spawnType > 0)
							{
								if (npc.ai[0] >= 0f)
								{
									int num = 0;
									for (int i = 0; i < Main.maxNPCs; i++)
									{
										NPC npc4 = Main.npc[i];
										if (npc4.active && npc4.type == ModContent.NPCType<ZealousJellyfish>())
										{
											num++;
										}
									}
									if (num < 3 && Main.netMode != 1) 
									{
										int num2 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<ZealousJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
										if (num2 < Main.maxNPCs && Main.netMode == 2)
										{
											NetMessage.SendData(23, -1, -1, null, num2, 0f, 0f, 0f, 0, 0, 0);
										}
									}
									npc.ai[0] = (float)(-300 + spawnFrequency);
								}
								if (spawnType > 1)
								{
									if (npc.ai[1] >= 0f)
									{
										int num3 = 0;
										for (int j = 0; j < Main.maxNPCs; j++)
										{
											NPC npc5 = Main.npc[j];
											if (npc5.active && npc5.type == ModContent.NPCType<SpittingJellyfish>())
											{
												num3++;
											}
										}
										if (num3 < 3 && Main.netMode != 1) 
										{
											int num4 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<SpittingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num4 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num4, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[1] = (float)(-600 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
									if (spawnType > 2 && npc.ai[2] >= 0f)
									{
										int num5 = 0;
										for (int k = 0; k < Main.maxNPCs; k++)
										{
											NPC npc6 = Main.npc[k];
											if (npc6.active && npc6.type == ModContent.NPCType<DistractingJellyfish>())
											{
												num5++;
											}
										}
										if (num5 < 3 && Main.netMode != 1)   
										{
											int num6 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<DistractingJellyfish>(), 0, (float)npc.whoAmI, 0f, 0f, 0f, 255);
											if (num6 < Main.maxNPCs && Main.netMode == 2)
											{
												NetMessage.SendData(23, -1, -1, null, num6, 0f, 0f, 0f, 0, 0, 0);
											}
											npc.ai[2] = (float)(-180 + spawnFrequency);
											npc.netUpdate = true;
										}
									}
								}
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.8) // Speeds up bubble bombs
							{
								speed = 7f;
								spawnType = 1;
								attackFrequency = 0;
								spawnFrequency = 0;
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.6 && (double)npc.life < (double)npc.lifeMax * 0.8) // Spawns Spitting Jellyfish 
							{
								speed = 8f;
								spawnType = 2;
								attackFrequency = 100;  // boosts attack frequency to 100
								spawnFrequency = 30;
							}
							if ((double)npc.life >= (double)npc.lifeMax * 0.4 && (double)npc.life < (double)npc.lifeMax * 0.6)
							{
								speed = 9f;
								spawnType = 3;
								attackFrequency = 160;  // Boosts attack frequency to 160
								spawnFrequency = 45;
							}
							if ((double)npc.life < (double)npc.lifeMax * 0.4)
							{
								speed = 11f;
								spawnType = 3;
								attackFrequency = 220; // Boosts attack frequency to 220
								spawnFrequency = 60;
							}  
							if ((double)npc.life < (double)npc.lifeMax * 0.6 && !player.dead && bubbleTimer >= 0 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
							{
								if (Main.netMode != 1)
								{
									Vector2 center = npc.Center;
									Vector2 vector = player.DirectionFrom(center);
									Projectile.NewProjectile(source_FromAI, center, vector * speed, ModContent.ProjectileType<BubbleBomb>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
								bubbleTimer = -300 + attackFrequency;
							}
						}
						else
						{
							bubbleTimer = -300 + attackFrequency;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4)
						{
							whirlPoolTimer++;
							if (whirlPoolTimer > 0 && whirlPoolTimer < 59)
							{
								for (int l = 0; l < 5; l++)
								{
									int num7 = Dust.NewDust(npc.position, npc.width, npc.height + 550, 29, 0f, -8f, 100, default(Color), 3f);
									Main.dust[num7].noGravity = true;
								}
							}
							if (whirlPoolTimer > 60) // Increased defense 
							{
								npc.defense = 6 * npc.defDefense;
								npc.velocity.X = 0f;
								npc.velocity.Y = 0f;
							}
							else
							{
								npc.defense = npc.defDefense;
							}
							if (Main.netMode != 1)
							{
								int num8 = 81;
								int num9 = 12;
								if (whirlPoolTimer <= num8 && whirlPoolTimer > num8 - 2 * num9)
								{
									int num10 = ModContent.ProjectileType<QueenTorrent>();
									for (int m = 0; m < num9; m++)
									{
										if (whirlPoolTimer == num8 - 2 * m)
										{
											int num11 = 100 + 40 * m;
											float num12;
											if (m >= 9)
											{
												num12 = 3f;
											}
											else if (m >= 7)
											{
												num12 = 2f;
											}
											else if (m >= 5)
											{
												num12 = 1f;
											}
											else
											{
												num12 = 0f;
											}
											Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + (float)num11, 0f, 0f, num10, 25, 0f, Main.myPlayer, num12, 0f, 0f);
										}
									}
								}
							}
							if (whirlPoolTimer == 540 && Main.netMode != 2)
							{
								Vector2 vector2 = npc.position + new Vector2((float)Main.rand.Next(npc.width - 8), (float)Main.rand.Next(npc.height / 2));
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore1").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore2").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore3").Type, 1f);
								Gore.NewGore(source_FromAI, vector2, npc.velocity, thorium.Find<ModGore>("QueenJellyfishWhirlpoolGore4").Type, 1f);
							}
							if (whirlPoolTimer > 600)
							{
								whirlPoolTimer = -600;
							}
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.4 && Main.expertMode && whirlPoolTimer > 60)
						{
							if (npc.localAI[0] == 0f)
							{
								if (Main.netMode != 1)
								{
									for (int n = 0; n < 6; n++)
									{
										if (Projectile.NewProjectile(new TentacleEntitySource(npc, n, null), npc.Center, Vector2.Zero, ModContent.ProjectileType<QueenJellyfishArm>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f) == 1000)
										{
											npc.active = false;
											return false;
										}
									}
								}
								npc.localAI[0] = 1f;
							}
							return true;
						}
						npc.rotation /= 5f;
						return false;	
			}
			else 
			{
				return true;
			}
		}
	}
}