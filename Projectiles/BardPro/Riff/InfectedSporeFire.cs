using System;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class InfectedSporeFire : BardProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        public static int Lifetime => 45;
        public ref float Time => ref Projectile.ai[0];

        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetBardDefaults()
        {
            Projectile.width = Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.MaxUpdates = 3;
            Projectile.timeLeft = Lifetime;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 12;
        }

        public override void AI()
        {
            Time++;

            if (Time >= 6f) 
                Projectile.scale = 1.5f * MathHelper.Clamp((Time - 6f) / (36f - 6f), 0f, 1f);
            else
                return;

            // Corruption smoke
            float smokeRot = MathHelper.ToRadians(3f);
            Color smokeColor = Color.Lerp(
                new Color(30, 80, 20),   // green
                new Color(40, 10, 50),    // purple
                0.5f + 0.3f * MathF.Sin(Main.GlobalTimeWrappedHourly * 5f));

            Particle smoke = new HeavySmokeParticle(
                Projectile.Center, Projectile.velocity * 0.5f,
                smokeColor, 20,
                Projectile.scale * Main.rand.NextFloat(0.6f, 1.2f),
                0.8f, smokeRot, required: true);
            GeneralParticleHandler.SpawnParticle(smoke);

            // Cursed flame glow overlay
            if (Main.rand.NextBool(4))
            {
                Color glowColor = Color.Lerp(smokeColor, new Color(80, 160, 30), 0.3f);
                Particle smokeGlow = new HeavySmokeParticle(
                    Projectile.Center, Projectile.velocity * 0.5f,
                    glowColor, 15,
                    Projectile.scale * Main.rand.NextFloat(0.4f, 0.7f),
                    0.8f, smokeRot, true, 0.005f);
                GeneralParticleHandler.SpawnParticle(smokeGlow);
            }

            // Cursed torch dust for extra green fire
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch,
                    Projectile.velocity * 0.3f + Main.rand.NextVector2Circular(1f, 1f),
                    80, default, Main.rand.NextFloat(0.8f, 1.4f));
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.1f * Projectile.scale, 0.3f * Projectile.scale, 0.05f * Projectile.scale);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CalamityUtils.CircularHitboxCollision(Projectile.Center, 52 * Projectile.scale * 0.5f, targetHitbox);
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 300);
        }
    }
}