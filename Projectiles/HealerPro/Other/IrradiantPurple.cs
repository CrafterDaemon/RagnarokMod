using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantPurple : ModProjectile
    {
        private int trail = 8;
        private const int Lifetime = 540;
        private const float PullRadius = 300f;
        private int healTimer;

        private static readonly Rectangle PixelRect = new Rectangle(0, 0, 1, 1);
        private static readonly Vector2 PixelOriginCenter = new Vector2(0.5f, 0.5f);

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
            Main.projFrames[Type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.oldPos = new Vector2[trail];
            Projectile.oldRot = new float[trail];
            Projectile.width = 480;
            Projectile.height = 480;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            if (++healTimer >= 30)
            {
                healTimer = 0;
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Projectile.TryGetOwner(out Player player);
                    Player searchPlayer = Main.player[i];
                    if (searchPlayer.active && !searchPlayer.dead && !searchPlayer.hostile || searchPlayer == player)
                    {
                        double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
                        if (distance <= Projectile.width)
                            Projectile.ThoriumHeal(4, radius: 1000, specificPlayer: searchPlayer.whoAmI);
                    }
                }
            }

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }

            Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.1f, 0.12f);

            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.boss) continue;
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < PullRadius && dist > 0)
                    {
                        Vector2 pull = (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero);
                        npc.velocity += pull * 0.2f * (1f - dist / PullRadius) * 5f;
                    }
                }
            }


            float time = Main.GameUpdateCount * 0.15f;

            bool collapsing = Projectile.timeLeft < 30;

            if (collapsing)
            {
                // Rapidly shrink scale, goes from current -> ~0.15 over 30 frames
                float collapseProgress = 1f - Projectile.timeLeft / 30f;
                float collapseEase = collapseProgress * collapseProgress; // accelerating
                Projectile.scale = MathHelper.Lerp(1f, 0.15f, collapseEase);

                // Violent inward-pulling particles
                for (int i = 0; i < (int)(8 * collapseProgress + 2); i++)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float dist = Main.rand.NextFloat(150f, 350f) * (1f - collapseProgress);
                    Vector2 spawnPos = Projectile.Center + angle.ToRotationVector2() * dist;
                    Vector2 inward = (Projectile.Center - spawnPos).SafeNormalize(Vector2.Zero);
                    float speed = MathHelper.Lerp(6f, 20f, collapseProgress);

                    int dustType = Main.rand.NextBool() ? DustID.PurpleTorch : DustID.GemAmethyst;
                    Dust dust = Dust.NewDustPerfect(spawnPos, dustType,
                        inward * speed, 0, default, Main.rand.NextFloat(1.5f, 2.5f));
                    dust.noGravity = true;
                }

                // Bright core flicker
                if (Main.rand.NextBool(2))
                {
                    Dust core = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(10f, 10f),
                        DustID.PurpleTorch, Vector2.Zero, 0, default, Main.rand.NextFloat(2f, 3.5f));
                    core.noGravity = true;
                }
            }
            else
            {
                float pulse = 1f + MathF.Sin(time) * 0.1f + MathF.Sin(time * 2.3f) * 0.05f;
                Projectile.scale = pulse;
            }


            float lifeProgress = 1f - (float)Projectile.timeLeft / Lifetime;

            for (int i = 0; i < 3; i++)
            {
                if (!Main.rand.NextBool(2)) continue;
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(Projectile.width * 0.3f, Projectile.width * 0.8f);
                Vector2 spawnPos = Projectile.Center + angle.ToRotationVector2() * dist;
                Vector2 inward = (Projectile.Center - spawnPos).SafeNormalize(Vector2.Zero);
                Vector2 tangent = new Vector2(-inward.Y, inward.X);
                int dustType = Main.rand.NextBool() ? DustID.PurpleTorch : DustID.GemAmethyst;
                Dust dust = Dust.NewDustPerfect(spawnPos, dustType,
                    inward * 3f + tangent * 2f, 0, default, Main.rand.NextFloat(1.2f, 1.8f));
                dust.noGravity = true;
                dust.fadeIn = 0.6f;
            }

            if (Main.rand.NextBool(3))
            {
                Vector2 sparkPos = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.5f, Projectile.height * 0.5f);
                Dust spark = Dust.NewDustPerfect(sparkPos, DustID.PurpleTorch,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 5f), 0, default, Main.rand.NextFloat(1.5f, 2.5f));
                spark.noGravity = true;
            }

            if (Projectile.timeLeft < 90)
            {
                float deathUrgency = 1f - Projectile.timeLeft / 90f;
                for (int i = 0; i < (int)(5 * deathUrgency); i++)
                {
                    Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 10f * deathUrgency);
                    int dustType = Main.rand.NextBool() ? DustID.PurpleTorch : DustID.RedTorch;
                    Dust chaos = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(30f, 30f),
                        dustType, vel, 0, default, Main.rand.NextFloat(1.5f, 2.5f));
                    chaos.noGravity = true;
                }
            }

            float lightIntensity = Projectile.scale * (0.6f + lifeProgress * 0.4f);
            Lighting.AddLight(Projectile.Center, 0.4f * lightIntensity, 0.15f * lightIntensity, 0.7f * lightIntensity);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<NoProj>()].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            float drawScale = (float)Projectile.width / texture.Width * Projectile.scale;
            float time = Main.GlobalTimeWrappedHourly;
            float lifeProgress = 1f - (float)Projectile.timeLeft / Lifetime;
            float vi = 0.7f + lifeProgress * 0.3f; // visual intensity
            bool collapsing = Projectile.timeLeft < 30;
            float collapseProgress = collapsing ? 1f - Projectile.timeLeft / 30f : 0f;

            // Make the core glow intensify during collapse
            float coreBoost = 1f + collapseProgress * 3f;
            // STATE: BEGUN (vanilla)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)

            float pulse = 1f + MathF.Sin(time * 4f) * 0.1f;

            // Wide ambient glow
            Color outerColor = Color.Lerp(new Color(60, 15, 120), new Color(120, 20, 80), lifeProgress);
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                outerColor * 0.1f * vi,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 2f * pulse), SpriteEffects.None, 0f);

            // Inner glow
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(140, 40, 200) * 0.15f * vi,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 1f * pulse), SpriteEffects.None, 0f);

            // Hot core

            Color coreColor = Color.Lerp(new Color(180, 80, 255), new Color(255, 200, 255), collapseProgress);
            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                coreColor * 0.2f * vi * coreBoost,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 0.4f * pulse), SpriteEffects.None, 0f);

            // Afterimage trail
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;
                float t = 1f - (float)k / Projectile.oldPos.Length;
                Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;

                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(80, 15, 140) * 0.3f * t * vi,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * t), SpriteEffects.None, 0f);
                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(180, 60, 255) * 0.5f * t * vi,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * 0.4f * t), SpriteEffects.None, 0f);
            }

            // STATE: BEGUN (additive)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal)

            // === PRIMARY VORTEX — Purple ===
            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.9f * vi);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(80, 10, 160));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(180, 60, 255));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi * i / 6f + time * MathHelper.TwoPi * 0.5f;
                Color dc = Color.White * 0.8f * vi;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 4f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    Projectile.scale * 3f, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal)

            // === SECONDARY VORTEX — Red, counter-rotating ===
            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.5f * vi);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(180, 20, 30));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(255, 60, 80));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f - time * MathHelper.TwoPi * 0.8f;
                Color dc = Color.White * 0.6f * vi;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    Projectile.scale * 2f, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal) — correct for vanilla

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                SoundEngine.PlaySound(RagnarokModSounds.PurpleExplode, Projectile.Center);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                    ModContent.ProjectileType<IrradiantExplosion>(),
                    Projectile.damage * 3, 10f, Projectile.owner);
            }
        }
    }
}