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
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class EncroachingEnergyAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("EncroachingEnergy").Type;
		}
		
		public override bool PreAI(NPC npc) {
			if(!(ModContent.GetInstance<BossConfig>().granite == ThoriumBossRework_selection_mode.Ragnarok)) {
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
				int dust_type = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity * 0.5f, npc.width, npc.height, 15, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust_type].velocity *= 0.5f;
				Main.dust[dust_type].noGravity = true;
				float npcX = npc.position.X;
				float npcY = npc.position.Y;
				float maxdistance = 100000f;
				bool flag = false;
				npc.ai[0] += 1f;
				if (npc.ai[0] > 60f){
					npc.chaseable = true;
					npc.dontTakeDamage = false;
					npc.spriteDirection = ((npc.velocity.X > 0f) ? 1 : -1);
					npc.rotation += ((npc.spriteDirection > 0) ? 0.15f : -0.15f);
					for (int i = 0; i < Main.maxNPCs; i++){
						float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
						float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
						float distancetoplayer = Math.Abs(npc.position.X + (float)(npc.width / 2) - playerX) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - playerY);
						if (distancetoplayer < 800f && distancetoplayer < maxdistance){
							maxdistance = distancetoplayer;
							npcX = playerX;
							npcY = playerY;
							flag = true;
						}
					}
					npc.ai[0] = 60f;
				}
				else{
					npc.chaseable = false;
					npc.dontTakeDamage = true;
				}
				if (!flag){
					npcX = npc.position.X + (float)(npc.width / 2) + npc.velocity.X * 100f;
					npcY = npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y * 100f;
				}
				float maximumspeed = 20f;
				float acceleration = 0.55f;
				Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float distanceX = npcX - vector.X;
				float distanceY = npcY - vector.Y;
				float distance = (float)Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
				if (distance > 0f){
					distance = maximumspeed / distance;
				}
				distanceX *= distance;
				distanceY *= distance;
				if (npc.velocity.X < distanceX){
					npc.velocity.X = npc.velocity.X + acceleration;
					if (npc.velocity.X < 0f && distanceX > 0f){
						npc.velocity.X = npc.velocity.X + acceleration * 2f;
					}
				}
				else if (npc.velocity.X > distanceX){
					npc.velocity.X = npc.velocity.X - acceleration;
					if (npc.velocity.X > 0f && distanceX < 0f){
						npc.velocity.X = npc.velocity.X - acceleration * 2f;
					}
				}
				if (npc.velocity.Y < distanceY){
					npc.velocity.Y = npc.velocity.Y + acceleration;
					if (npc.velocity.Y < 0f && distanceY > 0f){
						npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
						return false;
					}
				}
				else if (npc.velocity.Y > distanceY){
					npc.velocity.Y = npc.velocity.Y - acceleration;
					if (npc.velocity.Y > 0f && distanceY < 0f){
						npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
					}
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isDeath) {
				int dust_type = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity * 0.5f, npc.width, npc.height, 15, 0f, 0f, 100, default(Color), 1f);
				Main.dust[dust_type].velocity *= 0.5f;
				Main.dust[dust_type].noGravity = true;
				float npcX = npc.position.X;
				float npcY = npc.position.Y;
				float maxdistance = 100000f;
				bool flag = false;
				npc.ai[0] += 1f;
				if (npc.ai[0] > 60f){
					npc.chaseable = true;
					npc.dontTakeDamage = false;
					npc.spriteDirection = ((npc.velocity.X > 0f) ? 1 : -1);
					npc.rotation += ((npc.spriteDirection > 0) ? 0.15f : -0.15f);
					for (int i = 0; i < Main.maxNPCs; i++){
						float playerX = Main.player[i].position.X + (float)(Main.player[i].width / 2);
						float playerY = Main.player[i].position.Y + (float)(Main.player[i].height / 2);
						float distancetoplayer = Math.Abs(npc.position.X + (float)(npc.width / 2) - playerX) + Math.Abs(npc.position.Y + (float)(npc.height / 2) - playerY);
						if (distancetoplayer < 800f && distancetoplayer < maxdistance){
							maxdistance = distancetoplayer;
							npcX = playerX;
							npcY = playerY;
							flag = true;
						}
					}
					npc.ai[0] = 60f;
				}
				else{
					npc.chaseable = false;
					npc.dontTakeDamage = true;
				}
				if (!flag){
					npcX = npc.position.X + (float)(npc.width / 2) + npc.velocity.X * 100f;
					npcY = npc.position.Y + (float)(npc.height / 2) + npc.velocity.Y * 100f;
				}
				float maximumspeed = 6f; // Increased maxspeed
				float acceleration = 0.25f; // Increased acceleration
				Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float distanceX = npcX - vector.X;
				float distanceY = npcY - vector.Y;
				float distance = (float)Math.Sqrt((double)(distanceX * distanceX + distanceY * distanceY));
				if (distance > 0f){
					distance = maximumspeed / distance;
				}
				distanceX *= distance;
				distanceY *= distance;
				if (npc.velocity.X < distanceX){
					npc.velocity.X = npc.velocity.X + acceleration;
					if (npc.velocity.X < 0f && distanceX > 0f){
						npc.velocity.X = npc.velocity.X + acceleration * 2f;
					}
				}
				else if (npc.velocity.X > distanceX){
					npc.velocity.X = npc.velocity.X - acceleration;
					if (npc.velocity.X > 0f && distanceX < 0f){
						npc.velocity.X = npc.velocity.X - acceleration * 2f;
					}
				}
				if (npc.velocity.Y < distanceY){
					npc.velocity.Y = npc.velocity.Y + acceleration;
					if (npc.velocity.Y < 0f && distanceY > 0f){
						npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
						return false;
					}
				}
				else if (npc.velocity.Y > distanceY){
					npc.velocity.Y = npc.velocity.Y - acceleration;
					if (npc.velocity.Y > 0f && distanceY < 0f){
						npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
					}
				}
				return false;
			}
			else {
					return true;
			}
		}
	}
}