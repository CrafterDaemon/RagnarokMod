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
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifier) 
		{
			ModLoader.TryGetMod("ThoriumMod", out Mod thorium);
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