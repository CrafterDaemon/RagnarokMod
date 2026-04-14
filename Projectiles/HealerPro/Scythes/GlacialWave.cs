using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GlacialWave : ModProjectile
    {
        // ai[0] = wave index (0 = closest, 6 = furthest)
        // ai[1] = direction
        // ai[2] = target scale (set by GlacialSlam)
        // localAI[0] = timer
        // localAI[1] = grounded flag (1 = ground, 0 = air)

        private int Timer
        {
            get => (int)Projectile.localAI[0];
            set => Projectile.localAI[0] = value;
        }

        private bool Grounded => Projectile.localAI[1] == 1f;
        private int WaveIndex => (int)Projectile.ai[0];
        private float TargetScale => Projectile.ai[2];

        private const int WaveCount = 6;
        private const int ShrinkTime = 10;
        private const int BaseWidth = 166;
        private const int BaseHeight = 82;

        // Growth time: ALL waves burst upward fast.
        // The nearest wave is quickest. far waves are still fast but just barely slower
        private const int NearGrowFrames = 5;
        private const int FarGrowFrames = 9;

        private int GrowFrames
        {
            get
            {
                float t = WaveIndex / (float)(WaveCount - 1);
                return (int)MathHelper.Lerp(NearGrowFrames, FarGrowFrames, t);
            }
        }

        // Staggered delay: increases non-linearly so closer waves are tightly packed
        // distant waves will have a more dramatic pause between them.
        // delay(i) = BaseDelay * i^1.6 creates a "chasing shockwave" feel.
        private const float BaseDelay = 3.5f;
        private int SpawnDelay => (int)(BaseDelay * MathF.Pow(WaveIndex, 1.6f));

        // Hold frames: how long the spike stays at full size before shrinking.
        // Larger (farther) waves linger longer, so the eye can track their size.
        private const int NearHoldFrames = 8;
        private const int FarHoldFrames = 22;
        private int HoldFrames
        {
            get
            {
                float t = WaveIndex / (float)(WaveCount - 1);
                return (int)MathHelper.Lerp(NearHoldFrames, FarHoldFrames, t);
            }
        }

        // Total lifetime = SpawnDelay + GrowFrames + HoldFrames + ShrinkTime + small buffer
        private int NeededLifetime => SpawnDelay + GrowFrames + HoldFrames + ShrinkTime + 5;

        private bool initialized = false;

        public override void SetDefaults()
        {
            Projectile.width = BaseWidth;
            Projectile.height = BaseHeight;
            Projectile.friendly = true;
            // timeLeft is overridden in OnSpawn once we know the wave index.
            Projectile.timeLeft = 120;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.scale = 0.01f;
            Projectile.tileCollide = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.direction = (int)Projectile.ai[1];
            Projectile.spriteDirection = Projectile.direction;
            // Now that WaveIndex is readable, set the correct lifetime.
            Projectile.timeLeft = NeededLifetime;
        }

        public override void AI()
        {
            Timer++;

            if (!initialized)
            {
                initialized = true;
                DetectGround();
            }

            int activeTime = Timer - SpawnDelay;
            if (activeTime <= 0)
            {
                Projectile.scale = 0.01f;
                Projectile.friendly = false;
                return;
            }

            Projectile.friendly = true;

            // Save anchor before resizing
            Vector2 anchor = Grounded ? Projectile.Bottom : Projectile.Center;

            // --- Growth phase: fast EaseOutQuart burst upward ---
            // EaseOutQuart means the spike shoots up quickly and slows at the peak,
            // which reads as a sharp ice spike punching through the ground.
            float grownScale;
            if (activeTime <= GrowFrames)
            {
                float growT = (float)activeTime / GrowFrames;
                grownScale = TargetScale * EaseOutQuart(growT);
            }
            else
            {
                grownScale = TargetScale;
            }

            // --- Shrink envelope: only after hold period expires ---
            int timeIntoShrink = activeTime - GrowFrames - HoldFrames;
            if (timeIntoShrink > 0)
            {
                // timeLeft counts down naturally; use it for the shrink fraction.
                float shrinkT = (float)Projectile.timeLeft / ShrinkTime;
                Projectile.scale = grownScale * MathHelper.Clamp(shrinkT, 0f, 1f);
            }
            else
            {
                Projectile.scale = grownScale;
            }

            if (Projectile.scale <= 0f)
            {
                Projectile.Kill();
                return;
            }

            // Resize hitbox
            Projectile.width = (int)(BaseWidth * Projectile.scale);
            Projectile.height = Grounded
                ? (int)(BaseHeight * Projectile.scale)
                : (int)(BaseHeight * Projectile.scale * 2f);

            if (Grounded)
                Projectile.Bottom = anchor;
            else
                Projectile.Center = anchor;

            // Burst of tile dust exactly when the spike erupts (activeTime 1–3)
            if (Grounded && activeTime >= 1 && activeTime <= 3)
                SpawnTileDust(Projectile.Bottom, 5);

            // Sound: plays the moment the spike pops up, not at spawn.
            // Pitch rises with wave index so far spikes sound sharper/higher —
            // reinforcing the sense that they're bigger impacts.
            if (activeTime == 1 && Main.netMode != NetmodeID.Server)
            {
                float volumeFalloff = 0.95f - (WaveIndex / (float)WaveCount) * 0.35f;
                float pitch = -0.5f + WaveIndex * 0.12f;   // climbs from low to bright
                SoundEngine.PlaySound(SoundID.Item62 with
                {
                    Volume = volumeFalloff,
                    Pitch = pitch
                }, Projectile.Center);
            }
        }
        private static float EaseOutQuart(float t)
        {
            float u = 1f - t;
            return 1f - u * u * u * u;
        }

        private void DetectGround()
        {
            Point tile = Projectile.Center.ToTileCoordinates();
            for (int y = -1; y <= 4; y++)
            {
                if (IsSolidOrPlatform(tile.X, tile.Y + y))
                {
                    Projectile.Bottom = new Vector2(Projectile.Center.X, (tile.Y + y) * 16f);
                    Projectile.localAI[1] = 1f;
                    return;
                }
            }
            Projectile.localAI[1] = 0f;
        }

        private static bool IsSolidOrPlatform(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]);
        }

        private static void SpawnTileDust(Vector2 position, int count)
        {
            Point pt = position.ToTileCoordinates();
            for (int x = -1; x <= 1; x++)
            {
                int tx = pt.X + x;
                int ty = pt.Y;
                Tile tile = Framing.GetTileSafely(tx, ty);
                if (!tile.HasTile) continue;

                for (int i = 0; i < count; i++)
                {
                    int d = WorldGen.KillTile_MakeTileDust(tx, ty, tile);
                    if (d < 0 || d >= Main.maxDust) continue;
                    Main.dust[d].velocity = new Vector2(
                        Main.rand.NextFloat(-4f, 4f),
                        Main.rand.NextFloat(-7f, -2f));
                    Main.dust[d].scale *= Main.rand.NextFloat(1.2f, 1.8f);
                    Main.dust[d].noGravity = false;
                }
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.rand.NextBool(3))
                target.AddBuff(BuffID.Frostburn, 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Vector2 bottomOrigin = new Vector2(tex.Width / 2f, tex.Height);
            Vector2 topOrigin = new Vector2(tex.Width / 2f, 0f);

            SpriteEffects fx = Projectile.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            if (Grounded)
            {
                Vector2 drawPos = Projectile.Bottom - Main.screenPosition;
                Main.EntitySpriteDraw(tex, drawPos, null, lightColor,
                    Projectile.rotation, bottomOrigin, Projectile.scale, fx, 0);
            }
            else
            {
                Vector2 meetPos = Projectile.Center - Main.screenPosition;

                Main.EntitySpriteDraw(tex, meetPos, null, lightColor,
                    Projectile.rotation, bottomOrigin, Projectile.scale, fx, 0);

                Main.EntitySpriteDraw(tex, meetPos, null, lightColor,
                    Projectile.rotation, topOrigin, Projectile.scale,
                    fx | SpriteEffects.FlipVertically, 0);
            }

            return false;
        }
    }
}