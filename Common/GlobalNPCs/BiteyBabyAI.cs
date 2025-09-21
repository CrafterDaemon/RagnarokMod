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
    public class BiteyBabyAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return npc.type == thorium.Find<ModNPC>("BiteyBaby").Type;
		}
		public int counter;
	
		public override void FindFrame(NPC npc, int frameHeight){
			if(CalamityGamemodeCheck.isBossrush && !OtherModsCompat.tbr_loaded && ModContent.GetInstance<BossConfig>().viscount == ThoriumBossRework_selection_mode.Ragnarok && ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok ) {	
				npc.frameCounter += 1.0;
				if (npc.frameCounter > 5.0){
					counter++;
					npc.frameCounter = 0.0;
				}
				if (counter >= 4){
					counter = 0;
				}
				npc.frame.Y = counter * frameHeight;
				return;
			}
		}
	
		public override bool PreAI(NPC npc) {
			if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().viscount))) {
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
				NPCHelper.BatAI(npc, 0, 3.5f);
				Player player = Main.player[npc.target];
				if (!player.active || player.dead){
					npc.active = false;
					return false;
				}
				if (npc.wet){
					NPC npc2 = npc;
					npc2.velocity.Y = npc2.velocity.Y - 0.5f;
				}
				if (player.Center.X > npc.Center.X){
					npc.spriteDirection = 1;
					return false;
				}
				npc.spriteDirection = -1;
				return false;
			}
			else {
					return true;
			}
		}
	}
}