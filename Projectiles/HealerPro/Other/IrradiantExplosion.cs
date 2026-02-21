using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantExplosion : ModProjectile
    {
        private int trail = 8;
        private const int Lifetime = 45;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.oldPos = new Vector2[trail];
            Projectile.oldRot = new float[trail];
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.scale = 2f;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Lifetime;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            float progress = 1f - (float)Projectile.timeLeft / Lifetime;
            Projectile.scale = progress * 6f;

            int burstCount = (int)(8 * (1f - progress) + 2);
            for (int i = 0; i < burstCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Projectile.width * Projectile.scale * 0.4f * Main.rand.NextFloat(0.5f, 1f);
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * dist;
                Vector2 outVel = angle.ToRotationVector2() * Main.rand.NextFloat(4f, 12f) * (1f - progress);
                int dustType = Main.rand.NextBool() ? DustID.PurpleTorch : DustID.GemAmethyst;
                Dust dust = Dust.NewDustPerfect(pos, dustType, outVel, 0, default, Main.rand.NextFloat(1.5f, 3f));
                dust.noGravity = true;
                dust.fadeIn = 0.4f;
            }

            if (progress < 0.5f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Dust spark = Dust.NewDustPerfect(Projectile.Center, DustID.PurpleCrystalShard,
                        Main.rand.NextVector2Unit() * Main.rand.NextFloat(8f, 20f), 0, default, Main.rand.NextFloat(2f, 3.5f));
                    spark.noGravity = true;
                }
            }

            if (progress > 0.6f)
            {
                for (int i = 0; i < 2; i++)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float dist = Main.rand.NextFloat(100f, 200f);
                    Vector2 spawnPos = Projectile.Center + angle.ToRotationVector2() * dist;
                    Vector2 inward = (Projectile.Center - spawnPos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(5f, 12f);
                    Dust dust = Dust.NewDustPerfect(spawnPos, DustID.PurpleTorch, inward, 0, default, 2f);
                    dust.noGravity = true;
                }
            }

            float lightScale = (1f - progress) * Projectile.scale * 0.3f;
            Lighting.AddLight(Projectile.Center, 0.5f * lightScale, 0.2f * lightScale, 0.8f * lightScale);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<NoProj>()].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            float drawScale = (float)Projectile.width / texture.Width * Projectile.scale;
            float time = Main.GlobalTimeWrappedHourly;

            float progress = 1f - (float)Projectile.timeLeft / Lifetime;
            float alpha = 1f - progress;
            float eased = MathF.Sqrt(progress);

            // STATE: BEGUN (vanilla)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)

            // Massive expanding outer glow
            float outerSize = Projectile.width * Projectile.scale * 2.5f;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(80, 20, 140) * 0.12f * alpha,
                0f, PixelOriginCenter, new Vector2(outerSize), SpriteEffects.None, 0f);

            // Mid glow
            float midSize = Projectile.width * Projectile.scale * 1.4f;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(160, 40, 220) * 0.18f * alpha,
                0f, PixelOriginCenter, new Vector2(midSize), SpriteEffects.None, 0f);

            // Hot core
            float coreAlpha = MathF.Pow(alpha, 2f);
            float coreSize = Projectile.width * Projectile.scale * 0.5f;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(220, 160, 255) * 0.3f * coreAlpha,
                0f, PixelOriginCenter, new Vector2(coreSize), SpriteEffects.None, 0f);

            // Expanding ring edge
            float ringSize = Projectile.width * Projectile.scale * 1.8f + 30f * alpha;
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(200, 80, 255) * 0.25f * alpha,
                0f, PixelOriginCenter, new Vector2(ringSize), SpriteEffects.None, 0f);

            // Main sprite fading
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRect,
                new Color(180, 50, 255) * alpha, Projectile.rotation, origin, drawScale, SpriteEffects.None);

            // STATE: BEGUN (additive)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal)

            // === EXPANDING PURPLE VORTEX ===
            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(alpha * 0.9f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(80, 10, 160));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(200, 80, 255));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            float vortexBaseScale = Projectile.scale * 3f;
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6f + time * MathHelper.TwoPi * 0.7f;
                Color dc = Color.White * alpha * 0.8f;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * (5f * eased),
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexBaseScale, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal)

            // === COUNTER-ROTATING RED VORTEX ===
            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(alpha * 0.5f);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(200, 20, 40));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 80, 100));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            float innerScale = Projectile.scale * 0.35f;
            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f - time * MathHelper.TwoPi * 1.5f;
                Color dc = Color.White * alpha * 0.6f;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    innerScale, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal) — correct for vanilla

            return false;
        }
    }
}