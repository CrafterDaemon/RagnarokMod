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
using ThoriumMod;
using ThoriumMod.NPCs;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.Projectiles.Boss;
using ThoriumMod.Projectiles;
using ThoriumMod.Core.EntitySources;
using ThoriumMod.Sounds;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class GraniteEnergyStormAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("GraniteEnergyStorm").Type;
		}
		
		public int effectFrameCount = 6;
		public int effectFrameY;
		public int rage;
		public int generate;
		public float scalePulse;
		public bool scaleShift;
		public string Texture_ges => "ThoriumMod/NPCs/BossGraniteEnergyStorm/GraniteEnergyStorm";
		
		public override void FindFrame(NPC npc, int frameHeight){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().granite)){
				npc.frameCounter += 1.0;
				if (npc.frameCounter > 8.0){
					effectFrameY++;
					if (effectFrameY >= 6){
						effectFrameY = 0;
					}
					npc.frameCounter = 0.0;
				}
				return;
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().granite)) {
				Texture2D effectTexture = ModContent.Request<Texture2D>(Texture_ges + "_Effect", AssetRequestMode.ImmediateLoad).Value;
				Rectangle effectFrame = Terraria.Utils.Frame(effectTexture, 1, 6, 0, effectFrameY, 0, 0);
				Vector2 yOffset = npc.Center;
				if (npc.IsABestiaryIconDummy){
					yOffset = new Vector2(npc.Center.X, npc.Center.Y + 8f);
				}
				Color color = new Color(255, 255, 255, 150) * 1f;
				SpriteEffects effects = (SpriteEffects)((npc.spriteDirection < 0) ? 0 : 1);
				spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_ges + "_Effect2", AssetRequestMode.ImmediateLoad).Value, yOffset - screenPos, null, color, npc.rotation, new Vector2(32f, 32f), npc.scale, effects, 0f);
				spriteBatch.Draw(effectTexture, yOffset - screenPos, new Rectangle?(effectFrame), color, 0f, new Vector2(12f, 10f), npc.scale, effects, 0f);
			}
		}
		
		public override bool PreAI(NPC npc) {
		    if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().granite))) {
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
				NPCHelper.BatAI(npc, 0, 3.5f); // Going batshit insane
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				rage = 120;
				rage += ((!NPC.npcsFoundForCheckActive[ModContent.NPCType<CoalescedEnergy>()]) ? 25 : 0);
				rage += (((double)npc.life < (double)npc.lifeMax * 0.6) ? 5 : 0);
				rage += (((double)npc.life < (double)npc.lifeMax * 0.3) ? 5 : 0); 
				scalePulse += ((!scaleShift) ? 0.01f : -0.01f);
				if (scalePulse > 0.25f && !scaleShift){
					scaleShift = true;
				}
				if (scalePulse <= 0f){
					scaleShift = false;
				}
				IEntitySource source = npc.GetSource_FromAI(null);
				npc.ai[3] += 1f; // Counter for when to spawn CoalescedEnergy
				if (npc.ai[3] >= 120f){ // Spawn CoalescedEnergy
					SoundEngine.PlaySound(SoundID.Item88, new Vector2?(npc.Center), null);
					for (int i = 0; i < 8; i++){
						int eng = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CoalescedEnergy>(), 0, (float)npc.whoAmI, (float)i, 0f, 0f, 255);
						if (eng < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, eng, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					npc.ai[3] = -1650f; // Cooldown for CoalescedEnergy to spawn again, increased a bit
				}
				if (npc.ai[3] == 1f){
					SoundEngine.PlaySound(SoundID.Item72, new Vector2?(npc.Center), null);
					for (int j = 0; j < 8; j++){
						if (Main.netMode != 1){
							int eng2 = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EnergyBarrier>(), 0, (float)npc.whoAmI, 0f, (float)j, 0f, 255);
							if (eng2 < Main.maxNPCs && Main.netMode == 2){
								NetMessage.SendData(23, -1, -1, null, eng2, 0f, 0f, 0f, 0, 0, 0);
							}
						}
					}
					npc.netUpdate = true;
				}
				if (!player.dead){ // Shoot Granite Charges
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 center = npc.Center;
							Projectile.NewProjectile(source, center, npc.DirectionTo(player.Center) * 32f, ModContent.ProjectileType<GraniteCharge>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoot speed 32 instead of 12
						}
						npc.ai[1] = (float)rage;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.75){ // Spawn conduits, now at 75 percent health
					generate++;
					if (generate > 240 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){ // Now generates a new conduit every 4 instead of 10 seconds
						int minionType = ModContent.NPCType<EnergyConduit>();
						int minionCount = 0;
						for (int u = 0; u < Main.maxNPCs; u++){
							NPC target = Main.npc[u];
							if (target.active && target.type == minionType){
								minionCount++;
							}
						}
						if (minionCount < 10){  // Now spawns up to 10 conduits instead of 5
							SoundEngine.PlaySound(SoundID.Item70, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, minionType, 0, 0f, 0f, 0f, 0f, 255);
							}
							generate = 0;
						}
					}
				}
				if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					npc.ai[0] += 1f;
					if (npc.ai[0] >= 400f){ // Prepare Charge, prepare time much lower now
						npc.ai[1] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int k = 0; k < 1; k++){
							Vector2 offset;
							offset = new Vector2((float)Main.rand.Next(-75, 76), (float)Main.rand.Next(-75, 76));
							Dust dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 2f, npc.Center.Y - 6f) + offset, 20, 20, 15, 0f, 0f, 255, default(Color), 1.5f);
							dust.noGravity = true;
							dust.velocity = -offset * 0.075f;
						}
					}
					if (npc.ai[0] >= 430f){   // Charge-Attack more often
						npc.velocity = npc.DirectionTo(player.Center) * 48f; // Charge speed increased from 12 to 48
						npc.netUpdate = true;
						npc.ai[0] = (float)rage;
					}
				}
				if (npc.ai[0] < 400f){ // When not charging do regulare movement
					npc.directionY = ((player.position.Y - 150f > npc.position.Y) ? 1 : -1);
					if (npc.direction == -1 && npc.velocity.X > -18f){ // 16 instead of 4
						npc.velocity.X = npc.velocity.X - 1.6f; // 1.6 instead of 0.4
						if (npc.velocity.X > 15f){
							npc.velocity.X = npc.velocity.X - 1.6f;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.2f; // 0.2 instead of 0.08
						}
						if (npc.velocity.X < -15f){
							npc.velocity.X = -15f;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 18f){
						npc.velocity.X = npc.velocity.X + 1.6f;
						if (npc.velocity.X < -15f){
							npc.velocity.X = npc.velocity.X + 1.6f;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.2f;
						}
						if (npc.velocity.X > 15f){
							npc.velocity.X = 15f;
						}
					}
					if (npc.directionY == -1 && npc.velocity.Y > -11f){
						npc.velocity.Y = npc.velocity.Y - 0.6f;
						if (npc.velocity.Y < -9f){
							npc.velocity.Y = -9f;
						}
					}
					else if (npc.directionY == 1 && npc.velocity.Y < 11f){
						npc.velocity.Y = npc.velocity.Y + 0.6f;
						if (npc.velocity.Y > 9f){
							npc.velocity.Y = 9f;
						}
					}
				}
				npc.rotation -= 0.05f;
				return false;
			}
			else if (CalamityGamemodeCheck.isDeath){
				NPCHelper.BatAI(npc, 0);
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				rage = 10;
				rage += ((!NPC.npcsFoundForCheckActive[ModContent.NPCType<CoalescedEnergy>()]) ? 60 : 0);
				rage += (((double)npc.life < (double)npc.lifeMax * 0.5) ? 40 : 0); // add 40 insted of 30 rage
				rage += (((double)npc.life < (double)npc.lifeMax * 0.25) ? 40 : 0); // add 40 insted of 30 rage
				scalePulse += ((!scaleShift) ? 0.01f : -0.01f);
				if (scalePulse > 0.25f && !scaleShift){
					scaleShift = true;
				}
				if (scalePulse <= 0f){
					scaleShift = false;
				}
				IEntitySource source = npc.GetSource_FromAI(null);
				npc.ai[3] += 1f; // Counter for when to spawn CoalescedEnergy
				if (npc.ai[3] >= 120f){ // Spawn CoalescedEnergy
					SoundEngine.PlaySound(SoundID.Item88, new Vector2?(npc.Center), null);
					for (int i = 0; i < 8; i++){
						int eng = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CoalescedEnergy>(), 0, (float)npc.whoAmI, (float)i, 0f, 0f, 255);
						if (eng < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, eng, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					npc.ai[3] = -1500f; // Cooldown for CoalescedEnergy to spawn again
				}
				if (npc.ai[3] == 1f){
					SoundEngine.PlaySound(SoundID.Item72, new Vector2?(npc.Center), null);
					for (int j = 0; j < 8; j++){
						if (Main.netMode != 1){
							int eng2 = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EnergyBarrier>(), 0, (float)npc.whoAmI, 0f, (float)j, 0f, 255);
							if (eng2 < Main.maxNPCs && Main.netMode == 2){
								NetMessage.SendData(23, -1, -1, null, eng2, 0f, 0f, 0f, 0, 0, 0);
							}
						}
					}
					npc.netUpdate = true;
				}
				if (!player.dead && (double)npc.life < (double)npc.lifeMax * 0.75){ // Shoot Granite Charges
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 pos = npc.Center;
							Vector2 destinationToPlayer = player.DirectionFrom(pos);
							Projectile.NewProjectile(source, pos, destinationToPlayer * 14f, ModContent.ProjectileType<GraniteCharge>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoot speed 14 instead of 12
						}
						npc.ai[1] = (float)rage;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5){
					generate++;
					if (generate > 540 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){ // Now generates a new conduit every 9 instead of 10 seconds
						int minionType = ModContent.NPCType<EnergyConduit>();
						int minionCount = 0;
						for (int u = 0; u < Main.maxNPCs; u++){
							NPC target = Main.npc[u];
							if (target.active && target.type == minionType){
								minionCount++;
							}
						}
						if (minionCount < 6){  // Now spawns up to 6 conduits instead of 5
							SoundEngine.PlaySound(SoundID.Item70, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, minionType, 0, 0f, 0f, 0f, 0f, 255);
							}
							generate = 0;
						}
					}
				}
				if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					npc.ai[0] += 1f;
					if (npc.ai[0] >= 510f){
						npc.ai[1] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int k = 0; k < 1; k++){
							Vector2 offset;
							offset = new Vector2((float)Main.rand.Next(-75, 76), (float)Main.rand.Next(-75, 76));
							Dust dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 2f, npc.Center.Y - 6f) + offset, 20, 20, 15, 0f, 0f, 255, default(Color), 1.5f);
							dust.noGravity = true;
							dust.velocity = -offset * 0.075f;
						}
					}
					if (npc.ai[0] >= 600f){   // Charge-Attack
						npc.velocity = npc.DirectionTo(player.Center) * 15f; // Charge speed increased from 12 to 15
						npc.netUpdate = true;
						npc.ai[0] = (float)rage;
					}
				}
				if (npc.ai[0] < 510f){
					npc.directionY = ((player.position.Y - 150f > npc.position.Y) ? 1 : -1);
					if (npc.direction == -1 && npc.velocity.X > -4f){
						npc.velocity.X = npc.velocity.X - 0.4f;
						if (npc.velocity.X > 2f){
							npc.velocity.X = npc.velocity.X - 0.4f;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.08f;
						}
						if (npc.velocity.X < -2f){
							npc.velocity.X = -2f;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 4f){
						npc.velocity.X = npc.velocity.X + 0.4f;
						if (npc.velocity.X < -2f){
							npc.velocity.X = npc.velocity.X + 0.4f;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.08f;
						}
						if (npc.velocity.X > 2f){
							npc.velocity.X = 2f;
						}
					}
					if (npc.directionY == -1 && npc.velocity.Y > -2f){
						npc.velocity.Y = npc.velocity.Y - 0.15f;
						if (npc.velocity.Y < -2f){
							npc.velocity.Y = -2f;
						}
					}
					else if (npc.directionY == 1 && npc.velocity.Y < 2f){
						npc.velocity.Y = npc.velocity.Y + 0.15f;
						if (npc.velocity.Y > 2f){
							npc.velocity.Y = 2f;
						}
					}
				}
				npc.rotation -= 0.05f;
				return false;
			}				
			else if(CalamityGamemodeCheck.isRevengeance) {
				NPCHelper.BatAI(npc, 0);
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){
						npc.timeLeft = 20;
					}
					return false;
				}
				rage = 0;
				rage += ((!NPC.npcsFoundForCheckActive[ModContent.NPCType<CoalescedEnergy>()]) ? 60 : 0);
				rage += (((double)npc.life < (double)npc.lifeMax * 0.5) ? 35 : 0); // add 35 insted of 30 rage
				rage += (((double)npc.life < (double)npc.lifeMax * 0.25) ? 35 : 0); // add 35 insted of 30 rage
				scalePulse += ((!scaleShift) ? 0.01f : -0.01f);
				if (scalePulse > 0.25f && !scaleShift){
					scaleShift = true;
				}
				if (scalePulse <= 0f){
					scaleShift = false;
				}
				IEntitySource source = npc.GetSource_FromAI(null);
				npc.ai[3] += 1f; // Counter for when to spawn CoalescedEnergy
				if (npc.ai[3] >= 120f){ // Spawn CoalescedEnergy
					SoundEngine.PlaySound(SoundID.Item88, new Vector2?(npc.Center), null);
					for (int i = 0; i < 8; i++){
						int eng = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CoalescedEnergy>(), 0, (float)npc.whoAmI, (float)i, 0f, 0f, 255);
						if (eng < Main.maxNPCs && Main.netMode == 2){
							NetMessage.SendData(23, -1, -1, null, eng, 0f, 0f, 0f, 0, 0, 0);
						}
					}
					npc.ai[3] = -1500f; // Cooldown for CoalescedEnergy to spawn again
				}
				if (npc.ai[3] == 1f){
					SoundEngine.PlaySound(SoundID.Item72, new Vector2?(npc.Center), null);
					for (int j = 0; j < 8; j++){
						if (Main.netMode != 1){
							int eng2 = NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EnergyBarrier>(), 0, (float)npc.whoAmI, 0f, (float)j, 0f, 255);
							if (eng2 < Main.maxNPCs && Main.netMode == 2){
								NetMessage.SendData(23, -1, -1, null, eng2, 0f, 0f, 0f, 0, 0, 0);
							}
						}
					}
					npc.netUpdate = true;
				}
				if (!player.dead && (double)npc.life < (double)npc.lifeMax * 0.75){ // Shoot Granite Charges
					npc.ai[1] += 1f;
					if (npc.ai[1] >= 180f && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						if (Main.netMode != 1){
							Vector2 pos = npc.Center;
							Vector2 destinationToPlayer = player.DirectionFrom(pos);
							Projectile.NewProjectile(source, pos, destinationToPlayer * 13f, ModContent.ProjectileType<GraniteCharge>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f); // Shoot speed 13 instead of 12
						}
						npc.ai[1] = (float)rage;
					}
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.5){
					generate++;
					if (generate > 540 && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){ // Now generates a new conduit every 9 instead of 10 seconds
						int minionType = ModContent.NPCType<EnergyConduit>();
						int minionCount = 0;
						for (int u = 0; u < Main.maxNPCs; u++){
							NPC target = Main.npc[u];
							if (target.active && target.type == minionType){
								minionCount++;
							}
						}
						if (minionCount < 6){  // Now spawns up to 6 conduits instead of 5
							SoundEngine.PlaySound(SoundID.Item70, new Vector2?(npc.Center), null);
							if (Main.netMode != 1){
								NPC.NewNPC(source, (int)npc.Center.X, (int)npc.Center.Y, minionType, 0, 0f, 0f, 0f, 0f, 255);
							}
							generate = 0;
						}
					}
				}
				if (!player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
					npc.ai[0] += 1f;
					if (npc.ai[0] >= 510f){
						npc.ai[1] = 0f;
						npc.velocity.X = 0f;
						npc.velocity.Y = 0f;
						for (int k = 0; k < 1; k++){
							Vector2 offset;
							offset = new Vector2((float)Main.rand.Next(-75, 76), (float)Main.rand.Next(-75, 76));
							Dust dust = Dust.NewDustDirect(new Vector2(npc.Center.X - 2f, npc.Center.Y - 6f) + offset, 20, 20, 15, 0f, 0f, 255, default(Color), 1.5f);
							dust.noGravity = true;
							dust.velocity = -offset * 0.075f;
						}
					}
					if (npc.ai[0] >= 600f){   // Charge-Attack
						npc.velocity = npc.DirectionTo(player.Center) * 13f; // Charge speed increased from 12 to 13
						npc.netUpdate = true;
						npc.ai[0] = (float)rage;
					}
				}
				if (npc.ai[0] < 510f){
					npc.directionY = ((player.position.Y - 150f > npc.position.Y) ? 1 : -1);
					if (npc.direction == -1 && npc.velocity.X > -4f){
						npc.velocity.X = npc.velocity.X - 0.4f;
						if (npc.velocity.X > 2f){
							npc.velocity.X = npc.velocity.X - 0.4f;
						}
						else if (npc.velocity.X > 0f){
							npc.velocity.X = npc.velocity.X + 0.08f;
						}
						if (npc.velocity.X < -2f){
							npc.velocity.X = -2f;
						}
					}
					else if (npc.direction == 1 && npc.velocity.X < 4f){
						npc.velocity.X = npc.velocity.X + 0.4f;
						if (npc.velocity.X < -2f){
							npc.velocity.X = npc.velocity.X + 0.4f;
						}
						else if (npc.velocity.X < 0f){
							npc.velocity.X = npc.velocity.X - 0.08f;
						}
						if (npc.velocity.X > 2f){
							npc.velocity.X = 2f;
						}
					}
					if (npc.directionY == -1 && npc.velocity.Y > -2f){
						npc.velocity.Y = npc.velocity.Y - 0.15f;
						if (npc.velocity.Y < -2f){
							npc.velocity.Y = -2f;
						}
					}
					else if (npc.directionY == 1 && npc.velocity.Y < 2f){
						npc.velocity.Y = npc.velocity.Y + 0.15f;
						if (npc.velocity.Y > 2f){
							npc.velocity.Y = 2f;
						}
					}
				}
				npc.rotation -= 0.05f;
				return false;
			}
			else {
					return true;
			}
		}
	}
}