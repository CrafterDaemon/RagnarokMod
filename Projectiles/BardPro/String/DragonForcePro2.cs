using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class DragonForcePro2 : BardProjectile, ILocalizedModType
    {
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.BallofFire}";

        private int noHomeTimer = 0;
        private const int NoHomeTime = 10;

        private const int TrailLength = 12;
        private Vector2[] trailPositions = new Vector2[TrailLength];
        private bool trailInitialized = false;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginLeft = new Vector2(0, 0.5f);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void SetBardDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 180;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            if (!trailInitialized)
            {
                for (int i = 0; i < TrailLength; i++)
                    trailPositions[i] = Projectile.Center;
                trailInitialized = true;
            }

            for (int i = TrailLength - 1; i > 0; i--)
                trailPositions[i] = trailPositions[i - 1];
            trailPositions[0] = Projectile.Center;

            noHomeTimer++;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if (noHomeTimer > NoHomeTime)
            {
                NPC npc = Projectile.FindNearestNPC(900);
                if (npc != null)
                {
                    Projectile.HomeInOnTarget(npc, 22f, 0.2f);
                }
                else if (noHomeTimer > NoHomeTime + 40)
                {
                    Projectile.Kill();
                    return;
                }
            }

            // Deep orange embers
            if (Main.rand.NextBool(2))
            {
                Dust fire = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(6f, 6f),
                    DustID.Torch,
                    -Projectile.velocity * 0.15f + Main.rand.NextVector2Circular(1.5f, 1.5f),
                    80, default, Main.rand.NextFloat(1.0f, 1.8f));
                fire.noGravity = true;
            }

            // Dark smoke trail
            if (Main.rand.NextBool(3))
            {
                Dust smoke = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(4f, 4f),
                    DustID.Smoke,
                    -Projectile.velocity * 0.1f + new Vector2(0, Main.rand.NextFloat(-1f, -0.3f)),
                    120, new Color(50, 25, 10), Main.rand.NextFloat(1.2f, 2f));
                smoke.noGravity = true;
            }

            // Falling sparks
            if (Main.rand.NextBool(5))
            {
                Dust ember = Dust.NewDustPerfect(
                    Projectile.Center,
                    DustID.Torch,
                    -Projectile.velocity * 0.05f + Main.rand.NextVector2Unit() * Main.rand.NextFloat(0.5f, 2f),
                    0, default, Main.rand.NextFloat(0.6f, 1.2f));
                ember.noGravity = false;
            }

            // Deep warm lighting
            Lighting.AddLight(Projectile.Center, 0.8f, 0.3f, 0.05f);
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 180, false);

            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolumeScale(0.5f), Projectile.Center);

            // Fiery burst
            for (int i = 0; i < 15; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 9f);
                Dust burst = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, vel, 60, default, Main.rand.NextFloat(1.5f, 2.8f));
                burst.noGravity = true;
            }

            // Dark smoke puff
            for (int i = 0; i < 8; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 4f);
                Dust smoke = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, vel, 150, new Color(60, 30, 10), Main.rand.NextFloat(2f, 3.5f));
                smoke.noGravity = true;
            }

            // Gravity embers for ground scatter
            for (int i = 0; i < 6; i++)
            {
                Dust ember = Dust.NewDustPerfect(Projectile.Center, DustID.Torch,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 6f),
                    0, default, Main.rand.NextFloat(1f, 2f));
                ember.noGravity = false;
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!trailInitialized)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            Vector2 vortexOrigin = vortexNoise.Size() * 0.5f;
            float time = Main.GlobalTimeWrappedHourly;

            // === Trail vortexes ===
            Main.spriteBatch.EnterShaderRegion();

            for (int i = TrailLength - 1; i >= 1; i--)
            {
                Vector2 pos = trailPositions[i];
                if (pos == trailPositions[0] && i > 1)
                    continue;

                float t = i / (float)TrailLength;
                float alpha = (1f - t);
                alpha *= alpha;

                float scale = MathHelper.Lerp(0.15f, 0.06f, t);
                float spinSpeed = MathHelper.Lerp(1.5f, 0.6f, t);

                GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(alpha * 0.5f);
                GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 10, 5));
                GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 60, 15));
                GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

                for (int j = 0; j < 3; j++)
                {
                    float angle = MathHelper.TwoPi * j / 3f - time * MathHelper.TwoPi * spinSpeed;
                    Color drawColor = Color.White * alpha * 0.7f;
                    drawColor.A = 0;
                    Vector2 drawPosition = pos - Main.screenPosition + angle.ToRotationVector2() * 2f;

                    Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                        angle + MathHelper.PiOver2, vortexOrigin,
                        scale, SpriteEffects.None, 0);
                }
            }

            Main.spriteBatch.ExitShaderRegion();

            // === Head vortex ===
            Main.spriteBatch.EnterShaderRegion();

            float pulse = 1f + (float)Math.Sin(time * 10f) * 0.12f;

            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.8f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(220, 20, 5));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 70, 20));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f - time * MathHelper.TwoPi * 1.4f;
                Color drawColor = Color.White * 0.85f;
                drawColor.A = 0;
                Vector2 drawPosition = Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f;

                Main.EntitySpriteDraw(vortexNoise, drawPosition, null, drawColor,
                    angle + MathHelper.PiOver2, vortexOrigin,
                    0.25f * pulse, SpriteEffects.None, 0);
            }

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 6f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Torch, vel, 0, default, Main.rand.NextFloat(1f, 1.8f));
                d.noGravity = true;
            }

            for (int i = 0; i < 5; i++)
            {
                Dust smoke = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(1f, 3f),
                    120, new Color(50, 25, 10), Main.rand.NextFloat(1.5f, 2.5f));
                smoke.noGravity = true;
            }
        }
    }
}