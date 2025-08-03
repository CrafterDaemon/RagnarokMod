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
using ThoriumMod.NPCs.BossViscount;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class ViscountAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("Viscount").Type;
		}
		public static int counting;
		public static int counter;
		public static int rage;
		public static int flux;
		public static int dodgeTimer;
		public static bool shift;
		public static bool openMouth;
		public static bool blood;
		public static bool stomp;
		public static bool stomped;
		public static bool scream;
		public string Texture_viscount => "ThoriumMod/NPCs/BossViscount/Viscount";
		public string Texture_viscountglow => "ThoriumMod/NPCs/BossViscount/Viscount_Glow";
	
		public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter writer){
			if(CalamityGamemodeCheck.isRevengeance) {
				BitsByte bitsByte = default(BitsByte);
				bitsByte[0] = (npc.aiStyle == -1);
				bitsByte[1] = openMouth;
				bitsByte[2] = blood;
				bitsByte[3] = stomp;
				bitsByte[4] = stomped;
				bitsByte[5] = scream;
				writer.Write(bitsByte);
			}
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader reader){
			if(CalamityGamemodeCheck.isRevengeance) {
				BitsByte bitsByte = reader.ReadByte();
				npc.aiStyle = (bitsByte[0] ? -1 : -2);
				openMouth = bitsByte[1];
				blood = bitsByte[2];
				stomp = bitsByte[3];
				stomped = bitsByte[4];
				scream = bitsByte[5];
			}
		}
		public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor){
			
			if(CalamityGamemodeCheck.isRevengeance) {
				Vector2 vector = npc.Center - screenPos;
				Color color = Lighting.GetColor((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f));
				Vector2 vector2 = Terraria.Utils.Size(npc.frame) / 2f + new Vector2(0f, 4f);
				SpriteEffects spriteEffects = (SpriteEffects)((npc.spriteDirection < 0) ? 0 : 1);
				if (openMouth){
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_viscountglow + "1", AssetRequestMode.ImmediateLoad).Value, vector, new Rectangle?(npc.frame), color, npc.rotation, vector2, npc.scale, spriteEffects, 0f);
				}
				if (blood){
					spriteBatch.Draw(ModContent.Request<Texture2D>(Texture_viscountglow, AssetRequestMode.ImmediateLoad).Value, vector, new Rectangle?(npc.frame), color, npc.rotation, vector2, npc.scale, spriteEffects, 0f);
				}
				if (stomped){
					spriteBatch.Draw(ModContent.Request<Texture2D>("ThoriumMod/Textures/Sword_Indicator", AssetRequestMode.ImmediateLoad).Value, vector, null, Color.White, 0f, new Vector2(13f, 90f), 1f, 0, 0f);
				}
			}
		}
		public override void FindFrame(NPC npc, int frameHeight){
			if(CalamityGamemodeCheck.isRevengeance) {
				if (!stomp && !stomped && !scream){
					counting++;
					int num;
					if (scream){
						num = 4;
					}
					else{
						num = 0;
					}
					if (counting > 4 + num){
						counter++;
						counting = 0;
					}
					if (counter >= 9){
						counter = 0;
					}
				}
				if (scream){
					counting++;
					if (counting > 10 && counter <= 22){
						counter++;
						counting = 0;
					}
					if (counter > 22){
						counter = 0;
						scream = false;
					}
				}
				if (stomp){
					counting++;
					if (counting > 4 && counter < 13){
						counter++;
						counting = 0;
					}
				}
				else if (stomped){
					if (counter < 17){
						counting++;
						if (counting > 6){
							counter++;
							counting = 0;
						}
					}
					else{
						counting++;
						counter = 17;
						if (counting > 60){
							counter = 0;
							stomped = false;
							npc.aiStyle = -1;
							if (counting == 61){
								npc.netUpdate = true;
							}
						}
					}
				}
				npc.frame.Y = counter * frameHeight;
				return;
			}
		}
		
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
		{
			if(stomped) {
				modifiers.FinalDamage *= 3f;
			}
		}
		
		public override bool PreAI(NPC npc) 
		{
			if(!(ModContent.GetInstance<BossConfig>().viscount == ThoriumBossRework_selection_mode.Ragnarok)) {
					return true;
			}
			if(CalamityGamemodeCheck.isBossrush) {	
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0, 3.5f);
				}
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){ // Ends the fight if there is not valid target{
					shift = false;
					openMouth = false;
					blood = false;
					stomp = false;
					stomped = false;
					scream = false;
					//NPC npc = npc;
					npc.velocity.Y = npc.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){ npc.timeLeft = 20;}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (!stomp && !stomped){
					npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X <= npc.Center.X);
				}
				if (!stomp && !stomped && !scream){  // Movement and sprite direction
					if (!shift){
						flux += 3;
					}
					else{
						flux -= 3;
					}
					if (flux > 66 && !shift){
						shift = true;
					}
					if (flux <= -66){
						shift = false;
					}
					dodgeTimer++;
					if (dodgeTimer > 60){
						dodgeTimer = 0;
						NPC npc2 = npc;
						npc2.velocity.X = npc2.velocity.X + (float)(-1 * npc.spriteDirection);
					}
					if (player.position.Y < npc.position.Y + 250f + (float)flux){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y - ((npc.velocity.Y > 0f) ? 0.85f : 0.07f);
					}
					if (player.position.Y > npc.position.Y + 250f + (float)flux){
						NPC npc4 = npc;
						npc4.velocity.Y = npc4.velocity.Y + ((npc.velocity.Y < 0f) ? 0.85f : 0.07f);
					}
				}
				else{
					dodgeTimer = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f; // Attack counter
				if ((double)npc.life < (double)npc.lifeMax * 0.25) {
					rage = 70; // original 60
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5) {
					rage = 55; // original 45
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75) {
					rage = 40; // original 30
				}
				else {
						rage = 20; //orginal 15
				}
				if (npc.aiStyle != -2){
					npc.ai[1] -= 1f;
				}
				if (npc.ai[0] == 0f && Main.netMode != 1){ // Select an attack option based on current health
					if ((double)npc.life < (double)npc.lifeMax * 0.6){
						npc.ai[1] = (float)Main.rand.Next(4);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.75){
						npc.ai[1] = (float)Main.rand.Next(3);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.9){
						npc.ai[1] = (float)Main.rand.Next(2);
					}
					else{
						npc.ai[1] = 0f;
					}
					if(npc.ai[1] == 1) { // No stomp in bossrush
						npc.ai[1] = 2;
					}
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}
		
				if (!stomp && !stomped && !blood && !scream) { // Ripple attacks
					if (npc.ai[2] == 0f){
						openMouth = false;
					}
					Vector2 vector;
					vector = new Vector2(npc.Center.X, npc.Center.Y - 10f);
					Vector2 vector2 = player.DirectionFrom(vector);
					if (npc.ai[2] >= (float)(100 - rage) && !player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){
						if (!player.dead && !Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 18f, ModContent.ProjectileType<ViscountRipple2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -10f;
						}
						else if (!player.dead && player.DistanceSQ(npc.Center) > 250000f){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 18f, ModContent.ProjectileType<ViscountRipple3>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -20f;
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 15f, ModContent.ProjectileType<ViscountRipple>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 7.5f
							}
							npc.ai[2] = -20f;
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
					}
					else if (npc.ai[2] >= 0f && !player.dead && player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){ // When player is bat
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, vector, vector2 * 9f, ModContent.ProjectileType<ViscountRipple2>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 6
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
						npc.ai[2] = -20f;
					}
				}
				else{
					openMouth = false;
					npc.ai[2] = -60f;
				}
				if (npc.ai[1] == 0f){
					if (npc.ai[0] == 60f){
						float num = 40f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector3 = Vector2.UnitX * 0f;
							vector3 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(50f, 50f);
							vector3 = Terraria.Utils.RotatedBy(vector3, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num3 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = npc.Center + vector3;
							Main.dust[num3].velocity = -(npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector3, Vector2.UnitY) * 3.25f);
							num2++;
						}
					}
					if (npc.ai[0] > 60f){ // Viscount Blood attack
							blood = true;
							if (!player.dead && npc.ai[0] == (float)(140 - rage) && Main.netMode != 1){
								for (int i = 0; i < 7; i++)
								{
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -4f, 4f), Terraria.Utils.NextFloat(Main.rand, -3f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] == (float)(190 - rage) && Main.netMode != 1){
								for (int j = 0; j < 12; j++)
								{
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -4f, 4f), Terraria.Utils.NextFloat(Main.rand, -3.5f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] >= (float)(250 - rage)){
								if (Main.netMode != 1){
									for (int k = 0; k < 15; k++)
									{
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -6f, 6f), Terraria.Utils.NextFloat(Main.rand, -4f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
									}
								}
								npc.ai[0] = (float)(-180 - rage);
							}
							if (npc.ai[0] == (float)(139 - rage) || npc.ai[0] == (float)(189 - rage) || npc.ai[0] == (float)(249 - rage)){
								SoundEngine.PlaySound(SoundID.Item87, new Vector2?(npc.Center), null);
								float num4 = 30f;
								int num5 = 0;
								while ((float)num5 < num4){
									Vector2 vector4 = Vector2.UnitX * 0f;
									vector4 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num5 * (6.2831855f / num4)), default(Vector2)) * new Vector2(20f, 20f);
									vector4 = Terraria.Utils.RotatedBy(vector4, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
									int num6 = Dust.NewDust(npc.Center, 0, 0, 5, 0f, 0f, 100, default(Color), 1.5f);
									Main.dust[num6].noGravity = true;
									Main.dust[num6].position = npc.Center + vector4;
									Main.dust[num6].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector4, Vector2.UnitY) * 5f;
									num5++;
								}
							}
					}
					else { blood = false;}
				}
				else if (npc.ai[1] == 1f){ // stomp
					if (npc.ai[0] >= 300f && !stomp && !player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						stomp = true;
						counter = 9;
						npc.velocity.Y = -5f;
						npc.aiStyle = -2;
						npc.ai[0] = 301f;
						npc.netUpdate = true;
					}
					if (stomp){
						npc.velocity.X = 0f;
						NPC npc5 = npc;
						npc5.velocity.Y = npc5.velocity.Y + 0.32f;
						if (npc.velocity.Y > 19f){
							npc.velocity.Y = 19f;
						}
						bool flag = npc.ai[0] >= 390f;
						if (npc.ai[0] >= 345f && (flag || (npc.IsOnStandableGround(0f, false) && npc.Bottom.Y > player.Bottom.Y - 20f))){
							stomp = false;
							npc.velocity.Y = 0f;
							counter = 14;
							if (!flag){
								if (Main.netMode != 1){
									float num7 = 1f;
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 60f, 0f, 1f, ModContent.ProjectileType<ViscountStomp>(), 30, 0f, Main.myPlayer, num7, 0f, 0f);
									for (int l = 0; l < 255; l++)
									{
										Player player2 = Main.player[l];
										AttachedEntityEntitySource_Parent attachedEntityEntitySource_Parent = new AttachedEntityEntitySource_Parent(npc, player2, null);
										if (player2.active && !player2.dead && player2.DistanceSQ(npc.Center) < 250000f)
										{
											Projectile.NewProjectile(attachedEntityEntitySource_Parent, player2.Center, new Vector2(Terraria.Utils.NextFloat(Main.rand, -0.25f, 0.25f), 0f), ModContent.ProjectileType<ViscountStomp2>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
									}
								}
								stomped = true;
								npc.ai[0] = (float)(-150 - rage);
							}
							else{
								npc.ai[0] = 0f;
							}
						}
					}
					if (stomped){
						npc.velocity.Y = 0f;
					}
				}
				else if (npc.ai[1] == 2f){
					if (npc.ai[0] >= 60f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0.1f;
					}
					if (npc.ai[0] == (float)(160 - rage)){
						scream = true;
						counter = 19;
						counting = 0;
					}
					if (npc.ai[0] == (float)(180 - rage)){
						SoundEngine.PlaySound(SoundID.Zombie98, new Vector2?(npc.Center), null);
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 20f, (float)npc.spriteDirection * 0.1f, 0f, ModContent.ProjectileType<CountScream>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							for (int m = 0; m < 19; m++){
								Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-600, 600), player.Center.Y + 16f, 0f, Terraria.Utils.NextFloat(Main.rand, -17f, -14f), ModContent.ProjectileType<ViscountRockSummon>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							for (int n = 0; n < 24; n++){
								Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -8f, 8f), Terraria.Utils.NextFloat(Main.rand, -8f, 8f), ModContent.ProjectileType<ViscountRockSummon2>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
						}
						float num8 = 60f;
						int num9 = 0;
						while ((float)num9 < num8){
							Vector2 vector5 = Vector2.UnitX * 0f;
							vector5 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num9 * (6.2831855f / num8)), default(Vector2)) * new Vector2(30f, 30f);
							vector5 = Terraria.Utils.RotatedBy(vector5, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num10 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num10].noGravity = true;
							Main.dust[num10].position = npc.Center + vector5;
							Main.dust[num10].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector5, Vector2.UnitY) * 4f;
							num9++;
						}
					}
					if (npc.ai[0] == (float)(185 - rage)){
						float num11 = 60f;
						int num12 = 0;
						while ((float)num12 < num11){
							Vector2 vector6 = Vector2.UnitX * 0f;
							vector6 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num12 * (6.2831855f / num11)), default(Vector2)) * new Vector2(30f, 30f);
							vector6 = Terraria.Utils.RotatedBy(vector6, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num13 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num13].noGravity = true;
							Main.dust[num13].position = npc.Center + vector6;
							Main.dust[num13].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector6, Vector2.UnitY) * 6f;
							num12++;
						}
					}
					if (npc.ai[0] >= (float)(190 - rage)){
						float num14 = 60f;
						int num15 = 0;
						while ((float)num15 < num14){
							Vector2 vector7 = Vector2.UnitX * 0f;
							vector7 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num15 * (6.2831855f / num14)), default(Vector2)) * new Vector2(30f, 30f);
							vector7 = Terraria.Utils.RotatedBy(vector7, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num16 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num16].noGravity = true;
							Main.dust[num16].position = npc.Center + vector7;
							Main.dust[num16].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector7, Vector2.UnitY) * 8f;
							num15++;
						}
						npc.ai[0] = (float)(-120 - rage);
					}
				}
				else if (npc.ai[1] == 3f && npc.ai[0] >= 90f){
					SoundEngine.PlaySound(SoundID.Zombie40, new Vector2?(npc.Center), null);
					if (Main.netMode != 1){
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 30, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
					}
					for (int num17 = 0; num17 < 25; num17++){
						int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1.5f);
						Main.dust[num18].noGravity = true;
					}
					npc.ai[0] = (float)(-180 - rage);
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.33){
					npc.ai[3] += 1f;
					if (npc.ai[3] == 1000f || npc.ai[3] == 1050f || npc.ai[3] == 1100f || npc.ai[3] == 1125f || npc.ai[3] == 1150f || npc.ai[3] == 1175f || npc.ai[3] == 1180f || npc.ai[3] == 1185f || npc.ai[3] == 1190f || npc.ai[3] == 1195f){
						for (int num19 = 0; num19 < 255; num19++){
							Player player3 = Main.player[num19];
							if (player3.active && !player3.dead && player3.DistanceSQ(npc.Center) < 1562500f){
								float num20 = 30f;
								int num21 = 0;
								while ((float)num21 < num20){
									Vector2 vector8 = Vector2.UnitX * 0f;
									vector8 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num21 * (6.2831855f / num20)), default(Vector2)) * new Vector2(50f, 50f);
									vector8 = Terraria.Utils.RotatedBy(vector8, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
									int num22 = Dust.NewDust(player.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
									Main.dust[num22].noGravity = true;
									Main.dust[num22].position = player.Center + vector8;
									Main.dust[num22].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector8, Vector2.UnitY) * 3f);
									num21++;
								}
							}
						}
					}
					if (npc.ai[3] > 1200f){
						SoundEngine.PlaySound(SoundID.Zombie100, new Vector2?(npc.Center), null);
						for (int num23 = 0; num23 < 25; num23++){
							int num24 = Dust.NewDust(npc.position, npc.width, npc.height, 90, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1f);
							Main.dust[num24].noGravity = true;
						}
						float num25 = 65f;
						int num26 = 0;
						while ((float)num26 < num25){
							Vector2 vector9 = Vector2.UnitX * 0f;
							vector9 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num26 * (6.2831855f / num25)), default(Vector2)) * new Vector2(40f, 40f);
							vector9 = Terraria.Utils.RotatedBy(vector9, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num27 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
							Main.dust[num27].noGravity = true;
							Main.dust[num27].position = npc.Center + vector9;
							Main.dust[num27].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector9, Vector2.UnitY) * 10f;
							num26++;
						}
						int num28 = ModContent.BuffType<VampiresCurseBuff>();
						for (int num29 = 0; num29 < 255; num29++){
							Player player4 = Main.player[num29];
							if (player4.active && !player4.dead && player4.DistanceSQ(npc.Center) < 1562500f && !player4.HasBuff(156) && !player4.HasBuff(num28)){
								player4.AddBuff(num28, 600, true, false);
								for (int num30 = 0; num30 < 20; num30++){
									int num31 = Dust.NewDust(player4.position, player4.width, player4.height, 5, 0f, 0f, 75, default(Color), 1.25f);
									Main.dust[num31].noGravity = true;
									Main.dust[num31].velocity *= 0.75f;
									int num32 = Main.rand.Next(-50, 51);
									int num33 = Main.rand.Next(-50, 51);
									Dust dust = Main.dust[num31];
									dust.position.X = dust.position.X + (float)num32;
									Dust dust2 = Main.dust[num31];
									dust2.position.Y = dust2.position.Y + (float)num33;
									Main.dust[num31].velocity.X = -(float)num32 * 0.065f;
									Main.dust[num31].velocity.Y = -(float)num33 * 0.065f;
								}
							}
						}
						if (npc.ai[1] != 1f){
							npc.ai[0] = -300f;
						}
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isDeath) {
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
				}
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){ // Ends the fight if there is not valid target{
					shift = false;
					openMouth = false;
					blood = false;
					stomp = false;
					stomped = false;
					scream = false;
					//NPC npc = npc;
					npc.velocity.Y = npc.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){ npc.timeLeft = 20;}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (!stomp && !stomped){
					npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X <= npc.Center.X);
				}
				if (!stomp && !stomped && !scream){  // Movement and sprite direction
					if (!shift){
						flux += 3;
					}
					else{
						flux -= 3;
					}
					if (flux > 66 && !shift){
						shift = true;
					}
					if (flux <= -66){
						shift = false;
					}
					dodgeTimer++;
					if (dodgeTimer > 60){
						dodgeTimer = 0;
						NPC npc2 = npc;
						npc2.velocity.X = npc2.velocity.X + (float)(-1 * npc.spriteDirection);
					}
					if (player.position.Y < npc.position.Y + 250f + (float)flux){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y - ((npc.velocity.Y > 0f) ? 0.85f : 0.07f);
					}
					if (player.position.Y > npc.position.Y + 250f + (float)flux){
						NPC npc4 = npc;
						npc4.velocity.Y = npc4.velocity.Y + ((npc.velocity.Y < 0f) ? 0.85f : 0.07f);
					}
				}
				else{
					dodgeTimer = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f; // Attack counter
				if ((double)npc.life < (double)npc.lifeMax * 0.25) {
					rage = 70; // original 60
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5) {
					rage = 60; // original 45
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75) {
					rage = 50; // original 30
				}
				else {
						rage = 30; //orginal 15
				}
				if (npc.aiStyle != -2){
					npc.ai[1] -= 1f;
				}
				if (npc.ai[0] == 0f && Main.netMode != 1){ // Select an attack option based on current health
					if ((double)npc.life < (double)npc.lifeMax * 0.6){ // Summon Bitey Babies as attack option
						npc.ai[1] = (float)Main.rand.Next(4);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.75){ // Add Rock Falls to the attack options
						npc.ai[1] = (float)Main.rand.Next(3);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.9){ // Add stomp to the attack options
						npc.ai[1] = (float)Main.rand.Next(2);
					}
					else{ // Only Ripples and blood attacks
						npc.ai[1] = 0f; 
					}
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}
		
				if (!stomp && !stomped && !blood && !scream) { // Ripple attacks
					if (npc.ai[2] == 0f){
						openMouth = false;
					}
					Vector2 vector;
					vector = new Vector2(npc.Center.X, npc.Center.Y - 10f);
					Vector2 vector2 = player.DirectionFrom(vector);
					if (npc.ai[2] >= (float)(100 - rage) && !player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){
						if (!player.dead && !Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 12.5f, ModContent.ProjectileType<ViscountRipple2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -10f;
						}
						else if (!player.dead && player.DistanceSQ(npc.Center) > 250000f){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 12.5f, ModContent.ProjectileType<ViscountRipple3>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -20f;
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 8f, ModContent.ProjectileType<ViscountRipple>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 7.5f
							}
							npc.ai[2] = -20f;
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
					}
					else if (npc.ai[2] >= 0f && !player.dead && player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){ // When player is bat
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, vector, vector2 * 7.5f, ModContent.ProjectileType<ViscountRipple2>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 6
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
						npc.ai[2] = -20f;
					}
				}
				else{
					openMouth = false;
					npc.ai[2] = -60f;
				}
				if (npc.ai[1] == 0f){ // Viscount Blood attack
					if (npc.ai[0] == 60f){
						float num = 40f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector3 = Vector2.UnitX * 0f;
							vector3 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(50f, 50f);
							vector3 = Terraria.Utils.RotatedBy(vector3, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num3 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = npc.Center + vector3;
							Main.dust[num3].velocity = -(npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector3, Vector2.UnitY) * 3.25f);
							num2++;
						}
					}
					if (npc.ai[0] > 60f){ 
							blood = true;
							if (!player.dead && npc.ai[0] == (float)(140 - rage) && Main.netMode != 1){
								for (int i = 0; i < 5; i++){
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] == (float)(190 - rage) && Main.netMode != 1){
								for (int j = 0; j < 10; j++){
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3.5f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] >= (float)(250 - rage)){
								if (Main.netMode != 1){
									for (int k = 0; k < 15; k++){
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -4f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
									}
								}
								npc.ai[0] = (float)(-180 - rage);
							}
							if (npc.ai[0] == (float)(139 - rage) || npc.ai[0] == (float)(189 - rage) || npc.ai[0] == (float)(249 - rage)){
								SoundEngine.PlaySound(SoundID.Item87, new Vector2?(npc.Center), null);
								float num4 = 30f;
								int num5 = 0;
								while ((float)num5 < num4){
									Vector2 vector4 = Vector2.UnitX * 0f;
									vector4 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num5 * (6.2831855f / num4)), default(Vector2)) * new Vector2(20f, 20f);
									vector4 = Terraria.Utils.RotatedBy(vector4, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
									int num6 = Dust.NewDust(npc.Center, 0, 0, 5, 0f, 0f, 100, default(Color), 1.5f);
									Main.dust[num6].noGravity = true;
									Main.dust[num6].position = npc.Center + vector4;
									Main.dust[num6].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector4, Vector2.UnitY) * 5f;
									num5++;
								}
							}
					}
					else { blood = false;}
				}
				else if (npc.ai[1] == 1f){  // stomp
					if (npc.ai[0] >= 300f && !stomp && !player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						stomp = true;
						counter = 9;
						npc.velocity.Y = -5f;
						npc.aiStyle = -2;
						npc.ai[0] = 301f;
						npc.netUpdate = true;
					}
					if (stomp){ 
						npc.velocity.X = 0f;
						NPC npc5 = npc;
						npc5.velocity.Y = npc5.velocity.Y + 0.35f; // Stomp faster (original 0.32f)
						
						if (npc.velocity.Y > 20f){ // original 19f
							npc.velocity.Y = 20f;
						}
						bool flag = npc.ai[0] >= 390f;
						if (npc.ai[0] >= 345f && (flag || (npc.IsOnStandableGround(0f, false) && npc.Bottom.Y > player.Bottom.Y - 20f))){
							stomp = false;
							npc.velocity.Y = 0f;
							counter = 14;
							if (!flag){
								if (Main.netMode != 1){
									float num7 = 1f;
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 60f, 0f, 1f, ModContent.ProjectileType<ViscountStomp>(), 30, 0f, Main.myPlayer, num7, 0f, 0f);
									for (int l = 0; l < 255; l++)
									{
										Player player2 = Main.player[l];
										AttachedEntityEntitySource_Parent attachedEntityEntitySource_Parent = new AttachedEntityEntitySource_Parent(npc, player2, null);
										if (player2.active && !player2.dead && player2.DistanceSQ(npc.Center) < 250000f)
										{
											Projectile.NewProjectile(attachedEntityEntitySource_Parent, player2.Center, new Vector2(Terraria.Utils.NextFloat(Main.rand, -0.25f, 0.25f), 0f), ModContent.ProjectileType<ViscountStomp2>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
									}
								}
								stomped = true;
								npc.ai[0] = (float)(-150 - rage);
							}
							else{
								npc.ai[0] = 0f;
							}
						}
					}
					if (stomped){
						npc.velocity.Y = 0f;
					}
				}
				else if (npc.ai[1] == 2f){ // Rock attack
					if (npc.ai[0] >= 60f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0.1f;
					}
					if (npc.ai[0] == (float)(160 - rage)){
						scream = true;
						counter = 19;
						counting = 0;
					}
					if (npc.ai[0] == (float)(180 - rage)){
						SoundEngine.PlaySound(SoundID.Zombie98, new Vector2?(npc.Center), null);
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 20f, (float)npc.spriteDirection * 0.1f, 0f, ModContent.ProjectileType<CountScream>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							for (int m = 0; m < 12; m++){
								Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-300, 300), player.Center.Y + 16f, 0f, Terraria.Utils.NextFloat(Main.rand, -12f, -8f), ModContent.ProjectileType<ViscountRockSummon>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							for (int n = 0; n < 24; n++){
								Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -8f, 8f), Terraria.Utils.NextFloat(Main.rand, -8f, 8f), ModContent.ProjectileType<ViscountRockSummon2>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
						}
						float num8 = 60f;
						int num9 = 0;
						while ((float)num9 < num8){
							Vector2 vector5 = Vector2.UnitX * 0f;
							vector5 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num9 * (6.2831855f / num8)), default(Vector2)) * new Vector2(30f, 30f);
							vector5 = Terraria.Utils.RotatedBy(vector5, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num10 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num10].noGravity = true;
							Main.dust[num10].position = npc.Center + vector5;
							Main.dust[num10].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector5, Vector2.UnitY) * 4f;
							num9++;
						}
					}
					if (npc.ai[0] == (float)(185 - rage)){
						float num11 = 60f;
						int num12 = 0;
						while ((float)num12 < num11){
							Vector2 vector6 = Vector2.UnitX * 0f;
							vector6 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num12 * (6.2831855f / num11)), default(Vector2)) * new Vector2(30f, 30f);
							vector6 = Terraria.Utils.RotatedBy(vector6, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num13 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num13].noGravity = true;
							Main.dust[num13].position = npc.Center + vector6;
							Main.dust[num13].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector6, Vector2.UnitY) * 6f;
							num12++;
						}
					}
					if (npc.ai[0] >= (float)(190 - rage)){
						float num14 = 60f;
						int num15 = 0;
						while ((float)num15 < num14){
							Vector2 vector7 = Vector2.UnitX * 0f;
							vector7 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num15 * (6.2831855f / num14)), default(Vector2)) * new Vector2(30f, 30f);
							vector7 = Terraria.Utils.RotatedBy(vector7, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num16 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num16].noGravity = true;
							Main.dust[num16].position = npc.Center + vector7;
							Main.dust[num16].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector7, Vector2.UnitY) * 8f;
							num15++;
						}
						npc.ai[0] = (float)(-120 - rage);
					}
				}
				else if (npc.ai[1] == 3f && npc.ai[0] >= 90f){   // Summon bats
					SoundEngine.PlaySound(SoundID.Zombie40, new Vector2?(npc.Center), null);
					if (Main.netMode != 1){
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 30, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
					}
					for (int num17 = 0; num17 < 25; num17++){
						int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1.5f);
						Main.dust[num18].noGravity = true;
					}
					npc.ai[0] = (float)(-180 - rage);
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.33){
					npc.ai[3] += 1f;
					if (npc.ai[3] == 1000f || npc.ai[3] == 1050f || npc.ai[3] == 1100f || npc.ai[3] == 1125f || npc.ai[3] == 1150f || npc.ai[3] == 1175f || npc.ai[3] == 1180f || npc.ai[3] == 1185f || npc.ai[3] == 1190f || npc.ai[3] == 1195f){
						for (int num19 = 0; num19 < 255; num19++){
							Player player3 = Main.player[num19];
							if (player3.active && !player3.dead && player3.DistanceSQ(npc.Center) < 1562500f){
								float num20 = 30f;
								int num21 = 0;
								while ((float)num21 < num20){
									Vector2 vector8 = Vector2.UnitX * 0f;
									vector8 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num21 * (6.2831855f / num20)), default(Vector2)) * new Vector2(50f, 50f);
									vector8 = Terraria.Utils.RotatedBy(vector8, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
									int num22 = Dust.NewDust(player.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
									Main.dust[num22].noGravity = true;
									Main.dust[num22].position = player.Center + vector8;
									Main.dust[num22].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector8, Vector2.UnitY) * 3f);
									num21++;
								}
							}
						}
					}
					if (npc.ai[3] > 1200f){
						SoundEngine.PlaySound(SoundID.Zombie100, new Vector2?(npc.Center), null);
						for (int num23 = 0; num23 < 25; num23++){
							int num24 = Dust.NewDust(npc.position, npc.width, npc.height, 90, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1f);
							Main.dust[num24].noGravity = true;
						}
						float num25 = 65f;
						int num26 = 0;
						while ((float)num26 < num25){
							Vector2 vector9 = Vector2.UnitX * 0f;
							vector9 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num26 * (6.2831855f / num25)), default(Vector2)) * new Vector2(40f, 40f);
							vector9 = Terraria.Utils.RotatedBy(vector9, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num27 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
							Main.dust[num27].noGravity = true;
							Main.dust[num27].position = npc.Center + vector9;
							Main.dust[num27].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector9, Vector2.UnitY) * 10f;
							num26++;
						}
						int vampirecurse = ModContent.BuffType<VampiresCurseBuff>();
						for (int num29 = 0; num29 < 255; num29++){
							Player player4 = Main.player[num29];
							if (player4.active && !player4.dead && player4.DistanceSQ(npc.Center) < 1562500f && !player4.HasBuff(156) && !player4.HasBuff(vampirecurse)){
								player4.AddBuff(vampirecurse, 600, true, false);
								for (int num30 = 0; num30 < 20; num30++){
									int num31 = Dust.NewDust(player4.position, player4.width, player4.height, 5, 0f, 0f, 75, default(Color), 1.25f);
									Main.dust[num31].noGravity = true;
									Main.dust[num31].velocity *= 0.75f;
									int num32 = Main.rand.Next(-50, 51);
									int num33 = Main.rand.Next(-50, 51);
									Dust dust = Main.dust[num31];
									dust.position.X = dust.position.X + (float)num32;
									Dust dust2 = Main.dust[num31];
									dust2.position.Y = dust2.position.Y + (float)num33;
									Main.dust[num31].velocity.X = -(float)num32 * 0.065f;
									Main.dust[num31].velocity.Y = -(float)num33 * 0.065f;
								}
							}
						}
						if (npc.ai[1] != 1f){
							npc.ai[0] = -300f;
						}
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isRevengeance){
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
				}
				if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active){
					npc.TargetClosest(true);
				}
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){ // Ends the fight if there is not valid target{
					shift = false;
					openMouth = false;
					blood = false;
					stomp = false;
					stomped = false;
					scream = false;
					//NPC npc = npc;
					npc.velocity.Y = npc.velocity.Y + 0.1f;
					if (npc.timeLeft > 20){ npc.timeLeft = 20;}
					return false;
				}
				IEntitySource source_FromAI = npc.GetSource_FromAI(null);
				if (!stomp && !stomped){
					npc.spriteDirection = Terraria.Utils.ToDirectionInt(player.Center.X <= npc.Center.X);
				}
				if (!stomp && !stomped && !scream){  // Movement and sprite direction
					if (!shift){
						flux += 3;
					}
					else{
						flux -= 3;
					}
					if (flux > 66 && !shift){
						shift = true;
					}
					if (flux <= -66){
						shift = false;
					}
					dodgeTimer++;
					if (dodgeTimer > 60){
						dodgeTimer = 0;
						NPC npc2 = npc;
						npc2.velocity.X = npc2.velocity.X + (float)(-1 * npc.spriteDirection);
					}
					if (player.position.Y < npc.position.Y + 250f + (float)flux){
						NPC npc3 = npc;
						npc3.velocity.Y = npc3.velocity.Y - ((npc.velocity.Y > 0f) ? 0.85f : 0.07f);
					}
					if (player.position.Y > npc.position.Y + 250f + (float)flux){
						NPC npc4 = npc;
						npc4.velocity.Y = npc4.velocity.Y + ((npc.velocity.Y < 0f) ? 0.85f : 0.07f);
					}
				}
				else
				{
					dodgeTimer = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f; // Attack counter
				if ((double)npc.life < (double)npc.lifeMax * 0.25) {
					rage = 65; // original 60
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.5) {
					rage = 55; // original 45
				}
				else if ((double)npc.life < (double)npc.lifeMax * 0.75) {
					rage = 40; // original 30
				}
				else {
						rage = 20; //orginal 15
				}
				if (npc.aiStyle != -2){
					npc.ai[1] -= 1f;
				}
				if (npc.ai[0] == 0f && Main.netMode != 1){ // Select an attack option based on current health
					if ((double)npc.life < (double)npc.lifeMax * 0.6){
						npc.ai[1] = (float)Main.rand.Next(4);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.75){
						npc.ai[1] = (float)Main.rand.Next(3);
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.9){
						npc.ai[1] = (float)Main.rand.Next(2);
					}
					else{
						npc.ai[1] = 0f;
					}
					npc.TargetClosest(true);
					npc.netUpdate = true;
				}
		
				if (!stomp && !stomped && !blood && !scream) { // Ripple attacks
					if (npc.ai[2] == 0f){
						openMouth = false;
					}
					Vector2 vector;
					vector = new Vector2(npc.Center.X, npc.Center.Y - 10f);
					Vector2 vector2 = player.DirectionFrom(vector);
					if (npc.ai[2] >= (float)(100 - rage) && !player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){
						if (!player.dead && !Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 12.5f, ModContent.ProjectileType<ViscountRipple2>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -10f;
						}
						else if (!player.dead && player.DistanceSQ(npc.Center) > 250000f){
							if (Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 12.5f, ModContent.ProjectileType<ViscountRipple3>(), 20, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 12
							}
							npc.ai[2] = -20f;
						}
						else{
							if (!player.dead && Main.netMode != 1){
								Projectile.NewProjectile(source_FromAI, vector, vector2 * 8f, ModContent.ProjectileType<ViscountRipple>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 7.5f
							}
							npc.ai[2] = -20f;
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
					}
					else if (npc.ai[2] >= 0f && !player.dead && player.HasBuff(ModContent.BuffType<VampiresCurseBuff>())){ // When player is bat
						if (Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, vector, vector2 * 7.5f, ModContent.ProjectileType<ViscountRipple2>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f); // Original speed 6
						}
						SoundEngine.PlaySound(SoundID.Zombie71, new Vector2?(npc.Center), null);
						openMouth = true;
						npc.ai[2] = -20f;
					}
				}
				else{
					openMouth = false;
					npc.ai[2] = -60f;
				}
				if (npc.ai[1] == 0f){
					if (npc.ai[0] == 60f){
						float num = 40f;
						int num2 = 0;
						while ((float)num2 < num){
							Vector2 vector3 = Vector2.UnitX * 0f;
							vector3 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num2 * (6.2831855f / num)), default(Vector2)) * new Vector2(50f, 50f);
							vector3 = Terraria.Utils.RotatedBy(vector3, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num3 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.25f);
							Main.dust[num3].noGravity = true;
							Main.dust[num3].position = npc.Center + vector3;
							Main.dust[num3].velocity = -(npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector3, Vector2.UnitY) * 3.25f);
							num2++;
						}
					}
					if (npc.ai[0] > 60f){ // Viscount Blood attack
							blood = true;
							if (!player.dead && npc.ai[0] == (float)(140 - rage) && Main.netMode != 1){
								for (int i = 0; i < 5; i++){
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] == (float)(190 - rage) && Main.netMode != 1){
								for (int j = 0; j < 10; j++){
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3.5f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
								}
							}
							if (!player.dead && npc.ai[0] >= (float)(250 - rage)){
								if (Main.netMode != 1){
									for (int k = 0; k < 15; k++){
										Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -4f, -2f), ModContent.ProjectileType<ViscountBlood>(), 15, 0f, Main.myPlayer, 0f, 0f, 0f);
									}
								}
								npc.ai[0] = (float)(-180 - rage);
							}
							if (npc.ai[0] == (float)(139 - rage) || npc.ai[0] == (float)(189 - rage) || npc.ai[0] == (float)(249 - rage)){
								SoundEngine.PlaySound(SoundID.Item87, new Vector2?(npc.Center), null);
								float num4 = 30f;
								int num5 = 0;
								while ((float)num5 < num4){
									Vector2 vector4 = Vector2.UnitX * 0f;
									vector4 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num5 * (6.2831855f / num4)), default(Vector2)) * new Vector2(20f, 20f);
									vector4 = Terraria.Utils.RotatedBy(vector4, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
									int num6 = Dust.NewDust(npc.Center, 0, 0, 5, 0f, 0f, 100, default(Color), 1.5f);
									Main.dust[num6].noGravity = true;
									Main.dust[num6].position = npc.Center + vector4;
									Main.dust[num6].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector4, Vector2.UnitY) * 5f;
									num5++;
								}
							}
					}
					else { blood = false;}
				}
				else if (npc.ai[1] == 1f){
					if (npc.ai[0] >= 300f && !stomp && !player.dead && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height)){
						stomp = true;
						counter = 9;
						npc.velocity.Y = -5f;
						npc.aiStyle = -2;
						npc.ai[0] = 301f;
						npc.netUpdate = true;
					}
					if (stomp){
						npc.velocity.X = 0f;
						NPC npc5 = npc;
						npc5.velocity.Y = npc5.velocity.Y + 0.32f;
						if (npc.velocity.Y > 19f){
							npc.velocity.Y = 19f;
						}
						bool flag = npc.ai[0] >= 390f;
						if (npc.ai[0] >= 345f && (flag || (npc.IsOnStandableGround(0f, false) && npc.Bottom.Y > player.Bottom.Y - 20f))){
							stomp = false;
							npc.velocity.Y = 0f;
							counter = 14;
							if (!flag){
								if (Main.netMode != 1){
									float num7 = 1f;
									Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y + 60f, 0f, 1f, ModContent.ProjectileType<ViscountStomp>(), 30, 0f, Main.myPlayer, num7, 0f, 0f);
									for (int l = 0; l < 255; l++)
									{
										Player player2 = Main.player[l];
										AttachedEntityEntitySource_Parent attachedEntityEntitySource_Parent = new AttachedEntityEntitySource_Parent(npc, player2, null);
										if (player2.active && !player2.dead && player2.DistanceSQ(npc.Center) < 250000f)
										{
											Projectile.NewProjectile(attachedEntityEntitySource_Parent, player2.Center, new Vector2(Terraria.Utils.NextFloat(Main.rand, -0.25f, 0.25f), 0f), ModContent.ProjectileType<ViscountStomp2>(), 30, 0f, Main.myPlayer, 0f, 0f, 0f);
										}
									}
								}
								stomped = true;
								npc.ai[0] = (float)(-150 - rage);
							}
							else{
								npc.ai[0] = 0f;
							}
						}
					}
					if (stomped){
						npc.velocity.Y = 0f;
					}
				}
				else if (npc.ai[1] == 2f){
					if (npc.ai[0] >= 60f){
						npc.velocity.X = 0f;
						npc.velocity.Y = 0.1f;
					}
					if (npc.ai[0] == (float)(160 - rage)){
						scream = true;
						counter = 19;
						counting = 0;
					}
					if (npc.ai[0] == (float)(180 - rage)){
						SoundEngine.PlaySound(SoundID.Zombie98, new Vector2?(npc.Center), null);
						if (!player.dead && Main.netMode != 1){
							Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y - 20f, (float)npc.spriteDirection * 0.1f, 0f, ModContent.ProjectileType<CountScream>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							for (int m = 0; m < 12; m++){
								Projectile.NewProjectile(source_FromAI, player.Center.X + (float)Main.rand.Next(-300, 300), player.Center.Y + 16f, 0f, Terraria.Utils.NextFloat(Main.rand, -12f, -8f), ModContent.ProjectileType<ViscountRockSummon>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
							for (int n = 0; n < 24; n++){
								Projectile.NewProjectile(source_FromAI, npc.Center.X, npc.Center.Y, Terraria.Utils.NextFloat(Main.rand, -8f, 8f), Terraria.Utils.NextFloat(Main.rand, -8f, 8f), ModContent.ProjectileType<ViscountRockSummon2>(), 0, 0f, Main.myPlayer, 0f, 0f, 0f);
							}
						}
						float num8 = 60f;
						int num9 = 0;
						while ((float)num9 < num8){
							Vector2 vector5 = Vector2.UnitX * 0f;
							vector5 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num9 * (6.2831855f / num8)), default(Vector2)) * new Vector2(30f, 30f);
							vector5 = Terraria.Utils.RotatedBy(vector5, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num10 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num10].noGravity = true;
							Main.dust[num10].position = npc.Center + vector5;
							Main.dust[num10].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector5, Vector2.UnitY) * 4f;
							num9++;
						}
					}
					if (npc.ai[0] == (float)(185 - rage)){
						float num11 = 60f;
						int num12 = 0;
						while ((float)num12 < num11){
							Vector2 vector6 = Vector2.UnitX * 0f;
							vector6 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num12 * (6.2831855f / num11)), default(Vector2)) * new Vector2(30f, 30f);
							vector6 = Terraria.Utils.RotatedBy(vector6, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num13 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num13].noGravity = true;
							Main.dust[num13].position = npc.Center + vector6;
							Main.dust[num13].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector6, Vector2.UnitY) * 6f;
							num12++;
						}
					}
					if (npc.ai[0] >= (float)(190 - rage)){
						float num14 = 60f;
						int num15 = 0;
						while ((float)num15 < num14){
							Vector2 vector7 = Vector2.UnitX * 0f;
							vector7 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num15 * (6.2831855f / num14)), default(Vector2)) * new Vector2(30f, 30f);
							vector7 = Terraria.Utils.RotatedBy(vector7, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num16 = Dust.NewDust(npc.Center, 0, 0, 110, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num16].noGravity = true;
							Main.dust[num16].position = npc.Center + vector7;
							Main.dust[num16].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector7, Vector2.UnitY) * 8f;
							num15++;
						}
						npc.ai[0] = (float)(-120 - rage);
					}
				}
				else if (npc.ai[1] == 3f && npc.ai[0] >= 90f){
					SoundEngine.PlaySound(SoundID.Zombie40, new Vector2?(npc.Center), null);
					if (Main.netMode != 1){
						NPC.NewNPC(source_FromAI, (int)npc.Center.X - 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X, (int)npc.Center.Y + 30, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
						NPC.NewNPC(source_FromAI, (int)npc.Center.X + 40, (int)npc.Center.Y + 10, ModContent.NPCType<BiteyBaby>(), 0, 0f, 0f, 0f, 0f, 255);
					}
					for (int num17 = 0; num17 < 25; num17++){
						int num18 = Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1.5f);
						Main.dust[num18].noGravity = true;
					}
					npc.ai[0] = (float)(-180 - rage);
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.33){
					npc.ai[3] += 1f;
					if (npc.ai[3] == 1000f || npc.ai[3] == 1050f || npc.ai[3] == 1100f || npc.ai[3] == 1125f || npc.ai[3] == 1150f || npc.ai[3] == 1175f || npc.ai[3] == 1180f || npc.ai[3] == 1185f || npc.ai[3] == 1190f || npc.ai[3] == 1195f){
						for (int num19 = 0; num19 < 255; num19++){
							Player player3 = Main.player[num19];
							if (player3.active && !player3.dead && player3.DistanceSQ(npc.Center) < 1562500f){
								float num20 = 30f;
								int num21 = 0;
								while ((float)num21 < num20){
									Vector2 vector8 = Vector2.UnitX * 0f;
									vector8 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num21 * (6.2831855f / num20)), default(Vector2)) * new Vector2(50f, 50f);
									vector8 = Terraria.Utils.RotatedBy(vector8, (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
									int num22 = Dust.NewDust(player.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
									Main.dust[num22].noGravity = true;
									Main.dust[num22].position = player.Center + vector8;
									Main.dust[num22].velocity = -(player.velocity * 0f + Terraria.Utils.SafeNormalize(vector8, Vector2.UnitY) * 3f);
									num21++;
								}
							}
						}
					}
					if (npc.ai[3] > 1200f){
						SoundEngine.PlaySound(SoundID.Zombie100, new Vector2?(npc.Center), null);
						for (int num23 = 0; num23 < 25; num23++){
							int num24 = Dust.NewDust(npc.position, npc.width, npc.height, 90, (float)Main.rand.Next(-8, 8), (float)Main.rand.Next(-8, 8), 100, default(Color), 1f);
							Main.dust[num24].noGravity = true;
						}
						float num25 = 65f;
						int num26 = 0;
						while ((float)num26 < num25){
							Vector2 vector9 = Vector2.UnitX * 0f;
							vector9 += -Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)num26 * (6.2831855f / num25)), default(Vector2)) * new Vector2(40f, 40f);
							vector9 = Terraria.Utils.RotatedBy(vector9, (double)Terraria.Utils.ToRotation(npc.velocity), default(Vector2));
							int num27 = Dust.NewDust(npc.Center, 0, 0, 90, 0f, 0f, 100, default(Color), 1.75f);
							Main.dust[num27].noGravity = true;
							Main.dust[num27].position = npc.Center + vector9;
							Main.dust[num27].velocity = npc.velocity * 0f + Terraria.Utils.SafeNormalize(vector9, Vector2.UnitY) * 10f;
							num26++;
						}
						int num28 = ModContent.BuffType<VampiresCurseBuff>();
						for (int num29 = 0; num29 < 255; num29++){
							Player player4 = Main.player[num29];
							if (player4.active && !player4.dead && player4.DistanceSQ(npc.Center) < 1562500f && !player4.HasBuff(156) && !player4.HasBuff(num28)){
								player4.AddBuff(num28, 600, true, false);
								for (int num30 = 0; num30 < 20; num30++){
									int num31 = Dust.NewDust(player4.position, player4.width, player4.height, 5, 0f, 0f, 75, default(Color), 1.25f);
									Main.dust[num31].noGravity = true;
									Main.dust[num31].velocity *= 0.75f;
									int num32 = Main.rand.Next(-50, 51);
									int num33 = Main.rand.Next(-50, 51);
									Dust dust = Main.dust[num31];
									dust.position.X = dust.position.X + (float)num32;
									Dust dust2 = Main.dust[num31];
									dust2.position.Y = dust2.position.Y + (float)num33;
									Main.dust[num31].velocity.X = -(float)num32 * 0.065f;
									Main.dust[num31].velocity.Y = -(float)num33 * 0.065f;
								}
							}
						}
						if (npc.ai[1] != 1f){
							npc.ai[0] = -300f;
						}
						npc.ai[2] = -60f;
						npc.ai[3] = -60f;
					}
				}
				return false;
			}
			else { // No bossrush, revengegance or death detected, use original ai instead
				return true;
			}
		}
	}
}