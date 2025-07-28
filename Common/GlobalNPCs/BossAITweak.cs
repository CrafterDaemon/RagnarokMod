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
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.Projectiles;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.Sounds;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class BossAITweak : GlobalNPC
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) 
		{
			return (
			npc.type == thorium.Find<ModNPC>("TheGrandThunderBird").Type
			|| npc.type == thorium.Find<ModNPC>("StormHatchling").Type
			|| npc.type == thorium.Find<ModNPC>("QueenJellyfish").Type
			|| npc.type == thorium.Find<ModNPC>("DistractingJellyfish").Type
			|| npc.type == thorium.Find<ModNPC>("ZealousJellyfish").Type
			|| npc.type == thorium.Find<ModNPC>("SpittingJellyfish").Type
			);
		}
		
		// GrandThunderBird 
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
		
		// GrandThunderBird END
		
		// QueenJellyfish
		public static int whirlPoolTimer;
		public static int bubbleTimer;
		public static int spawnType = 1;
		public static int spawnFrequency;
		public static float speed;
		public static int attackFrequency;
		public static int diverOffset;
		public static int counting;
		public static int counter;
		// QueenJellyfish END
		
		// For NPC changes without changing the bass AI
		public override void AI(NPC npc) 
		{
			if(CalamityGamemodeCheck.isBossrush) 
			{
				//Thorium Rework is still bugged, so bossrush is always Thorium Rework for now
				if(OtherModsCompat.tbr_loaded)
				{
					return;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return;
				}
				
				if (npc.type == thorium.Find<ModNPC>("StormHatchling").Type)
				{
						Player player = Main.player[npc.target];
							if (player.Center.X < npc.position.X && npc.velocity.X > -10f) 
								{
									npc.velocity.X = npc.velocity.X - 0.13f;
									if (npc.velocity.X > 10f)
									{
										npc.velocity.X = npc.velocity.X - 0.13f;
									}
									else if (npc.velocity.X > 0f)
									{
										npc.velocity.X = npc.velocity.X + 0.075f;
									}
									if (npc.velocity.X < -10f)
									{
										npc.velocity.X = -10f;
									}
								}
								else if (player.Center.X > npc.position.X && npc.velocity.X < 10f)
								{
									npc.velocity.X = npc.velocity.X + 0.13f;
									if (npc.velocity.X < -10f)
									{
										npc.velocity.X = npc.velocity.X + 0.13f;
									}
									else if (npc.velocity.X < 0f)
									{
										npc.velocity.X = npc.velocity.X - 0.075f;
									}
									if (npc.velocity.X > 10f)
									{
										npc.velocity.X = 10f;
									}
								}
								
							if (player.Center.Y < npc.position.Y && npc.velocity.Y > -5f) 
								{
									npc.velocity.Y = npc.velocity.Y - 0.09f;
									if (npc.velocity.Y > 5f)
									{
										npc.velocity.Y = npc.velocity.Y - 0.09f;
									}
									else if (npc.velocity.Y > 0f)
									{
										npc.velocity.Y = npc.velocity.Y + 0.06f;
									}
								}
								else if (player.Center.Y > npc.position.Y && npc.velocity.Y < 5f)
								{
									npc.velocity.Y = npc.velocity.Y + 0.09f;
									if (npc.velocity.Y < -5f)
									{
										npc.velocity.Y = npc.velocity.Y + 0.09f;
									}
									else if (npc.velocity.Y < 5f)
									{
										npc.velocity.Y = npc.velocity.Y - 0.06f;
									}
								}
				}
			}	
		}
		
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
		
				if(npc.type == thorium.Find<ModNPC>("TheGrandThunderBird").Type) 
					{
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
				else if(npc.type == thorium.Find<ModNPC>("QueenJellyfish").Type) 
				{
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
				else if (npc.type == thorium.Find<ModNPC>("DistractingJellyfish").Type)
				{
					NPC npc1 = Main.npc[(int)npc.ai[0]];
					if (!npc1.active)
					{
						npc.active = false;
						npc.netUpdate = true;
					}
					if (npc.ai[1] == 0f)
					{
						Vector2 vector;
						vector = new Vector2(npc.Center.X, npc.Center.Y);
						float num = npc1.Center.X - vector.X;
						float num2 = npc1.Center.Y - vector.Y;
						float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
						if (num3 > 90f)
						{
							num3 = 8f / num3;
							num *= num3;
							num2 *= num3;
							npc.velocity.X = (npc.velocity.X * 15f + num) / 16f;
							npc.velocity.Y = (npc.velocity.Y * 15f + num2) / 16f;
						}
						if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 15f)
						{
							npc.velocity.Y = npc.velocity.Y * 1.07f;
							npc.velocity.X = npc.velocity.X * 1.07f;
						}
						if (Main.netMode != 1 && (( Terraria.Utils.NextBool(Main.rand, 100)) || Terraria.Utils.NextBool(Main.rand, 200)))
						{
							npc.TargetClosest(true);
							vector = new Vector2(npc.Center.X, npc.Center.Y);
							num = Main.player[npc.target].Center.X - vector.X;
							num2 = Main.player[npc.target].Center.Y - vector.Y;
							num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
							if (num3 > 0f)
							{
								num3 = 8f / num3;
							}
							npc.velocity.X = num * num3 * 1.5f;
							npc.velocity.Y = num2 * num3 * 1.5f;
							npc.ai[1] = 1f;
							npc.netUpdate = true;
							return false;
						}
					}
					else
					{
						Vector2 vector2 = Main.player[npc.target].Center - npc.Center;
						vector2.Normalize();
						vector2 *= 13f;
						npc.velocity = (npc.velocity * 99f + vector2) / 100f;
						Vector2 vector3;
						vector3 = new Vector2(npc.Center.X, npc.Center.Y);
						float num4 = npc1.Center.X - vector3.X;
						float num5 = npc1.Center.Y - vector3.Y;
						if ((float)Math.Sqrt((double)(num4 * num4 + num5 * num5)) > 700f || npc.justHit)
						{
							npc.ai[1] = 0f;
						}
					}
					return false;
				}
				else if(npc.type == thorium.Find<ModNPC>("ZealousJellyfish").Type) 
				{
					NPCHelper.BatAI(npc, thorium.Find<ModNPC>("QueenJellyfish").AIType, 2f);
					Player player = Main.player[npc.target];
					if (!player.active || player.dead)
					{
						npc.active = false;
						return false;
					}
					if (player.position.Y - 150f > npc.position.Y)
					{
						npc.directionY = 1;
					}
					else
					{
						npc.directionY = -1;
					}
					if (npc.direction == -1 && npc.velocity.X > -4f)
					{
						npc.velocity.X = npc.velocity.X - 0.8f;
						if (npc.velocity.X > 4f)
						{
							npc.velocity.X = npc.velocity.X - 0.8f;
						}
						else if (npc.velocity.X > 0f)
						{
							npc.velocity.X = npc.velocity.X + 0.16f;
						}
						if (npc.velocity.X < -4f)
						{
							npc.velocity.X = -4f;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 8f)
					{
						npc.velocity.X = npc.velocity.X + 0.2f;
						if (npc.velocity.X < -4f)
						{
							npc.velocity.X = npc.velocity.X + 0.2f;
						}
						else if (npc.velocity.X < 0f)
						{
							npc.velocity.X = npc.velocity.X - 0.16f;
						}
						if (npc.velocity.X > 4f)
						{
							npc.velocity.X = 4f;
						}
					}
					if (npc.directionY == -1 && (double)npc.velocity.Y > -3.0)
					{
						npc.velocity.Y = npc.velocity.Y - 0.16f;
						if ((double)npc.velocity.Y < -3.0)
						{
							npc.velocity.Y = -3.0f;
						}
					}
					else if (npc.directionY == 1 && (double)npc.velocity.Y < 3.0)
					{
						npc.velocity.Y = npc.velocity.Y + 0.16f;
						if ((double)npc.velocity.Y > 3.0)
						{
							npc.velocity.Y = 3.0f;
						}
					}
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 180f)
					{
						npc.velocity = npc.DirectionTo(player.Center) * 16f;
						npc.ai[1] = 0f;
					}
					return false;
					
				}
				else if(npc.type == thorium.Find<ModNPC>("SpittingJellyfish").Type)
				{
					NPCHelper.BatAI(npc, thorium.Find<ModNPC>("QueenJellyfish").AIType, 2f);
					Player player = Main.player[npc.target];
					if (!player.active || player.dead)
					{
						npc.active = false;
						return false;
					}
					if (player.position.Y < npc.position.Y + 350f)
					{
						NPC npc1 = npc;
						npc.velocity.Y = npc.velocity.Y - ((npc.velocity.Y > 0f) ? 1.2f : 0.14f);
					}
					if (player.position.Y > npc.position.Y + 350f)
					{
						NPC npc2 = npc;
						npc2.velocity.Y = npc2.velocity.Y + ((npc.velocity.Y < 0f) ? 1.2f : 0.14f);
					}
					npc.ai[2] += 1f;
					if (npc.ai[1] == 0f)
					{
						if (player.position.X < npc.position.X && npc.velocity.X > -16f)
						{
							NPC npc3 = npc;
							npc3.velocity.X = npc3.velocity.X - 0.2f;
						}
						if (player.position.X > npc.position.X && npc.velocity.X < 16f)
						{
							NPC npc4 = npc;
							npc4.velocity.X = npc4.velocity.X + 0.2f;
						}
						if (player.position.Y < npc.position.Y + 300f)
						{
							if (npc.velocity.Y < 0f)
							{
								if (npc.velocity.Y > -8f)
								{
									NPC npc5 = npc;
									npc5.velocity.Y = npc5.velocity.Y - 0.8f;
								}
							}
							else
							{
								NPC npc6 = npc;
								npc6.velocity.Y = npc6.velocity.Y - 1.6f;
							}
						}
						if (player.position.Y > npc.position.Y + 300f)
						{
							if (npc.velocity.Y > 0f)
							{
								if (npc.velocity.Y < 8f)
								{
									NPC npc7 = npc;
									npc7.velocity.Y = npc7.velocity.Y + 0.8f;
								}
							}
							else
							{
								NPC npc8 = npc;
								npc8.velocity.Y = npc8.velocity.Y + 1.2f;
							}
						}
					}
					if (!player.dead && npc.ai[2] >= 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
					{
						if (Main.netMode != 1)
						{
							IEntitySource source_FromAI = npc.GetSource_FromAI(null);
							Vector2 vector;
							vector = new Vector2(npc.Center.X, npc.Center.Y + 10f);
							Vector2 vector2 = player.DirectionFrom(vector);
							Projectile.NewProjectile(source_FromAI, vector, vector2 * 20f, ModContent.ProjectileType<BubblePulse>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f); // Way faster Bubbles
						}
						npc.ai[2] = 0f;
					}
					return false;
				}
				else 
				{
					return true;
				}				
			}
			else if (CalamityGamemodeCheck.isDeath)
			{	
					if(npc.type == thorium.Find<ModNPC>("TheGrandThunderBird").Type) 
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
					else if (npc.type == thorium.Find<ModNPC>("QueenJellyfish").Type)
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
					else 
					{
						return true;
					}	
			} 
			else if(CalamityGamemodeCheck.isRevengeance) 
			{
					if(npc.type == thorium.Find<ModNPC>("TheGrandThunderBird").Type) 
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
					else if(npc.type == thorium.Find<ModNPC>("QueenJellyfish").Type) 
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
			else 
			{
				return true;
			}
		}
	}
}