using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Electronic
{
    public class TendrilStrike : BardProjectile, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        private const int TendrilCount = 7;
        private const int LifeTime = 50;
        private const float StartRadius = 120f;
        private const float BaseWidth = 10f;
        private const int SegmentsPerTendril = 12;

        // ai[0] = timer, ai[1] = target NPC index

        private float[] angles;
        private float[] lengthMult;
        private float[] delayNorm;
        private float[] curveBias;
        private bool init;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 999;
            Projectile.timeLeft = LifeTime;
        }

        private void Init()
        {
            if (init) return;
            init = true;

            angles = new float[TendrilCount];
            lengthMult = new float[TendrilCount];
            delayNorm = new float[TendrilCount];
            curveBias = new float[TendrilCount];

            var rng = new Random(Projectile.whoAmI * 31 + (int)Projectile.ai[1] * 17);
            float spacing = MathHelper.TwoPi / TendrilCount;

            for (int i = 0; i < TendrilCount; i++)
            {
                angles[i] = spacing * i + (float)(rng.NextDouble() - 0.5) * spacing * 0.5f;
                lengthMult[i] = 0.85f + (float)rng.NextDouble() * 0.3f;
                delayNorm[i] = (float)rng.NextDouble() * 0.2f;
                curveBias[i] = ((float)rng.NextDouble() - 0.5f) * 2f;
            }
        }

        public override void AI()
        {
            Init();

            int idx = (int)Projectile.ai[1];
            if (idx < 0 || idx >= Main.maxNPCs || !Main.npc[idx].active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = Main.npc[idx].Center;
            Projectile.ai[0]++;

            // Green cursed dust drifting inward
            if (Projectile.ai[0] % 2 == 0)
            {
                float a = Main.rand.NextFloat(MathHelper.TwoPi);
                float d = Main.rand.NextFloat(40f, StartRadius);
                Vector2 pos = Projectile.Center + a.ToRotationVector2() * d;
                Dust dust = Dust.NewDustDirect(pos, 1, 1, DustID.CursedTorch, 0f, 0f, 180, default, 0.9f);
                dust.noGravity = true;
                dust.velocity = (Projectile.Center - pos).SafeNormalize(Vector2.Zero) * 2f;
            }
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.CursedInferno, 180);
            if (Main.player[Projectile.owner].GetRagnarokModPlayer().redglassMonocle)
                target.AddBuff(ModContent.BuffType<Charmed>(), 300);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(BuffID.CursedInferno, 180);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!init) return false;

            float progress = Projectile.ai[0] / (float)LifeTime;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            Vector2 center = Projectile.Center - Main.screenPosition;

            float fadeOut = 1f - MathHelper.Clamp((progress - 0.5f) / 0.5f, 0f, 1f);

            // solid black tendril
            for (int i = 0; i < TendrilCount; i++)
            {
                float t = GetTendrilT(i, progress);
                if (t <= 0f) continue;

                float alpha = MathHelper.Clamp(t * 4f, 0f, 1f) * fadeOut;
                if (alpha <= 0.01f) continue;

                DrawSolidTendril(sb, pixel, center, i, t, progress, alpha);
            }

            // green glow (drawn on top so it's visible)
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.LinearClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            // Central glow
            float pulse = 1f + 0.2f * MathF.Sin(progress * MathHelper.TwoPi * 3f);
            DrawSoftGlow(sb, pixel, center, 70f * pulse, new Color(30, 180, 40) * 0.45f * fadeOut);
            DrawSoftGlow(sb, pixel, center, 40f * pulse, new Color(50, 255, 70) * 0.3f * fadeOut);

            // Glow along each tendril path
            for (int i = 0; i < TendrilCount; i++)
            {
                float t = GetTendrilT(i, progress);
                if (t <= 0f) continue;

                float stabT = EaseOutCubic(MathHelper.Clamp(t * 2.5f, 0f, 1f));
                float radius = MathHelper.Lerp(StartRadius * lengthMult[i], 0f, stabT);
                Vector2 glowPos = center + angles[i].ToRotationVector2() * radius;

                float glowAlpha = MathHelper.Clamp(t * 3f, 0f, 1f) * fadeOut * 0.4f;
                DrawSoftGlow(sb, pixel, glowPos, 25f, new Color(40, 220, 60) * glowAlpha);
            }

            // Restore default spritebatch state
            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

            return false;
        }

        private void DrawSolidTendril(SpriteBatch sb, Texture2D pixel, Vector2 center, int i, float t, float progress, float alpha)
        {
            float stabT = EaseOutCubic(MathHelper.Clamp(t * 2.5f, 0f, 1f));
            float reach = StartRadius * lengthMult[i];
            float angle = angles[i];
            float curve = curveBias[i];

            Vector2 dir = angle.ToRotationVector2();
            Vector2 perp = new Vector2(-dir.Y, dir.X);

            // Tip pierces past center for a "stab through" feel
            float tipDist = MathHelper.Lerp(reach, -10f, stabT);
            float baseDist = reach + 15f;

            // Subtle writhing motion
            float writhe = MathF.Sin(progress * MathHelper.Pi * 4f + i * 2.1f) * 6f * (1f - stabT * 0.7f);

            // Cubic bezier control points
            Vector2 p0 = center + dir * baseDist;
            Vector2 p3 = center + dir * tipDist;
            Vector2 p1 = center + dir * (baseDist * 0.65f) + perp * (curve * 30f + writhe);
            Vector2 p2 = center + dir * (tipDist + reach * 0.3f) + perp * (curve * 15f - writhe * 0.5f);

            // Sample bezier curve
            Vector2[] points = new Vector2[SegmentsPerTendril];
            for (int s = 0; s < SegmentsPerTendril; s++)
                points[s] = CubicBezier(p0, p1, p2, p3, s / (float)(SegmentsPerTendril - 1));

            Color black = new Color(6, 6, 8) * alpha;
            Color darkCore = new Color(2, 2, 3) * alpha;

            for (int s = 0; s < SegmentsPerTendril - 1; s++)
            {
                float st = s / (float)(SegmentsPerTendril - 1);
                float nextSt = (s + 1) / (float)(SegmentsPerTendril - 1);

                float widthHere = BaseWidth * Taper(st) * alpha;
                float widthNext = BaseWidth * Taper(nextSt) * alpha;
                float avgWidth = (widthHere + widthNext) * 0.5f;
                if (avgWidth < 0.4f) continue;

                Vector2 segDir = points[s + 1] - points[s];
                float segLen = segDir.Length();
                if (segLen < 0.1f) continue;
                float rot = segDir.ToRotation();
                Vector2 mid = (points[s] + points[s + 1]) * 0.5f;

                // Solid black body
                sb.Draw(pixel, mid, new Rectangle(0, 0, 1, 1), black, rot,
                    new Vector2(0.5f), new Vector2(segLen + 1f, avgWidth), SpriteEffects.None, 0f);

                // Even darker core for depth
                sb.Draw(pixel, mid, new Rectangle(0, 0, 1, 1), darkCore, rot,
                    new Vector2(0.5f), new Vector2(segLen + 1f, avgWidth * 0.4f), SpriteEffects.None, 0f);
            }

            // Sharp spike at the tip
            if (SegmentsPerTendril >= 2)
            {
                Vector2 tipDir = points[SegmentsPerTendril - 1] - points[SegmentsPerTendril - 2];
                float tipLen = tipDir.Length();
                if (tipLen > 0.5f)
                {
                    sb.Draw(pixel, points[SegmentsPerTendril - 1], new Rectangle(0, 0, 1, 1), black,
                        tipDir.ToRotation(), new Vector2(1f, 0.5f),
                        new Vector2(tipLen * 0.7f, BaseWidth * 0.15f * alpha), SpriteEffects.None, 0f);
                }
            }
        }

        /// <summary>Thick at base (t=0), tapers to a point at tip (t=1).</summary>
        private static float Taper(float t)
        {
            if (t < 0.3f) return 1f;
            return MathHelper.Lerp(1f, 0f, (t - 0.3f) / 0.7f);
        }

        private float GetTendrilT(int index, float overallProgress)
        {
            float delay = delayNorm[index];
            if (overallProgress < delay) return 0f;
            return (overallProgress - delay) / (1f - delay);
        }

        private static Vector2 CubicBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1f - t;
            return u * u * u * p0
                 + 3f * u * u * t * p1
                 + 3f * u * t * t * p2
                 + t * t * t * p3;
        }

        private void DrawSoftGlow(SpriteBatch sb, Texture2D pixel, Vector2 pos, float size, Color color)
        {
            for (int layer = 0; layer < 4; layer++)
            {
                float scale = size * (1f + layer * 0.6f) / pixel.Width;
                Color c = color * (0.2f / (1f + layer * 0.8f));
                sb.Draw(pixel, pos, new Rectangle(0, 0, 1, 1), c, 0f, new Vector2(0.8f), scale, SpriteEffects.None, 0f);
            }
        }

        private static float EaseOutCubic(float t)
        {
            return 1f - MathF.Pow(1f - MathHelper.Clamp(t, 0f, 1f), 3f);
        }
    }
}