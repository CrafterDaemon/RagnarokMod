using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;
using CalamityMod;
using CalamityMod.NPCs;
using ThoriumMod;
using ThoriumMod.NPCs;


namespace RagnarokMod.Common.GlobalNPCs
{
    public class BossStatTweak : GlobalNPC
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            foreach (var entry in tweaked_npcs) 
			{
				if ( npc.type == thorium.Find<ModNPC>(entry).Type ) 
				{
					return true;
				}
			}
			return false;
        }
		
		private static List<string> tweaked_npcs = new List<string> {
			"TheGrandThunderBirdv2",
			"Viscount",
			"QueenJelly", 
			"GraniteEnergyStorm",
			"TheBuriedWarrior", 
			"ThePrimeScouter",
			"BoreanStrider", 
			"BoreanStriderPopped",
			"FallenDeathBeholder", 
			"FallenDeathBeholder2", 
			"Lich",
			"LichHeadless",
			"Abyssion",
			"AbyssionCracked",
			"AbyssionReleased",
			"Omnicide",
			"RealityBreaker",
			"SlagFury",
			"Aquaius",
			"ViscountBaby",
			"Hatchling",
			"ZealousJelly",
			"DistractJelly",
			"SpittingJelly",
			"GraniteEnergy",
			"EncroachingEnergy",
			"EnergyStormConduit",
			"PyroCore",
			"CryoCore",
			"BioCore",
			"BoreanMyte1",
			"BoreanHopper",
			"EnemyBeholder",
			"ThousandSoulPhalactry",
			"AbyssalSpawn",
			"AquaiusBubble",
			"LucidBubble"
		};
		
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
			{"Lich", 1.45f},
			{"LichHeadless", 1.45f},
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
			{"PyroCore", 1.35f},
			{"CryoCore", 1.35f},
			{"BioCore", 1.35f},
			{"BoreanMyte1", 1.35f},
			{"BoreanHopper", 1.35f},
			{"EnemyBeholder", 1.35f},
			{"ThousandSoulPhalactry", 1.35f},
			{"AbyssalSpawn", 1.35f},
			{"AquaiusBubble", 1.35f},
			{"LucidBubble", 1.35f},
			{"CorpseBloom", 1.35f},
			{"CorpsePetal", 1.35f},
			{"CorpseWeed", 1.35f},
			{"Illusionist", 1.35f},
			{"IllusionistDecoy", 1.35f},
			{"PatchWerk", 1.35f},
			{"Maggot", 1.35f},
	     	{"BurstingMaggot" ,1.35f}
		};
		
		private static Dictionary<string,float> thorium_bosses_bossrush_health_modifier = new Dictionary<string,float>
		{
			{"TheGrandThunderBirdv2", 135f},
			{"Viscount", 85f},
			{"QueenJelly", 95f},
			{"GraniteEnergyStorm", 65f},
			{"TheBuriedWarrior", 65f},
			{"ThePrimeScouter", 65f},
			{"BoreanStrider", 45f},
			{"BoreanStriderPopped", 45f},
			{"FallenDeathBeholder", 45f},
			{"FallenDeathBeholder2", 45f},
			{"Lich", 27f},
			{"LichHeadless", 27f},
			{"Abyssion", 14f},
			{"AbyssionCracked", 14f},
			{"AbyssionReleased", 14f},
			{"Omnicide", 5f},
			{"RealityBreaker", 5f},
			{"SlagFury", 5f},
			{"Aquaius", 5f},
			{"ViscountBaby", 85f},
			{"Hatchling", 160f},
			{"ZealousJelly", 110f},
			{"DistractJelly", 110f},
			{"SpittingJelly", 110f},
			{"GraniteEnergy", 80f},
			{"EncroachingEnergy", 80f},
			{"EnergyStormConduit", 80f},
			{"PyroCore", 85f},
			{"CryoCore", 85f},
			{"BioCore", 85f},
			{"BoreanMyte1", 60f},
			{"BoreanHopper", 60f},
			{"EnemyBeholder", 60f},
			{"ThousandSoulPhalactry", 35f},
			{"AbyssalSpawn", 20f},
			{"AquaiusBubble", 12f},
			{"LucidBubble", 12f}
		};
		
		private static Dictionary<string,float> thorium_bosses_bossrush_damage_modifier = new Dictionary<string,float>
		{
			{"TheGrandThunderBirdv2", 4f},
			{"Viscount", 4f},
			{"QueenJelly", 4f},
			{"GraniteEnergyStorm", 3.5f},
			{"TheBuriedWarrior", 3.5f},
			{"ThePrimeScouter", 3.5f},
			{"BoreanStrider", 3f},
			{"BoreanStriderPopped", 3f},
			{"FallenDeathBeholder", 3f},
			{"FallenDeathBeholder2", 3f},
			{"Lich", 2f},
			{"LichHeadless", 2f},
			{"Abyssion", 2f},
			{"AbyssionCracked", 2f},
			{"AbyssionReleased", 2f},
			{"Omnicide", 1.5f},
			{"RealityBreaker", 1.5f},
			{"SlagFury", 1.5f},
			{"Aquaius", 1.5f},
			{"ViscountBaby", 4f},
			{"Hatchling", 4f},
			{"ZealousJelly", 4f},
			{"DistractJelly", 4f},
			{"SpittingJelly", 4f},
			{"GraniteEnergy", 3.5f},
			{"EncroachingEnergy", 3.5f},
			{"EnergyStormConduit", 3.5f},
			{"PyroCore", 3.5f},
			{"CryoCore", 3.5f},
			{"BioCore", 3.5f},
			{"BoreanMyte1", 3f},
			{"BoreanHopper", 3f},
			{"EnemyBeholder", 3f},
			{"ThousandSoulPhalactry", 2f},
			{"AbyssalSpawn", 2f},
			{"AquaiusBubble", 1.5f},
			{"LucidBubble", 1.5f}
		};
		
		private static List<string> thorium_defense_damage_npcs = new List<string> {
			"TheGrandThunderBirdv2",
			"Viscount",
			"QueenJelly",
			"GraniteEnergyStorm",
			"TheBuriedWarrior",
			"ThePrimeScouter",
			"BoreanStrider",
			"BoreanStriderPopped",
			"FallenDeathBeholder",
			"FallenDeathBeholder2",
			"Lich", 
			"LichHeadless", 
			"Abyssion",
			"AbyssionCracked",
			"AbyssionReleased",
			"Omnicide",
			"RealityBreaker",
			"SlagFury",
			"Aquaius", 
			"GraniteEnergy",
			"EncroachingEnergy",
			"EnergyStormConduit",
			"TheBuriedWarrior1",
			"TheBuriedWarrior2",
			"ThousandSoulPhalactry",
			"AbyssalSpawn",
			"AquaiusBubble",
			"CorpseBloom",
			"Illusionist",
			"IllusionistDecoy",
			"PatchWerk", 
		};
		
		private static Dictionary<string,float> thorium_npcs_dr = new Dictionary<string,float>
		{
			{"Viscount", 0.05f},
			{"QueenJelly", 0.05f},
			{"GraniteEnergyStorm", 0.05f},
			{"TheBuriedWarrior", 0.05f},
			{"ThePrimeScouter", 0.1f},
			{"BoreanStrider", 0.15f},
			{"BoreanStriderPopped", 0.05f},
			{"FallenDeathBeholder", 0.05f},
			{"FallenDeathBeholder2", 0.1f},
			{"Lich", 0.15f},
			{"LichHeadless", 0.1f},
			{"Abyssion", 0.3f},
			{"AbyssionCracked", 0.2f},
			{"AbyssionReleased", 0.1f},
			{"Omnicide", 0.25f},
			{"RealityBreaker", 0.25f},
			{"SlagFury", 0.2f},
			{"Aquaius", 0.2f},
			{"DistractJelly", 0.1f},
			{"GraniteEnergy", 0.05f},
			{"EncroachingEnergy", 0.05f},
			{"EnergyStormConduit", 0.05f},
			{"PyroCore", 0.1f},
			{"CryoCore", 0.1f},
			{"BioCore", 0.1f},
			{"ThousandSoulPhalactry", 0.1f},
			{"AbyssalSpawn", 0.15f},
			{"AquaiusBubble", 0.1f},
			{"LucidBubble", 0.15f}
		};
		
		public override void ModifyHitPlayer (NPC npc, Player target, ref Player.HurtModifiers modifier) 
		{
			if(!ModContent.GetInstance<BossConfig>().bossstatstweak) 
			{
				return;
			}
			float currentDamageModifier = modifier.IncomingDamageMultiplier.Value;
			float desiredDamageModifier;
			
			if (CalamityGamemodeCheck.isBossrush) 
			{
				if(!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not enabled do not tweak stats
				{
					return;
				}
				foreach (var boss in thorium_bosses_bossrush_damage_modifier) 
				{
					if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type) 
					{
						if(CalamityGamemodeCheck.isDeath) 
						{
							desiredDamageModifier = currentDamageModifier + 0.35f + boss.Value;
						}
						else if (CalamityGamemodeCheck.isRevengeance) 
						{
							desiredDamageModifier = currentDamageModifier + 0.2f + boss.Value;
						} 
						else 
						{
							desiredDamageModifier = currentDamageModifier + boss.Value;
						}
						
						if(OtherModsCompat.tbr_loaded) // Even when TBR bossrush is not loaded the damage numbers are still loaded in Ragnarok boss rush, so we have to remove them.
						{
							desiredDamageModifier-= boss.Value;
						}
						modifier.IncomingDamageMultiplier *= desiredDamageModifier / currentDamageModifier;
						return;
					}
				}
			} 
			else 
			{
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
				foreach (var boss in thorium_bosses_base_health_modifier) 
				{
					if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
					{
						modifier.IncomingDamageMultiplier *= desiredDamageModifier / currentDamageModifier;
						return;
					}
				}
			}
		}
		
        public override void SetDefaults(NPC npc)
		{
			if(!ModContent.GetInstance<BossConfig>().bossstatstweak) 
			{
				return;
			}
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
				if(npc.type == thorium.Find<ModNPC>("TheGrandThunderBirdv2").Type) 
				{
					TotalHPModifier += 0.3;
				}
			}
			else if(CalamityGamemodeCheck.isRevengeance)
			{
				TotalHPModifier *= 1.2;
				TotalDamageModifier = 1.2;
				if(npc.type == thorium.Find<ModNPC>("TheGrandThunderBirdv2").Type) 
				{
						TotalHPModifier += 0.3;
				}
			} 
			
			// Apply health modifiers
			foreach (var boss in thorium_bosses_base_health_modifier) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					npc.life = npc.lifeMax = (int)((npc.lifeMax * TotalHPModifier * boss.Value) * (1 + CalamityHPBoost));
					break;
				}
			}
			
			// Apply bossrush health
			if(CalamityGamemodeCheck.isBossrush && !OtherModsCompat.tbr_loaded) 
			{
				foreach (var boss in thorium_bosses_bossrush_health_modifier) 
				{
					if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
					{
						npc.life = npc.lifeMax = (int)(npc.lifeMax * boss.Value);
						break;
					}
				}
			}

			// Check which npcs should apply defense damage, only apply if ThoriumRework is not loaded or the TBR config option is disabled
			if(!OtherModsCompat.tbr_defense_damage) 
			{
				foreach (var boss in thorium_defense_damage_npcs) 
				{
					if ( npc.type == thorium.Find<ModNPC>(boss).Type ) 
					{
						// Applying Defense Damage
						npc.Calamity().canBreakPlayerDefense = true;
					}
				}
			}
			
			//Check which npcs should be granted damage reduction
			foreach (var boss in thorium_npcs_dr) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					// Applying Damage Reduction
					npc.DR_NERD(boss.Value, null, null, null, null);
				}
			}
		}	
	}
}