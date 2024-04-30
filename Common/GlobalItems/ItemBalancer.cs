using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.Items;
using System;

namespace RagnarokMod.Common.GlobalItems
{
    public class ItemBalancer : GlobalItem
    {
		private static bool print_message=true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
	
		private static Dictionary<string,int> thorium_melee_and_other_damage_tweak = new Dictionary<string,int>
		{
			{"IceLance", 14}, 
			{"ColdFront", 16},
			{"BentZombieArm", 16},
			{"Fork", 16},
			{"fSandStoneSpear", 16},
			{"CoralPolearm", 15},
			{"ThoriumSpear", 17},
			{"Scorpain", 14},
			{"RedHourglass", 16},
			{"ThoriumBoomerang", 20},
			{"HarpyTalon", 18},
			{"Aerial", 17},
			{"ThunderTalon", 15},
			{"CoralSlasher", 13},
			{"IceBreaker", 8},
			{"dSandStoneScimtar", 16},
			{"Spoon", 21},
			{"SteelBlade", 18},
			{"TheSnowball", 9},
			{"ThoriumBlade", 18},
			{"TrackersSkinningBlade", 19},
			{"WhirlpoolSaber", 20},
			{"DrenchedDirk", 21},
			{"Knife", 22},
			{"Whip", 24},
			{"BloomingBlade", 26},
			{"PearlPike", 26},
			{"PollenPike", 26},
			{"EnergyStormPartisan", 28},
			{"GorgonsEye", 27},
			{"Heartstriker", 28},
			{"Illustrious", 28},
			{"BatWing", 29},
			{"Bellerose", 29},
			{"Sanguine", 31},
			{"eDarksteelBroadSword", 32},
			{"Moonlight", 32},
			{"Nocturnal", 32},
			{"BackStabber", 34},
			{"ChampionSwiftBlade", 35},
			{"GiantGlowstick", 34},
			{"GraniteReflector", 35},
			{"GrimFlayer", 35},
			{"RifleSpear", 35},
			{"Blitzzard", 37},
			{"ToothOfTheConsumer", 37},
			{"FleshSkewer", 38},
			{"Saba", 39},
			{"BloodDrinker", 40},
			{"DragonTalon", 40},
			{"HellishHalberd", 41},
			{"DragonTooth", 41},
			{"Rapier", 41},
			{"MeleeThorHammer", 41},
			{"DurasteelBlade", 43},
			{"PoseidonCharge", 44},
			{"Arthropod", 46},
			{"GoldenLocks", 46},
			{"ValadiumSlicer", 46},
			{"ValadiumSpear", 46},
			{"DoomFireAxe", 48},
			{"LodeStoneClaymore", 48},
			{"Schmelze", 48},
			{"Scalper", 51},
			{"MyceliumWhip", 52}
		};
		
		private static Dictionary<string,int> thorium_throwing_damage_tweak = new Dictionary<string,int> 
		{
			{"gSandStoneThrowingKnife", 13},
			{"StoneThrowingSpear", 10},
			{"CactusNeedle", 9},
			{"CoralCaltrop", 11},
			{"IronTomahawk", 12},
			{"IcyCaltrop", 12},
			{"LeadTomahawk", 14},
			{"ShinobiSlicer", 11},
			{"DemoniteTomahawk", 16},
			{"CrimtaneTomahawk", 17},
			{"LastingPliers", 12},
			{"SeveredHand", 14},
			{"GelGlove", 16},
			{"SteelBattleAxe", 16},
			{"BloomingShuriken", 18},
			{"HarpiesBarrage", 18},
			{"MeteoriteClusterBomb", 18},
			{"ObsidianStriker", 18},
			{"PhaseChopper", 18},
			{"ThoriumDagger", 19},
			{"BaseballBat", 21},
			{"DraculaFang", 20},
			{"EnchantedKnife",20},
			{"GoblinWarSpear",20},
			{"NaiadShiv",20},
			{"SeedBomb",20},
			{"StarfishSlicer",20},
			{"SpikyCaltrop",20},
			{"Bolas",21},
			{"WackWrench",21},
			{"AquaiteKnife",22},
			{"ArcaneAnelace",22},
			{"GraniteThrowingAxe",22},
			{"SpikeBomb",22},
			{"BlackDagger",22},
			{"BronzeThrowing",24},
			{"FungalPopper",24},
			{"Embowelment",26},
			{"MoltenKnife",26},
			{"Chum",28},
			{"LightAnguish",28},
			{"Kunai",30},
			{"GaussFlinger",31},
			{"TheCryoFang",31},
			{"MorelGrenade",32},
			{"CorrupterBalloon",33},
			{"CrystalBalloon",33},
			{"EvisceratingClaw",33},
			{"FesteringBalloon",33},
			{"GasContainer",33},
			{"RocketFist",33},
			{"CaptainsPoniard",34},
			{"AphrodisiacVial",35},
			{"CombustionFlask",35},
			{"CorrosionBeaker",35},
			{"NitrogenVial",35},
			{"ChampionsGodHand",36},
			{"VenomKunai",36},
			{"CobaltThrowingSpear",37},
			{"DurasteelJavelin",37},
			{"HotPot",37},
			{"LegionOrnament",37},
			{"AxeBlade",38},
			{"ClockWorkBomb",38},
			{"PalladiumThrowingSpear",39},
			{"HellRoller",40},
			{"AdamantiteGlaive",41},
			{"Carnwennan",41},
			{"TrueEmbowelment",41},
			{"ValadiumBattleAxe",41},
			{"LodestoneJavelin",43},
			{"RiftTearer",43},
			{"TitaniumGlaive",43},
			{"TrueLightAnguish",43},
			{"SparkTaser",45},
			{"TrueCarnwennan",45},
			{"TitanJavelin",48},
			{"ChlorophyteTomahawk",52},
			{"ShadowTippedJavelin",52},
			{"MagicCard",53},
			{"StalkersSnippers",54},
			{"Omniwrench",56},
			{"SoulCleaver",58},
			{"VoltHatchet",58},
			{"ShadowPurgeCaltrop",59},
			{"SwampSpike",59},
			{"SoftServeSunderer",60},
			{"BudBomb",63},
			{"HadronCollider",63},
			{"TerraKnife",63},
			{"SoulBomb",67},
			{"LihzahrdKukri",69},
			{"PlasmaVial",69},
			{"Soulslasher",69},
			{"BugenkaiShuriken",73},
			{"CosmicDagger",76},
			{"ShadeKunai",78},
			{"Witchblade",80},
			{"DragonFang",81},
			{"Brinefang",86},
			{"ProximityMine",91},
			{"StarEater",95},
			{"TerrariumRippleKnife",95},
			{"ElectroRebounder",97},
			{"WhiteDwarfKunai",104},
			{"ShadeKusarigama",108},
			{"FireAxe",129},
			{"TidalWave",145},
			{"DeitysTrefork",215},
			{"AngelsEnd",300}
		};
		
		private static Dictionary<string,int> thorium_healer_damage_tweak = new Dictionary<string,int>
		{
			{"LifeQuartzClaymore",15},
			{"LifeAndDeath", 120},
			{"DarkScythe",16},
			{"CrimsonScythe", 16}
		};
		
		private static Dictionary<string,int> thorium_bard_damage_tweak = new Dictionary<string,int>
		{
			{"Didgeridoo", 15}
		};
		
		private static Dictionary<string,int> thorium_summon_damage_tweak = new Dictionary<string,int>
		{
			{"PrehistoricAmberStaff", 14}
		};
	
		public override void SetDefaults(Item item)
        {
			if(item.defense > 0)
			{
				if (item.type == thorium.Find<ModItem>("SandStoneMail").Type) 
				{
					item.defense = 4;
				}	
				else if (item.type == thorium.Find<ModItem>("SandStoneHelmet").Type) 
				{
					item.defense = 2;
				}	
			} 
			else if (item.damage > 0) 
			{	
				//Summon
				if(item.DamageType == DamageClass.Summon) 
				{
					foreach (var compareditem in thorium_summon_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
				}
				//Healer
				else if(item.DamageType == ThoriumDamageBase<HealerDamage>.Instance) 
				{
					foreach (var compareditem in thorium_healer_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
				}
				//Bard
				else if(item.DamageType == ThoriumDamageBase<BardDamage>.Instance) 
				{
					foreach (var compareditem in thorium_bard_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
				}
				//Throwing
				else if(item.DamageType == DamageClass.Throwing) 
				{
					foreach (var compareditem in thorium_throwing_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
				}
				//Melee or anything else
				else 
				{
					foreach (var compareditem in thorium_melee_and_other_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
				}
				
				// Apply some other tweaks
			}	
			else 
			{
				return;
			}
			
		}
	}
}