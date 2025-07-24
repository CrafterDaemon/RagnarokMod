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
using System;
using System.IO;

namespace RagnarokMod.Common.GlobalProjectiles
{
    public class BossProjectileTweak : GlobalProjectile
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
		{
			foreach (var entry in thorium_projectile_list) 
			{
				if ( projectile.type == thorium.Find<ModProjectile>(entry).Type ) 
				{
					return true;
				}
			}
			return false;
		}
		
		private static List<string> thorium_projectile_list = new List<string> {
			"AbyssalStrike",
			"AbyssalStrike2",
			"AquaBomb",
			"AquaBomb2",
			"AquaRipple",
			"AquaSplash",
			"AquaTyphoon",
			"AquaTyphoon2",
			"BeholderBeam",
			"BeholderEyeEffect",
			"BeholderEyeEffect2",
			"BeholderLavaCascade",
			"BeholderLavaCascade1",
			"BioCoreBeam",
			"BioVaporize",
			"BlizzardBoom",
			"BlizzardCascade",
			"BlizzardFang",
			"BlizzardStart",
			"BubbleBomb",
			"BubblePulse",
			"BuriedArrow",
			"BuriedArrow2",
			"BuriedArrowC",
			"BuriedArrowF",
			"BuriedArrowFBoom",
			"BuriedArrowP",
			"BuriedDagger",
			"BuriedDaggerSpawner",
			"BuriedMagic",
			"BuriedMagicPop",
			"BuriedShock",
			"CountScream",
			"CryoCoreBeam",
			"CryoVaporize",
			"DeathCircle",
			"DeathCircle2",
			"DeathRay",
			"DeathRaySpawn",
			"DeathRaySpawn2",
			"DeathRaySpawn3",
			"DoomBeholderBeam",
			"DreadSpiritPro",
			"EncroachBlast",
			"FissureCascade",
			"FissureCascade2",
			"FlameFury",
			"FlameLash",
			"FlameNova",
			"FlameNovaBoom",
			"FlamePulsePro",
			"ForgottenOneSpit",
			"ForgottenOneSpit2",
			"GrandLightingEffect1",
			"GrandThunderBirdCloud",
			"GrandThunderBirdZap",
			"GrandThunderBirdZap2",
			"GraniteBurst",
			"GraniteCharge",
			"GravitonBoom",
			"GravitonCharge",
			"GravitonSurge",
			"GravitySpark",
			"LaserCloud",
			"LichFlare",
			"LichFlareDeath",
			"LichFlareDeathSpawn",
			"LichFlareDeathSpawn2",
			"LichFlareDeathSpawn3",
			"LichFlareSpawn",
			"LichFlareSpawn2",
			"LichFlareSpawn3",
			"LichGaze",
			"LichPumpkinPro",
			"LichPumpkinPro2",
			"LightShock",
			"LucidBomb",
			"LucidBomb2",
			"LucidFury",
			"LucidIndicator",
			"LucidIndicator2",
			"LucidMiasma",
			"LucidNovaBoom",
			"LucidPulse",
			"LucidRay",
			"LucidRaySpawn",
			"LucidTyphoon",
			"MainBeam",
			"MainBeamCheese",
			"MainBeamOuter",
			"MoltenCoreBeam",
			"MoltenVaporize",
			"OldGodSpit",
			"OldGodSpit2",
			"PatchFissure",
			"QueenTorrent",
			"RagFinish",
			"RagFinish2",
			"RagFinish3",
			"RagFinish4",
			"SoulRenderLich",
			"SoulSteal",
			"TheGrandThunderBirdEnd",
			"ThunderBirdScreech",
			"ThunderGust",
			"ThunderSpark",
			"Vaporize",
			"VaporizeBlast",
			"VaporizeBoom",
			"VaporizePulse",
			"ViscountBlood",
			"ViscountRipple",
			"ViscountRipple2",
			"ViscountRipple3",
			"ViscountRockFall",
			"ViscountRockSummon",
			"ViscountRockSummon2",
			"ViscountStomp",
			"ViscountStomp2",
			"VoidFlare",
			"VoidLaserPro",
			"VoidPortal1",
			"VoidPortal2",
			"Whirlpool"
		};
		
		
		private static Dictionary<string,float> thorium_bosses_bossrush_projectile_damage_modifier = new Dictionary<string,float> 
		{
			{"AbyssalStrike", 1.5f},
			{"AbyssalStrike2", 1.5f},
			{"AquaBomb", 1.5f},
			{"AquaBomb2", 1.5f},
			{"AquaRipple", 1.5f},
			{"AquaSplash", 1.5f},
			{"AquaTyphoon", 1.5f},
			{"AquaTyphoon2", 1.5f},
			{"BeholderBeam", 1.5f},
			{"BeholderEyeEffect", 1.5f},
			{"BeholderEyeEffect2", 1.5f},
			{"BeholderLavaCascade", 1.5f},
			{"BeholderLavaCascade1", 1.5f},
			{"BioCoreBeam", 1.5f},
			{"BioVaporize", 1.5f},
			{"BlizzardBoom", 1.5f},
			{"BlizzardCascade", 1.5f},
			{"BlizzardFang", 1.5f},
			{"BlizzardStart", 1.5f},
			{"BubbleBomb", 1.5f},
			{"BubblePulse", 1.5f},
			{"BuriedArrow", 1.5f},
			{"BuriedArrow2", 1.5f},
			{"BuriedArrowC", 1.5f},
			{"BuriedArrowF", 1.5f},
			{"BuriedArrowFBoom", 1.5f},
			{"BuriedArrowP", 1.5f},
			{"BuriedDagger", 1.5f},
			{"BuriedDaggerSpawner", 1.5f},
			{"BuriedMagic", 1.5f},
			{"BuriedMagicPop", 1.5f},
			{"BuriedShock", 1.5f},
			{"CountScream", 1.5f},
			{"CryoCoreBeam", 1.5f},
			{"CryoVaporize", 1.5f},
			{"DeathCircle", 1.5f},
			{"DeathCircle2", 1.5f},
			{"DeathRay", 1.5f},
			{"DeathRaySpawn", 1.5f},
			{"DeathRaySpawn2", 1.5f},
			{"DeathRaySpawn3", 1.5f},
			{"DoomBeholderBeam", 1.5f},
			{"DreadSpiritPro", 1.5f},
			{"EncroachBlast", 1.5f},
			{"FissureCascade", 1.5f},
			{"FissureCascade2", 1.5f},
			{"FlameFury", 1.5f},
			{"FlameLash", 1.5f},
			{"FlameNova", 1.5f},
			{"FlameNovaBoom", 1.5f},
			{"FlamePulsePro", 1.5f},
			{"ForgottenOneSpit", 1.5f},
			{"ForgottenOneSpit2", 1.5f},
			{"GrandLightingEffect1", 1.5f},
			{"GrandThunderBirdCloud", 1.5f},
			{"GrandThunderBirdZap", 1.5f},
			{"GrandThunderBirdZap2", 1.5f},
			{"GraniteBurst", 1.5f},
			{"GraniteCharge", 1.5f},
			{"GravitonBoom", 1.5f},
			{"GravitonCharge", 1.5f},
			{"GravitonSurge", 1.5f},
			{"GravitySpark", 1.5f},
			{"LaserCloud", 1.5f},
			{"LichFlare", 1.5f},
			{"LichFlareDeath", 1.5f},
			{"LichFlareDeathSpawn", 1.5f},
			{"LichFlareDeathSpawn2", 1.5f},
			{"LichFlareDeathSpawn3", 1.5f},
			{"LichFlareSpawn", 1.5f},
			{"LichFlareSpawn2", 1.5f},
			{"LichFlareSpawn3", 1.5f},
			{"LichGaze", 1.5f},
			{"LichPumpkinPro", 1.5f},
			{"LichPumpkinPro2", 1.5f},
			{"LightShock", 1.5f},
			{"LucidBomb", 1.5f},
			{"LucidBomb2", 1.5f},
			{"LucidFury", 1.5f},
			{"LucidIndicator", 1.5f},
			{"LucidIndicator2", 1.5f},
			{"LucidMiasma", 1.5f},
			{"LucidNovaBoom", 1.5f},
			{"LucidPulse", 1.5f},
			{"LucidRay", 1.5f},
			{"LucidRaySpawn", 1.5f},
			{"LucidTyphoon", 1.5f},
			{"MainBeam", 1.5f},
			{"MainBeamCheese", 1.5f},
			{"MainBeamOuter", 1.5f},
			{"MoltenCoreBeam", 1.5f},
			{"MoltenVaporize", 1.5f},
			{"OldGodSpit", 1.5f},
			{"OldGodSpit2", 1.5f},
			{"PatchFissure", 1.5f},
			{"QueenTorrent", 1.5f},
			{"RagFinish", 1.5f},
			{"RagFinish2", 1.5f},
			{"RagFinish3", 1.5f},
			{"RagFinish4", 1.5f},
			{"SoulRenderLich", 1.5f},
			{"SoulSteal", 1.5f},
			{"TheGrandThunderBirdEnd", 1.5f},
			{"ThunderBirdScreech", 1.5f},
			{"ThunderGust", 1.5f},
			{"ThunderSpark", 1.5f},
			{"Vaporize", 1.5f},
			{"VaporizeBlast", 1.5f},
			{"VaporizeBoom", 1.5f},
			{"VaporizePulse", 1.5f},
			{"ViscountBlood", 1.5f},
			{"ViscountRipple", 1.5f},
			{"ViscountRipple2", 1.5f},
			{"ViscountRipple3", 1.5f},
			{"ViscountRockFall", 1.5f},
			{"ViscountRockSummon", 1.5f},
			{"ViscountRockSummon2", 1.5f},
			{"ViscountStomp", 1.5f},
			{"ViscountStomp2", 1.5f},
			{"VoidFlare", 1.5f},
			{"VoidLaserPro", 1.5f},
			{"VoidPortal1", 1.5f},
			{"VoidPortal2", 1.5f},
			{"Whirlpool", 1.5f}
		};
		
		private static List<string> thorium_projectile_defense_damage_list = new List<string> {
			"AbyssalStrike",
			"AbyssalStrike2",
			"AquaBomb",
			"AquaBomb2",
			"AquaRipple",
			"AquaSplash",
			"AquaTyphoon",
			"AquaTyphoon2",
			"BeholderLavaCascade",
			"BioCoreBeam",
			"BlizzardBoom",
			"BlizzardCascade",
			"BuriedArrowC",
			"BuriedArrowF",
			"BuriedArrowFBoom",
			"BuriedArrowP",
			"BuriedDagger",
			"BuriedDaggerSpawner",
			"BuriedMagic",
			"CryoCoreBeam",
			"DeathCircle",
			"DeathCircle2",
			"DeathRay",
			"DeathRaySpawn",
			"DeathRaySpawn2",
			"DeathRaySpawn3",
			"DoomBeholderBeam",
			"DreadSpiritPro",
			"EncroachBlast",
			"FissureCascade",
			"FissureCascade2",
			"FlameFury",
			"FlameLash",
			"FlameNova",
			"FlameNovaBoom",
			"FlamePulsePro",
			"ForgottenOneSpit2",
			"GraniteBurst",
			"GravitonBoom",
			"GravitonCharge",
			"GravitonSurge",
			"GravitySpark",
			"LaserCloud",
			"LichFlare",
			"LichFlareDeath",
			"LichFlareDeathSpawn",
			"LichFlareDeathSpawn2",
			"LichFlareDeathSpawn3",
			"LichFlareSpawn",
			"LichFlareSpawn2",
			"LichFlareSpawn3",
			"LichGaze",
			"LichPumpkinPro",
			"LichPumpkinPro2",
			"LightShock",
			"LucidBomb",
			"LucidBomb2",
			"LucidFury",
			"LucidIndicator",
			"LucidIndicator2",
			"LucidMiasma",
			"LucidNovaBoom",
			"LucidPulse",
			"LucidRay",
			"LucidRaySpawn",
			"LucidTyphoon",
			"MainBeam",
			"MainBeamCheese",
			"MainBeamOuter",
			"MoltenCoreBeam",
			"MoltenVaporize",
			"PatchFissure",
			"RagFinish",
			"RagFinish2",
			"RagFinish3",
			"RagFinish4",
			"VaporizeBoom",
			"VaporizePulse",
			"ViscountStomp",
			"ViscountStomp2",
			"VoidFlare",
			"VoidLaserPro",
			"VoidPortal1",
			"VoidPortal2",
			"Whirlpool"
		};
		
		
		public override void ModifyHitPlayer (Projectile projectile, Player target, ref Player.HurtModifiers modifier) 
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
				foreach (var projectilename in thorium_bosses_bossrush_projectile_damage_modifier) 
				{
					if ( projectile.type == thorium.Find<ModProjectile>(projectilename.Key).Type ) 
					{
						if(CalamityGamemodeCheck.isDeath) 
						{
							desiredDamageModifier = currentDamageModifier + 0.35f + projectilename.Value;
						}
						else if (CalamityGamemodeCheck.isRevengeance) 
						{
							desiredDamageModifier = currentDamageModifier + 0.2f + projectilename.Value;
						} 
						else 
						{
							desiredDamageModifier = currentDamageModifier + projectilename.Value;
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
					desiredDamageModifier = currentDamageModifier +  0.35f;
				}
				else if (CalamityGamemodeCheck.isRevengeance) 
				{
					desiredDamageModifier = currentDamageModifier + 0.2f;
				}
				else 
				{
					return;
				}
				foreach (string projectilename in thorium_projectile_list) 
				{
					if ( projectile.type == thorium.Find<ModProjectile>(projectilename).Type ) 
					{
						modifier.IncomingDamageMultiplier *= desiredDamageModifier / currentDamageModifier;
						break;
					}
				}	
			}	
		}
		
		public override void SetDefaults(Projectile projectile) 
		{
			ModLoader.TryGetMod("ThoriumRework", out Mod ThoriumRework);
			if(ThoriumRework != null) 
			{
				return;
			}
			foreach (string projectilename in thorium_projectile_defense_damage_list) 
			{
				if ( projectile.type == thorium.Find<ModProjectile>(projectilename).Type ) 
				{
					// Applying Defense Damage
					projectile.Calamity().DealsDefenseDamage = true;
					break;
				}
			}	
		}
	}
}