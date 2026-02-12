using CalamityMod;
using CalamityMod.Projectiles.BaseProjectiles;
using CalamityMod.Sounds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class MiniDoGHead : BaseWormProjectile
    {
        #region Texture
        public override string Texture => "CalamityMod/Projectiles/Summon/VoidEaterMarionetteHead";
        public override string GlowTexture => "CalamityMod/Projectiles/Summon/VoidEaterMarionetteHeadGlow";

        public override List<string> SegmentTextures => new()
        {
            "CalamityMod/Projectiles/Summon/VoidEaterMarionetteBody",
            "CalamityMod/Projectiles/Summon/VoidEaterMarionetteTail"
        };

        public override int SegmentCount => 16;
        private float fadeInTimer = 0f;
        private int orbitTime = 0;

        public override List<float> SegmentTypePositionOffsets => new()
        {
            54, // Head
            38, // Body
            52  // Tail
        };
        #endregion

        // States
        private const float STATE_ORBIT = 0f;
        private const float STATE_HOMING = 1f;

        private ref float State => ref Projectile.ai[1];
        private ref float OrbitAngle => ref Projectile.ai[0];

        public float JawOpeningAmount = 0f;
        private bool spawned = false;

        private Asset<Texture2D> JawsAsset;
        private Asset<Texture2D> JawGlowAsset;
        private Asset<Texture2D> GlowTexAsset;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Spawn segments once
            if (!spawned && Projectile.owner == Main.myPlayer)
            {
                spawned = true;
                // BaseWormProjectile handles segment spawning via SegmentCount
                // so we just need to make sure segments are initialized
            }

            if (State == STATE_ORBIT)
            {
                HandleOrbit(player);
            }
            else
            {
                HandleHoming();
            }

            UpdateSegments();
        }

        private void HandleOrbit(Player player)
        {
            orbitTime++;
            fadeInTimer = Math.Min(fadeInTimer + 1f, 90f);
            Projectile.Opacity = fadeInTimer / 180f;
            foreach (var seg in Segments)
                seg.Opacity = Projectile.Opacity;

            OrbitAngle += 0.15f * player.direction;

            float orbitRadius = 120f;
            Vector2 desiredCenter = player.Center + new Vector2(
                (float)Math.Cos(OrbitAngle) * orbitRadius,
                (float)Math.Sin(OrbitAngle) * orbitRadius
            );

            // Let UpdateSegments handle position via velocity
            Projectile.velocity = desiredCenter - Projectile.Center;
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            JawOpeningAmount = MathHelper.Lerp(JawOpeningAmount, 0f, 0.1f);
        }


        private void HandleHoming()
        {

            NPC npc = Projectile.FindNearestNPCIgnoreTiles(800);
            if (npc != null)
            {
                // Open jaws as we home in
                JawOpeningAmount = MathHelper.Lerp(JawOpeningAmount, 0.75f, 0.1f);
                Projectile.HomeInOnTarget(npc, 25f, 0.3f);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
            else
            {
                JawOpeningAmount = MathHelper.Lerp(JawOpeningAmount, 0f, 0.1f);
                Projectile.timeLeft = Math.Min(Projectile.timeLeft, 60);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (State == STATE_HOMING)
            {
                TriggerDeathSequence();
                Projectile.Kill();
            }
        }

        private void TriggerDeathSequence()
        {
            SoundEngine.PlaySound(new SoundStyle("CalamityMod/Sounds/NPCKilled/DevourerDeath"), Projectile.Center);
            SpawnExplosionDust(Projectile.Center);

            if (Projectile.owner == Main.myPlayer)
            {
                int key = Projectile.whoAmI;
                MiniDoGDeathRenderer.SegmentSnapshots[key] = Segments
                    .Select(s => new SegmentSnapshot(s.Center, s.rotation, s.segmentType, s.Opacity))
                    .ToList();

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MiniDoGDeathRenderer>(),
                    0, 0f,
                    Projectile.owner,
                    key
                );
            }
        }

        // OnKill stays but only as a fallback for timeout death
        public override void OnKill(int timeLeft)
        {
            if (State == STATE_HOMING && !MiniDoGDeathRenderer.SegmentSnapshots.ContainsKey(Projectile.whoAmI))
            {
                TriggerDeathSequence();
            }
        }

        public void ReleaseToHoming()
        {
            State = STATE_HOMING;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.damage *= (Math.Min(orbitTime, 300) / 30);
        }

        private static void SpawnExplosionDust(Vector2 center)
        {
            for (int i = 0; i < 40; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(8f, 8f);
                Dust dust = Dust.NewDustDirect(
                    center, 16, 16,
                    DustID.ShadowbeamStaff, speed.X, speed.Y, 100, default, 1.5f);
                dust.noGravity = true;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(5f, 5f);
                Dust dust = Dust.NewDustDirect(
                    center, 16, 16,
                    DustID.Vortex, speed.X, speed.Y, 100, default, 1.2f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Draw segments back to front
            for (int i = Segments.Count - 1; i >= 0; i--)
                DrawSegment(ref lightColor, Segments[i]);

            var jawTex = CalamityUtils.GetTextureEfficient(ref JawsAsset,
                "CalamityMod/Projectiles/Summon/VoidEaterMarionetteJaw").Value;
            var jawGlowTex = CalamityUtils.GetTextureEfficient(ref JawGlowAsset,
                "CalamityMod/Projectiles/Summon/VoidEaterMarionetteJawGlow").Value;

            // Draw head
            Main.spriteBatch.Draw(
                TextureAssets.Projectile[Type].Value,
                Projectile.Center - Main.screenPosition,
                null, lightColor * Projectile.Opacity,
                Projectile.rotation,
                TextureAssets.Projectile[Type].Value.Size() / 2,
                Projectile.scale, // Scale down for "mini" version
                SpriteEffects.None, 1);

            // Left jaw
            Vector2 jawOffset = new Vector2(10, -18) * 0.5f;
            Main.spriteBatch.Draw(jawTex,
                Projectile.Center - jawOffset.RotatedBy(Projectile.rotation) - Main.screenPosition,
                null, lightColor * Projectile.Opacity,
                Projectile.rotation - JawOpeningAmount,
                jawTex.Size() / 2 - jawOffset,
                Projectile.scale,
                SpriteEffects.None, 1);

            // Only draw jaw glow when homing
            if (State == STATE_HOMING)
                Main.spriteBatch.Draw(jawGlowTex,
                    Projectile.Center - jawOffset.RotatedBy(Projectile.rotation) - Main.screenPosition,
                    null, Color.White * Projectile.Opacity,
                    Projectile.rotation - JawOpeningAmount,
                    jawTex.Size() / 2 - jawOffset,
                    Projectile.scale,
                    SpriteEffects.None, 1);

            // Right jaw (flipped)
            jawOffset.X *= -1;
            Main.spriteBatch.Draw(jawTex,
                Projectile.Center - jawOffset.RotatedBy(Projectile.rotation) - Main.screenPosition,
                null, lightColor * Projectile.Opacity,
                Projectile.rotation + JawOpeningAmount,
                jawTex.Size() / 2 - jawOffset,
                Projectile.scale,
                SpriteEffects.FlipHorizontally, 1);

            if (State == STATE_HOMING)
                Main.spriteBatch.Draw(jawGlowTex,
                    Projectile.Center - jawOffset.RotatedBy(Projectile.rotation) - Main.screenPosition,
                    null, Color.White * Projectile.Opacity,
                    Projectile.rotation + JawOpeningAmount,
                    jawTex.Size() / 2 - jawOffset,
                    Projectile.scale,
                    SpriteEffects.FlipHorizontally, 1);

            return false;
        }

        public override void PostDraw(Color lightColor)
        {
            // Draw segment glows
            for (int i = Segments.Count - 1; i >= 0; i--)
            {
                var seg = Segments[i];
                if (!SegmentTextureAssetsGlow.IndexInRange(seg.segmentType)) continue;

                var tex = SegmentTextureAssetsGlow[seg.segmentType].Value;
                Main.spriteBatch.Draw(tex,
                    seg.Center - Main.screenPosition,
                    null, Color.White * seg.Opacity,
                    seg.rotation,
                    tex.Size() / 2 + (SegmentTypeDrawOffsets[seg.segmentType]),
                    Projectile.scale,
                    SpriteEffects.None, 1);
            }

            // Head glow - only show when homing
            if (State == STATE_HOMING)
                Main.EntitySpriteDraw(
                    CalamityUtils.GetTextureEfficient(ref GlowTexAsset, GlowTexture).Value,
                    Projectile.Center - Main.screenPosition,
                    null, Color.White * Projectile.Opacity,
                    Projectile.rotation,
                    CalamityUtils.GetTextureEfficient(ref GlowTexAsset, GlowTexture).Size(),
                    Projectile.scale,
                    SpriteEffects.None, 0);
        }

        public List<Asset<Texture2D>> SegmentTextureAssetsGlow
        {
            get
            {
                if (internalTexAssetsGlow.Count == 0)
                    for (var i = 0; i < SegmentTextures.Count; i++)
                    {
                        internalTexAssetsGlow.Add(ModContent.Request<Texture2D>(SegmentTextures[i] + "Glow"));
                        if (SegmentTypeDrawOffsets.Count <= i)
                            SegmentTypeDrawOffsets.Add(Vector2.Zero);
                    }
                return internalTexAssetsGlow;
            }
        }
        private List<Asset<Texture2D>> internalTexAssetsGlow = new();
    }
}