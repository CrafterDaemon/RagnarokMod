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
using Terraria.ModLoader.IO;
using ThoriumMod;
using ThoriumMod.NPCs;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.Projectiles.Boss;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class FallenChampion2AI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("FallenChampion2").Type;
		}
		public int counter;
		public int flux;
		public bool shift;
		public bool sideRight;
		public bool strike;
		public bool charge;
		public bool charging;
		public int framecounter = 0;
		
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
		public override void FindFrame(NPC npc, int frameHeight){
			if((CalamityGamemodeCheck.isRevengeance || CalamityGamemodeCheck.isBossrush) && ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok) {
				if (strike){
					framecounter++;
					if (framecounter > 6.0){
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
				if (!strike && !charge){
					framecounter ++;
					if (framecounter > 4){
						counter++;
						if (counter >= 4){
							counter = 0;
						}
						framecounter = 0;
					}
				}
				npc.frame.Y = counter * frameHeight;
				return;
			}
		}
		
		public override bool PreAI(NPC npc) {
			if(!(ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok)) {
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
					npc.friendlyRegen = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				if (!NPC.npcsFoundForCheckActive[ModContent.NPCType<BuriedChampion>()]){
					npc.life = 0;
					npc.active = false;
					return false;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y + 1f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
						return false;
					}
				}
				if (npc.ai[3] == 170f){
					counter = 12;
					strike = true;
				}
				if (npc.ai[3] >= 180f){
					SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
					if (!player.dead && Main.netMode != 1){
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center, new Vector2((float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X), 0f) * 35f, ModContent.ProjectileType<BuriedShock>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f);
					}
					npc.ai[3] = 0f;
				}
				if (npc.ai[2] >= 360f){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					npc.ai[3] = 0f;
					charge = true;
					charging = true;
					for (int i = 0; i < 3; i++){
						int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 113, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
						Dust dust = Main.dust[num];
						dust.velocity *= 0.2f;
						dust.noGravity = true;
					}
				}
				if (npc.ai[2] >= 420f){
					npc.aiStyle = -2;
					sideRight = !sideRight;
					Vector2 vector;
					vector = new Vector2(player.Center.X, player.Center.Y - 350f);
					npc.velocity = npc.DirectionTo(vector) * 35f; // charge faster
					npc.netUpdate = true;
					npc.ai[2] = 0f;
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
					npc2.velocity.Y = npc2.velocity.Y - ((npc.velocity.Y > 0f) ? 1.1f : 0.1f); // increased acceleration
				}
				if (player.position.Y > npc.position.Y + 30f + (float)flux){
					NPC npc3 = npc;
					npc3.velocity.Y = npc3.velocity.Y + ((npc.velocity.Y < 0f) ? 1.1f : 0.1f);
				}
				npc.velocity.Y = MathHelper.Clamp(npc.velocity.Y, -32f, 32f);
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
					npc.velocity.X = MathHelper.Clamp(npc.velocity.X, -32f, 32f);
					return false;
				}
				if (player.Center.X > npc.Center.X - 250f || player.Center.X < npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else if (CalamityGamemodeCheck.isDeath) {
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
					npc.friendlyRegen = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				if (!NPC.npcsFoundForCheckActive[ModContent.NPCType<BuriedChampion>()]){
					npc.life = 0;
					npc.active = false;
					return false;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y + 1f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
						return false;
					}
				}
				if (npc.ai[3] == 170f){
					counter = 12;
					strike = true;
				}
				if (npc.ai[3] >= 180f){
					SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
					if (!player.dead && Main.netMode != 1){
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center, new Vector2((float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X), 0f) * 9f, ModContent.ProjectileType<BuriedShock>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f);
					}
					npc.ai[3] = 0f;
				}
				if (npc.ai[2] >= 360f){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					npc.ai[3] = 0f;
					charge = true;
					charging = true;
					for (int i = 0; i < 3; i++){
						int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 113, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
						Dust dust = Main.dust[num];
						dust.velocity *= 0.2f;
						dust.noGravity = true;
					}
				}
				if (npc.ai[2] >= 420f){
					npc.aiStyle = -2;
					sideRight = !sideRight;
					Vector2 vector;
					vector = new Vector2(player.Center.X, player.Center.Y - 350f);
					npc.velocity = npc.DirectionTo(vector) * 14f;
					npc.netUpdate = true;
					npc.ai[2] = 0f;
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
				if (player.Center.X > npc.Center.X - 250f || player.Center.X < npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isRevengeance) {
				if (npc.aiStyle != -2){
					NPCHelper.BatAI(npc, 0);
					npc.friendlyRegen = 0;
				}
				npc.ai[0] += 1f;
				npc.ai[2] += 1f;
				npc.ai[3] += 1f;
				if (npc.aiStyle == -1){
					npc.ai[1] -= 1f;
				}
				if (!NPC.npcsFoundForCheckActive[ModContent.NPCType<BuriedChampion>()]){
					npc.life = 0;
					npc.active = false;
					return false;
				}
				npc.TargetClosest(true);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					NPC npc1 = npc;
					npc1.velocity.Y = npc1.velocity.Y + 1f;
					if (npc.timeLeft > 10){
						npc.timeLeft = 10;
						return false;
					}
				}
				if (npc.ai[3] == 170f){
					counter = 12;
					strike = true;
				}
				if (npc.ai[3] >= 180f){
					SoundEngine.PlaySound(SoundID.Item1, new Vector2?(npc.Center), null);
					if (!player.dead && Main.netMode != 1){
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center, new Vector2((float)Terraria.Utils.ToDirectionInt(player.Center.X > npc.Center.X), 0f) * 9f, ModContent.ProjectileType<BuriedShock>(), 10, 0f, Main.myPlayer, 0f, 0f, 0f);
					}
					npc.ai[3] = 0f;
				}
				if (npc.ai[2] >= 360f){
					npc.velocity.X = 0f;
					npc.velocity.Y = 0f;
					npc.ai[3] = 0f;
					charge = true;
					charging = true;
					for (int i = 0; i < 3; i++){
						int num = Dust.NewDust(npc.position - npc.velocity, npc.width, npc.height, 113, -((float)npc.spriteDirection * 8f), 0f, 100, default(Color), 0.75f);
						Dust dust = Main.dust[num];
						dust.velocity *= 0.2f;
						dust.noGravity = true;
					}
				}
				if (npc.ai[2] >= 420f){
					npc.aiStyle = -2;
					sideRight = !sideRight;
					Vector2 vector;
					vector = new Vector2(player.Center.X, player.Center.Y - 350f);
					npc.velocity = npc.DirectionTo(vector) * 14f;
					npc.netUpdate = true;
					npc.ai[2] = 0f;
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
				if (player.Center.X > npc.Center.X - 250f || player.Center.X < npc.Center.X + 250f){
					charging = false;
				}
				return false;
			}
			else {
				return true;
			}	
		}
	}
}