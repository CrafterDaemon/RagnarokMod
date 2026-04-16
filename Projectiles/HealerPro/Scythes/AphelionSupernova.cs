using CalamityMod;
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

        private const float MaxRingRadius = 600f;
        private const float CoreMaxRadius = 120f;

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
                new Vector3(brightness * 1.2f, brightness * 0.8f, brightness * 0.1f));

            if (Timer >= TotalDuration)
                Projectile.Kill();
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

                float coreDrawScale = 400f / (noise.Width * 0.5f);

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
