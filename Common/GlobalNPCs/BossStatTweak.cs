using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using CalamityMod;
using CalamityMod.NPCs;
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
			{"TheGrandThunderBirdv2", 130f},
			{"Viscount", 85f},
			{"QueenJelly", 95f},
			{"GraniteEnergyStorm", 70f},
			{"TheBuriedWarrior", 70f},
			{"ThePrimeScouter", 70f},
			{"BoreanStrider", 50f},
			{"BoreanStriderPopped", 50f},
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
			{"Hatchling", 130f},
			{"ZealousJelly", 95f},
			{"DistractJelly", 95f},
			{"SpittingJelly", 95f},
			{"GraniteEnergy", 70f},
			{"EncroachingEnergy", 70f},
			{"EnergyStormConduit", 70f},
			{"PyroCore", 70f},
			{"CryoCore", 70f},
			{"BioCore", 70f},
			{"BoreanMyte1", 55f},
			{"BoreanHopper", 55f},
			{"EnemyBeholder", 50f},
			{"ThousandSoulPhalactry", 30f},
			{"AbyssalSpawn", 15f},
			{"AquaiusBubble", 10f},
			{"LucidBubble", 10f}
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
		
		public override void ModifyHitPlayer (NPC npc, Player target, ref Player.HurtModifiers modifier) 
		{
			ModLoader.TryGetMod("ThoriumRework", out Mod ThoriumRework);
			if(ThoriumRework != null) 
			{
				return;
			}
			
			float currentDamageModifier = modifier.IncomingDamageMultiplier.Value;
			float desiredDamageModifier;
			
			if (CalamityGamemodeCheck.isBossrush) 
			{
				ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
				foreach (var boss in thorium_bosses_bossrush_damage_modifier) 
				{
					if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
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
		}
		
        public override void SetDefaults(NPC npc)
		{
			ModLoader.TryGetMod("ThoriumRework", out Mod ThoriumRework);
			if(ThoriumRework != null) 
			{
				return;
			}
			
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
			if(CalamityGamemodeCheck.isBossrush) 
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
			
			
			// Check which npcs should apply defense damage
			foreach (var boss in thorium_defense_damage_npcs) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss).Type ) 
				{
					// Applying Defense Damage
					npc.Calamity().canBreakPlayerDefense = true;
				}
			}
		}	
		
		// Apply Multiplayer scalar
		public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
		{
			ModLoader.TryGetMod("ThoriumRework", out Mod ThoriumRework);
			if(ThoriumRework == null) 
			{
				return;
			}
			
			if (Main.netMode == 0 || numPlayers <= 1)
			{
				return;
			}
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
			foreach (var boss in thorium_bosses_base_health_modifier) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					double scalar;
					switch (numPlayers)
					{
						case 1:
						scalar = 1.0;
						break;
						case 2:
						scalar = 0.9;
						break;
						case 3:
						scalar = 0.82;
						break;
						case 4:
						scalar = 0.76;
						break;
						case 5:
						scalar = 0.71;
						break;
						case 6:
						scalar = 0.67;
						break;
						default:
						scalar = 0.64;
						break;
					}
					npc.lifeMax = (int)Math.Round((double)npc.lifeMax * scalar);
					return;
				}
			}	
		}
	}
}