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
    public class JellyfishesAI : GlobalNPC
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return (
			npc.type == thorium.Find<ModNPC>("DistractingJellyfish").Type
			|| npc.type == thorium.Find<ModNPC>("ZealousJellyfish").Type
			|| npc.type == thorium.Find<ModNPC>("SpittingJellyfish").Type
			);
		}
		
		public override bool PreAI(NPC npc) {
			if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().jelly))) {
					return true;
			}
			if(CalamityGamemodeCheck.isBossrush)  {
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}
				if (npc.type == thorium.Find<ModNPC>("DistractingJellyfish").Type){
					NPC npc1 = Main.npc[(int)npc.ai[0]];
					if (!npc1.active){
						npc.active = false;
						npc.netUpdate = true;
					}
					if (npc.ai[1] == 0f){
						Vector2 vector;
						vector = new Vector2(npc.Center.X, npc.Center.Y);
						float num = npc1.Center.X - vector.X;
						float num2 = npc1.Center.Y - vector.Y;
						float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
						if (num3 > 90f){
							num3 = 8f / num3;
							num *= num3;
							num2 *= num3;
							npc.velocity.X = (npc.velocity.X * 15f + num) / 16f;
							npc.velocity.Y = (npc.velocity.Y * 15f + num2) / 16f;
						}
						if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < 15f){
							npc.velocity.Y = npc.velocity.Y * 1.07f;
							npc.velocity.X = npc.velocity.X * 1.07f;
						}
						if (Main.netMode != 1 && (( Terraria.Utils.NextBool(Main.rand, 100)) || Terraria.Utils.NextBool(Main.rand, 200))){
							npc.TargetClosest(true);
							vector = new Vector2(npc.Center.X, npc.Center.Y);
							num = Main.player[npc.target].Center.X - vector.X;
							num2 = Main.player[npc.target].Center.Y - vector.Y;
							num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
							if (num3 > 0f){
								num3 = 8f / num3;
							}
							npc.velocity.X = num * num3 * 1.5f;
							npc.velocity.Y = num2 * num3 * 1.5f;
							npc.ai[1] = 1f;
							npc.netUpdate = true;
							return false;
						}
					}
					else{
						Vector2 vector2 = Main.player[npc.target].Center - npc.Center;
						vector2.Normalize();
						vector2 *= 13f;
						npc.velocity = (npc.velocity * 99f + vector2) / 100f;
						Vector2 vector3;
						vector3 = new Vector2(npc.Center.X, npc.Center.Y);
						float num4 = npc1.Center.X - vector3.X;
						float num5 = npc1.Center.Y - vector3.Y;
						if ((float)Math.Sqrt((double)(num4 * num4 + num5 * num5)) > 700f || npc.justHit){
							npc.ai[1] = 0f;
						}
					}
					return false;
				}
				else if(npc.type == thorium.Find<ModNPC>("ZealousJellyfish").Type) {
					NPCHelper.BatAI(npc, thorium.Find<ModNPC>("ZealousJellyfish").AIType, 2f);
					Player player = Main.player[npc.target];
					if (!player.active || player.dead){
						npc.active = false;
						return false;
					}
					if (player.position.Y - 150f > npc.position.Y){
						npc.directionY = 1;
					}
					else{
						npc.directionY = -1;
					}
					if (npc.direction == -1 && npc.velocity.X > -4f){
						npc.velocity.X = npc.velocity.X - 0.8f;
						if (npc.velocity.X > 4f){
							npc.velocity.X = npc.velocity.X - 0.8f;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.16f;
						}
						if (npc.velocity.X < -4f){
							npc.velocity.X = -4f;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 8f){
						npc.velocity.X = npc.velocity.X + 0.2f;
						if (npc.velocity.X < -4f){
							npc.velocity.X = npc.velocity.X + 0.2f;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.16f;
						}
						if (npc.velocity.X > 4f){
							npc.velocity.X = 4f;
						}
					}
					if (npc.directionY == -1 && (double)npc.velocity.Y > -3.0){
						npc.velocity.Y = npc.velocity.Y - 0.16f;
						if ((double)npc.velocity.Y < -3.0){
							npc.velocity.Y = -3.0f;
						}
					}
					else if (npc.directionY == 1 && (double)npc.velocity.Y < 3.0){
						npc.velocity.Y = npc.velocity.Y + 0.16f;
						if ((double)npc.velocity.Y > 3.0){
							npc.velocity.Y = 3.0f;
						}
					}
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 180f){
						npc.velocity = npc.DirectionTo(player.Center) * 16f;
						npc.ai[1] = 0f;
					}
					return false;	
				}
				else if(npc.type == thorium.Find<ModNPC>("SpittingJellyfish").Type){
					NPCHelper.BatAI(npc, thorium.Find<ModNPC>("SpittingJellyfish").AIType, 2f);
					Player player = Main.player[npc.target];
					if (!player.active || player.dead){
						npc.active = false;
						return false;
					}
					if (player.position.Y < npc.position.Y + 350f){
						NPC npc1 = npc;
						npc.velocity.Y = npc.velocity.Y - ((npc.velocity.Y > 0f) ? 1.2f : 0.14f);
					}
					if (player.position.Y > npc.position.Y + 350f){
						NPC npc2 = npc;
						npc2.velocity.Y = npc2.velocity.Y + ((npc.velocity.Y < 0f) ? 1.2f : 0.14f);
					}
					npc.ai[2] += 1f;
					if (npc.ai[1] == 0f){
						if (player.position.X < npc.position.X && npc.velocity.X > -16f){
							NPC npc3 = npc;
							npc3.velocity.X = npc3.velocity.X - 0.2f;
						}
						if (player.position.X > npc.position.X && npc.velocity.X < 16f){
							NPC npc4 = npc;
							npc4.velocity.X = npc4.velocity.X + 0.2f;
						}
						if (player.position.Y < npc.position.Y + 300f){
							if (npc.velocity.Y < 0f){
								if (npc.velocity.Y > -8f){
									NPC npc5 = npc;
									npc5.velocity.Y = npc5.velocity.Y - 0.8f;
								}
							}
							else{
								NPC npc6 = npc;
								npc6.velocity.Y = npc6.velocity.Y - 1.6f;
							}
						}
						if (player.position.Y > npc.position.Y + 300f){
							if (npc.velocity.Y > 0f){
								if (npc.velocity.Y < 8f){
									NPC npc7 = npc;
									npc7.velocity.Y = npc7.velocity.Y + 0.8f;
								}
							}
							else{
								NPC npc8 = npc;
								npc8.velocity.Y = npc8.velocity.Y + 1.2f;
							}
						}
					}
					if (!player.dead && npc.ai[2] >= 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
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
			else {
				return true;
			}
		}
	}
}