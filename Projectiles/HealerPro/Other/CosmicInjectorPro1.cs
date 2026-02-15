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
    public class CosmicInjectorPro1 : ModProjectile, ILocalizedModType
    {
        private int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 7;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.light = 0.5f;

        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
        }
    }
}