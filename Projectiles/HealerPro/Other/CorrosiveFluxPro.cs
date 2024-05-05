using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class CorrosiveFluxPro : ModProjectile, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.alpha = 180;
            Projectile.ignoreWater = true;
            
        }
        public override void AI() {
			Projectile.velocity *= 0.975f;
            Projectile.alpha -= 8;
		}
       
    }
}