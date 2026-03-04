using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Common.Configs;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace RagnarokMod.Common.ModSystems
{
    public class RiffScreenEffects : ModSystem
    {
        private class RiffTint
        {
            public byte RiffType;
            public float Intensity;
            public float MaxIntensity;
            public float FadeSpeed;

            public Func<float, SpriteBatch, Texture2D, Rectangle, Rectangle, float, bool> CustomDraw;

            // Simple single-color tints use this
            public Color? SimpleColor;
        }

        private static readonly Rectangle PixelSrc = new Rectangle(0, 0, 1, 1);
        private List<RiffTint> tints;

        public override void OnModLoad()
        {
            tints = new List<RiffTint>
            {
                // Shredder
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<ShredderRiff>(),
                    MaxIntensity = 0.1f,
                    FadeSpeed = 0.006f,
                    SimpleColor = new Color(255, 0, 0)
                },

                // DragonForce
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<DragonRiff>(),
                    MaxIntensity = 0.1f,
                    FadeSpeed = 0.008f,
                    SimpleColor = new Color(255, 100, 20)
                },

                // Hive Mind
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<HiveMindRiff>(),
                    MaxIntensity = 0.1f,
                    FadeSpeed = 0.006f,
                    CustomDraw = DrawHiveMindTint
                },

                // Astral
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<AureusRiff>(),
                    MaxIntensity = 0.1f,
                    FadeSpeed = 0.006f,
                    CustomDraw = DrawAstralTint
                },

                // Scourge
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<ScourgeRiff>(),
                    MaxIntensity = 0.3f,
                    FadeSpeed = 0.006f,
                    CustomDraw = DrawScourgeStorm
                },
                // Devourer
                new RiffTint
                {
                    RiffType = RiffLoader.RiffType<DevourerRiff>(),
                    MaxIntensity = 0.1f,
                    FadeSpeed = 0.006f,
                    SimpleColor = new Color(180, 40, 255)
                },
            };
        }

        public override void PostUpdateEverything()
        {
            Player player = Main.LocalPlayer;
            byte activeRiff = player.active && !player.dead
                ? player.GetRagnarokModPlayer().activeRiffType
                : (byte)0;

            for (int i = 0; i < tints.Count; i++)
            {
                RiffTint t = tints[i];
                if (activeRiff == t.RiffType)
                    t.Intensity = MathHelper.Min(t.Intensity + t.FadeSpeed, t.MaxIntensity);
                else
                    t.Intensity = MathHelper.Max(t.Intensity - t.FadeSpeed, 0f);
            }
        }

        public override void PostDrawTiles()
        {
            float configScale = ModContent.GetInstance<ClientConfig>().RiffScreenEffectIntensity;
            if (configScale <= 0f)
                return;
            bool anyActive = false;
            for (int i = 0; i < tints.Count; i++)
            {
                if (tints[i].Intensity > 0f)
                {
                    anyActive = true;
                    break;
                }
            }

            if (!anyActive)
                return;

            SpriteBatch sb = Main.spriteBatch;
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Rectangle screen = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
            float time = Main.GlobalTimeWrappedHourly;

            for (int i = 0; i < tints.Count; i++)
            {
                RiffTint t = tints[i];
                float scaledIntensity = t.Intensity * configScale;
                if (scaledIntensity <= 0f)
                    continue;

                if (t.CustomDraw != null)
                    t.CustomDraw(scaledIntensity, sb, pixel, screen, PixelSrc, time);
                else if (t.SimpleColor.HasValue)
                    sb.Draw(pixel, screen, PixelSrc, t.SimpleColor.Value * scaledIntensity);

                if (t.CustomDraw != null)
                {
                    t.CustomDraw(t.Intensity, sb, pixel, screen, PixelSrc, time);
                }
                else if (t.SimpleColor.HasValue)
                {
                    sb.Draw(pixel, screen, PixelSrc, t.SimpleColor.Value * t.Intensity);
                }
            }

            sb.End();
        }

        private static bool DrawHiveMindTint(float intensity, SpriteBatch sb, Texture2D pixel, Rectangle screen, Rectangle src, float time)
        {
            // Base corruption purple
            sb.Draw(pixel, screen, src, new Color(40, 10, 50) * intensity);

            // Breathing toxic green overlay
            float breathe = MathF.Sin(time * 2.5f) * 0.5f + 0.5f;
            sb.Draw(pixel, screen, src, new Color(30, 60, 10) * intensity * breathe * 0.7f);

            // Subtle sickly yellow flicker
            float flicker = MathF.Sin(time * 7.3f) * MathF.Sin(time * 3.1f);
            if (flicker > 0.3f)
                sb.Draw(pixel, screen, src, new Color(40, 35, 5) * intensity * (flicker - 0.3f) * 0.5f);

            return true;
        }
        private static bool DrawAstralTint(float intensity, SpriteBatch sb, Texture2D pixel, Rectangle screen, Rectangle src, float time)
        {
            Color astralRed = new Color(237, 93, 83);
            Color astralCyan = new Color(66, 189, 181);
            float t = MathF.Sin(time * 1.5f) * 0.5f + 0.5f; // oscillates 0-1
            sb.Draw(pixel, screen, src, Color.Lerp(astralRed, astralCyan, t) * intensity);
            return true;
        }

        private static bool DrawScourgeStorm(float intensity, SpriteBatch sb, Texture2D pixel, Rectangle screen, Rectangle src, float time)
        {
            // Sandy base tint
            sb.Draw(pixel, screen, src, new Color(120, 100, 60) * intensity * 0.3f);

            // Scrolling cloud layers using Calamity's Perlin noise
            Texture2D noise = ModContent.Request<Texture2D>(
                "CalamityMod/ExtraTextures/GreyscaleGradients/Perlin",
                ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            for (int layer = 0; layer < 3; layer++)
            {
                float scrollSpeed = (layer + 1) * 0.5f;
                float scrollX = time * scrollSpeed * 200f % noise.Width;
                float scrollY = MathF.Sin(time * 0.8f + layer * 1.5f) * 20f;
                float layerAlpha = intensity * (0.35f - layer * 0.08f);
                float scale = 1.5f + layer * 0.5f;

                Color sandColor = new Color(180, 155, 100) * layerAlpha;

                float scaleX = 1.5f + layer * 0.5f;
                float scaleY = (float)Main.screenHeight / noise.Height * 1.3f;
                float yPos = -Main.screenHeight * 0.15f + scrollY + layer * 50f;

                for (int x = -(int)scrollX - noise.Width; x < Main.screenWidth + noise.Width * 2; x += (int)(noise.Width * scaleX))
                {
                    sb.Draw(noise,
                        new Vector2(x, yPos),
                        null, sandColor, 0f, Vector2.Zero, new Vector2(scaleX, scaleY),
                        SpriteEffects.None, 0f);
                }
            }

            return true;
        }
    }
}