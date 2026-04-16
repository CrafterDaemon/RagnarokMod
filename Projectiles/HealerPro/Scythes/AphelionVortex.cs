using CalamityMod.Graphics.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Core;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    /// <summary>
    /// Purely visual swirling vortex drawn with Calamity's ExoVortex shader.
    ///
    ///   var shader = GameShaders.Misc["CalamityMod:ExoVortex"];
    ///   shader.UseColor(...)          // sets uColor
    ///         .UseSecondaryColor(...) // sets uSecondaryColor
    ///         .UseOpacity(...)        // sets uOpacity
    ///         .UseImage(tex, 0, ...)  // sets uImage0 (s0)
    ///         .Apply(drawData);       // uploads all uniforms + applies
    ///   spriteBatch.Draw(drawData);
    ///
    /// The ExoVortex shader's uTime is driven internally by Calamity via
    /// Main.GlobalTimeWrappedHourly, we do not set it ourselves.
    ///
    /// ai[0] = parent whoAmI (AphelionPro)
    ///
    /// TEXTURE:
    ///   uImage0 is what the shader actually samples and distorts.
    ///   We feed it CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak.
    ///   It's the same heat-map texture used by the sun, so the two effects
    ///   share a visual language. Any tileable noise/gradient works here.
    /// </summary>
    public class AphelionVortex : ModProjectile
    {
        public override string Texture =>
            "RagnarokMod/Projectiles/NoProj";
        private const string NoisePath =
            "CalamityMod/ExtraTextures/GreyscaleGradients/EternityStreak";

        private static Texture2D NoiseTexture =>
            ModContent.Request<Texture2D>(NoisePath,
                AssetRequestMode.ImmediateLoad).Value;

        // Parent accessor
        private int ParentID => (int)Projectile.ai[0];
        private Player owner => Main.player[Projectile.owner];
        private bool TryGetParent(out Projectile parent)
        {
            int id = ParentID;
            if (id < 0 || id >= Main.maxProjectiles)
            {
                parent = null;
                return false;
            }
            parent = Main.projectile[id];
            return parent.active &&
                   parent.type == ModContent.ProjectileType<AphelionPro>();
        }

        // Charge 0 -> 1, pulled from parent each frame.
        private float _chargeProgress;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 3;
            Projectile.ignoreWater = true;
        }
        // Pull radius and force for enemy attraction
        private const float PullRadius = 400f;
        private const float PullForce = 0.8f;

        public override void AI()
        {
            if (!TryGetParent(out Projectile raw))
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.timeLeft = 3;

            _chargeProgress = Math.Min(
                raw.ai[0] / AphelionPro.MaxChargeFrames, 1f);

            float lum = _chargeProgress * 0.8f;
            Lighting.AddLight(Projectile.Center,
                new Vector3(lum * 0.5f, lum * 0.2f, lum * 1.2f));

            if (Main.netMode != NetmodeID.MultiplayerClient && _chargeProgress > 0.4f)
            {
                float pullStrength = MathHelper.Lerp(0f, PullForce,
                    (_chargeProgress - 0.4f) / 0.6f);

                foreach (NPC npc in Main.ActiveNPCs)
                {
                    if (!npc.active || npc.boss || npc.friendly) continue;

                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist > PullRadius || dist < 5f) continue;

                    // Pull toward center, weakens at close range so enemies
                    // orbit at the edge rather than collapsing into the player.
                    float edgeFactor = MathHelper.Clamp(dist / PullRadius, 0.1f, 1f);
                    Vector2 pullDir = (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero);
                    npc.velocity += pullDir * pullStrength * edgeFactor;

                    // Cap velocity so enemies don't rocket into the player.
                    if (npc.velocity.Length() > 8f)
                        npc.velocity = npc.velocity.SafeNormalize(Vector2.Zero) * 8f;
                }
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            // Ease vortex in from the threshold so it doesn't pop in.
            float appear = MathHelper.SmoothStep(0f, 1f,
                MathHelper.Clamp((_chargeProgress - 0.40f) / 0.15f, 0f, 1f));

            if (appear <= 0.01f)
                return false;

            SpriteBatch sb = Main.spriteBatch;
            Texture2D noise = NoiseTexture;

            // Colour ramp
            // uColor / uSecondaryColor are multiplied into the shader output.
            // Low charge: deep blue-violet.  Full charge: electric blue-white.
            Color primaryCol = Color.Lerp(
                new Color(160, 130, 180),
                new Color(200, 200, 255),
                _chargeProgress) * appear;

            Color secondaryCol = Color.Lerp(
                new Color(140, 110, 220),
                new Color(190, 240, 255),
                _chargeProgress) * appear;

            var shader = GameShaders.Misc["CalamityMod:ExoVortex"];

            // Draw three layers at different scales
            // ExoVortex is a single-pass shader, there's no built-in layering.
            // We get the multi-ring look by drawing three separate quads at different scales, each with its own opacity.
            // Calamity re-applies uTime internally each Apply() call, so the three layers animate at the same rate but look distinct because of size difference.

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive,
                SamplerState.LinearWrap, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            Vector2 center = Projectile.Center - Main.screenPosition;
            Vector2 origin = noise.Size() * 0.5f;

            // Outer ring
            float outerScale = 2f * appear * _chargeProgress;
            var outerData = new DrawData(noise, center, null,
                primaryCol * 0.8f, 0f, origin, outerScale,
                owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            shader.UseColor(primaryCol)
                  .UseSecondaryColor(secondaryCol)
                  .UseOpacity(appear * 0.95f)
                  .Apply(outerData);

            sb.Draw(noise, center, null, primaryCol * 0.8f,
                0f, origin, outerScale, owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            // Mid ring
            float midScale = 1.5f * appear
                           * MathHelper.SmoothStep(0.45f, 1.0f, _chargeProgress);
            var midData = new DrawData(noise, center, null,
                primaryCol * 0.9f, 0f, origin, midScale,
                owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            shader.UseOpacity(appear * 1.0f)
                  .Apply(midData);

            sb.Draw(noise, center, null, primaryCol * 0.9f,
                0f, origin, midScale,
                owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

            // Inner core
            float innerScale = 1f * appear
                             * MathHelper.SmoothStep(0.55f, 1.0f, _chargeProgress);
            Color coreCol = Color.Lerp(primaryCol, Color.White * appear, 0.3f);
            var innerData = new DrawData(noise, center, null,
                coreCol, 0f, origin, innerScale,
                owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            shader.UseColor(coreCol)
                  .UseOpacity(appear * 0.90f)
                  .Apply(innerData);

            sb.Draw(noise, center, null, coreCol,
                0f, origin, innerScale, owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                
            // Full-charge
            if (_chargeProgress >= 0.95f)
            {
                float pulse = 0.5f + 0.5f * MathF.Sin(Main.GameUpdateCount * 0.15f);
                float flashScale = 0.25f * (0.85f + 0.15f * pulse) * appear;
                Color flashCol = Color.White * (0.7f + 0.3f * pulse) * appear;

                var flashData = new DrawData(noise, center, null,
                    flashCol, 0f, origin, flashScale,
                    owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

                Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
                shader.UseColor(flashCol)
                      .UseOpacity(appear * (0.8f + 0.2f * pulse))
                      .Apply(flashData);

                sb.Draw(noise, center, null, flashCol,
                    0f, origin, flashScale, owner.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }

            // Restore
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                Main.DefaultSamplerState, DepthStencilState.None,
                Main.Rasterizer, null,
                Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}