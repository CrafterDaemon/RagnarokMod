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
		
		private static Dictionary<string,bool> thorium_npc_debuff_cold_vulnerability = new Dictionary<string,bool>
		{
			// If value is 1, the npc is vulnerable, 0 is resistance and no entry is neutral
			// Bosses and minions
			{"TheGrandThunderBirdv2", true},
			{"QueenJelly", true},
			{"ZealousJelly", true},
			{"DistractJelly", true},
			{"SpittingJelly", true},
			{"TheBuriedWarrior", true},
			{"GraniteEnergyStorm", true},
			{"ThePrimeScouter", false},
			{"BoreanStrider", false},
			{"BoreanStriderPopped", false},
			{"FallenDeathBeholder", false},
			{"FallenDeathBeholder2", false},
			{"Abyssion", false},
			{"AbyssionCracked", false},
			{"Hatchling", true},
			{"GraniteEnergy", true},
			{"EncroachingEnergy", true},
			{"EnergyStormConduit", true},
			{"CryoCore", false},
			{"BoreanMyte1", false},
			{"BoreanHopper", false},
			{"EnemyBeholder", false},
			{"CorpseBloom", true},
			{"CorpsePetal", true},
			{"CorpseWeed", true},
			// All other NPCs
			{"AncientArcher", true},
			{"AncientCharger", true},
			{"AncientPhalanx", true},
			{"ArmyAnt", true},
			{"Biter", true},
			{"BizarreRockFormation", true},
			{"Blowfish", true},
			{"LivingHemorrhage", true},
			{"Clot", true},
			{"Coolmera", false},
			{"EarthenBat", true},
			{"EarthenGolem", true},
			{"FrostWurmHead", false},
			{"FrostWurmBody1", false},
			{"FrostWurmBody2", false},
			{"FrostWurmTail", false},
			{"FrozenFace", false},
			{"GildedBat", true},
			{"GraniteEradicator", true},
			{"GraniteFusedSlime", true},
			{"GraniteSurger", true},
			{"CyanHag", false},
			{"LifeCrystalMimic", true},
			{"BloomMahoganyEnt", true},
			{"MahoganyEnt", true},
			{"Nestling", false},
			{"SnowBall",  false},
			{"SnowEater", false},
			{"SpaceSlime", true},
			{"StrangeBulb", true},
			{"TheInnocent", true},
			{"UFO", false},
			{"UnstableEnergyAnomaly", true},
			{"BlackWidow", true},
			{"BrownRecluse", true},
			{"BrownRecluseBaby", true},
			{"Blister", true},
			{"BlisterPod", true},
			{"BoneFlayer", true},
			{"ChilledSpitter", false},
			{"Coldling", false},
			{"DissonanceSeer", true},
			{"Freezer", false},
			{"FrostFang", false},
			{"FrozenGross", false},
			{"GlitteringGolem", true},
			{"HorrificCharger", true},
			{"Lycan", true},
			{"MartianScout", false},
			{"MoltenMortar", true},
			{"MossWasp", true},
			{"SnowFlinxMatriarch", false},
			{"SoulCorrupter", true},
			{"Spectrumite", true},
			{"Tarantula", true},
			{"TheStarved", true},
			{"UnderworldPot1", true},
			{"UnderworldPot2", true},
			{"VileFloater", true},
			{"Abomination", true},
			{"BloodMage", true},
			{"BloodyWarg", true},
			{"EngorgedEye", true},
			{"GraveLimb", true},
			{"SeveredLegs", true},
			{"SmotheringShade", true},
			{"BlizzardBat", false},
			{"FrostBurnt", false},
			{"SnowElemental", false},
			{"SnowyOwl", false},
			{"GoblinSpiritGuide", true},
			{"SeaShantySinger", true},
			{"LeFantome", true},
			{"ScissorStalker", true},
			{"SnowSinga", false}
		};
		
		private static Dictionary<string,bool> thorium_npc_debuff_water_vulnerability = new Dictionary<string,bool>
		{
			// If value is 1, the npc is vulnerable, 0 is resistance and no entry is neutral
			// Bosses and minions
			{"QueenJelly", false},
			{"GraniteEnergyStorm", true},
			{"ThePrimeScouter", true},
			{"BoreanStrider", false},
			{"BoreanStriderPopped", false},
			{"FallenDeathBeholder", true},
			{"FallenDeathBeholder2", true},
			{"Abyssion", false},
			{"AbyssionCracked", false},
			{"AbyssionReleased", false},
			{"RealityBreaker", false},
			{"SlagFury", true},
			{"Aquaius", false},
			{"ZealousJelly", false},
			{"DistractJelly", false},
			{"SpittingJelly", false},
			{"EncroachingEnergy", true},
			{"PyroCore", true},
			{"BoreanMyte1", false},
			{"BoreanHopper", false},
			{"EnemyBeholder", true},
			{"AbyssalSpawn", false},
			{"AquaiusBubble", false},
			{"LucidBubble", false},
			{"CorpseBloom", false},
			{"CorpsePetal", false},
			{"CorpseWeed", false},
			// All other NPCs
			{"FireAnt", true},
			{"Barracuda", false},
			{"BatOutaHell", true},
			{"FlamekinCaster", true},
			{"Blowfish", false},
			{"GigaClam", false},
			{"BigBone", true},
			{"Globee", false},
			{"GraniteEradicator", true},
			{"GraniteSurger", true},
			{"BlueHag", false},
			{"RedHag", true},
			{"Hammerhead", false},
			{"LifeCrystalMimic", true},
			{"ManofWar", false},
			{"BloomMahoganyEnt", false},
			{"MahoganyEnt", false},
			{"Octopus", false},
			{"Shambler", true},
			{"Sharptooth", false},
			{"SpaceSlime", true},
			{"StrangeBulb", false},
			{"TheInnocent", false},
			{"UFO", true},
			{"UnstableEnergyAnomaly", true},
			{"AbyssalAngler", false},
			{"AstroBeetle", false},
			{"AquaticHallucination", false},
			{"Blobfish", false},
			{"BoneFlayer", true},
			{"CrownofThorns", false},
			{"EpiDermon", true},
			{"FeedingFrenzy", false},
			{"GlitteringGolem", true},
			{"HellBringerMimic", true},
			{"InfernalHound", true},
			{"Kraken", false},
			{"MoltenMortar", true},
			{"MyceliumMimic", false},
			{"NecroPot", true},
			{"PutridSerpent", false},
			{"SubmergedMimic", false},
			{"Whale", false},
			{"UnderworldPot1", true},
			{"UnderworldPot2", true},
			{"VampireSquid", false},
			{"VoltEelHead", false},
			{"VoltEelBody1", false},
			{"VoltEelBody2", false},
			{"VoltEelTail", false},
			{"BloodDrop", false},
			{"ShadowflameRevenant", true}
		};
		
		private static Dictionary<string,bool> thorium_npc_debuff_sickness_vulnerability = new Dictionary<string,bool>
		{
			// If value is 1, the npc is vulnerable, 0 is resistance and no entry is neutral
			// Bosses and minions
			{"TheGrandThunderBirdv2", true},
			{"Viscount", false},
			{"QueenJelly", false},
			{"GraniteEnergyStorm", false},
			{"TheBuriedWarrior", false},
			{"ThePrimeScouter", false},
			{"BoreanStrider", true},
			{"BoreanStriderPopped", true},
			{"FallenDeathBeholder", false},
			{"FallenDeathBeholder2", false},
			{"Lich", false},
			{"LichHeadless", false},
			{"Abyssion", true},
			{"AbyssionCracked", true},
			{"AbyssionReleased", true},
			{"Omnicide", false},
			{"RealityBreaker", false},
			{"ViscountBaby", false},
			{"Hatchling", true},
			{"ZealousJelly", false},
			{"DistractJelly", false},
			{"SpittingJelly", false},
			{"GraniteEnergy", false},
			{"EncroachingEnergy", false},
			{"EnergyStormConduit", false},
			{"PyroCore", false},
			{"CryoCore", false},
			{"BioCore", false},
			{"BoreanMyte1", true},
			{"BoreanHopper", true},
			{"ThousandSoulPhalactry", false},
			{"AbyssalSpawn", true},
			{"CorpseBloom", true},
			{"CorpsePetal", true},
			{"CorpseWeed", true},
			{"Illusionist", false},
			{"IllusionistDecoy", false},
			{"PatchWerk", false},
			{"Maggot", false},
	     	{"BurstingMaggot", false},
			// All other NPCs
			{"AncientArcher", false},
			{"AncientCharger", false},
			{"AncientPhalanx", false},
			{"ArmyAnt", false},
			{"FireAnt", false},
			{"BabySpider", false},
			{"BigBone", false},
			{"Biter", false},
			{"BizarreRockFormation", false},
			{"Blowfish", false},
			{"LivingHemorrhage", true},
			{"Clot", true},
			{"Coolmera", true},
			{"DarksteelKnight", false},
			{"EarthenBat", false},
			{"EarthenGolem", false},
			{"FrostWurmHead", true},
			{"FrostWurmBody1", true},
			{"FrostWurmBody2", true},
			{"FrostWurmTail", true},
			{"FrozenFace", true},
			{"GelatinousCube", true},
			{"GigaClam", true},
			{"GildedLycan", true},
			{"GraniteEradicator", false},
			{"GraniteFusedSlime", false},
			{"GraniteSurger", false},
			{"GreenHag", false},
			{"CyanHag", true},
			{"Hammerhead", true},
			{"HoppingSpider", false},
			{"LifeCrystalMimic", false},
			{"ManofWar", false},
			{"BloomMahoganyEnt", false},
			{"MahoganyEnt", false},
			{"MorayHead", false},
			{"MorayBody", false},
			{"MorayTail", false},
			{"MudMan", false},
			{"Nestling", true},
			{"Octopus", true},
			{"RagingMinotaur", true},
			{"Shambler", false},
			{"GelatinousSludge", true},
			{"SnowBall",  false},
			{"SnowEater", true},
			{"SpaceSlime", false},
			{"StrangeBulb", false},
			{"TheInnocent", true},
			{"UFO", false},
			{"UnstableEnergyAnomaly", false},
			{"WindElemental", false},
			{"AbyssalAngler", true},
			{"AstroBeetle", false},
			{"BlackWidow", false},
			{"BrownRecluse", false},
			{"BrownRecluseBaby", false},
			{"Blister", true},
			{"BlisterPod", true},
			{"BoneFlayer", false},
			{"ChilledSpitter", true},
			{"Coldling", true},
			{"CrownofThorns", true},
			{"DissonanceSeer", false},
			{"EpiDermon", true},
			{"FeedingFrenzy", true},
			{"Freezer", true},
			{"FrostFang", true},
			{"FrozenGross", true},
			{"GlitteringGolem", false},
			{"HorrificCharger", true},
			{"HellBringerMimic", false},
			{"InfernalHound", true},
			{"Lycan", true},
			{"LihzardMimic", false},
			{"LihzardPotMimic1", false},
			{"LihzardPotMimic2", false},
			{"MartianScout", true},
			{"MartianSentry", false},
			{"MossWasp", false},
			{"MyceliumMimic", false},
			{"NecroPot", false},
			{"PutridSerpent", true},
			{"SnowFlinxMatriarch", true},
			{"SoulCorrupter", true},
			{"Spectrumite", false},
			{"SubmergedMimic", false},
			{"SunPriestess", true},
			{"Tarantula", true},
			{"TheStarved", true},
			{"Whale", true},
			{"UnderworldPot1", false},
			{"UnderworldPot2", false},
			{"VampireSquid", true},
			{"VileFloater", true},
			{"VoltEelHead", true},
			{"VoltEelBody1", true},
			{"VoltEelBody2", true},
			{"VoltEelTail", true},
			{"BloodDrop", true},
			{"BloodMage", true},
			{"BloodyWarg", true},
			{"EngorgedEye", true},
			{"SeveredLegs", true},
			{"SmotheringShade", false},
			{"SnowElemental", false},
			{"GoblinDrummer", true},
			{"GoblinTrapper", true},
			{"GoblinSpiritGuide", true},
			{"ShadowflameRevenant", false},
			{"SeaShantySinger", true},
			{"LeFantome", true},
			{"ScissorStalker", true},
			{"SnowSinga", false}
		};
		
		private static Dictionary<string,bool> thorium_npc_debuff_electricity_vulnerability = new Dictionary<string,bool>
		{
			// If value is 1, the npc is vulnerable, 0 is resistance and no entry is neutral
			// Bosses and minions
			{"TheGrandThunderBirdv2", false},
			{"Viscount", true},
			{"QueenJelly", false},
			{"GraniteEnergyStorm", false},
			{"TheBuriedWarrior", true},
			{"ThePrimeScouter", true},
			{"BoreanStriderPopped", true},
			{"FallenDeathBeholder", true},
			{"FallenDeathBeholder2", true},
			{"Lich", true},
			{"LichHeadless", true},
			{"Abyssion", true},
			{"AbyssionCracked", true},
			{"AbyssionReleased", true},
			{"Aquaius", true},
			{"ViscountBaby", true},
			{"Hatchling", false},
			{"ZealousJelly", false},
			{"DistractJelly", false},
			{"SpittingJelly", false},
			{"GraniteEnergy", false},
			{"EncroachingEnergy", false},
			{"EnergyStormConduit", false},
			{"PyroCore", true},
			{"CryoCore", true},
			{"BioCore", true},
			{"EnemyBeholder", true},
			{"AbyssalSpawn", true},
			{"AquaiusBubble", true},
			{"LucidBubble", true},
			// All other NPCs
			{"AncientArcher", true},
			{"AncientCharger", true},
			{"AncientPhalanx", true},
			{"Barracuda", true},
			{"BigBone", false},
			{"BizarreRockFormation", false},
			{"Blowfish", true},
			{"DarksteelKnight", true},
			{"EarthenBat", false},
			{"EarthenGolem", false},
			{"GelatinousCube", false},
			{"GigaClam", true},
			{"GildedBat", true},
			{"GildedSlime", true},
			{"GildedSlimeling", true},
			{"GildedLycan", true},
			{"Globee", true},
			{"GraniteEradicator", false},
			{"GraniteFusedSlime", false},
			{"GraniteSurger", false},
			{"BlueHag", true},
			{"Hammerhead", true},
			{"ManofWar", false},
			{"MorayHead", true},
			{"MorayBody", true},
			{"MorayTail", true},
			{"Octopus", true},
			{"GelatinousSludge", false},
			{"Sharptooth", true},
			{"UFO", true},
			{"UnstableEnergyAnomaly", false},
			{"WindElemental", false},
			{"AbyssalAngler", true},
			{"AquaticHallucination", true},
			{"Blobfish", true},
			{"CrownofThorns", true},
			{"FeedingFrenzy", true},
			{"GlitteringGolem", false},
			{"Kraken", true},
			{"MartianSentry", true},
			{"MoltenMortar", false},
			{"PutridSerpent", true},
			{"Spectrumite", true},
			{"SubmergedMimic", true},
			{"Whale", true},
			{"VampireSquid", true},
			{"VoltEelHead", false},
			{"VoltEelBody1", false},
			{"VoltEelBody2", false},
			{"VoltEelTail", false}
			
		};
		
		private static Dictionary<string,bool> thorium_npc_debuff_heat_vulnerability = new Dictionary<string,bool>
		{
			// If value is 1, the npc is vulnerable, 0 is resistance and no entry is neutral
			// Bosses and minions
			{"TheGrandThunderBirdv2", false},
			{"QueenJelly", true},
			{"GraniteEnergyStorm", false},
			{"TheBuriedWarrior", false},
			{"ThePrimeScouter", false},
			{"BoreanStrider", true},
			{"BoreanStriderPopped", true},
			{"FallenDeathBeholder", false},
			{"FallenDeathBeholder2", false},
			{"Abyssion", false},
			{"AbyssionReleased", true},
			{"Omnicide", true},
			{"SlagFury", false},
			{"Hatchling", false},
			{"ZealousJelly", true},
			{"DistractJelly", true},
			{"SpittingJelly", true},
			{"GraniteEnergy", false},
			{"EncroachingEnergy", false},
			{"EnergyStormConduit", false},
			{"PyroCore", false},
			{"CryoCore", true},
			{"BoreanMyte1", true},
			{"BoreanHopper", true},
			{"EnemyBeholder", false},
			{"ThousandSoulPhalactry", false},
			{"LucidBubble", false},
			{"CorpseBloom", true},
			{"CorpsePetal", true},
			{"CorpseWeed", true},
			{"PatchWerk", true},
			{"Maggot", true},
	     	{"BurstingMaggot" , true},
			// All other NPCs
			{"AncientArcher", false},
			{"AncientCharger", false},
			{"AncientPhalanx", false},
			{"ArmyAnt", true},
			{"FireAnt", false},
			{"BabySpider", true},
			{"BatOutaHell", false},
			{"FlamekinCaster", false},
			{"Biter", true},
			{"BizarreRockFormation", false},
			{"LivingHemorrhage", true},
			{"Clot", true},
			{"Coolmera", true},
			{"EarthenBat", true},
			{"EarthenGolem", true},
			{"FrostWurmHead", true},
			{"FrostWurmBody1", true},
			{"FrostWurmBody2", true},
			{"FrostWurmTail", true},
			{"FrozenFace", true},
			{"GelatinousCube", true},
			{"GildedBat", false},
			{"GildedLycan", false},
			{"GraniteEradicator", false},
			{"GraniteSurger", false},
			{"GreenHag", true},
			{"HoppingSpider", true},
			{"LifeCrystalMimic", false},
			{"ManofWar", true},
			{"BloomMahoganyEnt", true},
			{"MahoganyEnt", true},
			{"MudMan", true},
			{"Nestling", true},
			{"RagingMinotaur", false},
			{"GelatinousSludge", true},
			{"SnowBall",  true},
			{"SnowEater", true},
			{"SpaceSlime", false},
			{"StrangeBulb", true},
			{"TheInnocent", true},
			{"UnstableEnergyAnomaly", false},
			{"AstroBeetle", true},
			{"BlackWidow", true},
			{"BrownRecluse", true},
			{"BrownRecluseBaby", true},
			{"Blister", true},
			{"BlisterPod", true},
			{"BoneFlayer", false},
			{"ChilledSpitter", true},
			{"Coldling", true},
			{"DissonanceSeer", true},
			{"EpiDermon", false},
			{"Freezer", true},
			{"FrostFang", true},
			{"FrozenGross", true},
			{"GlitteringGolem", false},
			{"HorrificCharger", true},
			{"HellBringerMimic", false},
			{"InfernalHound", false},
			{"Lycan", true},
			{"MartianScout", true},
			{"MoltenMortar", false},
			{"MossWasp", true},
			{"MyceliumMimic", true},
			{"SnowFlinxMatriarch", true},
			{"Spectrumite", false},
			{"SunPriestess", false},
			{"Tarantula", true},
			{"TheStarved", true},
			{"UnderworldPot1", false},
			{"UnderworldPot2", false},
			{"VileFloater", true},
			{"Abomination", true},
			{"BloodDrop", true},
			{"BloodMage", true},
			{"BloodyWarg", true},
			{"EngorgedEye", true},
			{"GraveLimb", true},
			{"SeveredLegs", true},
			{"SmotheringShade", true},
			{"BlizzardBat", true},
			{"FrostBurnt", true},
			{"SnowElemental", true},
			{"SnowyOwl", true},
			{"GoblinSpiritGuide", true},
			{"ShadowflameRevenant", false},
			{"SeaShantySinger", true},
			{"LeFantome", true},
			{"ScissorStalker", true},
			{"SnowSinga", true}
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
			
			//Check which npcs should be granted damage reduction
			foreach (var boss in thorium_npcs_dr) 
			{
				if ( npc.type == thorium.Find<ModNPC>(boss.Key).Type ) 
				{
					// Applying Damage Reduction
					npc.DR_NERD(boss.Value, null, null, null, null);
				}
			}
			
			//Apply cold vulnerabilities
			foreach (var mob in thorium_npc_debuff_cold_vulnerability) 
			{
				if ( npc.type == thorium.Find<ModNPC>(mob.Key).Type ) 
				{
						npc.Calamity().VulnerableToCold = new bool?(mob.Value);
						break;
				}
			}
			
			//Apply water vulnerabilities
			foreach (var mob in thorium_npc_debuff_water_vulnerability) 
			{
				if ( npc.type == thorium.Find<ModNPC>(mob.Key).Type ) 
				{
						npc.Calamity().VulnerableToWater = new bool?(mob.Value);
						break;
				}
			}
			
		    //Apply sickness vulnerabilities
			foreach (var mob in thorium_npc_debuff_sickness_vulnerability) 
			{
				if ( npc.type == thorium.Find<ModNPC>(mob.Key).Type ) 
				{
						npc.Calamity().VulnerableToSickness = new bool?(mob.Value);
						break;
				}
			}
			
			//Apply electricity vulnerabilities
			foreach (var mob in thorium_npc_debuff_electricity_vulnerability) 
			{
				if ( npc.type == thorium.Find<ModNPC>(mob.Key).Type ) 
				{
						npc.Calamity().VulnerableToElectricity = new bool?(mob.Value);
						break;
				}
			}
			
			//Apply heat vulnerabilities
			foreach (var mob in thorium_npc_debuff_heat_vulnerability) 
			{
				if ( npc.type == thorium.Find<ModNPC>(mob.Key).Type ) 
				{
						npc.Calamity().VulnerableToHeat = new bool?(mob.Value);
						break;
				}
			}
		}	
	}
}