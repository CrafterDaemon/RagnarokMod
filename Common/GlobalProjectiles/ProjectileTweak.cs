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
    public class ProjectileTweak : GlobalProjectile
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
		{
			return ( projectile.type == thorium.Find<ModProjectile>("WhiteFlare").Type);
		}
		
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifier) 
		{
			if ( projectile.type == thorium.Find<ModProjectile>("WhiteFlare").Type)
			{
				if (projectile.TryGetOwner(out Player player) && player.GetWeaponDamage(player.HeldItem) > target.damage)
				{
					projectile.damage = player.GetWeaponDamage(player.HeldItem);
                }
				else 
				{
					projectile.damage = target.damage; 
				}
			}
		}
	}
}