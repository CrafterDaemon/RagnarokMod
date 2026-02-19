using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.Riffs
{
    public class ShredderTendrils : ModProjectile
    {
        public override string Texture => $"Terraria/Images/Item_{ItemID.CrimstoneBlock}";

<<<<<<< HEAD
        private const int TendrilCount = 8;
        private const int SegmentsPerTendril = 12;
        private const float TendrilLength = 640f;
=======
        private const int TendrilCount = 6;
        private const int SegmentsPerTendril = 12;
        private const float TendrilLength = 320f;
>>>>>>> 0e2a5dfd4a70bcbabefd53b2976f0dfe969e7e7d
        private const float SegmentSpacing = TendrilLength / SegmentsPerTendril;

        // Store random seeds for each tendril so they twitch independently
        private float[] tendrilSeeds = new float[TendrilCount];
        private bool initialized = false;
        private float growProgress = 0f; // 0 = fully curled, 1 = fully extended
        private const int GrowTime = 30; // frames to uncurl
        private const int RetractTime = 20; // frames to curl back before death

        // ai[0] = target player index
        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
            Projectile.timeLeft = 30;
        }

        public override void AI()
        {
            if (!initialized)
            {
                for (int i = 0; i < TendrilCount; i++)
                    tendrilSeeds[i] = Main.rand.NextFloat(MathHelper.TwoPi);

                // Fleshy burst sound
                SoundEngine.PlaySound(new("CalamityMod/Sounds/NPCKilled/PerfHiveDeath"), Projectile.Center);

                // Explosive blood burst on spawn
                for (int i = 0; i < 80; i++)
                {
                    float speed = Main.rand.NextFloat(2f, 15f);
                    Vector2 velocity = Main.rand.NextVector2Unit() * speed;
                    Dust blood = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, velocity, 0, default, Main.rand.NextFloat(1.5f, 3.5f));
                    blood.noGravity = Main.rand.NextBool(2);
                    blood.fadeIn = Main.rand.NextFloat(0.5f, 1.5f);
                }

                // Add crimson gore chunks
                for (int i = 0; i < 12; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                    Dust gore = Dust.NewDustPerfect(Projectile.Center, DustID.Crimson, velocity, 0, default, Main.rand.NextFloat(2f, 4f));
                    gore.noGravity = true;
                }

                initialized = true;
            }

            // Grow animation
            if (growProgress < 1f)
            {
                growProgress += 1f / GrowTime;
                if (growProgress > 1f)
                    growProgress = 1f;
            }

            // Check if should retract
            bool shouldRetract = false;

            if (Main.player[Projectile.owner].GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<ShredderRiff>())
                Projectile.timeLeft++;
            else
                shouldRetract = true;

            // Start retracting near death
            if (Projectile.timeLeft <= RetractTime)
                shouldRetract = true;

            if (shouldRetract && growProgress > 0f)
            {
                growProgress -= 1.5f / RetractTime;
                if (growProgress <= 0f)
                {
                    growProgress = 0f;
                    Projectile.Kill();
                    return;
                }
            }

            int targetIndex = (int)Projectile.ai[0];
            if (targetIndex >= 0 && targetIndex < Main.maxPlayers && Main.player[targetIndex].active)
            {
                if (Main.player[targetIndex].GetRagnarokModPlayer().activeRiffType != RiffLoader.RiffType<ShredderRiff>())
                {
                    return;
                }
                Projectile.Center = Main.player[targetIndex].Center;
            }
            else
            {
                Projectile.Kill();
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Fleshy death sound
            SoundEngine.PlaySound(new("CalamityMod/Sounds/NPCKilled/PerfHiveDeath"), Projectile.Center);

            // Massive explosive blood burst on death
            for (int i = 0; i < 120; i++)
            {
                float speed = Main.rand.NextFloat(5f, 18f);
                Vector2 velocity = Main.rand.NextVector2Unit() * speed;
                Dust blood = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, velocity, 0, default, Main.rand.NextFloat(1.5f, 4f));
                blood.noGravity = Main.rand.NextBool();
                blood.fadeIn = Main.rand.NextFloat(1f, 2f);
            }

            // Add massive crimson sparks/gore chunks
            for (int i = 0; i < 40; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(15f, 15f);
                Dust spark = Dust.NewDustPerfect(Projectile.Center, DustID.Crimson, velocity, 0, default, Main.rand.NextFloat(2f, 4.5f));
                spark.noGravity = true;
            }

            // Add some blood droplets with gravity for ground splatter effect
            for (int i = 0; i < 30; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(10f, 8f);
                Dust droplet = Dust.NewDustPerfect(Projectile.Center, DustID.Blood, velocity, 0, default, Main.rand.NextFloat(2f, 3f));
                droplet.noGravity = false;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            // Use the Crimstone texture
            Texture2D fleshTexture = TextureAssets.Item[ItemID.CrimstoneBlock].Value;

            float time = Main.GameUpdateCount * 0.02f;

            // Apply easing to grow progress for smoother animation
            float easedGrowth = (float)Math.Pow(growProgress, 0.5f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int tendrilIndex = 0; tendrilIndex < TendrilCount; tendrilIndex++)
            {
                float baseAngle = MathHelper.TwoPi * tendrilIndex / TendrilCount;
                float seed = tendrilSeeds[tendrilIndex];

                // Base direction for this tendril
                Vector2 baseDir = new Vector2((float)Math.Cos(baseAngle), (float)Math.Sin(baseAngle));

                Vector2 prevPos = Projectile.Center;

                for (int segmentIndex = 0; segmentIndex < SegmentsPerTendril; segmentIndex++)
                {
                    float progress = segmentIndex / (float)SegmentsPerTendril + 0.01f;

                    // Skip segments beyond grow progress
                    if (progress > easedGrowth)
                        break;

                    // Organic movement with three layers for complexity
                    float twitch1 = (float)Math.Sin(time * 1.2f + seed + segmentIndex * 0.2f) * 4f;
                    float twitch2 = (float)Math.Sin(time * 1.8f - seed + segmentIndex * 0.3f) * 4f;
                    float twitch3 = (float)Math.Sin(time * 2.5f + seed * 1.5f + segmentIndex * 0.4f) * 4f;

                    float totalTwitch = (twitch1 + twitch2 + twitch3) * progress * easedGrowth; // scaled by growth

                    // Add random jitter for rough edges
                    float jitterX = (float)Math.Sin((seed + segmentIndex) * 17.3f) * 3f;
                    float jitterY = (float)Math.Cos((seed + segmentIndex) * 23.7f) * 3f;

                    // Rotate the direction for this segment
                    Vector2 segmentDir = baseDir.RotatedBy(totalTwitch);

                    // Add minimal length variation for slight organic feel
                    float lengthMult = 1f + (float)Math.Sin(time * 0.8f + seed + progress * MathHelper.Pi) * 0.05f;

                    Vector2 segmentPos = prevPos + segmentDir * SegmentSpacing * lengthMult * easedGrowth + new Vector2(jitterX, jitterY) * easedGrowth;

                    // Fade toward the tip
                    float alpha = (1f - progress * 0.2f) * easedGrowth;

                    // Thickness with random variation for lumpy organic look
                    float baseThickness = MathHelper.Lerp(36f, 8f, progress);
                    float thicknessVariation = (float)Math.Sin((seed + segmentIndex) * 11.4f) * 2f;
                    float thickness = (baseThickness + thicknessVariation) * easedGrowth;

                    // Draw segment connection
                    Vector2 diff = segmentPos - prevPos;
                    float rotation = diff.ToRotation();
                    float distance = diff.Length();

                    // Flesh colors - dark crimson, very organic
                    Color fleshColor = Color.Lerp(new Color(160, 20, 20), new Color(120, 15, 15), progress) * alpha;

                    // Draw texture chunks along the tendril with minimum step to prevent infinite loops
                    float step = Math.Max(thickness * 0.4f, 8f);
                    for (float d = 0; d < distance; d += step)
                    {
                        Vector2 pos = prevPos + diff * (d / distance);
                        float scale = thickness / fleshTexture.Width;

                        spriteBatch.Draw(fleshTexture, pos - Main.screenPosition, null,
                            fleshColor, rotation, fleshTexture.Size() / 2f, scale, SpriteEffects.None, 0f);
                    }

                    prevPos = segmentPos;
                }
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}