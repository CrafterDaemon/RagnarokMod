using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RagnarokMod.Utils
{
    public static class NPCHelper
    {
        public static bool IsHostile(this NPC npc, object attacker = null, bool ignoreDontTakeDamage = false)
		{
			return !npc.friendly && npc.lifeMax > 5 && npc.chaseable && (!npc.dontTakeDamage || ignoreDontTakeDamage) && !npc.immortal;
		}
		
		public static void BatAI(NPC npc, int mimicType = 0, float boostfactor = 1f)
		{
			int num = ((mimicType > 0) ? mimicType : npc.type);
			npc.noGravity = true;
			if (npc.collideX){
				npc.velocity.X = npc.oldVelocity.X * -0.5f;
				if (npc.direction == -1 && npc.velocity.X > 0f && npc.velocity.X < 2f * boostfactor){
					npc.velocity.X = 2f * boostfactor;
				}
				if (npc.direction == 1 && npc.velocity.X < 0f && npc.velocity.X > -2f * boostfactor){
					npc.velocity.X = -2f * boostfactor;
				}
			}
			if (npc.collideY){
				npc.velocity.Y = npc.oldVelocity.Y * -0.5f;
				if (npc.velocity.Y > 0f && npc.velocity.Y < 1f * boostfactor){
					npc.velocity.Y = 1f * boostfactor;
				}
				if (npc.velocity.Y < 0f && npc.velocity.Y > -1f * boostfactor){
					npc.velocity.Y = -1f * boostfactor;
				}
			}
			if (num == 226){
				int num2 = 1;
				int num3 = 1;
				if (npc.velocity.X < 0f){
					num2 = -1;
				}
				if (npc.velocity.Y < 0f){
					num3 = -1;
				}
				npc.TargetClosest(true);
				if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)){
					npc.direction = num2;
					npc.directionY = num3;
				}
			}
			else{
				npc.TargetClosest(true);
			}
			Player player = Main.player[npc.target];
			if (num == 158){
				if ((double)npc.position.Y < Main.worldSurface * 16.0 && Main.IsItDay() && !Main.eclipse){
					npc.directionY = -1;
					npc.direction *= -1;
				}
				if (npc.direction == -1 && npc.velocity.X > -7f * boostfactor){
					npc.velocity.X = npc.velocity.X - 0.2f * boostfactor;
					if (npc.velocity.X > 4f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
					}
					else if (npc.velocity.X > 0f){
						npc.velocity.X = npc.velocity.X + 0.05f * boostfactor;
					}
					if (npc.velocity.X < -7f * boostfactor){
						npc.velocity.X = -7f * boostfactor;
					}
				}
				else if (npc.direction == 1 && npc.velocity.X < 7f * boostfactor){
					npc.velocity.X = npc.velocity.X + 0.2f * boostfactor;
					if (npc.velocity.X < -4f * boostfactor){
						npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
					}
					else if (npc.velocity.X < 0f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.05f * boostfactor;
					}
					if (npc.velocity.X > 7f * boostfactor){
						npc.velocity.X = 7f * boostfactor;
					}
				}
				if (npc.directionY == -1 && npc.velocity.Y > -7f * boostfactor){
					npc.velocity.Y = npc.velocity.Y - 0.2f * boostfactor;
					if (npc.velocity.Y > 4f * boostfactor){
						npc.velocity.Y = npc.velocity.Y - 0.1f * boostfactor;
					}
					else if (npc.velocity.Y > 0f){
						npc.velocity.Y = npc.velocity.Y + 0.05f * boostfactor;
					}
					if (npc.velocity.Y < -7f * boostfactor){
						npc.velocity.Y = -7f * boostfactor;
					}
				}
				else if (npc.directionY == 1 && npc.velocity.Y < 7f * boostfactor){
					npc.velocity.Y = npc.velocity.Y + 0.2f * boostfactor;
					if (npc.velocity.Y < -4f * boostfactor){
						npc.velocity.Y = npc.velocity.Y + 0.1f * boostfactor;
					}
					else if (npc.velocity.Y < 0f){
						npc.velocity.Y = npc.velocity.Y - 0.05f * boostfactor;
					}
					if (npc.velocity.Y > 7f * boostfactor){
						npc.velocity.Y = 7f * boostfactor;
					}
				}
			}
			else if (num == 226)
			{
				if (npc.direction == -1 && npc.velocity.X > -4f * boostfactor){
					npc.velocity.X = npc.velocity.X - 0.2f * boostfactor;
					if (npc.velocity.X > 4f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
					}
					else if (npc.velocity.X > 0f){
						npc.velocity.X = npc.velocity.X + 0.05f * boostfactor;
					}
					if (npc.velocity.X < -4f * boostfactor){
						npc.velocity.X = -4f * boostfactor;
					}
				}
				else if (npc.direction == 1 && npc.velocity.X < 4f * boostfactor){
					npc.velocity.X = npc.velocity.X + 0.2f * boostfactor;
					if (npc.velocity.X < -4f * boostfactor){
						npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
					}
					else if (npc.velocity.X < 0f){
						npc.velocity.X = npc.velocity.X - 0.05f * boostfactor;
					}
					if (npc.velocity.X > 4f * boostfactor){
						npc.velocity.X = 4f * boostfactor;
					}
				}
				if (npc.directionY == -1 && (double)npc.velocity.Y > -2.5 * boostfactor){
					npc.velocity.Y = npc.velocity.Y - 0.1f * boostfactor;
					if ((double)npc.velocity.Y > 2.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y - 0.05f * boostfactor;
					}
					else if (npc.velocity.Y > 0f){
						npc.velocity.Y = npc.velocity.Y + 0.03f * boostfactor;
					}
					if ((double)npc.velocity.Y < -2.5 * boostfactor){
						npc.velocity.Y = -2.5f * boostfactor;
					}
				}
				else if (npc.directionY == 1 && (double)npc.velocity.Y < 2.5 * boostfactor){
					npc.velocity.Y = npc.velocity.Y + 0.1f * boostfactor;
					if ((double)npc.velocity.Y < -2.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y + 0.05f * boostfactor;
					}
					else if (npc.velocity.Y < 0f){
						npc.velocity.Y = npc.velocity.Y - 0.03f * boostfactor;
					}
					if ((double)npc.velocity.Y > 2.5 * boostfactor){
						npc.velocity.Y = 2.5f * boostfactor;
					}
				}
			}
			else if (num == 660){
				float num4 = 0.1f;
				float num5 = 0.04f;
				float num6 = 4f;
				float num7 = 1.5f;
				if (num == 660){
					num4 = 0.35f;
					num5 = 0.3f;
					num6 = 6f;
					num7 = 5f;
				}
				if (npc.direction == -1 && npc.velocity.X > 0f - num6){
					npc.velocity.X = npc.velocity.X - num4;
					if (npc.velocity.X > num6){
						npc.velocity.X = npc.velocity.X - num4;
					}
					else if (npc.velocity.X > 0f){
						npc.velocity.X = npc.velocity.X + num4 * 0.5f;
					}
					if (npc.velocity.X < 0f - num6){
						npc.velocity.X = 0f - num6;
					}
				}
				else if (npc.direction == 1 && npc.velocity.X < num6){
					npc.velocity.X = npc.velocity.X + num4;
					if (npc.velocity.X < 0f - num6){
						npc.velocity.X = npc.velocity.X + num4;
					}
					else if (npc.velocity.X < 0f){
						npc.velocity.X = npc.velocity.X - num4 * 0.5f;
					}
					if (npc.velocity.X > num6){
						npc.velocity.X = num6;
					}
				}
				if (npc.directionY == -1 && npc.velocity.Y > 0f - num7){
					npc.velocity.Y = npc.velocity.Y - num5;
					if (npc.velocity.Y > num7){
						npc.velocity.Y = npc.velocity.Y - num5;
					}
					else if (npc.velocity.Y > 0f){
						npc.velocity.Y = npc.velocity.Y + num5 * 0.75f;
					}
					if (npc.velocity.Y < 0f - num7){
						npc.velocity.Y = 0f - num7;
					}
				}
				else if (npc.directionY == 1 && npc.velocity.Y < num7){
					npc.velocity.Y = npc.velocity.Y + num5;
					if (npc.velocity.Y < 0f - num7){
						npc.velocity.Y = npc.velocity.Y + num5;
					}
					else if (npc.velocity.Y < 0f){
						npc.velocity.Y = npc.velocity.Y - num5 * 0.75f;
					}
					if (npc.velocity.Y > num7){
						npc.velocity.Y = num7;
					}
				}
			}
			else{
				if (npc.direction == -1 && npc.velocity.X > -4f * boostfactor){
					npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
					if (npc.velocity.X > 4f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
					}
					else if (npc.velocity.X > 0f){
						npc.velocity.X = npc.velocity.X + 0.05f * boostfactor;
					}
					if (npc.velocity.X < -4f * boostfactor){
						npc.velocity.X = -4f * boostfactor;
					}
				}
				else if (npc.direction == 1 && npc.velocity.X < 4f * boostfactor){
					npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
					if (npc.velocity.X < -4f * boostfactor){
						npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
					}
					else if (npc.velocity.X < 0f){
						npc.velocity.X = npc.velocity.X - 0.05f * boostfactor;
					}
					if (npc.velocity.X > 4f * boostfactor){
						npc.velocity.X = 4f * boostfactor;
					}
				}
				if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5 * boostfactor){
					npc.velocity.Y = npc.velocity.Y - 0.04f * boostfactor;
					if ((double)npc.velocity.Y > 1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y - 0.05f * boostfactor;
					}
					else if (npc.velocity.Y > 0f){
						npc.velocity.Y = npc.velocity.Y + 0.03f * boostfactor;
					}
					if ((double)npc.velocity.Y < -1.5 * boostfactor){
						npc.velocity.Y = -1.5f * boostfactor;
					}
				}
				else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5 * boostfactor){
					npc.velocity.Y = npc.velocity.Y + 0.04f * boostfactor;
					if ((double)npc.velocity.Y < -1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y + 0.05f * boostfactor;
					}
					else if (npc.velocity.Y < 0f){
						npc.velocity.Y = npc.velocity.Y - 0.03f * boostfactor;
					}
					if ((double)npc.velocity.Y > 1.5 * boostfactor){
						npc.velocity.Y = 1.5f * boostfactor;
					}
				}
			}
			if (num == 49 || num == 51 || num == 60 || num == 62 || num == 66 || num == 93 || num == 137 || num == 150 || num == 151 || num == 152 || num == 634){
				if (npc.wet){
					if (npc.velocity.Y > 0f)
					{
						npc.velocity.Y = npc.velocity.Y * 0.95f * boostfactor;
					}
					npc.velocity.Y = npc.velocity.Y - 0.5f * boostfactor;
					if (npc.velocity.Y < -4f * boostfactor){
						npc.velocity.Y = -4f * boostfactor;
					}
					npc.TargetClosest(true);
				}
				if (num == 60){
					if (npc.direction == -1 && npc.velocity.X > -4f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
						if (npc.velocity.X > 4f * boostfactor){
							npc.velocity.X = npc.velocity.X - 0.07f * boostfactor;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.03f * boostfactor;
						}
						if (npc.velocity.X < -4f * boostfactor){
							npc.velocity.X = -4f * boostfactor;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 4f * boostfactor){
						npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
						if (npc.velocity.X < -4f * boostfactor){
							npc.velocity.X = npc.velocity.X + 0.07f * boostfactor;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.03f * boostfactor;
						}
						if (npc.velocity.X > 4f * boostfactor){
							npc.velocity.X = 4f * boostfactor;
						}
					}
					if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y - 0.04f * boostfactor;
						if ((double)npc.velocity.Y > 1.5 * boostfactor){
							npc.velocity.Y = npc.velocity.Y - 0.03f * boostfactor;
						}
						else if (npc.velocity.Y > 0f){
							npc.velocity.Y = npc.velocity.Y + 0.02f * boostfactor;
						}
						if ((double)npc.velocity.Y < -1.5 * boostfactor){
							npc.velocity.Y = -1.5f * boostfactor;
						}
					}
					else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y + 0.04f * boostfactor;
						if ((double)npc.velocity.Y < -1.5 * boostfactor){
							npc.velocity.Y = npc.velocity.Y + 0.03f * boostfactor;
						}
						else if (npc.velocity.Y < 0f){
							npc.velocity.Y = npc.velocity.Y - 0.02f * boostfactor;
						}
						if ((double)npc.velocity.Y > 1.5 * boostfactor){
							npc.velocity.Y = 1.5f * boostfactor;
						}
					}
				}
				else{
					if (npc.direction == -1 && npc.velocity.X > -4f * boostfactor){
						npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
						if (npc.velocity.X > 4f * boostfactor){
							npc.velocity.X = npc.velocity.X - 0.1f * boostfactor;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.05f * boostfactor;
						}
						if (npc.velocity.X < -4f * boostfactor){
							npc.velocity.X = -4f * boostfactor;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 4f * boostfactor){
						npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
						if (npc.velocity.X < -4f * boostfactor){
							npc.velocity.X = npc.velocity.X + 0.1f * boostfactor;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.05f * boostfactor;
						}
						if (npc.velocity.X > 4f * boostfactor){
							npc.velocity.X = 4f * boostfactor;
						}
					}
					if (npc.directionY == -1 && (double)npc.velocity.Y > -1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y - 0.04f * boostfactor;
						if ((double)npc.velocity.Y > 1.5 * boostfactor){
							npc.velocity.Y = npc.velocity.Y - 0.05f * boostfactor;
						}
						else if (npc.velocity.Y > 0f){
							npc.velocity.Y = npc.velocity.Y + 0.03f * boostfactor;
						}
						if ((double)npc.velocity.Y < -1.5 ){
							npc.velocity.Y = -1.5f * boostfactor;
						}
					}
					else if (npc.directionY == 1 && (double)npc.velocity.Y < 1.5 * boostfactor){
						npc.velocity.Y = npc.velocity.Y + 0.04f * boostfactor;
						if ((double)npc.velocity.Y < -1.5 * boostfactor){
							npc.velocity.Y = npc.velocity.Y + 0.05f * boostfactor;
						}
						else if (npc.velocity.Y < 0f){
							npc.velocity.Y = npc.velocity.Y - 0.03f * boostfactor;
						}
						if ((double)npc.velocity.Y > 1.5 * boostfactor){
							npc.velocity.Y = 1.5f * boostfactor;
						}
					}
				}
			}
			if (num == 48 && npc.wet){
				if (npc.velocity.Y > 0f){
					npc.velocity.Y = npc.velocity.Y * 0.95f * boostfactor;
				}
				npc.velocity.Y = npc.velocity.Y - 0.5f * boostfactor;
				if (npc.velocity.Y < -4f * boostfactor){
					npc.velocity.Y = -4f * boostfactor;
				}
				npc.TargetClosest(true);
			}
			if (num == 158 && Main.netMode != 1){
				Vector2 vector;
				vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
		
				float num8 = player.position.X + (float)player.width * 0.5f - vector.X;
				float num9 = player.position.Y + (float)player.height * 0.5f - vector.Y;
				if ((float)Math.Sqrt((double)(num8 * num8 + num9 * num9)) < 200f && npc.position.Y + (float)npc.height < player.position.Y + (float)player.height && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					npc.Transform(159);
				}
			}
			npc.ai[1] += 1f;
			if (num == 158){
				npc.ai[1] += 1f;
			}
			if (npc.ai[1] > 200f){
				if (!player.wet && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					npc.ai[1] = 0f;
				}
				float num10 = 0.2f;
				float num11 = 0.1f;
				float num12 = 4f;
				float num13 = 1.5f;
				if (num == 48 || num == 62 || num == 66){
					num10 = 0.12f;
					num11 = 0.07f;
					num12 = 3f;
					num13 = 1.25f;
				}
				if (npc.ai[1] > 1000f){
					npc.ai[1] = 0f;
				}
				npc.ai[2] += 1f;
				if (npc.ai[2] > 0f){
					if (npc.velocity.Y < num13){
						npc.velocity.Y = npc.velocity.Y + num11;
					}
				}
				else if (npc.velocity.Y > 0f - num13){
					npc.velocity.Y = npc.velocity.Y - num11;
				}
				if (npc.ai[2] < -150f || npc.ai[2] > 150f){
					if (npc.velocity.X < num12){
						npc.velocity.X = npc.velocity.X + num10;
					}
				}
				else if (npc.velocity.X > 0f - num12){
					npc.velocity.X = npc.velocity.X - num10;
				}
				if (npc.ai[2] > 300f){
					npc.ai[2] = -300f;
				}
			}
			if (Main.netMode == 1){
				return;
			}
			if (num == 48){
				npc.ai[0] += 1f;
				if (npc.ai[0] == 30f || npc.ai[0] == 60f || npc.ai[0] == 90f){
					if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						float num14 = 6f;
						Vector2 vector2;
						vector2 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num15 = player.position.X + (float)player.width * 0.5f - vector2.X + (float)Main.rand.Next(-100, 101);
						float num16 = player.position.Y + (float)player.height * 0.5f - vector2.Y + (float)Main.rand.Next(-100, 101);
						float num17 = (float)Math.Sqrt((double)(num15 * num15 + num16 * num16));
						num17 = num14 / num17;
						num15 *= num17;
						num16 *= num17;
						int num18 = 15;
						int num19 = 38;
						int num20 = Projectile.NewProjectile(npc.GetSource_FromThis(null), vector2.X, vector2.Y, num15, num16, num19, num18, 0f, Main.myPlayer, 0f, 0f, 0f);
						Main.projectile[num20].timeLeft = 300;
					}
				}
				else if (npc.ai[0] >= (float)(400 + Main.rand.Next(400))){
					npc.ai[0] = 0f;
				}
			}
			if (num == 62 || num == 66){
				npc.ai[0] += 1f;
				if (npc.ai[0] == 20f || npc.ai[0] == 40f || npc.ai[0] == 60f || npc.ai[0] == 80f){
					if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						float num21 = 0.2f;
						Vector2 vector3;
						vector3 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num22 = player.position.X + (float)player.width * 0.5f - vector3.X + (float)Main.rand.Next(-100, 101);
						float num23 = player.position.Y + (float)player.height * 0.5f - vector3.Y + (float)Main.rand.Next(-100, 101);
						float num24 = (float)Math.Sqrt((double)(num22 * num22 + num23 * num23));
						num24 = num21 / num24;
						num22 *= num24;
						num23 *= num24;
						int num25 = 21;
						int num26 = 44;
						int num27 = Projectile.NewProjectile(npc.GetSource_FromThis(null), vector3.X, vector3.Y, num22, num23, num26, num25, 0f, Main.myPlayer, 0f, 0f, 0f);
						Main.projectile[num27].timeLeft = 300;
					}
				}
				else if (npc.ai[0] >= (float)(300 + Main.rand.Next(300))){
					npc.ai[0] = 0f;
				}
			}
			if (num != 156){
				return;
			}
			npc.ai[0] += 1f;
			if (npc.ai[0] == 20f || npc.ai[0] == 40f || npc.ai[0] == 60f || npc.ai[0] == 80f || npc.ai[0] == 100f){
				if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					float num28 = 0.2f;
					Vector2 vector4;
					vector4 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num29 = player.position.X + (float)player.width * 0.5f - vector4.X + (float)Main.rand.Next(-50, 51);
					float num30 = player.position.Y + (float)player.height * 0.5f - vector4.Y + (float)Main.rand.Next(-50, 51);
					float num31 = (float)Math.Sqrt((double)(num29 * num29 + num30 * num30));
					num31 = num28 / num31;
					num29 *= num31;
					num30 *= num31;
					int num32 = 80;
					int num33 = 115;
					vector4 += npc.velocity * 5f * boostfactor;
					int num34 = Projectile.NewProjectile(npc.GetSource_FromThis(null), vector4.X + num29 * 100f, vector4.Y + num30 * 100f, num29, num30, num33, num32, 0f, Main.myPlayer, 0f, 0f, 0f);
					Main.projectile[num34].timeLeft = 300;
					return;
				}
			}
			else if (npc.ai[0] >= (float)(250 + Main.rand.Next(250))){
				npc.ai[0] = 0f;
			}
		}
   }
}
