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
			"GrandThunderBirdZap",
			"ThunderGust",
			"ViscountBlood",
			"ViscountRipple",
			"ViscountRipple2",
			"ViscountRipple3",
			"ViscountStomp",
			"ViscountStomp2",
			"ViscountRockFall",
			"BubblePulse",
			"BubbleBomb",
			"QueenTorrent",
			"QueenJellyArm",
			"BuriedShock",
			"BuriedDagger",
			"BuriedArrow",
			"BuriedArrow2",
			"BuriedArrowC",
			"BuriedArrowP",
			"BuriedArrowF",
			"BuriedMagic",
			"BuriedMagicPop",
			"GraniteCharge",
			"VaporizeBoom",
			"VaporizeBlast",
			"VaporizePulse",
			"GravitonSurge",
			"Vaporize",
			"GravitonCharge",
			"GravitonBoom",
			"GravitySpark",
			"CryoVaporize",
			"BioVaporize",
			"MoltenVaporize",
			"MoltenCoreBeam",
			"BioCoreBeam",
			"CryoCoreBeam",
			"BlizzardFang",
			"FrostSurge",
			"BlizzardBoom",
			"BlizzardCascade",
			"BlizzardStart",
			"IceAnomaly",
			"FrostMytePro",
			"BeholderBeam",
			"DoomBeholderBeam",
			"VoidLaserPro",
			"BeholderLavaCascade",
			"LichPumpkinPro",
			"LichPumpkinPro2",
			"LichFlare",
			"LichGaze",
			"SoulRenderLich",
			"SoulSiphon",
			"LichFlareDeath",
			"WhirlPool",
			"AbyssionSpit2",
			"AquaRipple",
			"OldGodSpit2",
			"AbyssalStrike2",
			"FlameLash",
			"AquaSplash",
			"FlamePulsePro",
			"FlameNova",
			"AquaBomb2",
			"DeathRay",
			"AquaTyphoon",
			"FlameFury",
			"DeathCircle2",
			"LucidPulse",
			"LucidRay",
			"LucidFury",
			"LucidMiasma",
			"LucidTyphoon",
			"LucidBomb2",
		};
		
		public override void ModifyHitPlayer (Projectile projectile, Player target, ref Player.HurtModifiers modifier) 
		{
			
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
			
			if (thorium_projectile_list.Contains(projectile.ModProjectile.Name)) 
			{
				modifier.IncomingDamageMultiplier *= desiredDamageModifier / currentDamageModifier;
			}		
		}
	}
}