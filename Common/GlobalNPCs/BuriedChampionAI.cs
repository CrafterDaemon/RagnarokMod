using System.IO;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
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
using ThoriumMod.Buffs;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.NPCs.BossBuriedChampion;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class BuriedChampionAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("BuriedChampion").Type;
		}
		public int phaseSwapTimer;
		public int counter;
		public int flux;
		public bool shift;
		public bool sideRight;
		public int shifted;
		public int angry;
		public int attackState;
		public bool strike;
		public bool charge;
		public bool charging;
		public int framecounter = 0;
		public string Texture_champion => "ThoriumMod/NPCs/BossBuriedChampion/BuriedChampion";
	
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter writer){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok) {
				BitsByte bitsByte = default(BitsByte);
				bitsByte[0] = (npc.aiStyle == -1);
				bitsByte[1] = sideRight;
				bitsByte[2] = strike;
				bitsByte[3] = charge;
				bitsByte[4] = charging;
				writer.Write(bitsByte);
			}
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader reader){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok) {
				BitsByte bitsByte = reader.ReadByte();
				npc.aiStyle = (bitsByte[0] ? -1 : -2);
				sideRight = bitsByte[1];
				strike = bitsByte[2];
				charge = bitsByte[3];
				charging = bitsByte[4];
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok) {
				if (phaseSwapTimer > 0){
				Texture2D value = ModContent.Request<Texture2D>(Texture_champion + "_Effect", AssetRequestMode.ImmediateLoad).Value;
				SpriteEffects spriteEffects = (SpriteEffects)((npc.spriteDirection < 0) ? 0 : 1);
				Vector2 vector = npc.Center - screenPos;
				Vector2 vector2;
				vector2 = new Vector2((float)(value.Width / 2), (float)(value.Height / 2 / Main.npcFrameCount[npc.type]));
				spriteBatch.Draw(value, vector, new Rectangle?(npc.frame), new Color(255, 255, 255, 50) * ((float)phaseSwapTimer * 0.025f), npc.rotation, vector2, npc.scale, spriteEffects, 0f);
				}
			}
		}
		public override void FindFrame(NPC npc, int frameHeight){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok) {
				if (strike){
					framecounter++;
					if (framecounter > 6){
						counter++;
						if (counter >= 14){
							strike = false;
							counter = 0;
						}
						framecounter = 0;
					}
				}
				if (charge){
					counter = 13;
				}
				if (attackState == 0 && !strike && !charge){ // Sword phase
					framecounter++;
					if (framecounter > 4){
						counter++;
						if (counter >= 4){
							counter = 0;
						}
						framecounter = 0;
					}
				}
				if (attackState == 1){ // Bow phase
					framecounter++;
					if (framecounter > 6){
						counter++;
						if (counter >= 8){
							counter = 4;
						}
						framecounter = 0;
					}
				}
				if (attackState == 2){ // Staff phase
					framecounter ++;
					if (framecounter > 4){
						counter++;
						if (counter >= 12){
							counter = 8;
						}
						framecounter = 0;
					}
				}
				npc.frame.Y = counter * frameHeight;
				return;
			}
		}
		
		public override bool PreAI(NPC npc) 
		{
			if(!(ModContent.GetInstance<BossConfig>().viscount == ThoriumBossRework_selection_mode.Ragnarok)) {
					return true;
			}
			if(CalamityGamemodeCheck.isBossrush || CalamityGamemodeCheck.isRevengeance) {	
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
				}
				if (phaseSwapTimer > 0){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					phaseSwapTimer--;
					return false;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.5f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				Vector2 vector = npc.DirectionTo(player.Center);
				if ((double)npc.life >= (double)npc.lifeMax * 0.66){ // Sword phase
					attackState = 0; // Sword phase
					if ((double)npc.life < (double)npc.lifeMax * 0.82){
						angry = 25;  // Swings more aggressively
					}
					else{
						angry = 15; // Swings more aggressively
					}
					if (npc.ai[0] == 80f){
						counter = 12;
						strike = true;
					}
					if (!player.dead && npc.ai[0] >= 90f){
						SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 25f, 0f, ModContent.ProjectileType<BuriedShock>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // speed 12 -> 25
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] >= 360f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						charge = true;
						charging = true;
						for (int i = 0; i < 3; i++){
							int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 57, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
							Dust dust = Main.dust[num];
							dust.velocity *= 0.2f;
							dust.noGravity = true;
						}
					}
					if (npc.ai[2] >= 420f){
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector2;
						vector2 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector2) * 35f;  // Charge faster 14 to 35
						npc.netUpdate = true;
						npc.ai[2] = 0f;
					}
					if (npc.ai[3] >= 600f){ // Buried dagger every 9 seconds instead of 8
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(new AttachedEntityEntitySource_Parent(npc, player, null), player.Center, Vector2.Zero, ModContent.ProjectileType<BuriedDaggerSpawner>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[3] = 0f;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.66 && (double)npc.life > (double)npc.lifeMax * 0.33){ // Bow phase
					strike = false;
					charge = false;
					attackState = 1;
					if ((double)npc.life < (double)npc.lifeMax * 0.5){
						angry = 30; // More angry :) 30
					}
					else{
						angry = 25; // More angry 25 instead of 15
					}
					if (shifted != 1){
						counter = 4;
						npc.ai[0] = -60f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						for (int j = 0; j < 20; j++){
							Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						shifted = 1;
					}
					if (!player.dead && npc.ai[0] >= 60f){ // Normal shots
						SoundEngine.PlaySound(SoundID.Item5, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 25f, 0f, ModContent.ProjectileType<BuriedArrow>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // faster 8 to 20
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] == 520f){
						npc.ai[3] -= 120f;
					}
					if (npc.ai[2] >= 520f){
						if (npc.ai[2] == 520f){
							npc.netUpdate = true;
						}
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector3;
						vector3 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector3) * 35f; // Charge faster 12 to 35
					}
					if (!player.dead && npc.ai[2] >= 540f){ // Special arrow attack every 5 instead of 10 seconds
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){   
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 0f, -3.5f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					if (!player.dead && npc.ai[3] >= 300f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (npc.ai[3] == 300f){
							SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
							for (int k = 0; k < 10; k++){
								Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 0, default(Color), 1.25f);
							}
						}
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						if(npc.ai[3] >= 450f){ // New follow up triple shot{
							Vector2 direction = vector.SafeNormalize(Vector2.Zero);
							Vector2 perp = direction.RotatedBy(MathHelper.PiOver4); 
							float arrowoffsetAmount = 20f;
							SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
							for(int i = 0; i < 3; i++){
								Vector2 spawnOffset = perp * ((-1 + i) * arrowoffsetAmount);
									if (Main.netMode != 1){ 
										Projectile.NewProjectile(source_FromAI, npc.Center.X - 10f + spawnOffset.X, npc.Center.Y + spawnOffset.Y, vector.X * 25f, vector.Y * 25f, ModContent.ProjectileType<BuriedArrow>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
									}	
							}
							npc.ai[3] = -60f;
						}
						else if (npc.ai[3] == 360f || npc.ai[3] == 390f || npc.ai[3] == 420f){ 
							int arrowtype = 113;
							float num3 = 1f;
							int num4 = ModContent.ProjectileType<BuriedArrowC>();
							if (npc.ai[3] == 390f){
								arrowtype = 44;
								num4 = ModContent.ProjectileType<BuriedArrowP>();
							}
							else if (npc.ai[3] == 420f){
								arrowtype = 6;
								num3 = 1.25f;
								num4 = ModContent.ProjectileType<BuriedArrowF>();
							}
							for (int l = 0; l < 8; l++){
								int arrowdust = Dust.NewDust(npc.position, npc.width, npc.height, arrowtype, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 125, default(Color), num3);
								Main.dust[arrowdust].noGravity = true;
							}
							SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center.X - 10f, npc.Center.Y, vector.X * 25f, vector.Y * 25f, num4, 30, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots faster 14 to 15
							}
						}
					}
				}
				if ((double)npc.life <= (double)npc.lifeMax * 0.33){
					npc.defense = 0;
					strike = false;
					charge = false;
					charging = false;
					attackState = 2;
					if (shifted != 2){
						counter = 8;
						SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
						npc.ai[0] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						for (int m = 0; m < 20; m++){
							Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion2>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion1>(), 0, 0f, 0f, 0f, 0f, 255);
						shifted = 2;
					}
					if (npc.ai[3] >= 85f){ // Magic shot, more often
						SoundEngine.PlaySound(SoundID.Item73, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X + (float)(Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 10), npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 25f, 0f, ModContent.ProjectileType<BuriedMagic>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // slightly faster
						}
						npc.ai[3] = 0f;
					}
					if (npc.ai[0] >= 580f){
						npc.ai[3] = -60f;
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector4;
						vector4 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector4) * 10f;
					}
					if (npc.ai[0] >= 600f){
						npc.ai[0] = 0f;
					}
					if (npc.ai[2] > 300f){
						npc.ai[3] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int n = 0; n < 1; n++){
							int num6 = Dust.NewDust(npc.position, npc.width, npc.height, 113, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num6].noGravity = true;
							Main.dust[num6].velocity *= 0.75f;
							int num7 = Main.rand.Next(-50, 51);
							int num8 = Main.rand.Next(-50, 51);
							Dust dust2 = Main.dust[num6];
							dust2.position.X = dust2.position.X + (float)num7;
							Dust dust3 = Main.dust[num6];
							dust3.position.Y = dust3.position.Y + (float)num8;
							Main.dust[num6].velocity.X = -(float)num7 * 0.05f;
							Main.dust[num6].velocity.Y = -(float)num8 * 0.05f;
						}
					}
					if (npc.ai[2] == 360f || npc.ai[2] == 420f || npc.ai[2] == 480f){
						for (int num9 = 0; num9 < 10; num9++){
							int num10 = Dust.NewDust(npc.position, npc.width, npc.height, 113, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f);
							Main.dust[num10].noGravity = true;
						}
						SoundEngine.PlaySound(SoundID.Item45, new Vector2?(npc.Center), null);
						int num11 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 16, ModContent.NPCType<MagicalBurst>(), 0, player.Center.X, player.Center.Y, 0f, 0f, 255);
						if (num11 < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, num11, 0f, 0f, 0f, 0, 0, 0);
						}
						if (npc.ai[2] == 480f){
							npc.ai[2] = -300f;
						}
					}
				}
				npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X);
				if (!shift){
					flux += 2;
				}
				else{
					flux -= 2;
				}
				if (flux > 120 && !shift){
					shift = true;
				}
				if (flux <= -60){
					shift = false;
				}
				if (player.position.Y < npc.position.Y + 30f + (float)flux){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.1f : 0.1f); // Increased acceleration
				}
				if (player.position.Y > npc.position.Y + 30f + (float)flux){
					NPC npc3 = npc;
					npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.1f : 0.1f);
				}
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -32f, 32f);  // faster 14 to 25
				if (!charging){
					npc.aiStyle = -1;
					charge = false;
					if (sideRight){
						if (player.position.X < npc.position.X + 375f + (float)(flux / 2)){
							NPC npc4 = npc;
							npc4.velocity.X = npc4.velocity.X - ((npc.velocity.X > 0f) ? 0.5f : 0.1f);
						}
						if (player.position.X > npc.position.X + 475f + (float)(flux / 2)){
							NPC npc5 = npc;
							npc5.velocity.X = npc5.velocity.X + ((npc.velocity.X < 0f) ? 0.5f : 0.1f);
						}
					}
					else{
						if (player.position.X > npc.position.X - 375f + (float)(flux / 2)){
							NPC npc6 = npc;
							npc6.velocity.X = npc6.velocity.X + ((npc.velocity.X < 0f) ? 0.5f : 0.1f);
						}
						if (player.position.X < npc.position.X - 475f + (float)(flux / 2)){
							NPC npc7 = npc;
							npc7.velocity.X = npc7.velocity.X - ((npc.velocity.X > 0f) ? 0.5f : 0.1f);
						}
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -32f, 32f); // faster 14 to 25
					return false;
				}
				if (player.Center.X < npc.Center.X - 250f || player.Center.X > npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isDeath) {
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
				}
				if (phaseSwapTimer > 0){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					phaseSwapTimer--;
					return false;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.5f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				Vector2 vector = npc.DirectionTo(player.Center);
				if ((double)npc.life >= (double)npc.lifeMax * 0.66){ // Sword phase
					attackState = 0; // Sword phase
					if ((double)npc.life < (double)npc.lifeMax * 0.82){
						angry = 25;  // Swings more aggressively
					}
					else{
						angry = 15; // Swings more aggressively
					}
					if (npc.ai[0] == 80f){
						counter = 12;
						strike = true;
					}
					if (!player.dead && npc.ai[0] >= 90f){
						SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 13f, 0f, ModContent.ProjectileType<BuriedShock>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Swing slightly faster 12 to 13
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] >= 360f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						charge = true;
						charging = true;
						for (int i = 0; i < 3; i++){
							int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 57, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
							Dust dust = Main.dust[num];
							dust.velocity *= 0.2f;
							dust.noGravity = true;
						}
					}
					if (npc.ai[2] >= 420f){
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector2;
						vector2 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector2) * 16f;  // Charge faster 14 to 16
						npc.netUpdate = true;
						npc.ai[2] = 0f;
					}
					if (npc.ai[3] >= 600f){ // Buried dagger every 9 seconds instead of 8
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(new AttachedEntityEntitySource_Parent(npc, player, null), player.Center, Vector2.Zero, ModContent.ProjectileType<BuriedDaggerSpawner>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[3] = 0f;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.66 && (double)npc.life > (double)npc.lifeMax * 0.33){ // Bow phase
					strike = false;
					charge = false;
					attackState = 1;
					if ((double)npc.life < (double)npc.lifeMax * 0.5){
						angry = 30; // More angry :) 30
					}
					else{
						angry = 25; // More angry 25 instead of 15
					}
					if (shifted != 1){
						counter = 4;
						npc.ai[0] = -60f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						for (int j = 0; j < 20; j++){
							Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						shifted = 1;
					}
					if (!player.dead && npc.ai[0] >= 60f){ // Normal shots
						SoundEngine.PlaySound(SoundID.Item5, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 9f, 0f, ModContent.ProjectileType<BuriedArrow>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // faster 8 to 9
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] == 520f){
						npc.ai[3] -= 120f;
					}
					if (npc.ai[2] >= 520f){
						if (npc.ai[2] == 520f){
							npc.netUpdate = true;
						}
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector3;
						vector3 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector3) * 16f; // Charge faster 12 to 16
					}
					if (!player.dead && npc.ai[2] >= 540f){ // Special arrow attack every 9 instead of 10 seconds
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){   
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 0f, -3.5f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					if (!player.dead && npc.ai[3] >= 300f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (npc.ai[3] == 300f){
							SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
							for (int k = 0; k < 10; k++){
								Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 0, default(Color), 1.25f);
							}
						}
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						if(npc.ai[3] >= 450f){ // New follow up triple shot{
							Vector2 direction = vector.SafeNormalize(Vector2.Zero);
							Vector2 perp = direction.RotatedBy(MathHelper.PiOver4); 
							float arrowoffsetAmount = 20f;
							SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
							for(int i = 0; i < 3; i++){
								Vector2 spawnOffset = perp * ((-1 + i) * arrowoffsetAmount);
									if (Main.netMode != 1){ 
										Projectile.NewProjectile(source_FromAI, npc.Center.X - 10f + spawnOffset.X, npc.Center.Y + spawnOffset.Y, vector.X * 15f, vector.Y * 15f, ModContent.ProjectileType<BuriedArrow>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
									}	
							}
							npc.ai[3] = -60f;
						}
						else if (npc.ai[3] == 360f || npc.ai[3] == 390f || npc.ai[3] == 420f){ 
							int arrowtype = 113;
							float num3 = 1f;
							int num4 = ModContent.ProjectileType<BuriedArrowC>();
							if (npc.ai[3] == 390f){
								arrowtype = 44;
								num4 = ModContent.ProjectileType<BuriedArrowP>();
							}
							else if (npc.ai[3] == 420f){
								arrowtype = 6;
								num3 = 1.25f;
								num4 = ModContent.ProjectileType<BuriedArrowF>();
							}
							for (int l = 0; l < 8; l++){
								int arrowdust = Dust.NewDust(npc.position, npc.width, npc.height, arrowtype, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 125, default(Color), num3);
								Main.dust[arrowdust].noGravity = true;
							}
							SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center.X - 10f, npc.Center.Y, vector.X * 15f, vector.Y * 15f, num4, 30, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots faster 14 to 15
							}
						}
					}
				}
				if ((double)npc.life <= (double)npc.lifeMax * 0.33)
				{
					npc.defense = 0;
					strike = false;
					charge = false;
					charging = false;
					attackState = 2;
					if (shifted != 2){
						counter = 8;
						SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
						npc.ai[0] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						for (int m = 0; m < 20; m++){
							Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion2>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion1>(), 0, 0f, 0f, 0f, 0f, 255);
						shifted = 2;
					}
					if (npc.ai[3] >= 85f){ // Magic shot, more often
						SoundEngine.PlaySound(SoundID.Item73, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X + (float)(Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 10), npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 9f, 0f, ModContent.ProjectileType<BuriedMagic>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // slightly faster
						}
						npc.ai[3] = 0f;
					}
					if (npc.ai[0] >= 580f){
						npc.ai[3] = -60f;
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector4;
						vector4 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector4) * 10f;
					}
					if (npc.ai[0] >= 600f){
						npc.ai[0] = 0f;
					}
					if (npc.ai[2] > 300f){
						npc.ai[3] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int n = 0; n < 1; n++){
							int num6 = Dust.NewDust(npc.position, npc.width, npc.height, 113, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num6].noGravity = true;
							Main.dust[num6].velocity *= 0.75f;
							int num7 = Main.rand.Next(-50, 51);
							int num8 = Main.rand.Next(-50, 51);
							Dust dust2 = Main.dust[num6];
							dust2.position.X = dust2.position.X + (float)num7;
							Dust dust3 = Main.dust[num6];
							dust3.position.Y = dust3.position.Y + (float)num8;
							Main.dust[num6].velocity.X = -(float)num7 * 0.05f;
							Main.dust[num6].velocity.Y = -(float)num8 * 0.05f;
						}
					}
					if (npc.ai[2] == 360f || npc.ai[2] == 420f || npc.ai[2] == 480f){
						for (int num9 = 0; num9 < 10; num9++){
							int num10 = Dust.NewDust(npc.position, npc.width, npc.height, 113, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f);
							Main.dust[num10].noGravity = true;
						}
						SoundEngine.PlaySound(SoundID.Item45, new Vector2?(npc.Center), null);
						int num11 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 16, ModContent.NPCType<MagicalBurst>(), 0, player.Center.X, player.Center.Y, 0f, 0f, 255);
						if (num11 < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, num11, 0f, 0f, 0f, 0, 0, 0);
						}
						if (npc.ai[2] == 480f){
							npc.ai[2] = -300f;
						}
					}
				}
				npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X);
				if (!shift){
					flux += 2;
				}
				else{
					flux -= 2;
				}
				if (flux > 120 && !shift){
					shift = true;
				}
				if (flux <= -60){
					shift = false;
				}
				if (player.position.Y < npc.position.Y + 30f + (float)flux){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.8f : 0.07f);
				}
				if (player.position.Y > npc.position.Y + 30f + (float)flux){
					NPC npc3 = npc;
					npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.8f : 0.07f);
				}
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
				if (!charging){
					npc.aiStyle = -1;
					charge = false;
					if (sideRight){
						if (player.position.X < npc.position.X + 375f + (float)(flux / 2)){
							NPC npc4 = npc;
							npc4.velocity.X = npc4.velocity.X - ((npc.velocity.X > 0f) ? 0.3f : 0.07f);
						}
						if (player.position.X > npc.position.X + 475f + (float)(flux / 2)){
							NPC npc5 = npc;
							npc5.velocity.X = npc5.velocity.X + ((npc.velocity.X < 0f) ? 0.3f : 0.07f);
						}
					}
					else{
						if (player.position.X > npc.position.X - 375f + (float)(flux / 2)){
							NPC npc6 = npc;
							npc6.velocity.X = npc6.velocity.X + ((npc.velocity.X < 0f) ? 0.3f : 0.07f);
						}
						if (player.position.X < npc.position.X - 475f + (float)(flux / 2)){
							NPC npc7 = npc;
							npc7.velocity.X = npc7.velocity.X - ((npc.velocity.X > 0f) ? 0.3f : 0.07f);
						}
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -14f, 14f);
					return false;
				}
				if (player.Center.X < npc.Center.X - 250f || player.Center.X > npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isRevengeance){
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
				}
				if (phaseSwapTimer > 0){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					phaseSwapTimer--;
					return false;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.5f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
					}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				Vector2 vector = npc.DirectionTo(player.Center);
				if ((double)npc.life >= (double)npc.lifeMax * 0.66){ // Sword phase
					attackState = 0; // Sword phase
					if ((double)npc.life < (double)npc.lifeMax * 0.82){
						angry = 20;  // Swings more aggressively
					}
					else{
						angry = 5; // Swings more aggressively
					}
					if (npc.ai[0] == 80f){
						counter = 12;
						strike = true;
					}
					if (!player.dead && npc.ai[0] >= 90f){
						SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 13f, 0f, ModContent.ProjectileType<BuriedShock>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Swing slightly faster 12 to 13
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] >= 360f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						charge = true;
						charging = true;
						for (int i = 0; i < 3; i++){
							int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 57, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
							Dust dust = Main.dust[num];
							dust.velocity *= 0.2f;
							dust.noGravity = true;
						}
					}
					if (npc.ai[2] >= 420f){
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector2;
						vector2 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector2) * 16f;  // Charge faster 14 to 16
						npc.netUpdate = true;
						npc.ai[2] = 0f;
					}
					if (npc.ai[3] >= 600f){ // Buried dagger every 9 seconds instead of 8
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(new AttachedEntityEntitySource_Parent(npc, player, null), player.Center, Vector2.Zero, ModContent.ProjectileType<BuriedDaggerSpawner>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[3] = 0f;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.66 && (double)npc.life > (double)npc.lifeMax * 0.33){ // Bow phase
					strike = false;
					charge = false;
					attackState = 1;
					if ((double)npc.life < (double)npc.lifeMax * 0.5){
						angry = 28; // More angry :) 28
					}
					else{
						angry = 20; // More angry 20 instead of 15
					}
					if (shifted != 1){
						counter = 4;
						npc.ai[0] = -60f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						for (int j = 0; j < 20; j++){
							Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						shifted = 1;
					}
					if (!player.dead && npc.ai[0] >= 60f){ // Normal shots
						SoundEngine.PlaySound(SoundID.Item5, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 9f, 0f, ModContent.ProjectileType<BuriedArrow>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // faster 8 to 9
						}
						npc.ai[0] = (float)angry;
					}
					if (npc.ai[2] == 520f){
						npc.ai[3] -= 120f;
					}
					if (npc.ai[2] >= 520f){
						if (npc.ai[2] == 520f){
							npc.netUpdate = true;
						}
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector3;
						vector3 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector3) * 14f; // Charge faster 12 to 14
					}
					if (!player.dead && npc.ai[2] >= 540f){ // Special arrow attack every 9 instead of 10 seconds
						SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){   
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -3.5f, 0f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 0f, -3.5f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, 2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, -2f, -2f, ModContent.ProjectileType<BuriedArrow2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f);
						}
						npc.ai[2] = 0f;
						npc.netUpdate = true;
					}
					if (!player.dead && npc.ai[3] >= 300f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (npc.ai[3] == 300f){
							SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
							for (int k = 0; k < 10; k++){
								Dust.NewDust(npc.position, npc.width, npc.height, 57, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 0, default(Color), 1.25f);
							}
						}
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						npc.ai[0] = 0f;
						if (npc.ai[3] == 360f || npc.ai[3] == 390f || npc.ai[3] >= 420f){ // Three shots
							int arrowtype = 113;
							float num3 = 1f;
							int num4 = ModContent.ProjectileType<BuriedArrowC>();
							if (npc.ai[3] == 390f){
								arrowtype = 44;
								num4 = ModContent.ProjectileType<BuriedArrowP>();
							}
							else if (npc.ai[3] >= 420f){
								arrowtype = 6;
								num3 = 1.25f;
								num4 = ModContent.ProjectileType<BuriedArrowF>();
								npc.ai[3] = -60f;
							}
							for (int l = 0; l < 8; l++){
								int arrowdust = Dust.NewDust(npc.position, npc.width, npc.height, arrowtype, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 125, default(Color), num3);
								Main.dust[arrowdust].noGravity = true;
							}
							SoundEngine.PlaySound(SoundID.Item102, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, npc.Center.X - 10f, npc.Center.Y, vector.X * 15f, vector.Y * 15f, num4, 30, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoots faster 14 to 15
							}
						}
					}
				}
				if ((double)npc.life <= (double)npc.lifeMax * 0.33)
				{
					npc.defense = 0;
					strike = false;
					charge = false;
					charging = false;
					attackState = 2;
					if (shifted != 2){
						counter = 8;
						SoundEngine.PlaySound(SoundID.Item43, new Vector2?(npc.Center), null);
						npc.ai[0] = 0f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						phaseSwapTimer = 120;
						for (int m = 0; m < 20; m++){
							Dust.NewDust(npc.position, npc.width, npc.height, 15, (float)Main.rand.Next(-6, 6), (float)Main.rand.Next(-6, 6), 255, default(Color), 1.5f);
						}
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion2>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 45, (int)npc.Center.Y, ModContent.NPCType<FallenChampion1>(), 0, 0f, 0f, 0f, 0f, 255);
						shifted = 2;
					}
					if (npc.ai[3] >= 90f){
						SoundEngine.PlaySound(SoundID.Item73, new Vector2?(npc.Center), null);
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X + (float)(Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 10), npc.Center.Y, (float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X) * 8.5f, 0f, ModContent.ProjectileType<BuriedMagic>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // slightly faster
						}
						npc.ai[3] = 0f;
					}
					if (npc.ai[0] >= 580f){
						npc.ai[3] = -60f;
						npc.aiStyle = -2;
						sideRight = !sideRight;
						Vector2 vector4;
						vector4 = new Vector2(player.Center.X, player.Center.Y - 350f);
						npc.velocity = npc.DirectionTo(vector4) * 10f;
					}
					if (npc.ai[0] >= 600f){
						npc.ai[0] = 0f;
					}
					if (npc.ai[2] > 300f){
						npc.ai[3] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int n = 0; n < 1; n++){
							int num6 = Dust.NewDust(npc.position, npc.width, npc.height, 113, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num6].noGravity = true;
							Main.dust[num6].velocity *= 0.75f;
							int num7 = Main.rand.Next(-50, 51);
							int num8 = Main.rand.Next(-50, 51);
							Dust dust2 = Main.dust[num6];
							dust2.position.X = dust2.position.X + (float)num7;
							Dust dust3 = Main.dust[num6];
							dust3.position.Y = dust3.position.Y + (float)num8;
							Main.dust[num6].velocity.X = -(float)num7 * 0.05f;
							Main.dust[num6].velocity.Y = -(float)num8 * 0.05f;
						}
					}
					if (npc.ai[2] == 360f || npc.ai[2] == 420f || npc.ai[2] == 480f){
						for (int num9 = 0; num9 < 10; num9++){
							int num10 = Dust.NewDust(npc.position, npc.width, npc.height, 113, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f);
							Main.dust[num10].noGravity = true;
						}
						SoundEngine.PlaySound(SoundID.Item45, new Vector2?(npc.Center), null);
						int num11 = NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 16, ModContent.NPCType<MagicalBurst>(), 0, player.Center.X, player.Center.Y, 0f, 0f, 255);
						if (num11 < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, num11, 0f, 0f, 0f, 0, 0, 0);
						}
						if (npc.ai[2] == 480f){
							npc.ai[2] = -300f;
						}
					}
				}
				npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X);
				if (!shift){
					flux += 2;
				}
				else{
					flux -= 2;
				}
				if (flux > 120 && !shift){
					shift = true;
				}
				if (flux <= -60){
					shift = false;
				}
				if (player.position.Y < npc.position.Y + 30f + (float)flux){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 0.8f : 0.07f);
				}
				if (player.position.Y > npc.position.Y + 30f + (float)flux){
					NPC npc3 = npc;
					npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 0.8f : 0.07f);
				}
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -14f, 14f);
				if (!charging){
					npc.aiStyle = -1;
					charge = false;
					if (sideRight){
						if (player.position.X < npc.position.X + 375f + (float)(flux / 2)){
							NPC npc4 = npc;
							npc4.velocity.X = npc4.velocity.X - ((npc.velocity.X > 0f) ? 0.3f : 0.07f);
						}
						if (player.position.X > npc.position.X + 475f + (float)(flux / 2)){
							NPC npc5 = npc;
							npc5.velocity.X = npc5.velocity.X + ((npc.velocity.X < 0f) ? 0.3f : 0.07f);
						}
					}
					else
					{
						if (player.position.X > npc.position.X - 375f + (float)(flux / 2)){
							NPC npc6 = npc;
							npc6.velocity.X = npc6.velocity.X + ((npc.velocity.X < 0f) ? 0.3f : 0.07f);
						}
						if (player.position.X < npc.position.X - 475f + (float)(flux / 2)){
							NPC npc7 = npc;
							npc7.velocity.X = npc7.velocity.X - ((npc.velocity.X > 0f) ? 0.3f : 0.07f);
						}
					}
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -14f, 14f);
					return false;
				}
				if (player.Center.X < npc.Center.X - 250f || player.Center.X > npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else { // No bossrush, revengegance or death detected, use original ai instead
				return true;
			}
		}
	}
}