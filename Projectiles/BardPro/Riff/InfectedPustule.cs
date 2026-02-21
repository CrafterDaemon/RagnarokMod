using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Riffs;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class InfectedPustule : BardProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Melee/DarkBall";

        private const int SwellTime = 60;          // frames to grow to full size
        private const int FireDuration = 240;       // how long it fires for
        private const int TotalLifetime = SwellTime + FireDuration;
        private const float MaxPustuleScale = 2.5f; // much larger than DarkBall
        private const float TargetSearchRadius = 500f;
        private const int FireRate = 8;             // frames between shots

        private int timer;
        private int fireTimer;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = TotalLifetime;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1; // no contact damage, fires projectiles instead
        }

        public override bool? CanDamage() => false; // damage comes from the SporeFire projectiles

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            timer++;

            float time = Main.GlobalTimeWrappedHourly;

            if (timer <= SwellTime)
            {
                // that's just swell
                float swellProgress = timer / (float)SwellTime;
                float baseScale = MathHelper.Lerp(0.3f, MaxPustuleScale, swellProgress);
                float throb = MathF.Sin(timer * 0.3f) * 0.1f * swellProgress;
                Projectile.scale = baseScale + throb;

                // Rotation slows as it settles
                Projectile.rotation += (1f - swellProgress) * 0.15f;

                // Growing seepage particles
                if (Main.rand.NextBool(3))
                {
                    Dust seep = Dust.NewDustPerfect(
                        Projectile.Center + Main.rand.NextVector2Circular(
                            Projectile.width * 0.4f * Projectile.scale,
                            Projectile.height * 0.4f * Projectile.scale),
                        DustID.CursedTorch,
                        new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1.5f, 0.2f)),
                        100, default, Main.rand.NextFloat(0.6f, 1.1f) * swellProgress);
                    seep.noGravity = true;
                }

                // Corruption ooze dripping
                if (Main.rand.NextBool(5) && swellProgress > 0.3f)
                {
                    Dust drip = Dust.NewDustPerfect(
                        Projectile.Center + new Vector2(Main.rand.NextFloat(-10f, 10f), Projectile.height * 0.3f * Projectile.scale),
                        DustID.CorruptionThorns,
                        new Vector2(0f, Main.rand.NextFloat(1f, 2f)),
                        100, default, 0.8f);
                    drip.noGravity = false;
                }
            }
            else
            {
                // fire in da hole
                float fireProgress = (timer - SwellTime) / (float)FireDuration;
                Projectile.scale = MaxPustuleScale;

                // Gentle throb while firing
                float throb = MathF.Sin(timer * 0.2f) * 0.06f;
                Projectile.scale += throb;

                // Slow idle rotation
                Projectile.rotation += 0.01f;

                // Find nearest enemy
                int targetIndex = FindNearestEnemy();

                if (targetIndex >= 0 && Main.myPlayer == Projectile.owner)
                {
                    NPC target = Main.npc[targetIndex];

                    fireTimer++;
                    if (fireTimer >= FireRate)
                    {
                        fireTimer = 0;
                        FireAtTarget(target);
                    }

                    // Aiming particles — wisps drift toward target
                    if (Main.rand.NextBool(3))
                    {
                        Vector2 toTarget = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                        Dust aim = Dust.NewDustPerfect(
                            Projectile.Center + toTarget * Projectile.width * Projectile.scale * 0.5f
                                + Main.rand.NextVector2Circular(6f, 6f),
                            DustID.CursedTorch,
                            toTarget * Main.rand.NextFloat(2f, 5f),
                            80, default, Main.rand.NextFloat(0.8f, 1.3f));
                        aim.noGravity = true;
                    }
                }

                // Ambient corruption particles
                if (Main.rand.NextBool(3))
                {
                    int dustType = Main.rand.NextBool() ? DustID.CursedTorch : DustID.CorruptionThorns;
                    Dust ambient = Dust.NewDustPerfect(
                        Projectile.Center + Main.rand.NextVector2Circular(
                            Projectile.width * 0.5f * Projectile.scale,
                            Projectile.height * 0.5f * Projectile.scale),
                        dustType,
                        new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-1f, 0.3f)),
                        120, default, Main.rand.NextFloat(0.6f, 1.2f));
                    ambient.noGravity = true;
                }

                // Fade and die near the end
                if (fireProgress > 0.85f)
                {
                    float fadeRatio = (fireProgress - 0.85f) / 0.15f;
                    Projectile.alpha = (int)(255 * fadeRatio);
                    Projectile.scale = MaxPustuleScale * (1f - fadeRatio * 0.5f);
                }
            }

            Lighting.AddLight(Projectile.Center,
                0.05f * Projectile.scale,
                0.25f * Projectile.scale,
                0.03f * Projectile.scale);
        }

        private int FindNearestEnemy()
        {
            int closest = -1;
            float closestDist = TargetSearchRadius;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                float dist = Vector2.Distance(npc.Center, Projectile.Center);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = i;
                }
            }

            return closest;
        }

        private void FireAtTarget(NPC target)
        {
            Vector2 toTarget = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);

            // Add slight spread for a flamethrower feel
            float spread = MathHelper.ToRadians(Main.rand.NextFloat(-8f, 8f));
            toTarget = toTarget.RotatedBy(spread);

            float speed = Main.rand.NextFloat(10f, 14f);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center + toTarget * Projectile.width * Projectile.scale * 0.4f,
                toTarget * speed,
                ModContent.ProjectileType<InfectedSporeFire>(),
                Projectile.damage,
                Projectile.knockBack * 0.5f,
                Projectile.owner
            );

            // Muzzle flash
            for (int i = 0; i < 3; i++)
            {
                Dust flash = Dust.NewDustPerfect(
                    Projectile.Center + toTarget * Projectile.width * Projectile.scale * 0.5f,
                    DustID.CursedTorch,
                    toTarget * Main.rand.NextFloat(3f, 7f) + Main.rand.NextVector2Circular(1.5f, 1.5f),
                    0, default, Main.rand.NextFloat(1.2f, 2f));
                flash.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Vector2 origin = texture.Size() * 0.5f;
            float time = Main.GlobalTimeWrappedHourly;
            float alpha = 1f - Projectile.alpha / 255f;

            // STATE: BEGUN (vanilla)
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)


            // Outer corruption glow
            float pulse = 1f + MathF.Sin(time * 4f) * 0.1f;
            float glowSize = Projectile.width * Projectile.scale * 3f * pulse;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(20, 50, 10) * 0.1f * alpha,
                0f, PixelOriginCenter, new Vector2(glowSize), SpriteEffects.None, 0f);

            // Inner cursed glow
            float innerSize = Projectile.width * Projectile.scale * 1.5f * pulse;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(40, 100, 15) * 0.15f * alpha,
                0f, PixelOriginCenter, new Vector2(innerSize), SpriteEffects.None, 0f);

            // Second pass slightly offset for a pulsing double-vision
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                null, new Color(30, 80, 15) * 0.3f * alpha,
                Projectile.rotation + 0.1f, origin, Projectile.scale * 1.1f * pulse, SpriteEffects.None, 0);

            // STATE: BEGUN (additive)
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal)

            // DoGPortal vortex
            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.5f * alpha);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(20, 40, 10));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(80, 100, 30));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 3; i++)
            {
                float angle = MathHelper.TwoPi * i / 3f + time * MathHelper.TwoPi * 0.2f;
                Color dc = Color.White * 0.5f * alpha;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise,
                    Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 2f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    Projectile.scale * 0.25f, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal)

            // Solid DarkBall sprite
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition,
                null, lightColor * alpha,
                Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0);

            // STATE: BEGUN (normal)
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath21 with { Pitch = -0.3f, Volume = 0.7f }, Projectile.Center);

            // Corruption burst
            for (int i = 0; i < 15; i++)
            {
                float angle = MathHelper.TwoPi * i / 15f;
                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(3f, 8f);
                int dustType = Main.rand.NextBool() ? DustID.Demonite : DustID.CursedTorch;
                Dust d = Dust.NewDustPerfect(Projectile.Center, dustType, vel, 80, default, Main.rand.NextFloat(1.2f, 2.2f));
                d.noGravity = true;
            }

            // Gunk splatter
            for (int i = 0; i < 8; i++)
            {
                Dust gunk = Dust.NewDustPerfect(Projectile.Center, DustID.CorruptionThorns,
                    new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-6f, -1f)),
                    100, default, Main.rand.NextFloat(1f, 1.8f));
                gunk.noGravity = false;
            }
        }
    }
}