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
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
			float currentDamageModifier = modifier.IncomingDamageMultiplier.Value;
			float desiredDamageModifier;
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
		
		public override void SetDefaults(Projectile projectile) 
		{
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
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