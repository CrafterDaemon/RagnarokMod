using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using ThoriumMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class ProjectileBalancer : GlobalProjectile
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		
		public override bool AppliesToEntity(Projectile projectile, bool lateInstantiation)
        {
            return (
			projectile.type == thorium.Find<ModProjectile>("LifeDeathPro1").Type
			);
        }
		
		public override void SetDefaults(Projectile projectile)
        {
			if (projectile.type == thorium.Find<ModProjectile>("LifeDeathPro1").Type) 
			{
				projectile.usesLocalNPCImmunity = true;
				projectile.localNPCHitCooldown = 15;
				projectile.timeLeft = 120;
				projectile.penetrate = 3;

			}
		}
	}
}