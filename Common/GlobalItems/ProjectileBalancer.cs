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
		private static bool print_message=true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
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