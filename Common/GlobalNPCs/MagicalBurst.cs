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
    public class MagicalBurstAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("MagicalBurst").Type;
		}
		public int timer = 0;
	
		public override bool PreAI(NPC npc) {
			if(!(ModContent.GetInstance<BossConfig>().champion == ThoriumBossRework_selection_mode.Ragnarok)) {
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
				
				timer++;
				if (timer >= 180){
					npc.life = 1;
					npc.AddBuff(20, 60, false);
					for (int i = 0; i < 10; i++){
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 113, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3f, 3f), 0, default(Color), 1f);
						Main.dust[num].noGravity = true;
					}
				}
				int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 113, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust = Main.dust[num2];
				Main.dust[num2].velocity *= 0f;
				Main.dust[num2].noGravity = true;
				int num3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 173, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust2 = Main.dust[num3];
				Main.dust[num3].velocity *= 0f;
				Main.dust[num3].noGravity = true;
				npc.rotation = Terraria.Utils.ToRotation(npc.velocity) - 1.5707964f;
				Vector2 vector;
				vector = new Vector2(npc.ai[0], npc.ai[1]);
				if (timer < 2){
					Vector2 vector2 = vector - npc.Center;
					float speed = 25f;  // changed from 8 to 25
					float num5 = vector2.Length();
					if (num5 > speed){
						num5 = speed / num5;
						vector2 *= num5;
					}
					npc.velocity = vector2;
				}
				return false;
			}
			else if (CalamityGamemodeCheck.isDeath) {
				timer++;
				if (timer >= 90){
					npc.life = 1;
					npc.AddBuff(20, 60, false);
					for (int i = 0; i < 10; i++){
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 113, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3f, 3f), 0, default(Color), 1f);
						Main.dust[num].noGravity = true;
					}
				}
				int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 113, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust = Main.dust[num2];
				Main.dust[num2].velocity *= 0f;
				Main.dust[num2].noGravity = true;
				int num3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 173, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust2 = Main.dust[num3];
				Main.dust[num3].velocity *= 0f;
				Main.dust[num3].noGravity = true;
				npc.rotation = Terraria.Utils.ToRotation(npc.velocity) - 1.5707964f;
				Vector2 vector;
				vector = new Vector2(npc.ai[0], npc.ai[1]);
				if (timer < 2){
					Vector2 vector2 = vector - npc.Center;
					float speed = 9f;  // changed from 8 to 9
					float num5 = vector2.Length();
					if (num5 > speed){
						num5 = speed / num5;
						vector2 *= num5;
					}
					npc.velocity = vector2;
				}
				return false;
			}
			else if(CalamityGamemodeCheck.isRevengeance) {
				timer++;
				if (timer >= 90){
					npc.life = 1;
					npc.AddBuff(20, 60, false);
					for (int i = 0; i < 10; i++){
						int num = Dust.NewDust(npc.position, npc.width, npc.height, 113, Terraria.Utils.NextFloat(Main.rand, -3f, 3f), Terraria.Utils.NextFloat(Main.rand, -3f, 3f), 0, default(Color), 1f);
						Main.dust[num].noGravity = true;
					}
				}
				int num2 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 113, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust = Main.dust[num2];
				Main.dust[num2].velocity *= 0f;
				Main.dust[num2].noGravity = true;
				int num3 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - npc.velocity, npc.width, npc.height, 173, 0f, 0f, 150, default(Color), 1.25f);
				Dust dust2 = Main.dust[num3];
				Main.dust[num3].velocity *= 0f;
				Main.dust[num3].noGravity = true;
				npc.rotation = Terraria.Utils.ToRotation(npc.velocity) - 1.5707964f;
				Vector2 vector;
				vector = new Vector2(npc.ai[0], npc.ai[1]);
				if (timer < 2){
					Vector2 vector2 = vector - npc.Center;
					float speed = 9f;  // changed from 8 to 9
					float num5 = vector2.Length();
					if (num5 > speed){
						num5 = speed / num5;
						vector2 *= num5;
					}
					npc.velocity = vector2;
				}
				return false;
			}
			else {
				return true;
			}	
		}
	}
}