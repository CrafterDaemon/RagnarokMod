using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class ShredderPro : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CrimstoneBlock}";

        private const int SegmentCount = 14;
        private const float MaxTendrilLength = 600f;

        private enum Phase { Extending, Holding, Retracting, Dead }
        private Phase currentPhase = Phase.Extending;

        private const int ExtendFrames = 12;
        private const int HoldFrames = 8;
        private const int RetractFrames = 18;

        private float progress = 0f;
        private int holdTimer = 0;
        private float seed;
        private bool initialized = false;

        private Vector2[] segmentPositions = new Vector2[SegmentCount + 1];
        private float[] segmentThicknesses = new float[SegmentCount + 1];
        private int activeSegmentCount = 0;

        // ai[0], ai[1] = target world position (cursor pos at spawn)
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 600;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (!initialized)
            {
                if (Main.player[Projectile.owner].GetRagnarokModPlayer().activeRiffType != RiffLoader.RiffType<ShredderRiff>())
                    SoundEngine.PlaySound(SoundID.Item88.WithPitchOffset(1.2f).WithVolumeScale(0.4f), Projectile.Center);
                seed = Main.rand.NextFloat(MathHelper.TwoPi);
                initialized = true;
            }

            Player owner = Main.player[Projectile.owner];
            Projectile.Center = owner.Center;

            switch (currentPhase)
            {
                case Phase.Extending:
                    progress += 1f / ExtendFrames;
                    if (progress >= 1f)
                    {
                        progress = 1f;
                        currentPhase = Phase.Holding;
                        holdTimer = HoldFrames;
                    }
                    break;

                case Phase.Holding:
                    holdTimer--;
                    if (holdTimer <= 0)
                        currentPhase = Phase.Retracting;
                    break;

                case Phase.Retracting:
                    progress -= 1f / RetractFrames;
                    if (progress <= 0f)
                    {
                        progress = 0f;
                        currentPhase = Phase.Dead;
                        Projectile.Kill();
                        return;
                    }
                    break;
            }

            BuildSegments();

            if (activeSegmentCount > 1 && Main.rand.NextBool(3))
            {
                Vector2 tipPos = segmentPositions[activeSegmentCount - 1];
                Dust drip = Dust.NewDustPerfect(tipPos, DustID.Blood, Vector2.UnitY * Main.rand.NextFloat(1f, 3f), 0, default, Main.rand.NextFloat(0.8f, 1.5f));
                drip.noGravity = false;
            }
        }

        private void BuildSegments()
        {
            Vector2 targetWorld = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Vector2 rootPos = Projectile.Center;

            Vector2 toTarget = targetWorld - rootPos;
            float totalDistance = toTarget.Length();

            if (totalDistance < 1f)
                toTarget = Vector2.UnitX;

            totalDistance = Math.Min(totalDistance, MaxTendrilLength);
            float dynamicSpacing = totalDistance / SegmentCount;

            float distanceFactor = totalDistance / MaxTendrilLength;

            Vector2 baseDir = Vector2.Normalize(toTarget);
            Vector2 perpDir = new Vector2(-baseDir.Y, baseDir.X);
            float easedProgress = EaseOutQuart(progress);
            float time = Main.GameUpdateCount * 0.025f;

            segmentPositions[0] = rootPos;
            segmentThicknesses[0] = 30f * easedProgress;
            activeSegmentCount = 1;

            Vector2 prevPos = rootPos;

            for (int i = 0; i < SegmentCount; i++)
            {
                float t = (i + 1) / (float)SegmentCount;

                if (t > easedProgress)
                    break;

                float lateralWave = (float)Math.Sin(time * 2f + seed + i * 0.5f) * 15f * t * distanceFactor;
                float flutter = (float)Math.Sin(time * 3.5f - seed + i * 0.7f) * 8f * t * t * distanceFactor;

                float retractFlail = 0f;
                if (currentPhase == Phase.Retracting)
                    retractFlail = (float)Math.Sin(time * 5f + i * 1.2f) * 20f * (1f - progress) * t * distanceFactor;

                float totalLateral = (lateralWave + flutter + retractFlail) * easedProgress;

                float jitterX = (float)Math.Sin((seed + i) * 17.3f) * 2f * easedProgress;
                float jitterY = (float)Math.Cos((seed + i) * 23.7f) * 2f * easedProgress;

                Vector2 segPos = prevPos
                    + baseDir * dynamicSpacing * easedProgress
                    + perpDir * totalLateral
                    + new Vector2(jitterX, jitterY);

                float baseThickness = MathHelper.Lerp(28f, 6f, t);
                float lumpVariation = (float)Math.Sin((seed + i) * 11.4f) * 3f;
                float thickness = (baseThickness + lumpVariation) * easedProgress;

                segmentPositions[activeSegmentCount] = segPos;
                segmentThicknesses[activeSegmentCount] = thickness;
                activeSegmentCount++;

                prevPos = segPos;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // Don't deal damage while retracting
            if (currentPhase == Phase.Retracting || currentPhase == Phase.Dead)
                return false;

            Vector2 targetCenter = targetHitbox.Center.ToVector2();
            float targetRadius = (targetHitbox.Width + targetHitbox.Height) * 0.25f;

            for (int s = 0; s < activeSegmentCount - 1; s++)
            {
                float radius = (segmentThicknesses[s] + segmentThicknesses[s + 1]) * 0.25f;
                float hitDist = radius + targetRadius;

                if (DistanceToSegment(targetCenter, segmentPositions[s], segmentPositions[s + 1]) < hitDist)
                    return true;
            }

            return false;
        }

        private static float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            float abLenSq = ab.LengthSquared();
            if (abLenSq < 0.001f)
                return Vector2.Distance(p, a);

            float t = MathHelper.Clamp(Vector2.Dot(p - a, ab) / abLenSq, 0f, 1f);
            Vector2 closest = a + ab * t;
            return Vector2.Distance(p, closest);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D fleshTexture = TextureAssets.Item[ItemID.CrimstoneBlock].Value;

            float easedProgress = EaseOutQuart(progress);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int s = 0; s < activeSegmentCount - 1; s++)
            {
                Vector2 posA = segmentPositions[s];
                Vector2 posB = segmentPositions[s + 1];

                float t = s / (float)SegmentCount;
                float alpha = (1f - t * 0.3f) * easedProgress;

                Color fleshColor = Color.Lerp(new Color(160, 20, 20), new Color(100, 10, 10), t) * alpha;

                Vector2 diff = posB - posA;
                float rotation = diff.ToRotation();
                float distance = diff.Length();

                float thickness = (segmentThicknesses[s] + segmentThicknesses[s + 1]) * 0.5f;
                float step = Math.Max(thickness * 0.35f, 6f);

                for (float d = 0; d < distance; d += step)
                {
                    Vector2 pos = posA + diff * (d / distance);
                    float scale = thickness / fleshTexture.Width;

                    spriteBatch.Draw(fleshTexture, pos - Main.screenPosition, null,
                        fleshColor, rotation, fleshTexture.Size() / 2f, scale, SpriteEffects.None, 0f);
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        private static float EaseOutQuart(float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            float inv = 1f - t;
            return 1f - inv * inv * inv * inv;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.player[Projectile.owner].GetRagnarokModPlayer().activeRiffType != RiffLoader.RiffType<ShredderRiff>())
                SoundEngine.PlaySound(SoundID.Item70, Projectile.Center);

            for (int i = 0; i < 40; i++)
            {
                float speed = Main.rand.NextFloat(3f, 12f);
                Vector2 velocity = Main.rand.NextVector2Unit() * speed;
                Dust blood = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, velocity, 0, default, Main.rand.NextFloat(1.2f, 3f));
                blood.noGravity = Main.rand.NextBool();
            }
        }
    }
}