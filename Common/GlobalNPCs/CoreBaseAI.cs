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
using Terraria.Graphics.Shaders;
using ThoriumMod;
using ThoriumMod.NPCs;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
namespace RagnarokMod.Common.GlobalNPCs
{
    public class BioCoreAI : GlobalNPC
    {
		public override bool InstancePerEntity => true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation) {
			return (npc.type == thorium.Find<ModNPC>("CryoCore").Type 
			|| npc.type == thorium.Find<ModNPC>("PyroCore").Type 
			|| npc.type == thorium.Find<ModNPC>("BioCore").Type 
			);
		}
	
		public float speed = 18f;
		public float inertia = 10f;
		public sbyte flux;
		public bool shift;
		
		public override void OnSpawn (NPC npc, IEntitySource source) {
			if(npc.type == thorium.Find<ModNPC>("CryoCore").Type) {
				inertia = 25f;
			}
			else if(npc.type == thorium.Find<ModNPC>("PyroCore").Type) {
				inertia = 15f;
			}
			else {
				inertia = 10f;
			}
		}
		
		public override bool PreAI(NPC npc) {
			if(CalamityGamemodeCheck.isBossrush) {	
				if(OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
				{
					return true;
				}
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
				{
					return true;
				}
				if(!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().scouter))) {
					return true;
				}
			
				if(npc.velocity.X != 0f && npc.velocity.Y != 0f){
					Player player = Main.player[npc.target];
					flux += (sbyte)(Terraria.Utils.ToDirectionInt(shift) * ((int)Math.Abs(npc.ai[0]) % 4));
					if (flux > 75 && shift){
						shift = false;
					}
					else if (flux <= -75 && !shift){
						shift = true;
					}
					if (npc.direction == 0){
						npc.direction = -1;
					}
					float num6 = speed;
					float num7 = 120f;
					npc.BasicMoveLeftRight(player, new Vector2(num7, (float)(-100 + flux)), num6, inertia, true);
				}
				return true;
			}
			else {
					return true;
			}
		}
	}
}