using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Core;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    /// <summary>
    /// Aphelion supernova explosion.
    ///
    /// Timeline (frames at 60fps):
    ///   0  –  20  Compression  — core shrinks to point, instability ramps to 1
    ///   20 –  60  Ring burst   — shockwave expands 0→MaxRingRadius, single hit window
    ///   20 –  80  Ejecta       — dust erupts outward
    ///   60 – 180  Remnant      — glowing core fades
    ///   180– 240  Fade         — full fadeout
    ///
    /// Shaders:
    ///   AphelionNovaCore — globalTime, spinTime, instability, remnantFade, coreScale,
    ///                      mainColor, darkerColor
    ///
    ///   AphelionNovaRing — progress, opacity, noiseStrength, spinTime, ringColor
    /// </summary>
    public class AphelionSupernova : ModProjectile
    {
        public override string Texture =>
            "RagnarokMod/Projectiles/NoProj";

        private const string NoisePath =
            "RagnarokMod/Effects/Assets/MiscNoise";

        private static Texture2D NoiseTexture =>
            ModContent.Request<Texture2D>(NoisePath,
                AssetRequestMode.ImmediateLoad).Value;

        private static Effect CoreEffect => RagnarokShaders.AphelionNovaCore.Value;
        private static Effect RingEffect => RagnarokShaders.AphelionNovaRing.Value;

        private const int TotalDuration = 240;
        private const int RingStartFrame = 20;
        private const int RingEndFrame = 60;
        private const int EjectaEndFrame = 80;
        private const int RemnantEndFrame = 180;

        private const float MaxRingRadius = 900f;
        private const float CoreMaxRadius = 180f;

        private static readonly Color CoreFlash = new Color(220, 235, 255);
        private static readonly Color BrightCol = new Color(100, 195, 255);
        private static readonly Color DeepCol = new Color(60, 120, 255);

        private static readonly Vector3 MainColV3 = new Color(100, 195, 255).ToVector3();
        private static readonly Vector3 DarkColV3 = new Color(60, 120, 255).ToVector3();

        private float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        private float _spinTime;
        private bool _hasDamaged;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = TotalDuration + 5;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        private Vector2 _spawnCenter;
        private bool _centerSet;

        public override void AI()
        {
            Timer++;
            _spinTime += 0.03f;

            if (!_centerSet)
            {
                _spawnCenter = Projectile.Center;
                _centerSet = true;
            }

            Projectile.timeLeft = Math.Max(Projectile.timeLeft,
                TotalDuration - (int)Timer + 2);

            if (Timer >= RingStartFrame && Timer <= RingEndFrame)
            {
                float ringT = (Timer - RingStartFrame) / (float)(RingEndFrame - RingStartFrame);
                float ringRadius = ringT * MaxRingRadius;
                int size = (int)(ringRadius * 2f);
                Projectile.width = size;
                Projectile.height = size;
            }
            else
            {
                Projectile.width = 32;
                Projectile.height = 32;
            }

            Projectile.Center = _spawnCenter;

            float brightness = Timer < RingEndFrame
                ? MathHelper.Lerp(0.5f, 2.5f, Timer / (float)RingEndFrame)
                : MathHelper.Lerp(2.5f, 0.2f,
                    (Timer - RingEndFrame) / (float)(TotalDuration - RingEndFrame));

            Lighting.AddLight(Projectile.Center,
                new Vector3(brightness * 0.4f, brightness * 0.75f, brightness * 1.5f));

            if (Main.netMode != NetmodeID.Server)
                SpawnParticles();

            if (Timer >= TotalDuration)
                Projectile.Kill();
        }

        private void SpawnParticles()
        {
            bool photosen = CalamityClientConfig.Instance.Photosensitivity;

            if (Timer <= RingStartFrame)
            {
                float compressionT = Timer / (float)RingStartFrame;
                float orbScale = MathHelper.Lerp(1.2f, 0.1f, compressionT);

                float inwardRadius = MaxRingRadius * 0.2f * MathHelper.Lerp(1f, 0.3f, compressionT);
                for (int i = 0; i < 2; i++)
                {
                    Vector2 spawnOffset = Main.rand.NextVector2CircularEdge(inwardRadius, inwardRadius);
                    Vector2 vel = -spawnOffset.SafeNormalize(Vector2.Zero)
                                * Main.rand.NextFloat(MaxRingRadius * 0.007f, MaxRingRadius * 0.016f);

                    GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(
                        _spawnCenter + spawnOffset, vel,
                        Main.rand.NextFloat(0.2f, 0.4f) * orbScale,
                        Color.Lerp(BrightCol, CoreFlash, Main.rand.NextFloat()),
                        6));
                }

                if (!photosen)
                {
                    GeneralParticleHandler.SpawnParticle(new CustomPulse(
                        _spawnCenter, Vector2.Zero,
                        BrightCol * (0.3f + 0.7f * compressionT),
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), Main.rand.NextFloat(-5f, 5f),
                        orbScale * 1.2f, orbScale * 1.2f, 3));
                }
            }

            if ((int)Timer == RingStartFrame)
            {
                if (!photosen)
                {
                    GeneralParticleHandler.SpawnParticle(new CustomPulse(
                        _spawnCenter, Vector2.Zero, CoreFlash,
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), 0f, 4f, 3f, 18));

                    GeneralParticleHandler.SpawnParticle(new CustomPulse(
                        _spawnCenter, Vector2.Zero, BrightCol,
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), 0f, 3.5f, 2.5f, 18));
                }

                float ringScale = MaxRingRadius / 600f;
                for (int i = 0; i < 10; i++)
                {
                    Color ringCol = Color.Lerp(BrightCol, CoreFlash, Main.rand.NextFloat());
                    GeneralParticleHandler.SpawnParticle(new CustomPulse(
                        _spawnCenter, Vector2.Zero,
                        ringCol * 0.7f,
                        "CalamityMod/Particles/FlameExplosion",
                        new Vector2(1f, 1f),
                        Main.rand.NextFloat(-20f, 20f),
                        0f, (4f - i * 0.28f) * ringScale, 50));
                }

                float sparkSpeed = MaxRingRadius * 0.007f;
                Vector2 dirX = Vector2.UnitX * sparkSpeed;
                Vector2 dirY = Vector2.UnitY * sparkSpeed;

                if (!photosen)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        float sparkScale = 0.12f + i * 0.025f;
                        Color sparkCol = Color.Lerp(CoreFlash, BrightCol, i / 6f);

                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, dirX * (i + 1f), false, 14, sparkScale, sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, -dirX * (i + 1f), false, 14, sparkScale, sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, dirY * (i + 1f), false, 14, sparkScale, sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, -dirY * (i + 1f), false, 14, sparkScale, sparkCol, new Vector2(2.5f, 1.2f), true));
                    }
                }

                float sparkRadius = MaxRingRadius * 0.025f;
                float sparkVel = MaxRingRadius * 0.022f;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(sparkVel, sparkVel);
                    Color sparkCol = Main.rand.NextBool() ? BrightCol : CoreFlash;
                    GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                        _spawnCenter + Main.rand.NextVector2Circular(sparkRadius, sparkRadius),
                        vel, false, Main.rand.Next(35, 50),
                        Main.rand.NextFloat(0.04f, 0.09f),
                        sparkCol, new Vector2(0.3f, 1.5f)));
                }

                float smokeSpread = MaxRingRadius * 0.06f;
                for (int i = 0; i < 30; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(smokeSpread, smokeSpread)
                                * Main.rand.NextFloat(0.3f, 1f);
                    GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(
                        _spawnCenter + vel,
                        vel * 0.4f,
                        DeepCol * 0.7f,
                        Main.rand.Next(20, 32),
                        Main.rand.NextFloat(0.8f, 1.8f),
                        0.4f));
                }

                if (Main.LocalPlayer.Distance(_spawnCenter) < 1800f)
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10f;
            }

            if (Timer > RingStartFrame && Timer <= EjectaEndFrame)
            {
                float ejectaT = (Timer - RingStartFrame) / (float)(EjectaEndFrame - RingStartFrame);
                float spawnRate = MathHelper.Lerp(4f, 1f, ejectaT);

                if (Main.rand.NextFloat() < 1f / spawnRate)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(1f, 1f).SafeNormalize(Vector2.Zero)
                                * Main.rand.NextFloat(MaxRingRadius * 0.01f, MaxRingRadius * 0.03f)
                                * MathHelper.Lerp(1f, 0.4f, ejectaT);

                    Color ejectaCol = Color.Lerp(CoreFlash, BrightCol, Main.rand.NextFloat());

                    GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(
                        _spawnCenter + vel * 2f, vel * 0.3f,
                        Main.rand.NextFloat(0.15f, 0.35f) * (1f - ejectaT * 0.5f),
                        ejectaCol,
                        Main.rand.Next(25, 45)));
                }

                if (Main.rand.NextFloat() < 0.3f && !photosen)
                {
                    float ringRadius = (Timer - RingStartFrame)
                        / (float)(RingEndFrame - RingStartFrame) * MaxRingRadius;
                    Vector2 ringEdge = _spawnCenter
                        + Main.rand.NextVector2CircularEdge(ringRadius, ringRadius);
                    GeneralParticleHandler.SpawnParticle(new GenericSparkle(
                        ringEdge, Vector2.Zero,
                        CoreFlash, BrightCol,
                        Main.rand.NextFloat(0.5f, 1.2f), 8,
                        Main.rand.NextBool() ? 1.5f : -1.5f, 2));
                }
            }

            if ((int)Timer == 35)
            {
                GeneralParticleHandler.SpawnParticle(new StaticPulseRing(
                    _spawnCenter, Vector2.Zero, BrightCol * 0.35f,
                    new Vector2(1f, 1f), 0.1f, 0.6f, 0f, 30));
            }
            if ((int)Timer == 50)
            {
                GeneralParticleHandler.SpawnParticle(new StaticPulseRing(
                    _spawnCenter, Vector2.Zero, BrightCol * 0.25f,
                    new Vector2(1f, 1f), 0.1f, 0.5f, 0f, 22));
            }

            if (Timer > RingEndFrame && Timer <= RemnantEndFrame)
            {
                float remnantT = (Timer - RingEndFrame) / (float)(RemnantEndFrame - RingEndFrame);

                if (Main.rand.NextFloat() < MathHelper.Lerp(0.5f, 0.05f, remnantT))
                {
                    // Embers drift upward from around the core radius
                    Vector2 vel = new Vector2(
                        Main.rand.NextFloat(-1.2f, 1.2f),
                        Main.rand.NextFloat(-2f, -0.5f));

                    Color emberCol = Color.Lerp(BrightCol, CoreFlash, Main.rand.NextFloat())
                                   * (1f - remnantT * 0.7f);

                    GeneralParticleHandler.SpawnParticle(new SquishyLightParticle(
                        _spawnCenter + Main.rand.NextVector2Circular(CoreMaxRadius * 0.5f, CoreMaxRadius * 0.5f),
                        vel,
                        Main.rand.NextFloat(0.08f, 0.2f),
                        emberCol,
                        Main.rand.Next(20, 40)));
                }
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Timer < RingStartFrame || Timer > RingEndFrame)
                return false;

            float ringT = (Timer - RingStartFrame) / (float)(RingEndFrame - RingStartFrame);
            float ringRadius = ringT * MaxRingRadius;

            return CalamityUtils.CircularHitboxCollision(_spawnCenter, ringRadius, targetHitbox);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<MiracleBlight>(), 300);
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 300);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 300);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Timer <= 0) return false;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D noise = NoiseTexture;

            Vector2 center = _spawnCenter - Main.screenPosition;
            Vector2 origin = noise.Size() * 0.5f;

            float ringT = MathHelper.Clamp(
                (Timer - RingStartFrame) / (float)(RingEndFrame - RingStartFrame),
                0f, 1f);

            float fadeT = MathHelper.Clamp(
                (Timer - RemnantEndFrame) / (float)(TotalDuration - RemnantEndFrame),
                0f, 1f);

            float instability = MathHelper.Clamp(Timer / (float)RingStartFrame, 0f, 1f);
            float remnantFade = Timer < RemnantEndFrame ? 1f : 1f - fadeT;
            float remnantT = MathHelper.Clamp(
                (Timer - RingEndFrame) / (float)(RemnantEndFrame - RingEndFrame), 0f, 1f);

            float coreScale = Timer < RingStartFrame
                ? MathHelper.Lerp(1f, 0.08f, instability * instability)
                : MathHelper.Lerp(0.08f, 2f, remnantT) * (1f - fadeT * 0.8f);

            float pulse = 0.5f + 0.5f * MathF.Sin(Main.GameUpdateCount * 0.15f);
            float coreFade = Timer < RingStartFrame ? 1f
                : 1f - MathHelper.Clamp(
                    (Timer - RingStartFrame) / (float)(RingEndFrame - RingStartFrame), 0f, 1f);

            if (Timer < RingEndFrame)
            {
                Effect coreEffect = CoreEffect;

                coreEffect.Parameters["globalTime"].SetValue(Main.GlobalTimeWrappedHourly);
                coreEffect.Parameters["spinTime"].SetValue(_spinTime);
                coreEffect.Parameters["instability"].SetValue(instability);
                coreEffect.Parameters["remnantFade"].SetValue(coreFade);
                coreEffect.Parameters["coreScale"].SetValue(coreScale);
                coreEffect.Parameters["mainColor"].SetValue(MainColV3);
                coreEffect.Parameters["darkerColor"].SetValue(DarkColV3);

                float coreDrawScale = (CoreMaxRadius * coreScale) / (noise.Width * 0.5f);

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                    SamplerState.LinearWrap, DepthStencilState.None,
                    Main.Rasterizer, coreEffect,
                    Main.GameViewMatrix.TransformationMatrix);

                coreEffect.CurrentTechnique.Passes[0].Apply();
                sb.Draw(noise, center, null, Color.White,
                    0f, origin, coreDrawScale, SpriteEffects.None, 0f);
            }

            if (Timer >= RingStartFrame && Timer <= RingEndFrame + 10f)
            {
                float ringOpacity = MathHelper.SmoothStep(0f, 1f, Math.Min(ringT * 4f, 1f))
                                  * MathHelper.SmoothStep(1f, 0f, Math.Max(0f, ringT - 0.7f) / 0.3f);

                Color ringCol = Color.Lerp(CoreFlash, BrightCol, ringT);

                Effect ringEffect = RingEffect;

                ringEffect.Parameters["progress"].SetValue(ringT);
                ringEffect.Parameters["opacity"].SetValue(ringOpacity);
                ringEffect.Parameters["noiseStrength"].SetValue(0.05f + ringT * 0.03f);
                ringEffect.Parameters["spinTime"].SetValue(_spinTime);
                ringEffect.Parameters["ringColor"].SetValue(ringCol.ToVector3());

                // Quad half-width = noise.Width * 0.5f * ringDrawScale.
                // We want that to equal MaxRingRadius so the shader's progress=1
                // ring edge lands exactly at MaxRingRadius pixels from center.
                float ringDrawScale = MaxRingRadius / (noise.Width * 0.5f);

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                    SamplerState.LinearWrap, DepthStencilState.None,
                    Main.Rasterizer, ringEffect,
                    Main.GameViewMatrix.TransformationMatrix);

                ringEffect.CurrentTechnique.Passes[0].Apply();
                sb.Draw(noise, center, null, Color.White,
                    0f, origin, ringDrawScale, SpriteEffects.None, 0f);
            }

            // Restore SpriteBatch state
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}