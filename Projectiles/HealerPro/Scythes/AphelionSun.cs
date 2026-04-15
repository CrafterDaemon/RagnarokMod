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
            "RagnarokMod/Effects/Assets/MiscNoise";

        private static Texture2D HeatMap =>
            ModContent.Request<Texture2D>(HeatMapPath,
                AssetRequestMode.ImmediateLoad).Value;

        private static Effect SunEffect => RagnarokShaders.AphelionSun.Value;

        private const float OrbitRadius = 300f;
        private const float SpawnInFrames = 30f;

        private const float QuadHalfSize = 90f;

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
        private float SpawnProgress => Math.Min(SpawnTimer / SpawnInFrames, 1f);

        // Independent spin time advances proportional to orbital speed so the surface appears to rotate with the orbit rather than at a fixed real-time rate.
        private float _spinTime;

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
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            // Parent check
            int pid = (int)Projectile.ai[0];
            bool parentAlive = pid >= 0
                            && pid < Main.maxProjectiles
                            && Main.projectile[pid].active
                            && Main.projectile[pid].type ==
                               ModContent.ProjectileType<AphelionPro>();

            if (!parentAlive) { Projectile.Kill(); return; }

            Projectile parent = Main.projectile[pid];
            Player owner = Main.player[Projectile.owner];

            Projectile.timeLeft = 3;
            SpawnTimer = Math.Min(SpawnTimer + 1f, SpawnInFrames);

            // Orbital speed from parent charge
            float chargeT = Math.Min(
                parent.ai[0] / AphelionPro.MaxChargeFrames, 0.01f);
            float radsPerFrame = MathHelper.Lerp(0.08f, 0.55f,
                MathF.Pow(chargeT, 0.55f));

            PhaseAngle += radsPerFrame;
            // Spin time advances slower than the orbit so the surface texture doesn't streak.
            _spinTime += radsPerFrame * 0.35f;

            // Orbit position
            float cursorAngle = (Main.MouseWorld - owner.Center).ToRotation();
            float orbitAngle = PhaseAngle + Projectile.ai[1] + cursorAngle;

            Projectile.Center = owner.Center
                                + orbitAngle.ToRotationVector2() * OrbitRadius;
            Projectile.rotation = orbitAngle;

            // Lighting
            float lum = SpawnProgress;
            Lighting.AddLight(Projectile.Center,
                new Vector3(lum * 1.3f, lum * 0.8f, lum * 0.15f));

            // Surface dust
            if (Main.rand.NextBool(3) && SpawnProgress > 0.5f)
            {
                float a = Main.rand.NextFloat(MathHelper.TwoPi);
                float spd = Main.rand.NextFloat(1.5f, 4f);
                Vector2 vel = a.ToRotationVector2() * spd;
                int d = Dust.NewDust(Projectile.position,
                    Projectile.width, Projectile.height,
                    DustID.Torch, vel.X, vel.Y);
                if (d >= 0 && d < Main.maxDust)
                {
                    Main.dust[d].noGravity = true;
                    Main.dust[d].scale = Main.rand.NextFloat(0.9f, 1.8f)
                                          * SpawnProgress;
                    Main.dust[d].color = Color.Lerp(
                        new Color(255, 180, 50),
                        new Color(255, 255, 200),
                        Main.rand.NextFloat());
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (SpawnProgress <= 0f)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Effect effect = SunEffect;
            Texture2D heat = HeatMap;

            // Sun colors
            Vector3 mainCol = new Color(255, 215, 100).ToVector3();
            Vector3 darkCol = new Color(200, 80, 20).ToVector3();
            Vector3 subtractAccent = new Color(180, 0, 0).ToVector3();

            float pulse = 0.5f + 0.5f * MathF.Sin(Main.GameUpdateCount * 0.04f);
            float coronaIntensity = SpawnProgress * (0.05f + pulse * 0.08f);

            effect.Parameters["globalTime"].SetValue(Main.GlobalTimeWrappedHourly);
            effect.Parameters["sphereSpinTime"].SetValue(_spinTime);
            effect.Parameters["spawnProgress"].SetValue(SpawnProgress);
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
            float scale = QuadHalfSize / (heat.Width * 0.5f);

            // Pass 1: AlphaBlend opaque disc + corona shape
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                SamplerState.LinearWrap, DepthStencilState.None,
                Main.Rasterizer, effect,
                Main.GameViewMatrix.TransformationMatrix);

            effect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(heat, drawPos, null, Color.White * SpawnProgress,
                0f, origin, scale*3, SpriteEffects.None, 0f);

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
            effect.Parameters["spawnProgress"].SetValue(SpawnProgress * 0.5f);
            effect.CurrentTechnique.Passes[0].Apply();
            sb.Draw(heat, drawPos, null, Color.White * (SpawnProgress * 0.5f),
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
            for (int i = 0; i < 24; i++)
            {
                float a = Main.rand.NextFloat(MathHelper.TwoPi);
                float spd = Main.rand.NextFloat(2f, 9f);
                Vector2 vel = a.ToRotationVector2() * spd;
                int d = Dust.NewDust(Projectile.position,
                    Projectile.width, Projectile.height,
                    DustID.Torch, vel.X, vel.Y);
                if (d >= 0 && d < Main.maxDust)
                {
                    Main.dust[d].noGravity = true;
                    Main.dust[d].scale = Main.rand.NextFloat(1.4f, 2.8f);
                }
            }
        }
    }
}