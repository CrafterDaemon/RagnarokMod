using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.NPCs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class BossStatTweak : GlobalNPC
    {
		private static Dictionary<string,float> thorium_bosses_base_health_modifier = new Dictionary<string,float>
		{
			{"TheGrandThunderBirdv2", 1.1f},
			{"Viscount", 1.25f},
			{"QueenJelly", 1.25f},
			{"GraniteEnergyStorm", 1.35f},
			{"TheBuriedWarrior", 1.35f},
			{"ThePrimeScouter", 1.35f},
			{"BoreanStrider", 1.35f},
			{"BoreanStriderPopped", 1.35f},
			{"FallenDeathBeholder", 1.35f},
			{"FallenDeathBeholder2", 1.35f},
			{"Lich", 1.4f},
			{"LichHeadless", 1.4f},
			{"Abyssion", 1.35f},
			{"AbyssionCracked", 1.35f},
			{"AbyssionReleased", 1.35f},
			{"Omnicide", 1.1f},
			{"RealityBreaker", 1.1f},
			{"SlagFury", 1.1f},
			{"Aquaius", 1.1f},
			{"ViscountBaby", 1.25f},
			{"Hatchling", 1.1f},
			{"ZealousJelly", 1.25f},
			{"DistractJelly", 1.25f},
			{"SpittingJelly", 1.25f},
			{"GraniteEnergy", 1.35f},
			{"EncroachingEnergy", 1.35f},
			{"EnergyStormConduit", 1.35f},
			{"TheBuriedWarrior1", 1f},
			{"TheBuriedWarrior2", 1f},
			{"PyroCore", 1.35f},
			{"CryoCore", 1.35f},
			{"BioCore", 1.35f},
			{"BoreanMyte1", 1.35f},
			{"BoreanHopper", 1.35f},
			{"EnemyBeholder", 1.35f},
			{"ThousandSoulPhalactry", 1.35f},
			{"AbyssalSpawn", 1.35f},
			{"AquaiusBubble", 1.35f},
			{"CorpseBloom", 1.35f},
			{"CorpsePetal", 1.35f},
			{"CorpseWeed", 1.35f},
			{"Illusionist", 1.35f},
			{"IllusionistDecoy", 1.35f},
			{"PatchWerk", 1.35f},
			{"Maggot", 1.35f},
	     	{"BurstingMaggot" ,1.35f}
		};
		
		public override void ModifyHitPlayer (NPC npc, Player target, ref Player.HurtModifiers modifier) 
		{
			float currentDamageModifier = modifier.IncomingDamageMultiplier.Value;
			float desiredDamageModifier;
			if(CalamityGamemodeCheck.isDeath) 
			{
				desiredDamageModifier = currentDamageModifier + 0.35f;
			}
			else if (CalamityGamemodeCheck.isRevengeance) 
			{
				desiredDamageModifier = currentDamageModifier + 0.2f;
			}
			else 
			{
				return;
			}	
			
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
			foreach (var boss in thorium_bosses_base_health_modifier) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					modifier.IncomingDamageMultiplier *= desiredDamageModifier / currentDamageModifier;
					return;
				}
			}
		}
		
        public override void SetDefaults(NPC npc)
		{
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
			// Get Calamity-Config Health Boost
			double CalamityHPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
			 
			// Total HP Modifier without the CalamityHPBoost;
			double TotalHPModifier = 1;
			double TotalDamageModifier = 1;
		
			// DeathMode has to be checked first
			if (CalamityGamemodeCheck.isDeath) 
			{
				TotalHPModifier *= 1.2;
				TotalDamageModifier = 1.35;
			}
			else if(CalamityGamemodeCheck.isRevengeance)
			{
				TotalHPModifier *= 1.2;
				TotalDamageModifier = 1.2;
			} 
			
			foreach (var boss in thorium_bosses_base_health_modifier) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					npc.life = npc.lifeMax = (int)((npc.lifeMax * TotalHPModifier * boss.Value) * (1 + CalamityHPBoost));
					return;
				}
			}
		}	
	}
}