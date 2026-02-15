using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public struct SegmentSnapshot
    {
        public Vector2 Center;
        public float Rotation;
        public int SegmentType;
        public float Opacity;

        public SegmentSnapshot(Vector2 center, float rotation, int segmentType, float opacity)
        {
            Center = center;
            Rotation = rotation;
            SegmentType = segmentType;
            Opacity = opacity;
        }
    }

    public class MiniDoGDeathRenderer : ModProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/InvisibleProj";

        // Keyed by the head's whoAmI at time of death
        public static Dictionary<int, List<SegmentSnapshot>> SegmentSnapshots = new();

        private ref float MyKey => ref Projectile.ai[0];

        // Tracks which segment index has been exploded up to
        private int lastExploded = -1;

        // Cached textures — same ones MiniDoGHead uses
        private Asset<Texture2D> bodyTexAsset;
        private Asset<Texture2D> tailTexAsset;
        private Asset<Texture2D> bodyGlowAsset;
        private Asset<Texture2D> tailGlowAsset;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            // 4 frames per segment × 24 segments + a little padding
            Projectile.timeLeft = (24 * 4) + 30;
        }

        public override void AI()
        {
            if (!SegmentSnapshots.TryGetValue((int)MyKey, out var snapshots))
            {
                Projectile.Kill();
                return;
            }

            // How many frames have elapsed since spawn
            int elapsed = ((24 * 4) + 30) - Projectile.timeLeft;
            int targetSegment = elapsed / 4;

            // Fire explosion dust for any newly reached segments
            while (lastExploded < targetSegment && lastExploded + 1 < snapshots.Count)
            {
                lastExploded++;
                SpawnExplosionDust(snapshots[lastExploded].Center);
            }

            // All segments done
            if (lastExploded >= snapshots.Count - 1 && elapsed > snapshots.Count * 4)
            {
                SegmentSnapshots.Remove((int)MyKey);
                Projectile.Kill();
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (!SegmentSnapshots.TryGetValue((int)MyKey, out var snapshots))
                return false;

            // Lazy-load textures
            bodyTexAsset ??= ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/VoidEaterMarionetteBody");
            tailTexAsset ??= ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/VoidEaterMarionetteTail");
            bodyGlowAsset ??= ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/VoidEaterMarionetteBodyGlow");
            tailGlowAsset ??= ModContent.Request<Texture2D>("CalamityMod/Projectiles/Summon/VoidEaterMarionetteTailGlow");

            int elapsed = ((24 * 4) + 30) - Projectile.timeLeft;

            // Draw back to front, skipping any already exploded
            for (int i = snapshots.Count - 1; i >= 0; i--)
            {
                // This segment has already exploded — don't draw it
                if (i <= lastExploded)
                    continue;

                var snap = snapshots[i];

                // Fade out the segment just before its explosion frame
                int framesUntilExplosion = (i * 4) - elapsed;
                float opacity = MathHelper.Clamp(framesUntilExplosion / 4f, 0f, 1f) * snap.Opacity;

                var tex = snap.SegmentType == 0 ? bodyTexAsset.Value : tailTexAsset.Value;
                var glowTex = snap.SegmentType == 0 ? bodyGlowAsset.Value : tailGlowAsset.Value;

                var color = Lighting.GetColor(snap.Center.ToTileCoordinates()) * opacity;

                Main.spriteBatch.Draw(
                    tex,
                    snap.Center - Main.screenPosition,
                    null,
                    color,
                    snap.Rotation,
                    tex.Size() / 2,
                    Projectile.scale,
                    SpriteEffects.None, 0);

                Main.spriteBatch.Draw(
                    glowTex,
                    snap.Center - Main.screenPosition,
                    null,
                    Color.White * opacity,
                    snap.Rotation,
                    glowTex.Size() / 2,
                    Projectile.scale,
                    SpriteEffects.None, 0);
            }

            return false;
        }

        private static void SpawnExplosionDust(Vector2 center)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(8f, 8f);
                Dust dust = Dust.NewDustDirect(center, 16, 16, DustID.ShadowbeamStaff,
                    speed.X, speed.Y, 100, default, 1.5f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(5f, 5f);
                Dust dust = Dust.NewDustDirect(center, 16, 16, DustID.Vortex,
                    speed.X, speed.Y, 100, default, 1.2f);
                dust.noGravity = true;
            }
        }
    }
}