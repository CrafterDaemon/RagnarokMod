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
    public class BoreanStriderAI : GlobalNPC{
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return (npc.type == thorium.Find<ModNPC>("BoreanStrider").Type || npc.type == thorium.Find<ModNPC>("BoreanStriderPopped").Type);
		}
		
		private short fangtimer = 0;
		private bool jump = false;
		private short jumptimer;
		private float lockedDirection = 0f;
		
		private void shootAdditionalBlizzardFang(NPC npc, Player target, float velocity, short timermax) {
			fangtimer++;
			if(fangtimer >= timermax) {
				if(fangtimer >= 8000) { fangtimer = timermax;}
				if(Collision.CanHit(npc.position, npc.width, npc.height, target.position, target.width, target.height)) {
					if (Main.netMode != NetmodeID.MultiplayerClient){
						Vector2 vector = npc.DirectionTo(target.Center);
						Projectile.NewProjectile(npc.GetSource_FromAI(null), npc.Center, vector * velocity, ModContent.ProjectileType<BlizzardFang>(), 25, 0f, Main.myPlayer, 0f, 0f, 0f);
					}
					fangtimer = 0;
				}
			}
		}
		
		public override void PostAI(NPC npc) {
			if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().strider))) {
					return;
			}
			if(CalamityGamemodeCheck.isBossrush){	
				if(OtherModsCompat.tbr_loaded){
					return;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)){
					return;
				}	
				
				/*
				if(npc.velocity.Y < -1f) {
					jump = true;
				}
				if(jump) {
					jumptimer++;
					if(jumptimer < 60) {
						npc.velocity.Y = -12f;
						if(npc.velocity.X > 1f) {
							npc.velocity.X = 15f;
						} else if(npc.velocity.X < -1f){
							npc.velocity.X = -15f;
						}
					}
					else if(jumptimer < 220){
						npc.velocity.Y += 0.05f;
						npc.velocity.X *= 0.95f;
					}
					else if(jumptimer < 300){
						npc.velocity.Y = 20f;
					}
					else{
						jump = false;
						jumptimer=0;
					}
					Main.NewText(npc.velocity.Y);
				}
				*/
				
				bool isOnGround = npc.velocity.Y == 0f && npc.collideY;
				if (isOnGround && !jump){
					jump = true;
					jumptimer = 0;
					npc.velocity.Y = -Main.rand.NextFloat(10f, 18f);
					lockedDirection = Main.rand.NextBool() ? Main.rand.NextFloat(10f, 20f) : -Main.rand.NextFloat(10f, 20f);
					npc.velocity.X = lockedDirection;
				}
		
				if (jump){
					jumptimer++;
					if (jumptimer < 45){
						npc.velocity.Y -= 0.3f;
						npc.velocity.X = lockedDirection;
					}
					else if (jumptimer < 200){
						npc.velocity.Y += Main.rand.NextFloat(-0.2f, 0.4f);
						npc.velocity.X = lockedDirection;
					}
					else{
						jump = false;
						jumptimer = 0;
					}
				}
				
				Player player = Main.player[npc.target];
				if((float)npc.life < (float)npc.lifeMax * 0.75f){
					shootAdditionalBlizzardFang(npc, player, 10f, 450);
				}else{
					shootAdditionalBlizzardFang(npc, player, 10f, 600);
				}
			}
			else if (CalamityGamemodeCheck.isDeath){
				Player player = Main.player[npc.target];
				if((float)npc.life < (float)npc.lifeMax * 0.75f){
					shootAdditionalBlizzardFang(npc, player, 10f, 1000);
				}else{
					shootAdditionalBlizzardFang(npc, player, 10f, 1250);
				}
			}				
			else if(CalamityGamemodeCheck.isRevengeance) {
				Player player = Main.player[npc.target];
				if((float)npc.life < (float)npc.lifeMax * 0.75f){
					shootAdditionalBlizzardFang(npc, player, 10f, 1200);
				}else{
					shootAdditionalBlizzardFang(npc, player, 10f, 1500);
				}
			}
		}
	}
}