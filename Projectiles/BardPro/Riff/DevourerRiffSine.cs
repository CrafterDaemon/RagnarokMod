using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    public class DevourerRiffSine : ModProjectile
    {
        public override string Texture => "RagnarokMod/Projectiles/NoProj";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 20;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        // ai[0] = target player index
        public override void AI()
        {
            if (Main.player[Projectile.owner].GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<DevourerRiff>())
                Projectile.timeLeft++;

            int targetIndex = (int)Projectile.ai[0];
            if (targetIndex >= 0 && targetIndex < Main.maxPlayers && Main.player[targetIndex].active)
                Projectile.Center = Main.player[targetIndex].Center;
            else
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D glow = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Rectangle pixelRect = new Rectangle(0, 0, 1, 1);

            SpriteBatch spriteBatch = Main.spriteBatch;

            int segments = 160;
            float waveWidth = 120f;
            float waveHeight = 35f;
            float yOffset = -55f;
            float frequency = 2.5f;
            float timeOffset = Main.GameUpdateCount * 0.08f;

            // Fade out near ends
            float EdgeFade(float progress) =>
                MathHelper.Clamp(progress * 8f, 0f, 1f) * MathHelper.Clamp((1f - progress) * 8f, 0f, 1f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < segments; i++)
            {
                float progress = i / (float)segments;
                float xOffset = (progress - 0.5f) * 2f * waveWidth;
                float rawWave = (float)Math.Sin(progress * MathHelper.TwoPi * frequency + timeOffset);
                float yWaveOffset = rawWave * waveHeight;

                float fade = EdgeFade(progress);
                float waveBrightness = 0.6f + 0.4f * ((rawWave + 1f) * 0.5f); // brighter at wave peaks

                Vector2 pos = Projectile.Center + new Vector2(xOffset, yOffset + yWaveOffset);

                // Outer soft glow
                Color outerGlow = new Color(80, 30, 180) * fade * waveBrightness * 0.35f;
                spriteBatch.Draw(glow, pos - Main.screenPosition, null, outerGlow, 0f,
                    glow.Size() / 2f, 0.5f, SpriteEffects.None, 0f);

                // Mid glow
                Color midGlow = new Color(160, 70, 255) * fade * waveBrightness * 0.55f;
                spriteBatch.Draw(glow, pos - Main.screenPosition, null, midGlow, 0f,
                    glow.Size() / 2f, 0.22f, SpriteEffects.None, 0f);

                // Core pixel line
                Color coreColor = new Color(230, 150, 255) * fade * waveBrightness;
                spriteBatch.Draw(pixel, pos - Main.screenPosition, pixelRect,
                    coreColor, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0f);
            }

            // Second wave slightly offset in time for a layered shimmer effect
            for (int i = 0; i < segments; i++)
            {
                float progress = i / (float)segments;
                float xOffset = (progress - 0.5f) * 2f * waveWidth;
                float rawWave = (float)Math.Sin(progress * MathHelper.TwoPi * frequency + timeOffset + MathHelper.Pi * 0.3f);
                float yWaveOffset = rawWave * waveHeight * 0.5f; // shorter second wave

                float fade = EdgeFade(progress);

                Vector2 pos = Projectile.Center + new Vector2(xOffset, yOffset + yWaveOffset);

                Color shimmerColor = new Color(200, 100, 255) * fade * 0.25f;
                spriteBatch.Draw(glow, pos - Main.screenPosition, null, shimmerColor, 0f,
                    glow.Size() / 2f, 0.15f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}