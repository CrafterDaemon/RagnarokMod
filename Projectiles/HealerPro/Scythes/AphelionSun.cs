using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
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
using ThoriumMod.Buffs.Healer;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    /// <summary>
    /// Orbiting miniature sun drawn with AphelionSun.fx.
    ///
    /// s0: CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak
    ///     Bound as the SpriteBatch draw texture serves as both the
    ///     stellar heat map and the corona angular modulation map.
    ///
    /// ai[0] = parent whoAmI (AphelionPro)
    /// ai[1] = orbital phase offset (0 or Pi)
    ///
    /// localAI[0] = accumulated phase angle
    /// localAI[1] = spawn-in timer (0 -> SpawnInFrames)
    /// </summary>
    public class AphelionSun : ModProjectile
    {
        public override string Texture =>
            "RagnarokMod/Projectiles/NoProj";
        private const string HeatMapPath =
            "CalamityMod/ExtraTextures/GreyscaleGradients/Neurons";

        private static Texture2D HeatMap =>
            ModContent.Request<Texture2D>(HeatMapPath,
                AssetRequestMode.ImmediateLoad).Value;

        private static Effect SunEffect => RagnarokShaders.AphelionSun.Value;

        private const float OrbitRadius = 250f;
        private const float SpawnInFrames = 30f;

        private const float QuadHalfSize = 90f; 
        private const float BaseOrbitSpeed = 0.012f;
        private const int LingerFrames = 60;
        private const int ShrinkFrames = 90;

        private float PhaseAngle
        {
            get => Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }
        private float SpawnTimer
        {
            get => Projectile.localAI[1];
            set => Projectile.localAI[1] = value;
        }
        private bool Collapsing => Projectile.ai[2] == 1f;
        private float _collapseTimer;
        private float SpawnProgress => Math.Min(SpawnTimer / SpawnInFrames, 1f);
        private bool _dying;
        private int _dyingTimer;
        private bool _supernovaReady;
        // Independent spin time advances proportional to orbital speed so the surface appears to rotate with the orbit rather than at a fixed real-time rate.
        private float _spinTime;
        private float CurrentOrbitRadius => _dying
            ? MathHelper.Lerp(OrbitRadius, OrbitRadius * 0.6f,
            MathHelper.Clamp((_dyingTimer - LingerFrames) / (float)ShrinkFrames, 0f, 1f))
            : OrbitRadius;

        private float LingerShrinkAlpha => _dyingTimer < LingerFrames ? 1f
            : 1f - MathHelper.Clamp((_dyingTimer - LingerFrames) / (float)ShrinkFrames, 0f, 1f);

        public override void SetDefaults()
        {
            Projectile.width = (int)(QuadHalfSize * 2f);
            Projectile.height = (int)(QuadHalfSize * 2f);
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
        }
        public override void AI()
        {
            int pid = (int)Projectile.ai[0];
            bool parentAlive = pid >= 0 && pid < Main.maxProjectiles
                            && Main.projectile[pid].active
                            && Main.projectile[pid].type == ModContent.ProjectileType<AphelionPro>();

            Player owner = Main.player[Projectile.owner];
            SpawnTimer = Math.Min(SpawnTimer + 1f, SpawnInFrames);

            if (_dying)
            {
                _dyingTimer++;

                Projectile.scale -= 0.1f;
                if (Projectile.scale <= 0)
                {
                    Projectile.Kill();
                }

                Projectile.timeLeft = 3;
                PhaseAngle += BaseOrbitSpeed;
                float orbitAngle = (PhaseAngle + Projectile.ai[1]
                                    + (Main.screenPosition - owner.Center).ToRotation());
                Projectile.Center = owner.Center
                                    + orbitAngle.ToRotationVector2() * CurrentOrbitRadius;
                Projectile.rotation = orbitAngle;
                _spinTime += BaseOrbitSpeed * 0.2f;
                if (Projectile.owner == Main.myPlayer && Main.mouseRight)
                {
                    _dying = false;
                    Projectile.ai[2] = 1f;
                }
                if (_dyingTimer >= LingerFrames + ShrinkFrames)
                    Projectile.Kill();
                return;
            }

            if (Collapsing)
            {
                _collapseTimer++;
                Projectile.timeLeft = 3;
                float t = Math.Min(_collapseTimer / 180f, 1f);

                float spiralSpeed = MathHelper.Lerp(BaseOrbitSpeed * 3f, 1f, MathF.Pow(t, 2f)) * owner.direction;
                float inwardT = MathF.Pow(t, 4f);
                float spiralRadius = MathHelper.Lerp(OrbitRadius, 0f, inwardT);

                PhaseAngle += spiralSpeed;
                _spinTime += spiralSpeed * 0.5f;

                float orbitAngle = PhaseAngle + Projectile.ai[1]
                                 + (Main.screenPosition - owner.Center).ToRotation();
                Projectile.Center = owner.Center + orbitAngle.ToRotationVector2() * spiralRadius;
                Projectile.rotation = orbitAngle;

                Lighting.AddLight(Projectile.Center, new Vector3(1.5f, 0.9f, 0.2f));

                if (spiralRadius < 20f || _collapseTimer >= 180f)
                {
                    bool isPrimary = Projectile.ai[1] == 0f;

                    if (isPrimary && Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        if (Main.LocalPlayer.Distance(owner.Center) < 2000f)
                            Main.LocalPlayer.Calamity().GeneralScreenShakePower = 15f;

                        SoundEngine.PlaySound(SoundID.Item62 with
                        {
                            Pitch = -0.25f,
                            Volume = 2f
                        }, owner.Center);

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            owner.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<AphelionSupernova>(),
                            Projectile.damage * 5,
                            10f,
                            Projectile.owner);
                    }

                    // Still signal parent if alive, so it can clean up vortex etc.
                    if (pid >= 0 && pid < Main.maxProjectiles)
                    {
                        Projectile parent = Main.projectile[pid];
                        if (parent.active && parent.type == ModContent.ProjectileType<AphelionPro>())
                            parent.localAI[2] += 1f;
                    }

                    Projectile.Kill();
                }
                return;
            }

            if (!parentAlive) { _dying = true; return; }

            Projectile.timeLeft = 3;

            PhaseAngle += BaseOrbitSpeed * owner.direction;
            _spinTime += BaseOrbitSpeed * 0.2f;
            float cursor = (Main.screenPosition - owner.Center).ToRotation();
            float orbit = PhaseAngle + Projectile.ai[1] + cursor;
            Projectile.Center = owner.Center + orbit.ToRotationVector2() * OrbitRadius;
            Projectile.rotation = orbit;
            Lighting.AddLight(Projectile.Center, new Vector3(1.3f, 0.8f, 0.15f) * SpawnProgress);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 600, false);
            target.AddBuff(ModContent.BuffType<GodSlayerInferno>(), 600, false);
            target.AddBuff(ModContent.BuffType<Daybroken>(), 600, false);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (SpawnProgress <= 0f)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Effect effect = SunEffect;
            Texture2D heat = HeatMap;

            // Sun colors
            Vector3 mainCol = new Color(180, 225, 255).ToVector3();
            Vector3 darkCol = new Color(150, 185, 255).ToVector3();
            Vector3 subtractAccent = new Color(100, 120, 255).ToVector3();

            float pulse = 0.1f * MathF.Sin(Main.GameUpdateCount * 0.04f);
            float coronaIntensity = 0.3f + Math.Abs(pulse);

            effect.Parameters["globalTime"].SetValue(Main.GlobalTimeWrappedHourly);
            effect.Parameters["spinTime"].SetValue(_spinTime % MathHelper.TwoPi);
            effect.Parameters["coronaIntensityFactor"].SetValue(coronaIntensity);
            effect.Parameters["mainColor"].SetValue(mainCol);
            effect.Parameters["darkerColor"].SetValue(darkCol);
            effect.Parameters["subtractiveAccentFactor"].SetValue(subtractAccent);

            Main.graphics.GraphicsDevice.Textures[1] = heat;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            Main.graphics.GraphicsDevice.Textures[2] = heat;
            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = heat.Size() * 0.5f;
            Vector2 scale = new Vector2((QuadHalfSize / heat.Width) * Projectile.scale, (QuadHalfSize / heat.Height) * Projectile.scale);

            // Pass 1: AlphaBlend opaque disc + corona shape
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearWrap, DepthStencilState.None,
                Main.Rasterizer, effect,
                Main.GameViewMatrix.TransformationMatrix);

            effect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(heat, drawPos, null, Color.Blue * SpawnProgress,
                0f, origin, scale * 3, SpriteEffects.None, 0f);

            // Pass 2: Additive corona bloom overdrawn at larger scale
            // Only the corona region (outside the disc) matters here;
            // the disc center is already saturated so additive stacking
            // just blooms the edges into the scene.
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                SamplerState.LinearWrap, DepthStencilState.None,
                Main.Rasterizer, effect,
                Main.GameViewMatrix.TransformationMatrix);

            effect.Parameters["coronaIntensityFactor"].SetValue(coronaIntensity * 1.4f);
            effect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(heat, drawPos, null, Color.Blue * (SpawnProgress * 0.5f),
                0f, origin, scale * 3f, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
        public override void OnKill(int timeLeft)
        {
            if (Main.netMode == NetmodeID.Server) return;
        }
    }
}