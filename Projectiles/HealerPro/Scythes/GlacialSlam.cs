using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class GlacialSlam : ModProjectile, ILocalizedModType
    {
        public override string Texture => "RagnarokMod/Items/HealerItems/Scythes/GlacialHarvester";

        // Windup is long and slow — the "charge up" telegraphing a heavy strike.
        // The actual downswing (SwingDuration) is short and snappy.
        private const int WindupFrames = 22;
        private const int SwingDuration = 12;   // fast — the slam itself is a snap
        private const int ImpactFreeze = 35;
        private const float ArmLength = 72f;

        // Narrowed arc — this is a straight-down SLAM, not a wide sweep.
        // Pi * 2/3 = 120 degrees, aimed downward.
        private const float SweepArc = MathHelper.Pi;

        // The active hit window starts late and ends early in the swing —
        // only the fast downswing portion deals damage.
        private const float WindupEnd = 0.15f;
        private const float FollowthroughStart = 0.90f;

        // --- Wave configuration (mirrored in GlacialWave) ---
        private const int WaveCount = 6;
        private const int BaseWaveWidth = 166;
        private const float NearScale = 0.55f;  // near waves are modest
        private const float FarScale = 3.8f;    // far waves are massive — exponential feel
        private const float AirScaleMult = 2f / 3f;
        private const float OverlapFactor = 0.55f;

        // ai[1] = frame counter, ai[2] = cocked angle
        // localAI[0]: 0 = pre-swing, 1 = active, 2 = freeze
        // localAI[1] = freeze counter

        private Player Owner => Main.player[Projectile.owner];
        private bool IsActive => Projectile.localAI[0] == 1f;
        private bool IsFrozen => Projectile.localAI[0] == 2f;

        private bool CanHit
        {
            get => Projectile.friendly;
            set => Projectile.friendly = value;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 10;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 54;
            Projectile.friendly = false;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = WindupFrames + SwingDuration + ImpactFreeze + 10;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Player owner = Owner;
            if (!owner.active || owner.dead)
            {
                Projectile.Kill();
                return;
            }

            owner.heldProj = Projectile.whoAmI;

            if (IsFrozen)
            {
                HandleImpactFreeze(owner);
                return;
            }

            if (!IsActive)
                BeginSwing(owner);
            else
                HandleSwing(owner);
        }

        private void BeginSwing(Player owner)
        {
            // Cocked angle: raise the scythe up and back over the shoulder.
            float cockedAngle = -(MathHelper.PiOver2 + (MathHelper.PiOver4 * 1.5f) * owner.direction);

            Projectile.ai[1] = 0f;
            Projectile.ai[2] = cockedAngle;
            Projectile.localAI[0] = 1f;
            Projectile.netUpdate = true;
        }

        private void HandleSwing(Player owner)
        {
            Projectile.ai[1]++;
            float cockedAngle = Projectile.ai[2];

            // --- WINDUP PHASE ---
            // Slow, weighted pullback. Uses SmoothStep so it eases out at the peak.
            if (Projectile.ai[1] <= WindupFrames)
            {
                float windupT = Projectile.ai[1] / (float)WindupFrames;
                // Pullback reaches 0.25 radians behind the cocked angle at the peak.
                float pullback = MathHelper.SmoothStep(0f, 0.25f, windupT);
                float angle = cockedAngle - pullback * owner.direction;

                PositionScythe(owner, angle);
                CanHit = false;

                // Low rumble/charge sound on the first windup frame
                if (Projectile.ai[1] == 1f)
                    SoundEngine.PlaySound(SoundID.Item71 with { Pitch = -0.5f, Volume = 0.7f }, owner.Center);

                // Near the end of the windup, play a second "load" cue
                if (Projectile.ai[1] == WindupFrames - 3)
                    SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 0.0f, Volume = 0.85f }, owner.Center);

                return;
            }

            float swingTime = Projectile.ai[1] - WindupFrames;
            float progress = swingTime / SwingDuration;

            float startAngle = cockedAngle;
            float endAngle = startAngle + SweepArc * owner.direction;

            // EaseInQuart on the downswing: slow start then FAST snap at the end.
            // This is the key to making the hit feel heavy — the scythe accelerates
            // into the ground rather than moving at a constant rate.
            float easedT = EaseInQuart(Math.Max(0f, (progress - WindupEnd) / (1f - WindupEnd)));
            float curAngle = MathHelper.Lerp(startAngle, endAngle, easedT);

            PositionScythe(owner, curAngle);

            CanHit = progress >= WindupEnd && progress <= FollowthroughStart;

            Lighting.AddLight(Projectile.Center, CanHit
                ? new Vector3(0.7f, 0.35f, 0.9f)
                : new Vector3(0.2f, 0.1f, 0.3f));

            // The impact whoosh — plays at the beginning of the actual downswing
            if (swingTime == 1f)
                SoundEngine.PlaySound(SoundID.Item71 with { Pitch = 0.5f, Volume = 1.1f }, owner.Center);

            if (swingTime >= SwingDuration)
                FinishSwing(owner);
        }

        // Quartic ease-in: t^4 — slow start, explosive finish.
        private static float EaseInQuart(float t) => t * t * t * t;

        private void PositionScythe(Player owner, float angle)
        {
            Projectile.Center = owner.Center + angle.ToRotationVector2() * ArmLength;
            Projectile.rotation = owner.direction == 1
                ? angle + MathHelper.PiOver4
                : angle - MathHelper.PiOver4 - MathHelper.Pi;

            owner.itemRotation = (Projectile.Center - owner.MountedCenter).ToRotation();
            if (owner.direction == -1)
                owner.itemRotation += MathHelper.Pi;
            owner.itemTime = 2;
            owner.itemAnimation = 2;
        }

        private void FinishSwing(Player owner)
        {
            CanHit = false;

            bool hitTile = CheckForGround(owner.Bottom, out int groundTileY);

            SpawnWaves(owner, hitTile, groundTileY);

            if (hitTile)
            {
                Projectile.localAI[0] = 2f;
                Projectile.localAI[1] = 0f;

                Vector2 groundPos = new Vector2(Projectile.Center.X, groundTileY * 16f);
                Point groundTile = groundPos.ToTileCoordinates();

                for (int x = -2; x <= 2; x++)
                {
                    int tx = groundTile.X + x;
                    int ty = groundTile.Y;
                    Tile tile = Framing.GetTileSafely(tx, ty);
                    if (!tile.HasTile) continue;

                    for (int i = 0; i < 5; i++)
                    {
                        int d = WorldGen.KillTile_MakeTileDust(tx, ty, tile);
                        if (d < 0 || d >= Main.maxDust) continue;
                        Main.dust[d].velocity = new Vector2(
                            Main.rand.NextFloat(-5f, 5f),
                            Main.rand.NextFloat(-8f, -2f));
                        Main.dust[d].scale *= Main.rand.NextFloat(1.2f, 2.2f);
                        Main.dust[d].noGravity = false;
                    }
                }

                SoundEngine.PlaySound(SoundID.Item27 with
                {
                    Pitch = -0.4f,
                    Volume = 1.2f
                }, groundPos);

                if (Main.LocalPlayer.Distance(Projectile.Center) < 1000f)
                    Main.LocalPlayer.Calamity().GeneralScreenShakePower = 8f;
            }
            else
            {
                Projectile.Kill();
            }

            // TODO: create GlacialHarvesterCooldown : Cooldown and uncomment
            // owner.AddCooldown<GlacialHarvesterCooldown>(60 * 3);
        }

        private void HandleImpactFreeze(Player owner)
        {
            Projectile.localAI[1]++;
            owner.itemTime = 2;
            owner.itemAnimation = 2;

            if (Projectile.localAI[1] % 12 == 0 && Projectile.localAI[1] < ImpactFreeze * 0.6f)
            {
                if (CheckForGround(Projectile.Center, out int ty))
                {
                    int tx = Projectile.Center.ToTileCoordinates().X;
                    Tile tile = Framing.GetTileSafely(tx, ty);
                    if (tile.HasTile)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int d = WorldGen.KillTile_MakeTileDust(tx, ty, tile);
                            if (d >= 0 && d < Main.maxDust)
                            {
                                Main.dust[d].velocity = new Vector2(
                                    Main.rand.NextFloat(-2f, 2f),
                                    Main.rand.NextFloat(-3f, -0.5f));
                            }
                        }
                    }
                }
            }

            if (Projectile.localAI[1] >= ImpactFreeze)
                Projectile.Kill();
        }

        // Exponential scale curve: waves grow dramatically toward the far end.
        // index 0 (nearest) = NearScale, index 6 (furthest) = FarScale.
        // Using an exponent of 2.2 gives a strong but not runaway curve.
        private static float GetTargetScale(int index, bool grounded)
        {
            float t = index / (float)(WaveCount - 1);
            float expT = MathF.Pow(t, 2.2f);                      // exponential ramp
            float scale = MathHelper.Lerp(NearScale, FarScale, expT);
            return grounded ? scale : scale * AirScaleMult;
        }

        private void SpawnWaves(Player owner, bool grounded, int groundTileY)
        {
            float[] scales = new float[WaveCount];
            float[] offsets = new float[WaveCount];

            for (int i = 0; i < WaveCount; i++)
                scales[i] = GetTargetScale(i, grounded);

            offsets[0] = 0f;
            for (int i = 1; i < WaveCount; i++)
            {
                float prevWidth = BaseWaveWidth * scales[i - 1];
                float thisWidth = BaseWaveWidth * scales[i];
                float avgWidth = (prevWidth + thisWidth) / 2f;
                offsets[i] = offsets[i - 1] + avgWidth * OverlapFactor;
            }

            // Spawn furthest first for draw order (back-to-front)
            for (int i = WaveCount - 1; i >= 0; i--)
            {
                float xOff = owner.direction * offsets[i];
                Vector2 pos;

                if (grounded)
                    pos = new Vector2(owner.Center.X + xOff, groundTileY * 16f);
                else
                    pos = Projectile.Center + new Vector2(xOff, 0f);

                Projectile.NewProjectileDirect(
                    Projectile.GetSource_FromThis(),
                    pos,
                    Vector2.Zero,
                    ModContent.ProjectileType<GlacialWave>(),
                    (int)(Projectile.damage * (grounded ? 1.5f : 1)),
                    Projectile.knockBack + (grounded ? 4 : -6),
                    Projectile.owner,
                    i,               // ai[0] = wave index
                    owner.direction, // ai[1] = direction
                    scales[i]);      // ai[2] = target scale
            }
        }

        private static bool CheckForGround(Vector2 position, out int groundTileY)
        {
            Point tile = position.ToTileCoordinates();
            for (int y = 0; y <= 6; y++)
            {
                if (IsSolidOrPlatform(tile.X, tile.Y + y))
                {
                    groundTileY = tile.Y + y;
                    return true;
                }
            }
            groundTileY = 0;
            return false;
        }

        private static bool IsSolidOrPlatform(int x, int y)
        {
            Tile tile = Framing.GetTileSafely(x, y);
            return tile.HasTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Owner;
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = tex.Size() / 2f;

            SpriteEffects fx = owner.direction == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Color trailColor = new Color(66, 189, 181);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < Projectile.oldPos.Length; i++)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    continue;

                float fade = 1f - (i / (float)Projectile.oldPos.Length);
                Vector2 trailPos = Projectile.oldPos[i] + (Projectile.Size / 2f) - Main.screenPosition;
                sb.Draw(tex, trailPos, null, trailColor * fade * 0.4f,
                    Projectile.oldRot[i], origin, 1.5f, fx, 0f);
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, owner.gfxOffY);
            if (IsFrozen)
            {
                float shake = MathHelper.Max(0f, 1f - Projectile.localAI[1] / (ImpactFreeze * 0.4f));
                drawPos += new Vector2(
                    Main.rand.NextFloat(-2f, 2f) * shake,
                    Main.rand.NextFloat(-1f, 1f) * shake);
            }

            sb.Draw(tex, drawPos, null, lightColor, Projectile.rotation, origin, 1.5f, fx, 0f);

            return false;
        }
    }
}