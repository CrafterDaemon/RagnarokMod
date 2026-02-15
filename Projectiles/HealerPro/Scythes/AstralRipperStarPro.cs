using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Dusts;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AstralRipperStarPro : ModProjectile, ILocalizedModType
    {
        private int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.timeLeft = 500;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            timer++;
            Projectile.rotation += 20;
            if (timer % 3 == 0)
            {
                Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<AstralBlue>());
            }
            if (timer % 2 == 0)
            {
                Dust.NewDust(Projectile.Center, 0, 0, ModContent.DustType<AstralOrange>());
            }
        }
    }
}