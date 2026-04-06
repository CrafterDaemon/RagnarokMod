using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    /// <summary>
    /// Single-projectile fractal laser pattern.
    /// The entire visual (two tiers of 5 rays each) is drawn in PreDraw.
    /// Damage is dealt by checking NPC intersections against each line segment in AI().
    /// No child projectiles are spawned at any point.
    /// </summary>
    public class FractalOrb : ModProjectile, ILocalizedModType
    {
        // Maximum arm lengths once fully grown
        private const float MaxTier1Length = 320f;
        private const float MaxTier2Length = 160f;
        private const float MaxTier3Length = 80f;
        private const int Arms = 5;
        private const float ArmAngle = MathHelper.TwoPi / Arms; // 72 degrees

        // Grow duration in frames. each tier blooms sequentially
        private const int GrowTime1 = 20;
        private const int GrowTime2 = 20;
        private const int GrowTime3 = 20;

        // Fade out over the last N frames
        private const int FadeTime = 30;

        // Tier 3 uses fewer arms than tier 1/2 to reduce draw calls and intersection checks.
        // 5 arms -> 5*5*3 = 75 segments instead of 5*5*5 = 125.
        private const int Tier3Arms = 3;

        // Maximum possible radius of the pattern -- used to pre-filter NPCs
        private const float MaxPatternRadius = MaxTier1Length + MaxTier2Length + MaxTier3Length;

        // Only run intersection checks every N frames to reduce CPU load.
        // Damage is gated by _hitCooldowns anyway so skipping frames doesn't affect feel.
        private const int IntersectCheckInterval = 3;

        private float Tier1Length => MaxTier1Length * MathHelper.Clamp(_age / (float)GrowTime1, 0f, 1f);
        private float Tier2Length => MaxTier2Length * MathHelper.Clamp((_age - GrowTime1) / (float)GrowTime2, 0f, 1f);
        private float Tier3Length => MaxTier3Length * MathHelper.Clamp((_age - GrowTime1 - GrowTime2) / (float)GrowTime3, 0f, 1f);

        // 0 = invisible, 1 = fully visible
        private float Alpha => MathHelper.Clamp(Projectile.timeLeft / (float)FadeTime, 0f, 1f);

        private int _age = 0;
        private int _intersectTimer = 0;

        // Damage cooldown per NPC. keyed by whoAmI
        private readonly Dictionary<int, int> _hitCooldowns = new();

        // Visual
        private float _spin = 0f;

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 81;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            _spin += 0.04f; // slow steady rotation so the pattern visually flows
            _age++;

            // Follow the player
            Projectile.Center = Main.player[Projectile.owner].Center;
            Projectile.velocity = Vector2.Zero;

            // Dust along the orb center
            if (Main.rand.NextBool(2))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(12f, 12f),
                    DustID.BlueFairy, Vector2.Zero, 0, default, Main.rand.NextFloat(0.8f, 1.4f));
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.3f, 0.8f);

            // Tick down cooldowns
            var keys = new List<int>(_hitCooldowns.Keys);
            foreach (int k in keys)
            {
                _hitCooldowns[k]--;
                if (_hitCooldowns[k] <= 0)
                    _hitCooldowns.Remove(k);
            }

            // Only run intersection checks every IntersectCheckInterval frames --
            // _hitCooldowns gate actual damage so throttling this doesn't affect hit feel
            _intersectTimer++;
            if (_intersectTimer >= IntersectCheckInterval)
            {
                _intersectTimer = 0;
                for (int n = 0; n < Main.maxNPCs; n++)
                {
                    NPC npc = Main.npc[n];
                    if (!npc.active || npc.friendly || npc.dontTakeDamage) continue;
                    if (_hitCooldowns.ContainsKey(npc.whoAmI)) continue;

                    // Radius pre-filter: skip expensive geometry if NPC is outside max pattern bounds
                    if (Vector2.DistanceSquared(npc.Center, Projectile.Center) >
                        (MaxPatternRadius + npc.width) * (MaxPatternRadius + npc.width)) continue;

                    if (IntersectsPattern(npc))
                    {
                        _hitCooldowns[npc.whoAmI] = 20; // frames between hits on the same NPC

                        if (Main.myPlayer == Projectile.owner)
                        {
                            // Route through player.ApplyDamageToNPC so hits register on the DPS counter
                            Player owner = Main.player[Projectile.owner];
                            int dmg = (int)(Projectile.damage * 0.6f);
                            NPC.HitInfo hitInfo = npc.CalculateHitInfo(dmg, 1, false,
                                Projectile.knockBack, Projectile.DamageType, false);
                            owner.ApplyDamageToNPC(npc, hitInfo.Damage, hitInfo.Knockback,
                                hitInfo.HitDirection, hitInfo.Crit);
                            npc.AddBuff(ModContent.BuffType<Nightwither>(), 180);

                            // 33% chance to generate a Stratus Starburst -- must be here since
                            // ApplyDamageToNPC does not trigger OnHitNPC
                            if (Main.rand.NextFloat() <= 0.33f)
                            {
                                owner.Calamity().StratusStarburst++;
                                owner.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(
                                    owner.Calamity().StratusStarburstResetTimer, 600);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if the NPC's hitbox intersects any segment in the fractal pattern.
        /// </summary>
        private bool IntersectsPattern(NPC npc)
        {
            Rectangle npcRect = npc.getRect();

            for (int i = 0; i < Arms; i++)
            {
                float baseAngle = _spin + ArmAngle * i;

                // Tier 1 segment: center -> tip1
                Vector2 tip1 = Projectile.Center + baseAngle.ToRotationVector2() * Tier1Length;
                if (SegmentIntersectsRect(Projectile.Center, tip1, npcRect)) return true;

                // Tier 2 segments: tip1 -> 5 sub-arms
                for (int j = 0; j < Arms; j++)
                {
                    float subAngle = baseAngle + ArmAngle * j;
                    Vector2 tip2 = tip1 + subAngle.ToRotationVector2() * Tier2Length;
                    if (SegmentIntersectsRect(tip1, tip2, npcRect)) return true;

                    // Tier 3 uses Tier3Arms instead of Arms to reduce intersection cost
                    float tier3AngleStep = MathHelper.TwoPi / Tier3Arms;
                    for (int k = 0; k < Tier3Arms; k++)
                    {
                        float subSubAngle = subAngle + tier3AngleStep * k;
                        Vector2 tip3 = tip2 + subSubAngle.ToRotationVector2() * Tier3Length;
                        if (SegmentIntersectsRect(tip2, tip3, npcRect)) return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Separating axis test: does segment AB intersect rectangle r?
        /// </summary>
        private static bool SegmentIntersectsRect(Vector2 a, Vector2 b, Rectangle r)
        {
            // Expand check: if either endpoint is inside, trivially true
            if (r.Contains((int)a.X, (int)a.Y) || r.Contains((int)b.X, (int)b.Y))
                return true;

            // Test segment against each of the 4 rectangle edges
            Vector2 tl = new(r.Left, r.Top);
            Vector2 tr = new(r.Right, r.Top);
            Vector2 bl = new(r.Left, r.Bottom);
            Vector2 br = new(r.Right, r.Bottom);

            return SegmentsIntersect(a, b, tl, tr)
                || SegmentsIntersect(a, b, tr, br)
                || SegmentsIntersect(a, b, br, bl)
                || SegmentsIntersect(a, b, bl, tl);
        }

        private static bool SegmentsIntersect(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            Vector2 ab = b - a, ac = c - a, ad = d - a;
            Vector2 cd = d - c, ca = a - c, cb = b - c;
            float d1 = Cross(ab, ac), d2 = Cross(ab, ad);
            float d3 = Cross(cd, ca), d4 = Cross(cd, cb);
            if (((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) &&
                ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0)))
                return true;
            return false;
        }

        private static float Cross(Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D glow = TextureAssets.Projectile[ModContent.ProjectileType<AuricGreatscytheBolt>()].Value;
            Vector2 glowOrig = glow.Size() * 0.5f;
            float glowW = glow.Width;
            float glowH = glow.Height;
            float time = Main.GlobalTimeWrappedHourly;

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Orb core glow
            float alpha = Alpha;
            float pulse = 1f + MathF.Sin(time * 6f) * 0.1f;
            DrawGlowCircle(sb, glow, glowOrig, glowW, glowH,
                Projectile.Center - Main.screenPosition,
                new Color(60, 140, 255) * 0.9f * alpha, 40f * pulse);
            DrawGlowCircle(sb, glow, glowOrig, glowW, glowH,
                Projectile.Center - Main.screenPosition,
                Color.White * 0.6f * alpha, 18f * pulse);

            // Draw fractal arms
            for (int i = 0; i < Arms; i++)
            {
                float baseAngle = _spin + ArmAngle * i;
                Vector2 tip1 = Projectile.Center + baseAngle.ToRotationVector2() * Tier1Length;

                // Tier 1 laser -- brightest
                DrawLaser(sb, glow, glowOrig, glowW, glowH,
                    Projectile.Center, tip1,
                    new Color(40, 120, 255) * alpha,
                    new Color(180, 220, 255) * alpha,
                    22f, 9f);

                DrawGlowCircle(sb, glow, glowOrig, glowW, glowH,
                    tip1 - Main.screenPosition,
                    new Color(120, 190, 255) * 0.9f * alpha, 28f);

                for (int j = 0; j < Arms; j++)
                {
                    float subAngle = baseAngle + ArmAngle * j;
                    Vector2 tip2 = tip1 + subAngle.ToRotationVector2() * Tier2Length;

                    // Tier 2 laser -- slightly dimmer
                    DrawLaser(sb, glow, glowOrig, glowW, glowH,
                        tip1, tip2,
                        new Color(20, 80, 220) * alpha,
                        new Color(120, 180, 255) * alpha,
                        12f, 5f);

                    DrawGlowCircle(sb, glow, glowOrig, glowW, glowH,
                        tip2 - Main.screenPosition,
                        new Color(80, 150, 255) * 0.7f * alpha, 16f);

                    // Tier 3 uses Tier3Arms to reduce draw calls (75 segments vs 125)
                    float t3AngleStep = MathHelper.TwoPi / Tier3Arms;
                    for (int k = 0; k < Tier3Arms; k++)
                    {
                        float subSubAngle = subAngle + t3AngleStep * k;
                        Vector2 tip3 = tip2 + subSubAngle.ToRotationVector2() * Tier3Length;

                        DrawLaser(sb, glow, glowOrig, glowW, glowH,
                            tip2, tip3,
                            new Color(10, 50, 180) * alpha,
                            new Color(80, 140, 255) * alpha,
                            6f, 2.5f);

                        DrawGlowCircle(sb, glow, glowOrig, glowW, glowH,
                            tip3 - Main.screenPosition,
                            new Color(60, 120, 255) * 0.5f * alpha, 9f);
                    }
                }
            }

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        /// <summary>
        /// Draws a laser between two world-space points as two overlapping glow ribbons
        /// (wide soft outer + narrow bright inner).
        /// </summary>
        private static void DrawLaser(SpriteBatch sb, Texture2D glow, Vector2 glowOrig,
            float glowW, float glowH,
            Vector2 from, Vector2 to,
            Color outerColor, Color innerColor,
            float outerWidth, float innerWidth)
        {
            Vector2 diff = to - from;
            float dist = diff.Length();
            if (dist < 0.1f) return;
            float rot = diff.ToRotation() - MathHelper.PiOver2;
            Vector2 mid = (from + to) * 0.5f - Main.screenPosition;

            sb.Draw(glow, mid, null, outerColor * 0.45f,
                rot, glowOrig, new Vector2(outerWidth / glowW, dist / glowH),
                SpriteEffects.None, 0f);
            sb.Draw(glow, mid, null, innerColor * 0.7f,
                rot, glowOrig, new Vector2(innerWidth / glowW, dist / glowH),
                SpriteEffects.None, 0f);
        }

        private static void DrawGlowCircle(SpriteBatch sb, Texture2D glow, Vector2 glowOrig,
            float glowW, float glowH, Vector2 screenPos, Color color, float radius)
        {
            sb.Draw(glow, screenPos, null, color,
                0f, glowOrig, new Vector2(radius / glowW, radius / glowH),
                SpriteEffects.None, 0f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 9f),
                    0, default, Main.rand.NextFloat(1f, 2f));
                d.noGravity = true;
            }
        }
    }
}