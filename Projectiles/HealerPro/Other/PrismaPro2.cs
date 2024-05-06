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
    public class PrismaPro2 : ModProjectile, ILocalizedModType
    {
        private int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            
        }
        public override void AI()
        {
            timer++;
            if (timer == 4)
            {
                timer = 0;
                Dust.NewDust(Projectile.Center, 0, 0, 323);
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}