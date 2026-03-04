using System;
using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Wind
{
    public class InfectedOcarinaPro : ForestOcarinaPro2
    {
        public override void AI()
        {
            Projectile.SineWaveMovement(Projectile.ai[0], 6f, MathF.PI / 15f, Projectile.ai[0] == 0f, BardProjectile.HomeInHelper);
            Projectile.ai[0] += 1f;

            Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.Zero);
            Vector2 perp = new Vector2(-forward.Y, forward.X);

            // Primary cursed flame trail
            for (int i = 0; i < 2; i++)
            {
                float offset = (i == 0 ? 1f : -1f) * 4f;
                Vector2 pos = Projectile.Center + perp * offset;
                Dust dust = Dust.NewDustPerfect(pos, DustID.CursedTorch, forward * 2f + perp * offset * 0.1f);
                dust.noGravity = true;
                dust.fadeIn = 1.4f;
                dust.scale = Main.rand.NextFloat(1.1f, 1.5f);
            }

            // Central bright flame
            Dust core = Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch, forward * 1.5f);
            core.noGravity = true;
            core.scale = Main.rand.NextFloat(1.4f, 1.8f);

            // Intermittent corruption wisps trailing behind
            if (Main.rand.NextBool(3))
            {
                Vector2 trailPos = Projectile.Center - Projectile.velocity * Main.rand.NextFloat(0.3f, 0.8f);
                trailPos += perp * Main.rand.NextFloat(-6f, 6f);
                Dust corruption = Dust.NewDustPerfect(trailPos, DustID.CorruptionThorns,
                    -forward * Main.rand.NextFloat(0.5f, 1.5f) + Main.rand.NextVector2Circular(0.5f, 0.5f),
                    100, default, Main.rand.NextFloat(0.8f, 1.3f));
                corruption.noGravity = true;
            }

            // Faint dripping particles with gravity
            if (Main.rand.NextBool(5))
            {
                Dust drip = Dust.NewDustPerfect(Projectile.Center + perp * Main.rand.NextFloat(-8f, 8f),
                    DustID.CursedTorch,
                    new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(1f, 2.5f)),
                    0, default, Main.rand.NextFloat(0.6f, 1f));
                drip.noGravity = false;
            }

            DelegateMethods.v3_1 = new Vector3(0.15f, 0.6f, 0.2f);
            Terraria.Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.velocity * 4f, 40f, DelegateMethods.CastLightOpen);
        }

        public override void OnKill(int timeLeft)
        {
            // Radial burst of cursed fire
            for (int i = 0; i < 12; i++)
            {
                float angle = MathHelper.TwoPi * i / 12f;
                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(2f, 6f);
                Dust burst = Dust.NewDustPerfect(Projectile.Center, DustID.CursedTorch, vel, 0, default, Main.rand.NextFloat(1.2f, 2f));
                burst.noGravity = true;
            }

            // Corruption gunk splatter
            for (int i = 0; i < 6; i++)
            {
                Dust gunk = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height,
                    DustID.CorruptionThorns, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                gunk.noGravity = true;
                gunk.scale = Main.rand.NextFloat(1f, 1.6f);
            }
        }
    }
}