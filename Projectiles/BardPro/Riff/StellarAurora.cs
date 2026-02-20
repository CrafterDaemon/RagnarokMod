using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Riffs;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class StellarAurora : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";

        // Vanilla projectile 874 is a long soft glow strip -- the same texture Calamity uses for CryogenSky.
        private static Texture2D AuroraTexture => TextureAssets.Projectile[874].Value;

        // One curtain ribbon -- mirrors Calamity's CryogenAurora struct
        private struct AuroraInstance
        {
            public float Depth;             // 1..2.2, controls scale and parallax feel
            public float ColorLerp;         // 0..1, position along the red->cyan gradient
            public float CenterOffsetRatio; // 0..1, angular position in the orbit ellipse
            public Vector2 Center;          // current screen-space draw position
            public SpriteEffects Flip;      // randomly flipped for variety
        }

        private const float AuroraHeight = 400f;
        private const float AuroraWidth = 1200f;

        private bool initialized = false;
        private AuroraInstance[] auroras;

        private int shootingStarTimer = 0;
        private int ShootingStarInterval = 8;

        // Astral Infection palette
        private Color astralRed = new Color(237, 93, 83);
        private Color astralCyan = new Color(66, 189, 181);

        public byte RiffType => RiffLoader.RiffType<AureusRiff>();

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
            Projectile.timeLeft = 20;
        }

        public override void AI()
        {

            if (Main.player[Projectile.owner].GetRagnarokModPlayer().activeRiffType == RiffType)
                Projectile.timeLeft++;
            else
                Projectile.Kill();
            if (!initialized)
            {
                SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.3f, Pitch = 0.5f }, Projectile.Center);

                for (int i = 0; i < 30; i++)
                {
                    Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                    Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                    Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, velocity, 0, color, Main.rand.NextFloat(1f, 2f));
                    star.noGravity = true;
                }

                // Initialize 150 ribbon instances, same count as Calamity's sky for dense coverage
                auroras = new AuroraInstance[150];
                float randomOffsetMax = 3f / auroras.Length;
                for (int i = 0; i < auroras.Length; i++)
                {
                    auroras[i].Depth = Main.rand.NextFloat(1f, 2.2f);
                    auroras[i].ColorLerp = Main.rand.NextFloat(0f, 0.8f);
                    auroras[i].CenterOffsetRatio = i / (float)auroras.Length + Main.rand.NextFloat(randomOffsetMax);
                    auroras[i].Flip = Main.rand.NextBool() ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                }

                initialized = true;
            }


            int targetIndex = Projectile.owner;
            if (targetIndex >= 0 && targetIndex < Main.maxPlayers && Main.player[targetIndex].active)
            {
                Projectile.Center = Main.player[targetIndex].Center;
            }
            else
            {
                Projectile.Kill();
                return;
            }

            // Calamity-style ribbon orbit update.
            // Anchored to screen-center so the aurora stays above the player
            // regardless of zoom or resolution, same as CryogenSky's Update().
            float time = Main.GlobalTimeWrappedHourly * 1.2f;
            float ellipseW = Main.screenWidth * 0.5f;
            float ellipseH = Main.screenHeight * 0.1f;
            Vector2 screenCenter = new Vector2(Main.screenWidth * 0.5f, 0f);

            // Drift rate -- Calamity's formula verbatim, strength fixed at 1 since riff is always active
            float drift = 1f / 1800f * MathHelper.Lerp(0.4f, 1f, 1f)
                        * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 0.9f);

            for (int i = 0; i < auroras.Length; i++)
            {
                Vector2 ellipseRange = new Vector2(ellipseW, ellipseH / auroras[i].Depth);
                auroras[i].Center = ellipseRange * (auroras[i].CenterOffsetRatio * MathHelper.TwoPi).ToRotationVector2() + screenCenter;
                // Lift above the player's head
                auroras[i].Center.Y -= 220f + (float)Math.Cos(time + auroras[i].CenterOffsetRatio * MathHelper.Pi) * 50f;
                auroras[i].CenterOffsetRatio += drift;
            }

            // Shooting stars
            shootingStarTimer++;
            if (shootingStarTimer >= ShootingStarInterval)
            {
                shootingStarTimer = 0;
                SpawnShootingStar();
            }
        }

        private void SpawnShootingStar()
        {
            List<NPC> nearbyEnemies = new List<NPC>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && !npc.dontTakeDamage)
                {
                    if (Vector2.Distance(Projectile.Center, npc.Center) < 800f)
                        nearbyEnemies.Add(npc);
                }
            }

            if (nearbyEnemies.Count > 0)
            {
                NPC target = nearbyEnemies[Main.rand.Next(nearbyEnemies.Count)];
                Vector2 spawnPos = Projectile.Center + new Vector2(Main.rand.NextFloat(-AuroraWidth / 2f, AuroraWidth / 2f), -Main.screenHeight);
                Vector2 toTarget = Vector2.Normalize(target.Center - spawnPos);

                Player owner = Main.player[Projectile.owner];
                int damage = 50;
                if (owner.GetThoriumPlayer() != null)
                    damage = (int)(damage * owner.GetTotalDamage(ThoriumDamageBase<BardDamage>.Instance).Additive);

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, toTarget * 12f,
                    ModContent.ProjectileType<StellarShootingStar>(), damage, 2f, Projectile.owner, target.whoAmI);
            }
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(8f, 8f);
                Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, velocity, 0, color, Main.rand.NextFloat(1.5f, 2.5f));
                star.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (auroras == null)
                return false;

            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D tex = AuroraTexture;
            Vector2 origin = tex.Size() * 0.5f;

            float time = Main.GlobalTimeWrappedHourly;
            float time2 = time * 0.7f;

            // Slow color drift over the red->cyan range, mirrors Calamity's hueOffset animation
            float colorDrift = (float)Math.Cos(time * 1.2f) * 0.15f;

            // Pulsing master brightness, Calamity's brightnessLerp formula verbatim
            float brightnessLerp = MathHelper.Lerp(0.75f, 1.25f, (float)Math.Sin(time / 1.8f) * 0.5f + 0.5f);

            spriteBatch.End();
            // Additive: overlapping ribbons glow without flattening to opaque blocks
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            for (int i = 0; i < auroras.Length; i++)
            {
                // Scale based on depth + per-instance pulse, Calamity's exact formula
                float scale = 1.4f / auroras[i].Depth;
                scale += (float)Math.Cos(time2 + auroras[i].CenterOffsetRatio * MathHelper.TwoPi) * 0.2f;

                // Color: blend red->cyan with per-instance offset and slow time drift
                float colorT = MathHelper.Clamp(auroras[i].ColorLerp + colorDrift, 0f, 1f);
                Color auroraColor = Color.Lerp(astralRed, astralCyan, colorT);

                // Height-based brightness: ribbons near the top of the screen glow brightest.
                // This is Calamity's yBrightness formula, tuned to our aurora height offset.
                float yBrightness = MathHelper.Lerp(1.5f, 0.5f,
                    1f - MathHelper.Clamp((auroras[i].Center.Y + 200f) / 200f, 0f, 1f)) * 1.3f;
                yBrightness *= brightnessLerp;
                yBrightness = Math.Min(yBrightness, 1.6f);

                // 1.5f alpha multiplier (vs Calamity's 0.85f * 1.1f ~= 0.94f) makes this
                // read as a vivid foreground effect rather than a background sky layer.
                float fadeOut = MathHelper.Clamp(Projectile.timeLeft / 20f, 0f, 1f);
                auroraColor *= yBrightness * 1.2f * fadeOut;

                // PiOver2 rotation matches Calamity: rotates the sprite so its long axis
                // lies horizontally, forming a curtain strip spanning the sky width.
                spriteBatch.Draw(tex, auroras[i].Center, null, auroraColor,
                    MathHelper.PiOver2, origin, scale, auroras[i].Flip, 0f);
            }

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}