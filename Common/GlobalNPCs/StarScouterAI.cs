using Microsoft.Xna.Framework;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.ModLoader.IO;
using ThoriumMod;
using ThoriumMod.NPCs;
using ThoriumMod.NPCs.BossStarScouter;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.Projectiles;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.Sounds;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class StarScouterAI : GlobalNPC{
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("StarScouter").Type;
		}
		
		public int framecounter = 0;
		public int counter;
		public int rage;
		public int laserTarget;
		public bool tripleSpawn;
		public int tripleSpawnTimer;
		public int animation_laserCannon;
		public int animation_laserCannonCount;
		public int animation_eyeCore;
		public int animation_eyeCoreCount;
		public int animation_eyeCore2;
		public int animation_eyeCoreCount2;
		
		public string Texture_scouter = "ThoriumMod/NPCs/BossStarScouter/StarScouter";
		
		public bool NoMinions(){
			return !NPC.npcsFoundForCheckActive[ModContent.NPCType<PyroCore>()] && !NPC.npcsFoundForCheckActive[ModContent.NPCType<BioCore>()] && !NPC.npcsFoundForCheckActive[ModContent.NPCType<CryoCore>()];
		}
		
		public override void FindFrame(NPC npc, int frameHeight){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().scouter)){
				if (framecounter > 3){
				counter++;
				if (counter >= 6){
					counter = 0;
				}
				framecounter = 0;
			}
			npc.frame.Y = counter * frameHeight;
				return;
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().scouter)) {
				Rectangle value1;
				value1 = new Rectangle(0, 0, 186, 84);
				value1.Y += 84 * animation_laserCannon;
				Rectangle value2;
				value2 = new Rectangle(0, 0, 186, 84);
				value2.Y += 84 * animation_eyeCore;
				Rectangle value3;
				value3 = new Rectangle(0, 0, 186, 84);
				value3.Y += 84 * animation_eyeCore2;
				if (npc.IsABestiaryIconDummy){
					Vector2 vector;
					vector = new Vector2(npc.Center.X, npc.Center.Y + 9f);
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Dome", AssetRequestMode.ImmediateLoad).Value, vector - screenPos, new Rectangle?(npc.frame), Color.White * 0.9f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_LaserCannon", AssetRequestMode.ImmediateLoad).Value, vector - screenPos, new Rectangle?(value1), drawColor, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye0", AssetRequestMode.ImmediateLoad).Value, vector - screenPos, new Rectangle?(value2), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
					return;
				}
				else if (((double)npc.life >= (double)npc.lifeMax * 0.25 || !tripleSpawn)){
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Dome", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.9f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_LaserCannon", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(value1), drawColor, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
				}
				if (npc.ai[2] >= 60f){
					spriteBatch.Draw(ModContent.Request<Texture2D>("ThoriumMod/Textures/Sword_Indicator", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, null, Color.White, 0f, new Vector2(13f, 100f), 1f, 0, 0f);
				}
				if (tripleSpawnTimer > 0 && tripleSpawnTimer < 180){
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Glow2", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
				}
				else{
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Glow", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.4f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
				}
				if (NoMinions()){
					if (npc.ai[3] < 1f){
						if (npc.ai[2] < 60f && npc.HasValidTarget){
							Player player = Main.player[npc.target];
							if (player.Center.Y < npc.Center.Y){
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye0", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
							else if (player.position.X < npc.Center.X - 150f){
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye1", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
							else if (player.position.X < npc.Center.X - 40f){
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye2", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
							else if (player.position.X > npc.Center.X + 40f){
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye4", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
							else if (player.position.X > npc.Center.X + 150f){
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye5", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
							else{
								spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_Eye3", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(npc.frame), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
							}
						}
						else{
							spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_EyeBeam", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(value2), Color.White, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
						}
					}
					else{
						spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_EyeGraviton", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(value3), Color.White, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
					}
				}
				else{
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_EyeWaiting", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(value2), Color.White * 0.85f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
				}
				if (tripleSpawnTimer > 0 && tripleSpawnTimer < 180){
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_scouter + "_EyeWaiting", AssetRequestMode.ImmediateLoad).Value, npc.Center - screenPos, new Rectangle?(value2), Color.White * 1f, npc.rotation, new Vector2(93f, 46f), npc.scale, 0, 0f);
				}
			}
		}
		
		public override bool PreAI(NPC npc) {
			if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().scouter))) {
					return true;
			}
			if(CalamityGamemodeCheck.isBossrush){	
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}	
				NPCHelper.BatAI(npc, 0);
				Lighting.AddLight(npc.Center, 0.25f, 0.15f, 0.3f);
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y - 0.3f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (npc.ai[2] > 60f){
					animation_laserCannonCount += 3;
				}
				else{
					animation_laserCannonCount++;
				}
				if (animation_laserCannonCount > 4){
					animation_laserCannon++;
					if (animation_laserCannon >= 4){
						animation_laserCannon = 0;
					}
					animation_laserCannonCount = 0;
				}
				animation_eyeCoreCount++;
				if (animation_eyeCoreCount > 2){
					animation_eyeCore++;
					if (animation_eyeCore >= 8){
						animation_eyeCore = 0;
					}
					animation_eyeCoreCount = 0;
				}
				animation_eyeCoreCount2++;
				if (animation_eyeCoreCount2 > 2){
					animation_eyeCore2++;
					if (animation_eyeCore2 >= 12){
						animation_eyeCore2 = 0;
					}
					animation_eyeCoreCount2 = 0;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){
					rage = 125; // 125 instead of 120
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5){
					rage = 110; // 110 instead of 90
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75){
					rage = 90; // 90 instead of 60
				}
				if (npc.ai[2] < 60f){
					if (player.position.Y < npc.position.Y + 250f){
						NPC npc2 = npc;
						npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.6f : 0.18f);
					}
					if (player.position.Y > npc.position.Y + 250f){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.6f : 0.18f);
					}
					npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -24f, 24f);
					if (player.Center.X < npc.Center.X){
						NPC npc4 = npc;
						npc4.velocity.X = npc4.velocity.X - 0.2f;
					}
					if (player.Center.X > npc.Center.X){
						NPC npc5 = npc;
						npc5.velocity.X = npc5.velocity.X + 0.2f;
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -24f, 24f); // Much quicker movement
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.9){  // Add Beam to moveset
					npc.ai[2] += 1f;
					if (npc.ai[2] == 60f){
						laserTarget = npc.target;
						npc.netUpdate = true;
						if (Main.netMode != 1){
							SoundEngine.PlaySound(SoundID.Zombie67, new Vector2?(npc.Center), null);
						}
						float num = 35f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector = Vector2.UnitX * 0f;
							vector += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(48f, 48f);
							vector = Terraria.Utils.RotatedBy(vector, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
							int num3 = Dust.NewDust(player.Center, 0, 0, 173, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = player.Center + vector;
							Main.dust[num3].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector, Vector2.UnitY) * 8f);
							num2++;
						}
					}
					if (npc.ai[2] >= 60f){
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[3] = -60f;
						Player player2 = Main.player[laserTarget];
						if (player2.position.Y < npc.position.Y + 165f){
							NPC npc6 = npc;
							npc6.velocity.Y = npc6.velocity.Y - ((npc.velocity.Y > 0f) ? 5f : 0.65f);
						}
						if (player2.position.Y > npc.position.Y + 165f){
							NPC npc7 = npc;
							npc7.velocity.Y = npc7.velocity.Y + ((npc.velocity.Y < 0f) ? 5f : 0.65f);
						}
						if (player2.Center.X < npc.Center.X){
							NPC npc8 = npc;
							npc8.velocity.X = npc8.velocity.X - ((npc.velocity.X > 0f) ? 4f : 0.5f);
						}
						if (player2.Center.X > npc.Center.X){
							NPC npc9 = npc;
							npc9.velocity.X = npc9.velocity.X + ((npc.velocity.X < 0f) ? 4f : 0.5f);
						}
					}
					if (npc.ai[2] > 150f){  // Pauses to prepare shot
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
					}
					if (npc.ai[2] >= 180f){
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeam>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeamCheese>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.TargetClosest(true);
						npc.ai[2] = -420f;
						npc.netUpdate = true;
					}
				}
				else{
					npc.ai[2] = -60f;
				}
				if ((double)npc.life >= (double)npc.lifeMax * 0.6){  // Behaviour while high health
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 0f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 vector2;
							vector2 = new Vector2(npc.Center.X, npc.Center.Y - 30f);
							Vector2 vector3 = player.DirectionFrom(vector2);
							Projectile.NewProjectile(source_FromAI, vector2, vector3 * 18f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.8){ //Graviton Surge
					npc.ai[3] += 1f;
					if (npc.ai[3] >= 60f){
						if ((double)npc.life < (double)npc.lifeMax * 0.4){
							npc.ai[1] -= 1f;
						}
						npc.ai[2] -= 1f;
						Vector2 vector4 = player.DirectionFrom(npc.Center);
						if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 6f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = (float)(-300 + rage);
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 10f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = -30f;
						}
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.6){  // Vaporize Blast
					Vector2 vector5 = player.DirectionFrom(npc.Center);
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 30f && npc.ai[0] <= 60f && npc.ai[0] % 10f == 0f && Main.netMode != 1){
						Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 18f, ModContent.ProjectileType<Vaporize>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
					}
					if (npc.ai[0] == 50f && (double)npc.life > (double)npc.lifeMax * 0.25){
						npc.ai[0] = (float)(-150 + rage);
					}
					if (!player.dead && npc.ai[0] >= 60f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 18f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5){ // Graviton Charge
					npc.ai[1] += 1f;
					if (!player.dead && npc.ai[1] > 120f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), Terraria.Utils.NextFloat(Main.rand, -8f, -5f), ModContent.ProjectileType<GravitonCharge>(), 30, 0f, Main.myPlayer, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), 0f, 0f);
						}
						npc.ai[1] = (float)(-240 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){
					tripleSpawnTimer++;
					if (tripleSpawnTimer % 30 == 0 && !tripleSpawn){
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
					}
					if (tripleSpawnTimer < 180 && !tripleSpawn){
						npc.velocity = Vector2.Zero;
						npc.dontTakeDamage = true;
						npc.behindTiles = true;
						npc.chaseable = false;
						npc.alpha = 75;
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
					if (tripleSpawnTimer >= 180 && !tripleSpawn){
						if (Main.netMode != 1){
							NPC.NewNPC(source_FromAI, (int)npc.Center.X + 80, (int)npc.Center.Y, ModContent.NPCType<CryoCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 60, ModContent.NPCType<BioCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X - 80, (int)npc.Center.Y, ModContent.NPCType<PyroCore>(), 0, 0f, 0f, 0f, 0f, 255);
						}
						if (Main.netMode != 2){
							SoundEngine.PlaySound(SoundID.Item33, new Vector2?(npc.Center), null);
							SoundEngine.PlaySound(SoundID.NPCHit44, new Vector2?(npc.Center), null);
							for (int i = 0; i < 25; i++){
								Dust.NewDustDirect(npc.Center, 20, 20, 173, Terraria.Utils.NextFloat(Main.rand, -15f, 15f), Terraria.Utils.NextFloat(Main.rand, -15f, 15f), 0, default(Color), 2.5f).noGravity = true;
							}
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore1").Type, npc.scale);
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore2").Type, npc.scale);
						}
						tripleSpawn = true;
						npc.netUpdate = true;
						return false;
					}
					if (NoMinions()){
						npc.dontTakeDamage = false;
						npc.behindTiles = false;
						npc.chaseable = true;
						npc.alpha = 0;
						return false;
					}
					npc.dontTakeDamage = true;
					npc.behindTiles = true;
					npc.chaseable = false;
					npc.alpha = 75;
					npc.ai[0] = -60f;
					npc.ai[1] = -60f;
					npc.ai[2] = -60f;
					npc.ai[3] = -60f;
				}
				return false;
			}
			else if (CalamityGamemodeCheck.isDeath){
				NPCHelper.BatAI(npc, 0);
				Lighting.AddLight(npc.Center, 0.25f, 0.15f, 0.3f);
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y - 0.3f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (npc.ai[2] > 60f){
					animation_laserCannonCount += 3;
				}
				else{
					animation_laserCannonCount++;
				}
				if (animation_laserCannonCount > 4){
					animation_laserCannon++;
					if (animation_laserCannon >= 4){
						animation_laserCannon = 0;
					}
					animation_laserCannonCount = 0;
				}
				animation_eyeCoreCount++;
				if (animation_eyeCoreCount > 2){
					animation_eyeCore++;
					if (animation_eyeCore >= 8){
						animation_eyeCore = 0;
					}
					animation_eyeCoreCount = 0;
				}
				animation_eyeCoreCount2++;
				if (animation_eyeCoreCount2 > 2){
					animation_eyeCore2++;
					if (animation_eyeCore2 >= 12){
						animation_eyeCore2 = 0;
					}
					animation_eyeCoreCount2 = 0;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){
					rage = 125; // 125 instead of 120
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5){
					rage = 110; // 110 instead of 90
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75){
					rage = 80; // 80 instead of 60
				}
				if (npc.ai[2] < 60f){
					if (player.position.Y < npc.position.Y + 250f){
						NPC npc2 = npc;
						npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.8f : 0.09f);
					}
					if (player.position.Y > npc.position.Y + 250f){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.8f : 0.09f);
					}
					npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
					if (player.Center.X < npc.Center.X){
						NPC npc4 = npc;
						npc4.velocity.X = npc4.velocity.X - 0.1f;
					}
					if (player.Center.X > npc.Center.X){
						NPC npc5 = npc;
						npc5.velocity.X = npc5.velocity.X + 0.1f;
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -14f, 14f);
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.9){  // Add Beam to moveset
					npc.ai[2] += 1f;
					if (npc.ai[2] == 60f){
						laserTarget = npc.target;
						npc.netUpdate = true;
						if (Main.netMode != 1){
							SoundEngine.PlaySound(SoundID.Zombie67, new Vector2?(npc.Center), null);
						}
						float num = 35f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector = Vector2.UnitX * 0f;
							vector += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(48f, 48f);
							vector = Terraria.Utils.RotatedBy(vector, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
							int num3 = Dust.NewDust(player.Center, 0, 0, 173, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = player.Center + vector;
							Main.dust[num3].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector, Vector2.UnitY) * 8f);
							num2++;
						}
					}
					if (npc.ai[2] >= 60f){
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[3] = -60f;
						Player player2 = Main.player[laserTarget];
						if (player2.position.Y < npc.position.Y + 165f){
							NPC npc6 = npc;
							npc6.velocity.Y = npc6.velocity.Y - ((npc.velocity.Y > 0f) ? 3f : 0.45f);   // Adjust the movement speed a bit faster
						}
						if (player2.position.Y > npc.position.Y + 165f){
							NPC npc7 = npc;
							npc7.velocity.Y = npc7.velocity.Y + ((npc.velocity.Y < 0f) ? 3f : 0.45f);
						}
						if (player2.Center.X < npc.Center.X){
							NPC npc8 = npc;
							npc8.velocity.X = npc8.velocity.X - ((npc.velocity.X > 0f) ? 2.5f : 0.35f);
						}
						if (player2.Center.X > npc.Center.X){
							NPC npc9 = npc;
							npc9.velocity.X = npc9.velocity.X + ((npc.velocity.X < 0f) ? 2.5f : 0.35f);
						}
					}
					if (npc.ai[2] > 150f){  // Pauses to prepare shot
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
					}
					if (npc.ai[2] >= 180f){
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeam>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeamCheese>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.TargetClosest(true);
						npc.ai[2] = -420f;
						npc.netUpdate = true;
					}
				}
				else{
					npc.ai[2] = -60f;
				}
				if ((double)npc.life >= (double)npc.lifeMax * 0.6){  // Behaviour while high health
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 0f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 vector2;
							vector2 = new Vector2(npc.Center.X, npc.Center.Y - 30f);
							Vector2 vector3 = player.DirectionFrom(vector2);
							Projectile.NewProjectile(source_FromAI, vector2, vector3 * 10f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.8){ //Graviton Surge
					npc.ai[3] += 1f;
					if (npc.ai[3] >= 60f){
						if ((double)npc.life < (double)npc.lifeMax * 0.4){
							npc.ai[1] -= 1f;
						}
						npc.ai[2] -= 1f;
						Vector2 vector4 = player.DirectionFrom(npc.Center);
						if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 6f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = (float)(-300 + rage);
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 10f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = -30f;
						}
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.6){  // Vaporize Blast
					Vector2 vector5 = player.DirectionFrom(npc.Center);
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 30f && npc.ai[0] <= 60f && npc.ai[0] % 10f == 0f && Main.netMode != 1){
						Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 10f, ModContent.ProjectileType<Vaporize>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
					}
					if (npc.ai[0] == 50f && (double)npc.life > (double)npc.lifeMax * 0.25){
						npc.ai[0] = (float)(-150 + rage);
					}
					if (!player.dead && npc.ai[0] >= 60f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 10f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5){ // Graviton Charge
					npc.ai[1] += 1f;
					if (!player.dead && npc.ai[1] > 120f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), Terraria.Utils.NextFloat(Main.rand, -8f, -5f), ModContent.ProjectileType<GravitonCharge>(), 30, 0f, Main.myPlayer, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), 0f, 0f);
						}
						npc.ai[1] = (float)(-240 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){ 
					tripleSpawnTimer++;
					if (tripleSpawnTimer % 30 == 0 && !tripleSpawn){
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
					}
					if (tripleSpawnTimer < 180 && !tripleSpawn){
						npc.velocity = Vector2.Zero;
						npc.dontTakeDamage = true;
						npc.behindTiles = true;
						npc.chaseable = false;
						npc.alpha = 75;
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
					if (tripleSpawnTimer >= 180 && !tripleSpawn){
						if (Main.netMode != 1){
							NPC.NewNPC(source_FromAI, (int)npc.Center.X + 80, (int)npc.Center.Y, ModContent.NPCType<CryoCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 60, ModContent.NPCType<BioCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X - 80, (int)npc.Center.Y, ModContent.NPCType<PyroCore>(), 0, 0f, 0f, 0f, 0f, 255);
						}
						if (Main.netMode != 2){
							SoundEngine.PlaySound(SoundID.Item33, new Vector2?(npc.Center), null);
							SoundEngine.PlaySound(SoundID.NPCHit44, new Vector2?(npc.Center), null);
							for (int i = 0; i < 25; i++){
								Dust.NewDustDirect(npc.Center, 20, 20, 173, Terraria.Utils.NextFloat(Main.rand, -15f, 15f), Terraria.Utils.NextFloat(Main.rand, -15f, 15f), 0, default(Color), 2.5f).noGravity = true;
							}
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore1").Type, npc.scale);
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore2").Type, npc.scale);
						}
						tripleSpawn = true;
						npc.netUpdate = true;
						return false;
					}
					if (NoMinions()){
						npc.dontTakeDamage = false;
						npc.behindTiles = false;
						npc.chaseable = true;
						npc.alpha = 0;
						return false;
					}
					npc.dontTakeDamage = true;
					npc.behindTiles = true;
					npc.chaseable = false;
					npc.alpha = 75;
					npc.ai[0] = -60f;
					npc.ai[1] = -60f;
					npc.ai[2] = -60f;
					npc.ai[3] = -60f;
				}
				return false;
			}				
			else if(CalamityGamemodeCheck.isRevengeance) {
				NPCHelper.BatAI(npc, 0);
				Lighting.AddLight(npc.Center, 0.25f, 0.15f, 0.3f);
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y - 0.3f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (npc.ai[2] > 60f){
					animation_laserCannonCount += 3;
				}
				else{
					animation_laserCannonCount++;
				}
				if (animation_laserCannonCount > 4){
					animation_laserCannon++;
					if (animation_laserCannon >= 4){
						animation_laserCannon = 0;
					}
					animation_laserCannonCount = 0;
				}
				animation_eyeCoreCount++;
				if (animation_eyeCoreCount > 2){
					animation_eyeCore++;
					if (animation_eyeCore >= 8){
						animation_eyeCore = 0;
					}
					animation_eyeCoreCount = 0;
				}
				animation_eyeCoreCount2++;
				if (animation_eyeCoreCount2 > 2){
					animation_eyeCore2++;
					if (animation_eyeCore2 >= 12){
						animation_eyeCore2 = 0;
					}
					animation_eyeCoreCount2 = 0;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){
					rage = 125; // 125 instead of 120
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5){
					rage = 100; // 100 instead of 90
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75){
					rage = 70; // 70 instead of 60
				}
				if (npc.ai[2] < 60f){
					if (player.position.Y < npc.position.Y + 250f){
						NPC npc2 = npc;
						npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.8f : 0.09f);
					}
					if (player.position.Y > npc.position.Y + 250f){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.8f : 0.09f);
					}
					npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
					if (player.Center.X < npc.Center.X){
						NPC npc4 = npc;
						npc4.velocity.X = npc4.velocity.X - 0.1f;
					}
					if (player.Center.X > npc.Center.X){
						NPC npc5 = npc;
						npc5.velocity.X = npc5.velocity.X + 0.1f;
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -14f, 14f);
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.9){  // Add Beam to moveset
					npc.ai[2] += 1f;
					if (npc.ai[2] == 60f){
						laserTarget = npc.target;
						npc.netUpdate = true;
						if (Main.netMode != 1){
							SoundEngine.PlaySound(SoundID.Zombie67, new Vector2?(npc.Center), null);
						}
						float num = 35f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector = Vector2.UnitX * 0f;
							vector += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(48f, 48f);
							vector = Terraria.Utils.RotatedBy(vector, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
							int num3 = Dust.NewDust(player.Center, 0, 0, 173, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = player.Center + vector;
							Main.dust[num3].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector, Vector2.UnitY) * 8f);
							num2++;
						}
					}
					if (npc.ai[2] >= 60f){
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[3] = -60f;
						Player player2 = Main.player[laserTarget];
						if (player2.position.Y < npc.position.Y + 165f){
							NPC npc6 = npc;
							npc6.velocity.Y = npc6.velocity.Y - ((npc.velocity.Y > 0f) ? 2.75f : 0.4f);   // Adjust the movement speed a bit faster
						}
						if (player2.position.Y > npc.position.Y + 165f){
							NPC npc7 = npc;
							npc7.velocity.Y = npc7.velocity.Y + ((npc.velocity.Y < 0f) ? 2.75f : 0.4f);
						}
						if (player2.Center.X < npc.Center.X){
							NPC npc8 = npc;
							npc8.velocity.X = npc8.velocity.X - ((npc.velocity.X > 0f) ? 2.25f : 0.3f);
						}
						if (player2.Center.X > npc.Center.X){
							NPC npc9 = npc;
							npc9.velocity.X = npc9.velocity.X + ((npc.velocity.X < 0f) ? 2.25f : 0.3f);
						}
					}
					if (npc.ai[2] > 150f){  // Pauses to prepare shot
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
					}
					if (npc.ai[2] >= 180f){
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X - 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeam>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 42f, 0f, 5f, ModContent.ProjectileType<MainBeamCheese>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 3f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X + 7f, npc.Center.Y + 42f, 0f, 6f, ModContent.ProjectileType<MainBeamOuter>(), 35, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.TargetClosest(true);
						npc.ai[2] = -420f;
						npc.netUpdate = true;
					}
				}
				else{
					npc.ai[2] = -60f;
				}
				if ((double)npc.life >= (double)npc.lifeMax * 0.6){  // Behaviour while high health
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 0f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 vector2;
							vector2 = new Vector2(npc.Center.X, npc.Center.Y - 30f);
							Vector2 vector3 = player.DirectionFrom(vector2);
							Projectile.NewProjectile(source_FromAI, vector2, vector3 * 10f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.8){ //Graviton Surge
					npc.ai[3] += 1f;
					if (npc.ai[3] >= 60f){
						if ((double)npc.life < (double)npc.lifeMax * 0.4){
							npc.ai[1] -= 1f;
						}
						npc.ai[2] -= 1f;
						Vector2 vector4 = player.DirectionFrom(npc.Center);
						if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 6f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = (float)(-300 + rage);
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center, vector4 * 10f, ModContent.ProjectileType<GravitonSurge>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							npc.ai[3] = -30f;
						}
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.6){  // Vaporize Blast
					Vector2 vector5 = player.DirectionFrom(npc.Center);
					npc.ai[0] += 1f;
					if (!player.dead && npc.ai[0] >= 30f && npc.ai[0] <= 60f && npc.ai[0] % 10f == 0f && Main.netMode != 1){
						Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 10f, ModContent.ProjectileType<Vaporize>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
					}
					if (npc.ai[0] == 50f && (double)npc.life > (double)npc.lifeMax * 0.25){
						npc.ai[0] = (float)(-150 + rage);
					}
					if (!player.dead && npc.ai[0] >= 60f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center, vector5 * 10f, ModContent.ProjectileType<VaporizeBlast>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots a bit faster
						}
						npc.ai[0] = (float)(-150 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5){ // Graviton Charge
					npc.ai[1] += 1f;
					if (!player.dead && npc.ai[1] > 120f){
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), Terraria.Utils.NextFloat(Main.rand, -8f, -5f), ModContent.ProjectileType<GravitonCharge>(), 30, 0f, Main.myPlayer, Terraria.Utils.NextFloat(Main.rand, -1f, 1f), 0f, 0f);
						}
						npc.ai[1] = (float)(-240 + rage);
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.25){ 
					tripleSpawnTimer++;
					if (tripleSpawnTimer % 30 == 0 && !tripleSpawn){
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
					}
					if (tripleSpawnTimer < 180 && !tripleSpawn){
						npc.velocity = Vector2.Zero;
						npc.dontTakeDamage = true;
						npc.behindTiles = true;
						npc.chaseable = false;
						npc.alpha = 75;
						npc.ai[0] = -60f;
						npc.ai[1] = -60f;
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
					if (tripleSpawnTimer >= 180 && !tripleSpawn){
						if (Main.netMode != 1){
							NPC.NewNPC(source_FromAI, (int)npc.Center.X + 80, (int)npc.Center.Y, ModContent.NPCType<CryoCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y - 60, ModContent.NPCType<BioCore>(), 0, 0f, 0f, 0f, 0f, 255);
							NPC.NewNPC(source_FromAI, (int)npc.Center.X - 80, (int)npc.Center.Y, ModContent.NPCType<PyroCore>(), 0, 0f, 0f, 0f, 0f, 255);
						}
						if (Main.netMode != 2){
							SoundEngine.PlaySound(SoundID.Item33, new Vector2?(npc.Center), null);
							SoundEngine.PlaySound(SoundID.NPCHit44, new Vector2?(npc.Center), null);
							for (int i = 0; i < 25; i++){
								Dust.NewDustDirect(npc.Center, 20, 20, 173, Terraria.Utils.NextFloat(Main.rand, -15f, 15f), Terraria.Utils.NextFloat(Main.rand, -15f, 15f), 0, default(Color), 2.5f).noGravity = true;
							}
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore1").Type, npc.scale);
							Gore.NewGore(source_FromAI, new Vector2(npc.Center.X, npc.Center.Y - 46f), npc.velocity, thorium.Find<ModGore>("StarScouterGore2").Type, npc.scale);
						}
						tripleSpawn = true;
						npc.netUpdate = true;
						return false;
					}
					if (NoMinions()){
						npc.dontTakeDamage = false;
						npc.behindTiles = false;
						npc.chaseable = true;
						npc.alpha = 0;
						return false;
					}
					npc.dontTakeDamage = true;
					npc.behindTiles = true;
					npc.chaseable = false;
					npc.alpha = 75;
					npc.ai[0] = -60f;
					npc.ai[1] = -60f;
					npc.ai[2] = -60f;
					npc.ai[3] = -60f;
				}
				return false;
			}
			else {
					return true;
			}
		}
	}
}