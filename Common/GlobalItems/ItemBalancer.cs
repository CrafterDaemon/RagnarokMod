using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.Items.Depths;
using ThoriumMod.Buffs;

namespace RagnarokMod.Common.GlobalItems
{
    public class ItemBalancer : GlobalItem
    {
		private static bool print_message=true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
	
	
		private static List<string> pointblankshots = new List<string> {
			"FrostFury",
			"eSandStoneBow",
			"TalonBurst",
			"SteelBow",
			"ThoriumBow",
			"YewWoodBow",
			"FeatherFoe",
			"GrassStringBow",
			"BloomingBow",
			"StreamSting",
			"EternalNight",
			"ChampionsTrifectaShot",
			"CometCrossfire",
			"CupidString",
			"NagaRecurve",
			"FleshBow",
			"CinderString",
			"GlacialSting",
			"LodeStoneBow",
			"ValadiumBow",
			"ShusWrath",
			"DestroyersRage",
			"TitanBow",
			"DecayingSorrow",
			"DemonBloodBow",
			"ShadowFlareBow",
			"TheBlackBow",
			"TerrariumLongbow",
			"gDarkSteelCrossBow",
			"DurasteelRepeater",
			"DemonBloodCrossBow",
			"IllumiteShotbow",
			"YewWoodFlintlock",
			"HarpyPelter",
			"BuccaneerBlunderBuss",
			"SharkStorm",
			"GuanoGunner",
			"Slugger",
			"HellfireMinigun",
			"EnergyStormBolter",
			"HitScanner",
			"SpudBomber",
			"AquaPelter",
			"Trapper",
			"Webgun",
			"GoblinWarpipe",
			"ManHacker",
			"SSDevastator",
			"Shockbuster",
			"TommyGun",
			"BulletStorm",
			"MicroLauncher",
			"Obliterator",
			"PalladiumSubmachineGun",
			"TheZapper",
			"ElephantGun",
			"FreezeRay",
			"ChargedSplasher",
			"CobaltPopper",
			"DragonsGaze",
			"BloodBelcher",
			"RangedThorHammer",
			"MythrilPelter",
			"Trigun",
			"ArmorBane",
			"Funggat",
			"LodeStoneQuickDraw",
			"Scorn",
			"ValadiumFoeBlaster",
			"OrichalcumPelter",
			"UmbraBlaster",
			"TranquilizerGun",
			"VegaPhaser",
			"BeetleBlaster",
			"OgreSnotGun",
			"SpiritBreaker",
			"TitaniumRifle",
			"MineralLauncher",
			"SpineBuster",
			"Teslanator",
			"HandCannon",
			"IllumiteBlaster",
			"IllumiteBarrage",
			"AdamantiteCarbine",
			"MyceliumGattlingGun",
			"PhantomArmCannon",
			"DreadLauncher",
			"RejectsBlowpipe",
			"TrenchSpitter",
			"TerrariumPulseRifle",
			"LittleRed",
			"OmniCannon",
			"EmperorsWill",
			"DMR",
			"WyrmDecimator",
			"NovaRifle"
		};
	
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
			{"DragonTalon", 42},
			{"HellishHalberd", 41},
			{"DragonTooth", 42},
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
			{"MyceliumWhip", 52},
			{"Executioner", 53},
			{"EyeoftheDestroyer", 55},
			{"IllumiteSpear", 55},
			{"MidasGavel", 55},
			{"TitanBoomerang", 55},
			{"DreadFork", 57},
			{"Glacier", 57},
			{"SparkingJellyBall", 28},
			{"TheBlackBlade", 57},
			{"TitanSword", 59},
			{"IllumiteBlade", 66},
			{"ShipsHelm", 66},
			{"BloodyHighClaws", 68},
			{"StarTrail", 35},
			{"DreadRazor", 73},
			{"Spearmint", 78},
			{"ClimbersIceAxe", 80},
			{"SoulReaver", 82},
			{"TheWhirlpool", 84},
			{"DemonBloodSpear", 89},
			{"GolemsGaze", 92},
			{"SoulRender", 93},
			{"TheJuggernaut", 48},
			{"QuakeGauntlet", 97},
			{"DemonBloodSword", 99},
			{"FleshMace", 52},
			{"PrimesFury", 104},
			{"SolScorchedSlab", 108},
			{"BoneFlayerTail", 55},
			{"EbonyTail", 55},
			{"TheSeaMine", 55},
			{"TerrariumHyperDisc", 119},
			{"LodeStoneBreaker", 64},
			{"SteamFlail", 64},
			{"MantisShrimpPunch", 132},
			{"TerrariumSaber", 132},
			{"EclipseFang", 138},
			{"TerrariumSpear",138},
			{"LivewireCrasher", 77},
			{"BloodGlory", 154},
			{"WyvernSlayer", 154},
			{"Skadoosh", 173},
			{"LingeringWill", 235},
			{"EssenceofFlame", 265},
			{"SevenSeasDevastator", 275}, // No change
			{"OceansJudgement", 325} // No change
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
			{"StarEater",88},
			{"ProximityMine",91},
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
			{"LifeAndDeath", 95},
			{"DarkScythe",16},
			{"CrimsonScythe", 16}
		};
		
		private static Dictionary<string,int> thorium_bard_damage_tweak = new Dictionary<string,int>
		{
			{"JingleBells", 85},
			{"DragonsWail", 35},
			{"BlackMIDI", 150},
			{"ShootingStarBlastGuitar", 150},
			{"TheSet", 290},
			{"EdgeofImagination", 165}
		};
		
		private static Dictionary<string,int> thorium_ranged_damage_tweak = new Dictionary<string,int>
		{
			{"YewWoodFlintlock", 14},
			{"HellfireMinigun", 8},
			{"CobaltPopper", 28},
			{"TitaniumRifle", 43},
			{"RangedThorHammer", 30},
			{"PalladiumSubmachineGun", 25},
			{"MythrilPelter", 33},
			{"OrichalcumPelter", 35},
			{"AdamantiteCarbine", 54},
			{"DragonsGaze", 31}
			
		};
		
		private static Dictionary<string,int> thorium_summon_damage_tweak = new Dictionary<string,int>
		{
			{"SeahorseWand", 8},
			{"LivingWoodAcorn", 12},
			{"StormHatchlingStaff", 13},
			{"RosySlimeStaff", 13},
			{"ButterflyStaff",13},
			{"PrehistoricAmberStaff", 15},
			{"ArsenalStaff", 16},
			{"MeteorHeadStaff", 17},
			{"YarnBall", 18},
			{"EnchantedCane", 20},
			{"ViscountCane", 19},
			{"TabooWand", 23},
			{"MantisCane", 27},
			{"SteamgunnerController", 18},
			{"IceFairyStaff", 25}, 
			{"LadyLight",25},
			{"BloodFeasterStaff", 30},
			{"DraconicMagmaStaff", 29},
			{"CrimsonHoundStaff", 31},
			{"CorruptlingStaff", 32},
			{"MastersLibram", 34},
			{"HailBomber", 37},
			{"TheBlackCane", 40},
			{"BlobhornCoralStaff", 33},
			{"ValkyrieBlade", 43},
			{"BeholderStaff", 50},
			{"EyeofOdin", 50},
			{"CorrodlingStaff", 64},
			{"TerrariumEnigmaStaff", 69},
			{"BloodyPaganStaff", 83},
			{"NebulaReflection", 143},
			{"EmberStaff", 250},
			{"AntlionStaff", 16},
			{"BleedingHeartStaff", 14},
			{"CreepingVineStaff", 17},
			{"DevourerStaff", 18},
			{"SpittingFish", 20},
			{"NanoClamCane", 23},
			{"WeedEater", 24},
			{"BoulderProbeStaff", 25},
			{"StrangeSkull", 32},
			{"TotemCaller", 35},
			{"InfernalAnimator", 36},
			{"DistressCaller", 36},
			{"StrongestLink", 39},
			{"FungalCane", 32},
			{"VoidLance", 46},
			{"TheIncubator", 48},
			{"RudeWand",55},
			{"MortarStaff",62},
			{"AeonStaff", 63},
			{"VoltModule",85},
			{"PrometheanStaff", 100}, // no change
			{"RodofFlocking", 20},
			{"StellarRod", 46},
			{"StellarSystem", 57},
			{"PhantomWand", 55},
			{"ShadowOrbStaff", 104}
		};
	
	
		private static Dictionary<string,int> thorium_magic_damage_tweak = new Dictionary<string,int>
		{
			{"IceCube", 8},
			{"Charm", 12},
			{"Confuse", 12},
			{"Dissolve", 12},
			{"Freeze", 12},
			{"Ignite", 12},
			{"Pierce", 12},
			{"Poison", 12},
			{"Siphon", 12},
			{"Stun", 12},
			{"MagickStaff", 14},
			{"OpalStaff", 17 },
			{"AquamarineStaff", 18},
			{"ThoriumStaff", 13},
			{"MagicThorHammer", 34},
			{"DragonsBreath", 32}
		};
			
		private static Dictionary<string,int> thorium_armor_defense_tweak = new Dictionary<string,int>
		{
			{"SandStoneMail", 4},
			{"SandStoneHelmet", 2},
			{"TideTurnerBreastplate", 26}, 
			{"TideTurnerGreaves", 25},
			{"TideTurnerHelmet", 27}, 
			{"AssassinsGuard", 29}, 
			{"AssassinsWalkers",29}, 
			{"RhapsodistBoots", 27}, 
			{"RhapsodistChestWoofer", 27},
			{"PyromancerTabard", 27},
			{"PyromancerLeggings", 24},
			{"DragonMask", 11},
			{"DragonGreaves", 12},
			{"DragonBreastplate", 15}	
		};
		
		public override void SetDefaults(Item item)
        {
			// Armor
			if(item.defense > 0)
			{
				foreach (var compareditem in thorium_armor_defense_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.defense = compareditem.Value;
							break;
						}
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
				//Ranged
				else if(item.DamageType == DamageClass.Ranged) 
				{
					foreach (var compareditem in thorium_ranged_damage_tweak) 
					{
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) 
						{
							item.damage = compareditem.Value;
							break;
						}
					}
					
					//AddPointBlankShots
					foreach (string weaponname in pointblankshots ) 
					{
						if ( item.type == thorium.Find<ModItem>(weaponname).Type) 
						{	
							item.Calamity().canFirePointBlankShots = true;
							break;
						}
					}
				}
				// Magic
				else if(item.DamageType == DamageClass.Magic) 
				{
					foreach (var compareditem in thorium_magic_damage_tweak) 
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
				if(item.type == thorium.Find<ModItem>("TerrariansLastKnife").Type) 
				{
					item.damage = 280;
					item.shootSpeed = 16f;
					item.scale = 1.7f;
				}
			}	
			else 
			{
				return;
			}
		}
		
		public override void UpdateEquip(Item item, Player player) 
		{
			if(item.type == thorium.Find<ModItem>("DepthDiverHelmet").Type) 
			{
				if (Main.netMode == 1 && Main.myPlayer != player.whoAmI)
				{
					Player localPlayer = Main.LocalPlayer;
					if (localPlayer.DistanceSQ(player.Center) < 62500f)
					{
						localPlayer.AddBuff(ModContent.BuffType<DepthBreath>(), 30, true, false);
					}
				}
			
				var calamityPlayer = player.Calamity();
				if (!calamityPlayer.ZoneAbyss)
				{
					if (player.breath <= player.breathMax + 2)
					{
						player.breath = player.breathMax + 3;
					}	
				} 
				else
				{
					player.moveSpeed += 0.2f;
					player.statDefense += 10;
					if(player.breath < player.breathMax - 25 && player.breath > 5) 
					{	
						Random rnd = new Random(); 
						if(rnd.Next(1, 600) == 1) 
						{
							player.breath = player.breath + 20;
						}
					}
				}
				player.GetCritChance(DamageClass.Generic) += 6f;
			}
			if(item.type == thorium.Find<ModItem>("WhiteDwarfMask").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.05f;
			}
			if(item.type == thorium.Find<ModItem>("WhiteDwarfGuard").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.05f;
			}
			if(item.type == thorium.Find<ModItem>("WhiteDwarfGreaves").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.05f;
			}
			if(item.type == thorium.Find<ModItem>("ShadeMasterMask").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.05f;
			}
			if(item.type == thorium.Find<ModItem>("ShadeMasterTreads").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.075f;
			}
			if(item.type == thorium.Find<ModItem>("LichCarapace").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.05f;
			}
			if(item.type == thorium.Find<ModItem>("LichTalon").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.025f;
			}
			if(item.type == thorium.Find<ModItem>("TideTurnersGaze").Type) 
			{
				player.GetDamage(DamageClass.Throwing) -= 0.15f;
			}
		}
		
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
				if(item.type == thorium.Find<ModItem>("DepthDiverHelmet").Type) 
				{
					tooltips[5].Text = tooltips[5].Text + "\nDoes not work in the Abyss but instead grants you and your allies +10 defense and +20% movement speed\nYou also randomly get some oxygen back";
					
				}
		}
		
	}
}