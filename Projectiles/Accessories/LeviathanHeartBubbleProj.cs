using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Buffs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.Accessories
{
    public class LeviathanHeartBubbleProj : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        private const int Resolution = 64;
        private const float BaseRadius = 36f;
        private const int PopDuration = 20;

        private static readonly Color TealColor = new Color(0, 180, 160);
        private static readonly Color PurpleColor = new Color(160, 40, 220);

        private bool isPopping = false;
        private int popTimer = 0;

        // ai[0] time alive
        // ai[1] 0f = teal (normal), 1f = purple (corrupted)
        private ref float TimeAlive => ref Projectile.ai[0];
        private ref float IsCorrupted => ref Projectile.ai[1];

        private Color BubbleColor => IsCorrupted == 1f ? PurpleColor : TealColor;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (isPopping)
            {
                popTimer++;
                Projectile.Center = player.Center;
                if (popTimer >= PopDuration)
                    Projectile.Kill();
                return;
            }

            // Check whichever buff corresponds to our color
            bool hasBuff = IsCorrupted == 1f
                ? player.HasBuff(ModContent.BuffType<LeviathanHeartBubbleCorrupted>())
                : player.HasBuff(ModContent.BuffType<LeviathanHeartBubble>());

            if (!hasBuff)
            {
                isPopping = true;
                return;
            }

            Projectile.Center = player.Center;
            TimeAlive++;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            float opacity;
            float radiusScale;

            if (isPopping)
            {
                float t = popTimer / (float)PopDuration;
                opacity = 1f - t;
                radiusScale = 1f + t * 0.6f;
            }
            else
            {
                opacity = Math.Min(TimeAlive / 30f, 1f);
                radiusScale = 1f;
            }

            DrawBubble(Main.spriteBatch, player.Center, opacity, radiusScale);
            return false;
        }

        private void DrawBubble(SpriteBatch spriteBatch, Vector2 center, float opacity, float radiusScale)
        {
            Color outerColor = BubbleColor * opacity;
            Color innerColor = Color.Lerp(BubbleColor, Color.White, 0.5f) * opacity * 0.4f;

            // Outer ring
            DrawWobblyRing(spriteBatch, center, BaseRadius, radiusScale, outerColor, 2f, wobbleScale: 1f);
            // Inner ring for depth
            DrawWobblyRing(spriteBatch, center, BaseRadius * 0.7f, radiusScale, innerColor, 1f, wobbleScale: 0.75f);

            // Glass sheen, fades out with the bubble during pop
            if (!isPopping || popTimer < PopDuration / 2)
                DrawSheen(spriteBatch, center, opacity * (isPopping ? 1f - popTimer / (float)PopDuration : 1f));

            // Pop burst lines
            if (isPopping)
            {
                float t = popTimer / (float)PopDuration;
                for (int i = 0; i < 12; i++)
                {
                    float angle = (i / 12f) * MathHelper.TwoPi;
                    float innerR = BaseRadius * (0.8f + t * 0.3f);
                    float outerR = BaseRadius * (1.0f + t * 1.2f);

                    Vector2 start = center - Main.screenPosition + new Vector2(
                        (float)Math.Cos(angle) * innerR,
                        (float)Math.Sin(angle) * innerR);
                    Vector2 end = center - Main.screenPosition + new Vector2(
                        (float)Math.Cos(angle) * outerR,
                        (float)Math.Sin(angle) * outerR);

                    DrawLine(spriteBatch, start, end, BubbleColor * (1f - t), 1.5f);
                }
            }
        }

        private void DrawWobblyRing(SpriteBatch spriteBatch, Vector2 center, float radius, float radiusScale, Color color, float thickness, float wobbleScale)
        {
            Vector2[] points = new Vector2[Resolution + 1];
            for (int i = 0; i <= Resolution; i++)
            {
                float angle = (i / (float)Resolution) * MathHelper.TwoPi;
                // soap bubble feels
                float wobble1 = (float)Math.Sin(angle * 2f + TimeAlive * 0.015f) * 1.5f * wobbleScale;
                float wobble2 = (float)Math.Sin(angle * 3f - TimeAlive * 0.022f) * 0.8f * wobbleScale;
                float r = (radius + wobble1 + wobble2) * radiusScale;

                points[i] = center - Main.screenPosition + new Vector2(
                    (float)Math.Cos(angle) * r,
                    (float)Math.Sin(angle) * r);
            }

            for (int i = 0; i < points.Length - 1; i++)
                DrawLine(spriteBatch, points[i], points[i + 1], color, thickness);
        }

        private void DrawSheen(SpriteBatch spriteBatch, Vector2 center, float opacity)
        {
            // Small squished ellipse in the upper-left, simulating light hitting a glass sphere
            float sheenAngle = MathHelper.Pi * 1.25f; // upper-left
            float sheenDist = BaseRadius * 0.35f;
            Vector2 sheenCenter = center - Main.screenPosition + new Vector2(
                (float)Math.Cos(sheenAngle) * sheenDist,
                (float)Math.Sin(sheenAngle) * sheenDist);

            Color sheenColor = Color.White * opacity * 0.35f;

            float rx = 7f;   // horizontal radius of highlight
            float ry = 3.5f; // vertical radius
            const int sheenRes = 32;
            for (int i = 0; i < sheenRes; i++)
            {
                float a = (i / (float)sheenRes) * MathHelper.TwoPi;
                Vector2 edge = sheenCenter + new Vector2(
                    (float)Math.Cos(a) * rx,
                    (float)Math.Sin(a) * ry);
                DrawLine(spriteBatch, sheenCenter, edge, sheenColor, 1f);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(
                Terraria.GameContent.TextureAssets.MagicPixel.Value,
                start,
                new Rectangle(0, 0, 1, 1),
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0f
            );
        }

        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            float opacity;
            if (isPopping)
            {
                float t = popTimer / (float)PopDuration;
                opacity = 1f - t;
            }
            else
            {
                opacity = Math.Min(TimeAlive / 30f, 1f);
            }

            DrawSheen(Main.spriteBatch, player.Center, opacity);
        }
    }
}