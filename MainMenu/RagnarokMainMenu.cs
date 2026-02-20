using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.MainMenu
{
    public class RagnarokMainMenu : ModMenu
    {
        public class Particle
        {
            public Vector2 Position;
            public Vector2 Velocity;
            public float Scale;
            public float Alpha;
            public float AlphaDecay;
            public Color Color;
            public int Life;
            public int MaxLife;
        }

        private static List<Particle> Particles = new();
        private static int timer = 0;

        public override string DisplayName => "Ragnarok Style";

        public override Asset<Texture2D> Logo => ModContent.Request<Texture2D>("RagnarokMod/MainMenu/Logo");
        public override Asset<Texture2D> SunTexture => ModContent.Request<Texture2D>("CalamityMod/Backgrounds/BlankPixel");
        public override Asset<Texture2D> MoonTexture => ModContent.Request<Texture2D>("CalamityMod/Backgrounds/BlankPixel");

        public override int Music => MusicLoader.GetMusicSlot("RagnarokMod/Sounds/Music/TitleMusic");

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            timer++;

            Texture2D background = ModContent.Request<Texture2D>("RagnarokMod/MainMenu/Background").Value;

            // Scale background to fill screen at any aspect ratio
            float xScale = (float)Main.screenWidth / background.Width;
            float yScale = (float)Main.screenHeight / background.Height;
            float scale = xScale;
            Vector2 drawOffset = Vector2.Zero;

            if (xScale != yScale)
            {
                if (yScale > xScale)
                {
                    scale = yScale;
                    drawOffset.X -= ((background.Width * scale - Main.screenWidth) * 0.5f) + 1;
                }
                else
                    drawOffset.Y -= (background.Height * scale - Main.screenHeight) * 0.5f;
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            spriteBatch.Draw(background, drawOffset, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Particles
            // Spawn new particles from the bottom
            float spawnX = Main.rand.NextFloat(0.0f, Main.screenWidth);
            int life = Main.rand.Next(120, 360);
            Color particleColor = Main.rand.NextBool()
                ? Color.Lerp(new Color(180, 80, 255), new Color(100, 40, 200), Main.rand.NextFloat())
                : Color.Lerp(new Color(220, 150, 255), new Color(255, 200, 255), Main.rand.NextFloat());

            Particles.Add(new Particle
            {
                Position = new Vector2(spawnX, Main.screenHeight + 10f),
                Velocity = new Vector2(Main.rand.NextFloat(-0.6f, 0.6f), Main.rand.NextFloat(-3f, -1.2f)),
                Scale = Main.rand.NextFloat(0.25f, 0.5f),
                Alpha = 0f,
                AlphaDecay = 1f / life,
                Color = particleColor,
                Life = 0,
                MaxLife = life
            });

            // Update and draw particles
            Texture2D glow = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            for (int i = Particles.Count - 1; i >= 0; i--)
            {
                Particle p = Particles[i];

                // Fade in for first 20% of life, fade out for last 30%
                float lifeRatio = p.Life / (float)p.MaxLife;
                float alpha = MathHelper.Clamp(lifeRatio * 5f, 0f, 1f) * MathHelper.Clamp((1f - lifeRatio) * 3.3f, 0f, 1f);

                // Slight horizontal drift
                p.Velocity.X += (float)Math.Sin(p.Life * 0.05f + i) * 0.02f;
                p.Position += p.Velocity;
                p.Life++;

                if (p.Life >= p.MaxLife || p.Position.Y < -20f)
                {
                    Particles.RemoveAt(i);
                    continue;
                }

                spriteBatch.Draw(glow, p.Position, null, p.Color * alpha, 0f,
                    glow.Size() * 0.5f, p.Scale, SpriteEffects.None, 0f);
            }

            // Logo
            drawColor = Color.White;
            Main.time = 27000;
            Main.dayTime = true;

            // Pulse scale
            float pulse = 1f + (float)Math.Sin(timer * 0.04f) * 0.015f;

            // Occasional stronger pulse every ~5 seconds
            float strongPulse = MathHelper.Clamp((float)Math.Sin(timer * 0.008f) * 0.5f + 0.5f, 0f, 1f);
            strongPulse = (float)Math.Pow(strongPulse, 6f); // sharp spike
            pulse += strongPulse * 0.04f;

            // Glow behind logo that pulses with it
            Texture2D logoGlow = ModContent.Request<Texture2D>("CalamityMod/Backgrounds/BlankPixel").Value;
            Vector2 logoPos = new Vector2(Main.screenWidth / 2f, 100f);
            Color glowColor = new Color(120, 50, 220) * (0.3f + strongPulse * 0.4f);
            spriteBatch.Draw(logoGlow, logoPos, null, glowColor, 0f,
                logoGlow.Size() * 0.5f, new Vector2(Logo.Value.Width * pulse * 1.2f, Logo.Value.Height * pulse * 0.5f), SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp,
                DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            spriteBatch.Draw(Logo.Value, logoPos, null, drawColor, 0f,
                Logo.Value.Size() * 0.5f, pulse, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            return false;
        }
    }
}