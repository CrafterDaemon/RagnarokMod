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
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.Projectiles;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.Sounds;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class ThunderbirdAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) 
		{
			return npc.type == thorium.Find<ModNPC>("TheGrandThunderBird").Type;
		}
		private static void DecideNewAttack(NPC npc, int frame, params int[] attackStatesToIgnore)
		{
			SetFrame(npc, frame);
			setAnimationState(npc, 0);
			setAttackStateTimer(npc, (float)(-90 + value_frenzy));
			float chargeTimer = getChargeTimer(npc);
			setChargeTimer(npc, chargeTimer + 1f);
			npc.TargetClosest(true);
			if (Main.netMode != 1)
			{
				List<int> list = new List<int>(value_attackOptions);
				if (attackStatesToIgnore.Length == 0)
				{
					attackStatesToIgnore = new int[] { getAttackState(npc) };
				}
				foreach (int num in attackStatesToIgnore)
				{
					list.Remove(num);
				}
				if (list.Count == 0)
				{
					list.Add(0);
				}
				setAttackState(npc, Terraria.Utils.Next<int>(Main.rand, list));
				npc.netUpdate = true;
			}
		}
		private static void SetFrame(NPC npc, int frame)
		{
			npc.frame.Y = frame * 126;
			npc.frameCounter = 0.0;
		}
		public static void setAnimationState(NPC npc, float thevalue) 
		{
			npc.ai[0] = (float)thevalue;
		}
		public static int getAnimationState(NPC npc) 
		{
			return (int)npc.ai[0];
		}
		public static void setChargeTimer(NPC npc, float thevalue) 
		{
			npc.ai[1] = thevalue;
		}
		public static float getChargeTimer(NPC npc) 
		{
			return npc.ai[1];
		}
		public static void setAttackStateTimer(NPC npc, float thevalue) 
		{
			npc.ai[2] = thevalue;
		}
		public static float getAttackStateTimer(NPC npc) 
		{
			return npc.ai[2];
		}
		public static void setAttackState(NPC npc, int thevalue) 
		{
			npc.ai[3] = (float)thevalue;
		}
		public static int getAttackState(NPC npc) 
		{
			return (int)npc.ai[3];
		}		
		public static byte chargeDecide;
		public static int value_frenzy;
		public static List<int> value_attackOptions = new List<int> { 0, 1, 2 };
		public static int Animation_Base = 0;
		public static int Animation_Screech = 1;
		public static int Animation_Stunned = 2;
		public static int Animation_Charging = 3;
		public static int Attack_LightningStrikes = 0;
		public static int Attack_Charge = 1;
		public static int Attack_Hatchling = 2;
		public static int Attack_SparkShot = 3;
		
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
				if(!(ModContent.GetInstance<BossConfig>().bird == ThoriumBossRework_selection_mode.Ragnarok)) 
						{
								return true;
						}
				
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
						{
							npc.TargetClosest(true);
						}
						Player player = Main.player[npc.target];
						//Vanishes when Player is dead or not active
						if (!player.active || player.dead)
						{
							npc.velocity.Y = npc.velocity.Y - 0.04f;
							if (npc.timeLeft > 30)
							{
								npc.timeLeft = 30;
							}
							return false;
						}
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						bool isfrenzythreshold = (double)npc.life <= (double)npc.lifeMax * 0.5;
						npc.spriteDirection = ((player.Center.X > npc.Center.X) ? 1 : (-1));
						Vector2 vector = npc.DirectionTo(player.Center);
						if (isfrenzythreshold)
						{
							if (!value_attackOptions.Contains(3))
							{
								value_attackOptions.Add(3);
							}
							value_frenzy = 60;
						}
						else
						{
							value_attackOptions.Remove(3);
							value_frenzy = 30;
						}
						
						if (getAttackState(npc) != 1) // No attack regular movement
						{
							if (player.Center.X + 850f < npc.Center.X) // Move much faster
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? (-10f) : (-7f));
							}
							if (player.Center.X - 850f > npc.Center.X)
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? 10f : 7f);
							}
						}
						if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) < -90f)
						{
							npc.velocity.Y = 0f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							SetFrame(npc, 6);
						}
						else if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) == -90f)
						{
							npc.velocity.Y = -10f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							DecideNewAttack(npc, 0, Array.Empty<int>());
						}
						else if ((getAnimationState(npc) == 0 && getAttackState(npc) != 1) || (getAttackState(npc) == 1 && getAttackStateTimer(npc) < 60f))
						{
							int desiredaltitudeaboveplayer = 325;
							if (player.position.Y < npc.position.Y + (float)desiredaltitudeaboveplayer)
							{
								NPC npc2 = npc;
								npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.2f : 0.13f);  // Move faster
							}
							if (player.position.Y > npc.position.Y + (float)desiredaltitudeaboveplayer)
							{
								NPC npc3 = npc;
								npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.2f : 0.13f);
							}
							npc.rotation = npc.velocity.X * 0.05f;
							if (player.Center.X < npc.position.X && npc.velocity.X > -11f) // Move way faster
							{
								npc.velocity.X = npc.velocity.X - 0.14f;
								if (npc.velocity.X > 11f)
								{
									npc.velocity.X = npc.velocity.X - 0.14f;
								}
								else if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X + 0.08f;
								}
								if (npc.velocity.X < -11f)
								{
									npc.velocity.X = -11f;
								}
							}
							else if (player.Center.X > npc.position.X && npc.velocity.X < 11f) // Move way faster
							{
								npc.velocity.X = npc.velocity.X + 0.13f;
								if (npc.velocity.X < -11f)
								{
									npc.velocity.X = npc.velocity.X + 0.14f;
								}
								else if (npc.velocity.X < 0f)
								{
									npc.velocity.X = npc.velocity.X - 0.08f;
								}
								if (npc.velocity.X > 11f)
								{
									npc.velocity.X = 11f;
								}
							}
						}
						if (getChargeTimer(npc) < 5f)
						{
							for (int i = 0; i < (int)getChargeTimer(npc); i++)
							{
								int dustparam = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, 15, npc.velocity.X * 0.1f, -5f, 125, default(Color), 1f);
								Main.dust[dustparam].noGravity = true;
								Main.dust[dustparam].velocity *= 1f;
								Dust dust = Main.dust[dustparam];
								dust.velocity.Y = dust.velocity.Y - 0.25f;
							}
							
							if (getAttackState(npc) == 0) // spawns thunderzap from above
							{
								float num3 = getAttackStateTimer(npc);
								setAttackStateTimer(npc, num3 + 1f);
								if (getAttackStateTimer(npc) >= 60f)
								{
									if (npc.spriteDirection == 1)
									{
										npc.velocity.X = 0.01f;
									}
									else
									{
										npc.velocity.X = -0.01f;
									}
									npc.velocity.Y = 0f;
								}
								if (getAttackStateTimer(npc) == 60f)
								{
									setAnimationState(npc, 1);
									SetFrame(npc, 5);
									if (!player.dead && Main.netMode != 1)
									{
										int num4 = ModContent.ProjectileType<GrandThunderBirdZap>();
										for (int j = 0; j < 18; j++)  // Orignally 8
										{
											Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-320, 320), player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 15f, num4, 12, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
										Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 15f, num4, 12, 0f, Main.myPlayer, 0f, 0f, 0f); // changed speed from 8f to 13f
										float num5 = -0.05f * (float)Terraria.Utils.ToDirectionInt(npc.velocity.X > 0f);
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 34f, num5, 0f, ModContent.ProjectileType<ThunderBirdScreech>(), 0, 0f, Main.myPlayer, npc.rotation, 0f, 0f);
									}
								}
								if (getAttackStateTimer(npc) >= 120f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 1) // Charge
							{
								npc.rotation = 0f;
								float chargealignspeed = 25f; // Adjust position before charging much quicker 
								float chargespeed = 50f;
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer + 1f);
								if (getAttackStateTimer(npc) == 90f && chargeDecide == 0)
								{
									chargeDecide = (byte)((player.Center.X < npc.Center.X) ? 1 : 2);
									npc.netUpdate = true;
								}
								if ((getAttackStateTimer(npc) == 290f || getAttackStateTimer(npc) == 460f ))
								{
									chargeDecide = (byte)((player.Center.X < npc.Center.X) ? 1 : 2);
									npc.netUpdate = true;
								}
								if (getAttackStateTimer(npc) > 120f && getAttackStateTimer(npc) < 240f) // initiate first charge
								{
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									Vector2 vector2 = player.Center + new Vector2((float)(-(float)chargedirection) * 400f, 0f);
									Vector2 vector3 = vector2 - npc.Center;
									if (vector3.LengthSquared() > 400f)
									{
										npc.velocity = Terraria.Utils.SafeNormalize(vector3, Vector2.UnitX) * chargealignspeed;
									}
									else
									{
										npc.Center = vector2;
									}
								}
								if ((getAttackStateTimer(npc) == 240f || getAttackStateTimer(npc) == 410f ) && Main.netMode != 2) // Dust right before charge
								{
									for (int k = 0; k < 20; k++)
									{
										int chargedustparam = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[chargedustparam].noGravity = true;
									}
									for (int l = 0; l < 20; l++)
									{
										int chargedustparam = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[chargedustparam].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) > 240f && getAttackStateTimer(npc) < 290f ) // First charge
								{
									int chargedustparam = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 15, 0f, 0f, 255, default(Color), 1.2f);
									Dust dust2 = Main.dust[chargedustparam];
									dust2.velocity *= 0.2f;
									dust2.noGravity = true;
									setAnimationState(npc, 3);
									npc.velocity.Y = 0f;
									SetFrame(npc, 7);
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									npc.velocity.X = (float)chargedirection * chargespeed;
								}
								else if ((getAttackStateTimer(npc) >= 290f && getAttackStateTimer(npc) < 410f) || (getAttackStateTimer(npc) >= 460f && getAttackStateTimer(npc) < 580f)) // Prepare additional charges
								{
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									setAnimationState(npc, 0);
									Vector2 vector2 = player.Center + new Vector2((float)(-(float)chargedirection) * 400f, 0f);
									Vector2 vector3 = vector2 - npc.Center;
									if (vector3.LengthSquared() > 400f)
									{
										npc.velocity = Terraria.Utils.SafeNormalize(vector3, Vector2.UnitX) * chargealignspeed;
									}
									else
									{
										npc.Center = vector2;
									}
								}
								else if((getAttackStateTimer(npc) >= 410f && getAttackStateTimer(npc) < 460) || getAttackStateTimer(npc) >= 580f) // Additional charges
								{
									int chargedustparam = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 15, 0f, 0f, 255, default(Color), 1.2f);
									Dust dust2 = Main.dust[chargedustparam];
									dust2.velocity *= 0.2f;
									dust2.noGravity = true;
									setAnimationState(npc, 3);
									npc.velocity.Y = 0f;
									SetFrame(npc, 7);
									int num11 = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = num11;
									npc.spriteDirection = num11;
									npc.velocity.X = (float)num11 * chargespeed;
								}
								if (getAttackStateTimer(npc) > 630f) // End charges
								{
									if (Main.netMode != 2)
									{
										for (int m = 0; m < 20; m++)
										{
											int num12 = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num12].noGravity = true;
										}
										for (int n = 0; n < 20; n++)
										{
											int num13 = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num13].noGravity = true;
										}
									}
									chargeDecide = 0;
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 2)  // Spawn StormHatchlings
							{
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc,  attacktimer + 1f);
								if (getAttackStateTimer(npc) > 180f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
								if (getAttackStateTimer(npc) >= 60f && getAttackStateTimer(npc) % 60f == 0f && Main.netMode != 1)
								{
									float num14 = ((getAttackStateTimer(npc) == 180f) ? 1f : 0f);
									NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<StormHatchling>(), 0, num14, 0f, 0f, 0f, 255);
									if (getAttackStateTimer(npc) % 120f == 0) // Spawn 1 extra 
									{
										NPC.NewNPC(source_FromAI, (int)npc.Center.X - 100, (int)npc.Center.Y - 10, ModContent.NPCType<StormHatchling>(), 0, num14, 0f, 0f, 0f, 255); 
									} 
									return false;
								}
							}
							else if (getAttackState(npc) == 3)  // shoots zaps at the player
							{
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer + 1f);
								if (getAttackStateTimer(npc) == 10f && Main.netMode != 2)
								{
									for (int num15 = 0; num15 < 15; num15++)
									{
										int num16 = Dust.NewDust(npc.Center, 30, 30, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 2f);
										Main.dust[num16].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) >= 30f && getAttackStateTimer(npc) % 10f == 0f) // Spawn more projectiles by replacing 15f with 10f
								{
									if (Main.netMode != 1)  // Increase projectile speed from 10 to 18
									{
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 30f, vector.X * 18f, vector.Y * 18f, ModContent.ProjectileType<GrandThunderBirdZap>(), 15, 0f, Main.myPlayer, 1f, 0f, 0f);
									}
									if (getAttackStateTimer(npc) == 60f)
									{
										DecideNewAttack(npc, 0, Array.Empty<int>());
										return false;
									}
								}
							}
						}
						else
						{
							setAttackStateTimer(npc, -60f);
							float currentchargetimer = getChargeTimer(npc);
							setChargeTimer(npc, currentchargetimer + 1f);
							if (getChargeTimer(npc) == 6f)
							{
								for (int num17 = 0; num17 < 20; num17++)
								{
									int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 255, default(Color), 3f);
									Main.dust[num18].noGravity = true;
								}
								if (Main.netMode != 1)
								{
									OrbitingDustPro.Spawn(new AttachedEntityEntitySource_Parent(npc, player, null), ModContent.ProjectileType<GrandLightingEffect1>(), 0f);
								}
								SoundEngine.PlaySound(ThoriumSounds.ThunderFullPower, new Vector2?(npc.Center), null);
							}
							if (!player.dead && getChargeTimer(npc) % 30f == 0f && Main.netMode != 1)
							{
								Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 100f, 0f, 0f, ModContent.ProjectileType<GrandThunderBirdCloud>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							if (getChargeTimer(npc) > 360f)
							{
								setChargeTimer(npc, 0f);
							}
						}
						return false;
						
			}
			else if (CalamityGamemodeCheck.isDeath)
			{
				// Checks if this boss ai should be reworked
						if(!(ModContent.GetInstance<BossConfig>().bird == ThoriumBossRework_selection_mode.Ragnarok)) 
						{
								return true;
						}
						
						if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
						{
							npc.TargetClosest(true);
						}
						Player player = Main.player[npc.target];
						if (!player.active || player.dead)
						{
							npc.velocity.Y = npc.velocity.Y - 0.04f;
							if (npc.timeLeft > 30)
							{
								npc.timeLeft = 30;
							}
							return false;
						}
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						bool isfrenzythreshold = (double)npc.life <= (double)npc.lifeMax * 0.5;  
						npc.spriteDirection = ((player.Center.X > npc.Center.X) ? 1 : (-1));
						Vector2 vector = npc.DirectionTo(player.Center);
						if (isfrenzythreshold)
						{
							if (!value_attackOptions.Contains(3))
							{
								value_attackOptions.Add(3);
							}
							value_frenzy = 60;
						}
						else
						{
							value_attackOptions.Remove(3);
							value_frenzy = 30;
						}
						if (getAttackState(npc) != 1)
						{
							if (player.Center.X + 850f < npc.Center.X)
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? (-9f) : (-6f));
							}
							if (player.Center.X - 850f > npc.Center.X)
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? 9f : 6f);
							}
						}
						if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) < -90f)
						{
							npc.velocity.Y = 0f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							SetFrame(npc, 6);
						}
						else if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) == -90f)
						{
							npc.velocity.Y = -10f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							DecideNewAttack(npc, 0, Array.Empty<int>());
						}
						else if ((getAnimationState(npc) == 0 && getAttackState(npc) != 1) || (getAttackState(npc) == 1 && getAttackStateTimer(npc) < 60f))
						{
							int desiredaltitudeaboveplayer = 325;
							if (player.position.Y < npc.position.Y + (float)desiredaltitudeaboveplayer) // Faster
							{
								NPC npc2 = npc;
								npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.0f : 0.1f);  // 1.0f and 0.1f instead of 0.8 and 0.07
							}
							if (player.position.Y > npc.position.Y + (float)desiredaltitudeaboveplayer)
							{
								NPC npc3 = npc;
								npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.0f : 0.1f);
							}
							npc.rotation = npc.velocity.X * 0.05f;
							if (player.Center.X < npc.position.X && npc.velocity.X > -8f) // Move way faster
							{
								npc.velocity.X = npc.velocity.X - 0.11f;
								if (npc.velocity.X > 8f)
								{
									npc.velocity.X = npc.velocity.X - 0.11f;
								}
								else if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X + 0.06f;
								}
								if (npc.velocity.X < -8f)
								{
									npc.velocity.X = -8f;
								}
							}
							else if (player.Center.X > npc.position.X && npc.velocity.X < 7f) // Move way faster
							{
								npc.velocity.X = npc.velocity.X + 0.11f;
								if (npc.velocity.X < -8f)
								{
									npc.velocity.X = npc.velocity.X + 0.11f;
								}
								else if (npc.velocity.X < 0f)
								{
									npc.velocity.X = npc.velocity.X - 0.06f;
								}
								if (npc.velocity.X > 8f)
								{
									npc.velocity.X = 8f;
								}
							}
						}
						if (getChargeTimer(npc) < 5f)
						{
							for (int i = 0; i < (int)getChargeTimer(npc); i++)
								{
									int dustparam = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, 15, npc.velocity.X * 0.1f, -5f, 125, default(Color), 1f);
									Main.dust[dustparam].noGravity = true;
									Main.dust[dustparam].velocity *= 1f;
									Dust dust = Main.dust[dustparam];
									dust.velocity.Y = dust.velocity.Y - 0.25f;
								}
							if (getAttackState(npc) == 0)
							{
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer  + 1f);
								if (getAttackStateTimer(npc) >= 60f)
								{
									if (npc.spriteDirection == 1)
									{
										npc.velocity.X = 0.01f;
									}
									else
									{
										npc.velocity.X = -0.01f;
									}
									npc.velocity.Y = 0f;
								}
								if (getAttackStateTimer(npc) == 60f)
								{
									setAnimationState(npc, 1);
									SetFrame(npc, 5);
									if (!player.dead && Main.netMode != 1)
									{
										int grandthunderbirdzapid = ModContent.ProjectileType<GrandThunderBirdZap>();
										for (int j = 0; j < 14; j++)  // Orignally 8
										{
											Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-320, 320), player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 10f, grandthunderbirdzapid, 12, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
										Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 10f, grandthunderbirdzapid, 12, 0f, Main.myPlayer, 0f, 0f, 0f);
										float screechdirection = -0.05f * (float)Terraria.Utils.ToDirectionInt(npc.velocity.X > 0f);
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 34f, screechdirection, 0f, ModContent.ProjectileType<ThunderBirdScreech>(), 0, 0f, Main.myPlayer, npc.rotation, 0f, 0f);
									}
								}
								if (getAttackStateTimer(npc) >= 120f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 1) // Charge
							{
								npc.rotation = 0f;
								float chargealignspeed = 5f;
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer + 1f);
								if (getAttackStateTimer(npc) == 90f && chargeDecide == 0) // pre allign
								{
									chargeDecide = (byte)((player.Center.X < npc.Center.X) ? 1 : 2);
									npc.netUpdate = true;
								}
								if (getAttackStateTimer(npc) == 350f) // pre allign 2
								{
									chargeDecide = (byte)((player.Center.X < npc.Center.X) ? 1 : 2);
									npc.netUpdate = true;
								}
								if (getAttackStateTimer(npc) > 120f && getAttackStateTimer(npc) < 300f) // alling first charge
								{
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									Vector2 vector2 = player.Center + new Vector2((float)(-(float)chargedirection) * 400f, 0f);
									Vector2 vector3 = vector2 - npc.Center;
									if (vector3.LengthSquared() > 400f)
									{
										npc.velocity = Terraria.Utils.SafeNormalize(vector3, Vector2.UnitX) * chargealignspeed;
									}
									else
									{
										npc.Center = vector2;
									}
								}
								if ((getAttackStateTimer(npc) == 300f || getAttackStateTimer(npc) == 470f )&& Main.netMode != 2)
								{
									for (int k = 0; k < 20; k++)
									{
										int chargedustparam = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[chargedustparam].noGravity = true;
									}
									for (int l = 0; l < 20; l++)
									{
										int chargedustparam = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[chargedustparam].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) > 300f && getAttackStateTimer(npc) < 350f ) // Charge is splitted into two phases, first charge
								{
									int chargedustparam = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 15, 0f, 0f, 255, default(Color), 1.2f);
									Dust dust2 = Main.dust[chargedustparam];
									dust2.velocity *= 0.2f;
									dust2.noGravity = true;
									setAnimationState(npc, 3);
									npc.velocity.Y = 0f;
									SetFrame(npc, 7);
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									npc.velocity.X = (float)chargedirection * 21f;
								}
								else if (getAttackStateTimer(npc) >= 350f && getAttackStateTimer(npc) < 470f )  //allign second time
								{
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									setAnimationState(npc, 0);
									Vector2 vector2 = player.Center + new Vector2((float)(-(float)chargedirection) * 400f, 0f);
									Vector2 vector3 = vector2 - npc.Center;
									if (vector3.LengthSquared() > 400f)
									{
										npc.velocity = Terraria.Utils.SafeNormalize(vector3, Vector2.UnitX) * chargealignspeed * 1.2f;
									}
									else
									{
										npc.Center = vector2;
									}
								}
								else if(getAttackStateTimer(npc) >= 470f) // Second charge
								{
									int chargedustparam = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 15, 0f, 0f, 255, default(Color), 1.2f);
									Dust dust2 = Main.dust[chargedustparam];
									dust2.velocity *= 0.2f;
									dust2.noGravity = true;
									setAnimationState(npc, 3);
									npc.velocity.Y = 0f;
									SetFrame(npc, 7);
									int chargedirection = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = chargedirection;
									npc.spriteDirection = chargedirection;
									npc.velocity.X = (float)chargedirection * 21f;
								}
								if (getAttackStateTimer(npc) > 520f)
								{
									if (Main.netMode != 2)
									{
										for (int m = 0; m < 20; m++)
										{
											int num12 = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num12].noGravity = true;
										}
										for (int n = 0; n < 20; n++)
										{
											int num13 = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num13].noGravity = true;
										}
									}
									chargeDecide = 0;
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 2)  // Spawn StormHatchlings
							{
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer + 1f);
								if (getAttackStateTimer(npc) > 180f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
								if (getAttackStateTimer(npc) >= 60f && getAttackStateTimer(npc) % 60f == 0f && Main.netMode != 1)
								{
									float num14 = ((getAttackStateTimer(npc) == 180f) ? 1f : 0f);
									NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<StormHatchling>(), 0, num14, 0f, 0f, 0f, 255);
									if (getAttackStateTimer(npc) % 120f == 0) // Spawn 1 extra 
									{
										NPC.NewNPC(source_FromAI, (int)npc.Center.X - 100, (int)npc.Center.Y - 10, ModContent.NPCType<StormHatchling>(), 0, num14, 0f, 0f, 0f, 255); 
									} 
									return false;
								}
							}
							else if (getAttackState(npc) == 3) // Shooting ThunderZaps
							{
								float attacktimer = getAttackStateTimer(npc);
								setAttackStateTimer(npc, attacktimer + 1f);
								if (getAttackStateTimer(npc) == 10f && Main.netMode != 2)
								{
									for (int num15 = 0; num15 < 15; num15++)
									{
										int num16 = Dust.NewDust(npc.Center, 30, 30, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 2f);
										Main.dust[num16].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) >= 30f && getAttackStateTimer(npc) % 10f == 0f) // Shoots more
								{
									if (Main.netMode != 1) // Increases Zap Speed
									{
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 30f, vector.X * 14f, vector.Y * 14f, ModContent.ProjectileType<GrandThunderBirdZap>(), 15, 0f, Main.myPlayer, 1f, 0f, 0f);
									}
									if (getAttackStateTimer(npc) == 60f)
									{
										DecideNewAttack(npc, 0, Array.Empty<int>());
										return false;
									}
								}
							}
						}
						else
						{
							setAttackStateTimer(npc, -60f);
							float currentchargetimer = getChargeTimer(npc);
							setChargeTimer(npc, currentchargetimer + 1f);
							if (getChargeTimer(npc) == 6f)
							{
								for (int num17 = 0; num17 < 20; num17++)
								{
									int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 255, default(Color), 3f);
									Main.dust[num18].noGravity = true;
								}
								if (Main.netMode != 1)
								{
									OrbitingDustPro.Spawn(new AttachedEntityEntitySource_Parent(npc, player, null), ModContent.ProjectileType<GrandLightingEffect1>(), 0f);
								}
								SoundEngine.PlaySound(ThoriumSounds.ThunderFullPower, new Vector2?(npc.Center), null);
							}
							if (!player.dead && getChargeTimer(npc) % 30f == 0f && Main.netMode != 1)
							{
								Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 100f, 0f, 0f, ModContent.ProjectileType<GrandThunderBirdCloud>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							if (getChargeTimer(npc) > 360f)
							{
								setChargeTimer(npc, 0f);
							}
						}
						return false;
			}				
			else if(CalamityGamemodeCheck.isRevengeance) 
			{
				if(!(ModContent.GetInstance<BossConfig>().bird == ThoriumBossRework_selection_mode.Ragnarok)) 
						{
								return true;
						}
						
						if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
						{
							npc.TargetClosest(true);
						}
						Player player = Main.player[npc.target];
						if (!player.active || player.dead)
						{
							npc.velocity.Y = npc.velocity.Y - 0.04f;
							if (npc.timeLeft > 30)
							{
								npc.timeLeft = 30;
							}
							return false;
						}
						IEntitySource source_FromAI = npc.GetSource_FromAI(null);
						bool isfrenzythreshold = (double)npc.life <= (double)npc.lifeMax * 0.5;
						npc.spriteDirection = ((player.Center.X > npc.Center.X) ? 1 : (-1));
						Vector2 vector = npc.DirectionTo(player.Center);
						if (isfrenzythreshold)
						{
							if (!value_attackOptions.Contains(3))
							{
								value_attackOptions.Add(3);
							}
							value_frenzy = 60;
						}
						else
						{
							value_attackOptions.Remove(3);
							value_frenzy = 30;
						}
						if (getAttackState(npc) != 1)
						{
							if (player.Center.X + 850f < npc.Center.X)
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? (-7f) : (-5f)); // Move faster
							}
							if (player.Center.X - 850f > npc.Center.X)
							{
								npc.velocity.X = ((npc.velocity.X < 0f) ? 7f : 5f); // Move faster
							}
						}
						if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) < -90f)
						{
							npc.velocity.Y = 0f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							SetFrame(npc, 6);
						}
						else if (getAnimationState(npc) == 2 && getAttackStateTimer(npc) == -90f)
						{
							npc.velocity.Y = -10f;
							npc.velocity.X = 0f;
							chargeDecide = 0;
							DecideNewAttack(npc, 0, Array.Empty<int>());
						}
						else if ((getAnimationState(npc) == 0 && getAttackState(npc) != 1) || (getAttackState(npc) == 1 && getAttackStateTimer(npc) < 60f))
						{
							int desiredaltitudeaboveplayer = 325;
							if (player.position.Y < npc.position.Y + (float)desiredaltitudeaboveplayer)
							{
								NPC npc2 = npc;
								npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.9f : 0.08f); // Move faster
							}
							if (player.position.Y > npc.position.Y + (float)desiredaltitudeaboveplayer)
							{
								NPC npc3 = npc;
								npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.9f : 0.08f);
							}
							npc.rotation = npc.velocity.X * 0.05f;
							if (player.Center.X < npc.position.X && npc.velocity.X > -5f) // Move faster
							{
								npc.velocity.X = npc.velocity.X - 0.1f;
								if (npc.velocity.X > 5f)
								{
									npc.velocity.X = npc.velocity.X - 0.1f;
								}
								else if (npc.velocity.X > 0f)
								{
									npc.velocity.X = npc.velocity.X + 0.05f;
								}
								if (npc.velocity.X < -5f)
								{
									npc.velocity.X = -5f;
								}
							}
							else if (player.Center.X > npc.position.X && npc.velocity.X < 5f)
							{
								npc.velocity.X = npc.velocity.X + 0.1f;
								if (npc.velocity.X < -5f)
								{
									npc.velocity.X = npc.velocity.X + 0.1f;
								}
								else if (npc.velocity.X < 0f)
								{
									npc.velocity.X = npc.velocity.X - 0.05f;
								}
								if (npc.velocity.X > 5f)
								{
									npc.velocity.X = 5f;
								}
							}
						}
						if (getChargeTimer(npc) < 5f)
						{
							for (int i = 0; i < (int)getChargeTimer(npc); i++)
							{
								int dustparam = Dust.NewDust(npc.position - new Vector2(2f, 2f), npc.width, npc.height, 15, npc.velocity.X * 0.1f, -5f, 125, default(Color), 1f);
								Main.dust[dustparam].noGravity = true;
								Main.dust[dustparam].velocity *= 1f;
								Dust dust = Main.dust[dustparam];
								dust.velocity.Y = dust.velocity.Y - 0.25f;
							}
							
							if (getAttackState(npc) == 0)
							{
								float num3 = getAttackStateTimer(npc);
								setAttackStateTimer(npc, num3 + 1f);
								if (getAttackStateTimer(npc) >= 60f)
								{
									if (npc.spriteDirection == 1)
									{
										npc.velocity.X = 0.01f;
									}
									else
									{
										npc.velocity.X = -0.01f;
									}
									npc.velocity.Y = 0f;
								}
								if (getAttackStateTimer(npc) == 60f)
								{
									setAnimationState(npc, 1);
									SetFrame(npc, 5);
									if (!player.dead && Main.netMode != 1)
									{
										int num4 = ModContent.ProjectileType<GrandThunderBirdZap>();
										for (int j = 0; j < 12; j++)  // Orignally 8 and fall a bit faster
										{
											Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-300, 300), player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 10f, num4, 12, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
										Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 800f + (float)Main.rand.Next(-30, 30), 0f, 9f, num4, 12, 0f, Main.myPlayer, 0f, 0f, 0f);
										float num5 = -0.05f * (float)Terraria.Utils.ToDirectionInt(npc.velocity.X > 0f);
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 34f, num5, 0f, ModContent.ProjectileType<ThunderBirdScreech>(), 0, 0f, Main.myPlayer, npc.rotation, 0f, 0f);
									}
								}
								if (getAttackStateTimer(npc) >= 120f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 1)
							{
								npc.rotation = 0f;
								float num6 = 5f;
								float num3 = getAttackStateTimer(npc);
								setAttackStateTimer(npc, num3 + 1f);
								if (getAttackStateTimer(npc) == 90f && chargeDecide == 0)
								{
									chargeDecide = (byte)((player.Center.X < npc.Center.X) ? 1 : 2);
									npc.netUpdate = true;
								}
								if (getAttackStateTimer(npc) > 120f && getAttackStateTimer(npc) < 300f)
								{
									int num7 = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = num7;
									npc.spriteDirection = num7;
									Vector2 vector2 = player.Center + new Vector2((float)(-(float)num7) * 400f, 0f);
									Vector2 vector3 = vector2 - npc.Center;
									if (vector3.LengthSquared() > 400f)
									{
										npc.velocity = Terraria.Utils.SafeNormalize(vector3, Vector2.UnitX) * num6;
									}
									else
									{
										npc.Center = vector2;
									}
								}
								if (getAttackStateTimer(npc) == 300f && Main.netMode != 2)
								{
									for (int k = 0; k < 20; k++)
									{
										int num8 = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[num8].noGravity = true;
									}
									for (int l = 0; l < 20; l++)
									{
										int num9 = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 3f);
										Main.dust[num9].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) > 300f)
								{
									int num10 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 15, 0f, 0f, 255, default(Color), 1.2f);
									Dust dust2 = Main.dust[num10];
									dust2.velocity *= 0.2f;
									dust2.noGravity = true;
									setAnimationState(npc, 3);
									npc.velocity.Y = 0f;
									SetFrame(npc, 7);
									int num11 = ((chargeDecide == 1) ? (-1) : 1);
									npc.direction = num11;
									npc.spriteDirection = num11;
									npc.velocity.X = (float)num11 * 18f; // Charge faster
								}
								if (getAttackStateTimer(npc) > 380f)
								{
									if (Main.netMode != 2)
									{
										for (int m = 0; m < 20; m++)
										{
											int num12 = Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num12].noGravity = true;
										}
										for (int n = 0; n < 20; n++)
										{
											int num13 = Dust.NewDust(npc.position, npc.width, npc.height, 88, (float)Main.rand.Next(-5, 5), (float)Main.rand.Next(-5, 5), 0, default(Color), 3f);
											Main.dust[num13].noGravity = true;
										}
									}
									chargeDecide = 0;
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
							}
							else if (getAttackState(npc) == 2)
							{
								float num3 = getAttackStateTimer(npc);
								setAttackStateTimer(npc,  num3 + 1f);
								if (getAttackStateTimer(npc) > 180f)
								{
									DecideNewAttack(npc, 0, Array.Empty<int>());
									return false;
								}
								if (getAttackStateTimer(npc) >= 60f && getAttackStateTimer(npc) % 60f == 0f && Main.netMode != 1)
								{
									float num14 = ((getAttackStateTimer(npc) == 180f) ? 1f : 0f);
									NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 10, ModContent.NPCType<StormHatchling>(), 0, num14, 0f, 0f, 0f, 255);
									return false;
								}
							}
							else if (getAttackState(npc) == 3) //shoots zaps
							{
								float num3 = getAttackStateTimer(npc);
								setAttackStateTimer(npc, num3 + 1f);
								if (getAttackStateTimer(npc) == 10f && Main.netMode != 2)
								{
									for (int num15 = 0; num15 < 15; num15++)
									{
										int num16 = Dust.NewDust(npc.Center, 30, 30, 57, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 0, default(Color), 2f);
										Main.dust[num16].noGravity = true;
									}
								}
								if (getAttackStateTimer(npc) >= 30f && getAttackStateTimer(npc) % 15f == 0f)
								{
									if (Main.netMode != 1) // a bit faster
									{
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 30f, vector.X * 12f, vector.Y * 12f, ModContent.ProjectileType<GrandThunderBirdZap>(), 15, 0f, Main.myPlayer, 1f, 0f, 0f);
									}
									if (getAttackStateTimer(npc) == 60f)
									{
										DecideNewAttack(npc, 0, Array.Empty<int>());
										return false;
									}
								}
							}
						}
						else
						{
							setAttackStateTimer(npc, -60f);
							float num3 = getChargeTimer(npc);
							setChargeTimer(npc, num3 + 1f);
							if (getChargeTimer(npc) == 6f)
							{
								for (int num17 = 0; num17 < 20; num17++)
								{
									int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 255, default(Color), 3f);
									Main.dust[num18].noGravity = true;
								}
								if (Main.netMode != 1)
								{
									OrbitingDustPro.Spawn(new AttachedEntityEntitySource_Parent(npc, player, null), ModContent.ProjectileType<GrandLightingEffect1>(), 0f);
								}
								SoundEngine.PlaySound(ThoriumSounds.ThunderFullPower, new Vector2?(npc.Center), null);
							}
							if (!player.dead && getChargeTimer(npc) % 30f == 0f && Main.netMode != 1)
							{
								Projectile.NewProjectile(source_FromAI, player.Center.X, player.Center.Y - 100f, 0f, 0f, ModContent.ProjectileType<GrandThunderBirdCloud>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							if (getChargeTimer(npc) > 360f)
							{
								setChargeTimer(npc, 0f);
							}
						}
						return false;
			}
			else 
			{
					return true;
			}
		}
	}
}