using CalamityMod;
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
    ///   20 –  60  Ring burst   — shockwave expands 0→600px, single hit window
    ///   20 –  80  Ejecta       — dust erupts outward
    ///   60 – 180  Remnant      — glowing core fades
    ///   180– 240  Fade         — full fadeout
    ///
    /// Shaders:
    ///   AphelionNovaCore — PackedA(globalTime,spinTime,instability,remnantFade)
    ///                      PackedB(coreScale,0,0,0)
    ///                      mainColor, darkerColor (float4)
    ///
    ///   AphelionNovaRing — PackedA(progress,opacity,noiseStrength,spinTime)
    ///                      PackedB(ringColor.r,ringColor.g,ringColor.b,0)
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

        private float MaxRingRadius = 540f;
        private float CoreMaxRadius = 180f;

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
            Projectile.localNPCHitCooldown = TotalDuration * 2;
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

            bool ringActive = Timer >= RingStartFrame && Timer <= RingEndFrame;
            if (ringActive)
            {
                float ringT = (Timer - RingStartFrame)
                                 / (float)(RingEndFrame - RingStartFrame);
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
                new Vector3(brightness * 0.6f, brightness * 0.8f, brightness * 1.5f));

            SpawnParticles();

            if (Timer >= TotalDuration)
                Projectile.Kill();
        }

        private void SpawnParticles()
        {
            bool photosen = CalamityClientConfig.Instance.Photosensitivity;

            // Compression phase
            if (Timer <= RingStartFrame)
            {
                float compressionT = Timer / (float)RingStartFrame;
                float orbScale = MathHelper.Lerp(1.2f, 0.1f, compressionT);

                // Inward-streaming energy particles
                for (int i = 0; i < 2; i++)
                {
                    Vector2 spawnOffset = Main.rand.NextVector2CircularEdge(180f, 180f)
                                        * MathHelper.Lerp(1f, 0.3f, compressionT);
                    Vector2 vel = (_spawnCenter - (_spawnCenter + spawnOffset))
                        .SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(6f, 14f);

                    SquishyLightParticle energy = new(
                        _spawnCenter + spawnOffset, vel,
                        Main.rand.NextFloat(0.2f, 0.4f) * orbScale,
                        Color.Lerp(new Color(180, 220, 255), Color.White, Main.rand.NextFloat()),
                        6);
                    GeneralParticleHandler.SpawnParticle(energy);
                }

                // Core bloom
                if (!photosen)
                {
                    Particle bloom = new CustomPulse(
                        _spawnCenter, Vector2.Zero,
                        new Color(160, 200, 255) * (0.3f + 0.7f * compressionT),
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), Main.rand.NextFloat(-5f, 5f),
                        orbScale * 1.2f, orbScale * 1.2f, 3);
                    GeneralParticleHandler.SpawnParticle(bloom);
                }
            }

            // Ring burst frame
            if ((int)Timer == RingStartFrame)
            {
                // white flash
                if (!photosen)
                {
                    Particle flash = new CustomPulse(
                        _spawnCenter, Vector2.Zero, Color.White,
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), 0f, 4f, 3f, 18);
                    GeneralParticleHandler.SpawnParticle(flash);

                    Particle flashBlue = new CustomPulse(
                        _spawnCenter, Vector2.Zero, new Color(180, 210, 255),
                        "CalamityMod/Particles/LargeBloom",
                        new Vector2(1f, 1f), 0f, 3.5f, 2.5f, 18);
                    GeneralParticleHandler.SpawnParticle(flashBlue);
                }

                for (int i = 0; i < 10; i++)
                {
                    Color ringCol = Color.Lerp(
                        new Color(180, 220, 255),
                        Color.White,
                        Main.rand.NextFloat());

                    Particle pulse2 = new CustomPulse(
                        _spawnCenter, Vector2.Zero,
                        ringCol * 0.7f,
                        "CalamityMod/Particles/FlameExplosion",
                        new Vector2(1f, 1f),
                        Main.rand.NextFloat(-20f, 20f),
                        0f, 1f - i * 0.07f, 50);
                    GeneralParticleHandler.SpawnParticle(pulse2);
                }

                // Directional cross-spark ejecta
                Vector2 dirX = Vector2.UnitX * 5f;
                Vector2 dirY = Vector2.UnitY * 5f;

                if (!photosen)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        float sparkScale = 0.12f + i * 0.025f;
                        Color sparkCol = Color.Lerp(Color.White, new Color(140, 190, 255), i / 6f);

                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, dirX * (i + 1f), false, 14, sparkScale * 1.5f,
                            sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, -dirX * (i + 1f), false, 14, sparkScale * 1.5f,
                            sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, dirY * (i + 1f), false, 14, sparkScale * 1.5f,
                            sparkCol, new Vector2(2.5f, 1.2f), true));
                        GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                            _spawnCenter, -dirY * (i + 1f), false, 14, sparkScale * 1.5f,
                            sparkCol, new Vector2(2.5f, 1.2f), true));
                    }
                }

                // Scattered radial sparks
                for (int i = 0; i < 20; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(18f, 18f);
                    Color sparkCol = Main.rand.NextBool()
                        ? new Color(200, 230, 255)
                        : Color.White;
                    GeneralParticleHandler.SpawnParticle(new GlowSparkParticle(
                        _spawnCenter + Main.rand.NextVector2Circular(20f, 20f),
                        vel, false, Main.rand.Next(35, 50),
                        Main.rand.NextFloat(0.04f, 0.09f),
                        sparkCol, new Vector2(0.3f, 1.5f)));
                }

                // Heavy smoke for the dissipating cloud
                for (int i = 0; i < 30; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(25f, 25f)
                                * Main.rand.NextFloat(0.3f, 1f);
                    GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(
                        _spawnCenter + vel,
                        vel * 0.4f,
                        new Color(60, 80, 140) * 0.7f,
                        Main.rand.Next(20, 32),
                        Main.rand.NextFloat(0.8f, 1.8f),
                        0.4f));
                }

                // Screen shake
                if (Main.LocalPlayer.Distance(_spawnCenter) < 1800f)
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 10f;
            }

            // Ejecta phase
            // Debris streaming outward as the ring expands.
            if (Timer > RingStartFrame && Timer <= EjectaEndFrame)
            {
                float ejectaT = (Timer - RingStartFrame) / (float)(EjectaEndFrame - RingStartFrame);
                float spawnRate = MathHelper.Lerp(4f, 1f, ejectaT); // fewer as it fades

                if (Main.rand.NextFloat() < 1f / spawnRate)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(1f, 1f).SafeNormalize(Vector2.Zero)
                                * Main.rand.NextFloat(8f, 22f)
                                * MathHelper.Lerp(1f, 0.4f, ejectaT);

                    Color ejectaCol = Color.Lerp(
                        new Color(200, 230, 255),
                        new Color(120, 160, 255),
                        Main.rand.NextFloat());

                    SquishyLightParticle debris = new(
                        _spawnCenter + vel * 2f, vel * 0.3f,
                        Main.rand.NextFloat(0.15f, 0.35f) * (1f - ejectaT * 0.5f),
                        ejectaCol,
                        Main.rand.Next(8, 16));
                    GeneralParticleHandler.SpawnParticle(debris);
                }

                // sparkle bursts along the ring edge
                if (Main.rand.NextFloat() < 0.3f && !photosen)
                {
                    float ringRadius = (Timer - RingStartFrame)
                        / (float)(RingEndFrame - RingStartFrame) * MaxRingRadius;
                    Vector2 ringEdge = _spawnCenter
                        + Main.rand.NextVector2CircularEdge(ringRadius, ringRadius);
                    GeneralParticleHandler.SpawnParticle(new GenericSparkle(
                        ringEdge, Vector2.Zero,
                        Color.White, new Color(160, 200, 255),
                        Main.rand.NextFloat(0.5f, 1.2f), 8,
                        Main.rand.NextBool() ? 1.5f : -1.5f, 2));
                }
            }

            // Mid-ring pulse rings
            if ((int)Timer == 35)
            {
                GeneralParticleHandler.SpawnParticle(new StaticPulseRing(
                    _spawnCenter, Vector2.Zero, new Color(160, 200, 255) * 0.35f,
                    new Vector2(1f, 1f), 0.1f, 0.6f, 0f, 30));
            }
            if ((int)Timer == 50)
            {
                GeneralParticleHandler.SpawnParticle(new StaticPulseRing(
                    _spawnCenter, Vector2.Zero, new Color(180, 220, 255) * 0.25f,
                    new Vector2(1f, 1f), 0.1f, 0.5f, 0f, 22));
            }

            // Remnant phase
            // ember drift as the core glows down.
            if (Timer > RingEndFrame && Timer <= RemnantEndFrame)
            {
                float remnantT = (Timer - RingEndFrame) / (float)(RemnantEndFrame - RingEndFrame);

                if (Main.rand.NextFloat() < MathHelper.Lerp(0.5f, 0.05f, remnantT))
                {
                    Vector2 vel = new Vector2(
                        Main.rand.NextFloat(-1.2f, 1.2f),
                        Main.rand.NextFloat(-2f, -0.5f)); // drift upward like embers

                    Color emberCol = Color.Lerp(
                        new Color(180, 210, 255),
                        new Color(220, 240, 255),
                        Main.rand.NextFloat()) * (1f - remnantT * 0.7f);

                    SquishyLightParticle ember = new(
                        _spawnCenter + Main.rand.NextVector2Circular(30f, 30f),
                        vel,
                        Main.rand.NextFloat(0.08f, 0.2f),
                        emberCol,
                        Main.rand.Next(20, 40));
                    GeneralParticleHandler.SpawnParticle(ember);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Timer < RingStartFrame || Timer > RingEndFrame || _hasDamaged)
                return false;

            float ringT = (Timer - RingStartFrame)
                             / (float)(RingEndFrame - RingStartFrame);
            float ringRadius = ringT * MaxRingRadius;
            float ringWidth = (0.08f + ringT * 0.04f) * MaxRingRadius;
            float dist = Vector2.Distance(
                targetHitbox.Center.ToVector2(), Projectile.Center);

            return dist >= ringRadius - ringWidth && dist <= ringRadius + ringWidth * 0.5f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            _hasDamaged = true;
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
            float compressionT = instability;
            float remnantT = MathHelper.Clamp(
                (Timer - RingEndFrame) / (float)(RemnantEndFrame - RingEndFrame), 0f, 1f);

            float coreScale = Timer < RingStartFrame
                ? MathHelper.Lerp(1f, 0.08f, compressionT * compressionT)
                : MathHelper.Lerp(0.08f, 2f, remnantT) * (1f - fadeT * 0.8f);

            float pulse = 0.5f + 0.5f * MathF.Sin(Main.GameUpdateCount * 0.15f);
            float pulsedFade = remnantFade * (0.7f + 0.3f * pulse);
            float coreFade = Timer < RingStartFrame ? 1f
                : 1f - MathHelper.Clamp((Timer - RingStartFrame) / (float)(RingEndFrame - RingStartFrame), 0f, 1f);
            bool drawCore = Timer < RingEndFrame;

            if (drawCore)
            {
                Effect coreEffect = CoreEffect;

                coreEffect.Parameters["globalTime"].SetValue(Main.GlobalTimeWrappedHourly);
                coreEffect.Parameters["spinTime"].SetValue(_spinTime);
                coreEffect.Parameters["instability"].SetValue(instability);
                coreEffect.Parameters["remnantFade"].SetValue(coreFade);
                coreEffect.Parameters["coreScale"].SetValue(coreScale);
                coreEffect.Parameters["mainColor"].SetValue(
                    new Color(200, 180, 100).ToVector3());
                coreEffect.Parameters["darkerColor"].SetValue(
                    new Color(150, 70, 20).ToVector3());

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
                float ringOpacity = MathHelper.SmoothStep(0f, 1f,
                        Math.Min(ringT * 4f, 1f))
                    * MathHelper.SmoothStep(1f, 0f,
                        Math.Max(0f, ringT - 0.7f) / 0.3f);

                Color ringCol = Color.Lerp(
                    new Color(255, 240, 180),
                    new Color(180, 120, 255),
                    ringT);

                Effect ringEffect = RingEffect;

                ringEffect.Parameters["progress"].SetValue(ringT);
                ringEffect.Parameters["opacity"].SetValue(ringOpacity);
                ringEffect.Parameters["noiseStrength"].SetValue(0.05f + ringT * 0.03f);
                ringEffect.Parameters["spinTime"].SetValue(_spinTime);
                ringEffect.Parameters["ringColor"].SetValue(ringCol.ToVector3());

                float ringDrawScale = MaxRingRadius / (noise.Width * 0.33f);

                sb.End();
                sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                    SamplerState.LinearWrap, DepthStencilState.None,
                    Main.Rasterizer, ringEffect,
                    Main.GameViewMatrix.TransformationMatrix);

                ringEffect.CurrentTechnique.Passes[0].Apply();
                sb.Draw(noise, center, null, Color.White,
                    0f, origin, ringDrawScale, SpriteEffects.None, 0f);
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
