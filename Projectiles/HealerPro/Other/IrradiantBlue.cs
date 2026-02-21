using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Sounds;
using RagnarokMod.Utils;
using ReLogic.Utilities;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantBlue : ModProjectile
    {
        private SlotId soundSlot;
        private int trail = 8;
        private float DamageThreshold = 20000f;
        private const float MaxScale = 2f;
        private const float PullRadius = 200f;
        private const float PullStrength = 0.15f;
        private const int StationaryLifetime = 300;
        private const int sideLength = 80;
        private const int MaxFollowTime = 600;
        private int followTimer;
        private int healTimer;
        private bool IsStationary => Projectile.ai[0] == 1;

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
            Projectile.width = sideLength;
            Projectile.height = sideLength;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.scale = 1f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void OnSpawn(IEntitySource source)
        {
            DamageThreshold *= Main.player[Projectile.owner].GetDamage(Projectile.DamageType).Additive;
            Projectile.damage /= 4;
            soundSlot = SoundEngine.PlaySound(RagnarokModSounds.Blue, Projectile.Center);
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
                            Projectile.ThoriumHeal(2, radius: 1000, specificPlayer: searchPlayer.whoAmI);
                    }
                }
            }

            if (!SoundEngine.TryGetActiveSound(soundSlot, out _))
                soundSlot = SoundEngine.PlaySound(RagnarokModSounds.Blue, Projectile.Center);
            else
                SoundEngine.TryGetActiveSound(soundSlot, out var s).ToString();

            if (++Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }

            if (!IsStationary)
            {
                Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.08f, 0.15f);

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
                            npc.velocity += pull * PullStrength * (1f - dist / PullRadius) * 5f;
                        }
                    }
                }

                if (++followTimer >= MaxFollowTime)
                    Projectile.Kill();
                else if (Projectile.ai[1] >= DamageThreshold)
                {
                    Projectile.ai[0] = 1;
                    followTimer = 0;
                }
                if (followTimer > MaxFollowTime - 120)
                    Projectile.alpha = (int)(255 * (followTimer - (MaxFollowTime - 120)) / 120f);
                else
                    Projectile.alpha = 0;
            }
            else
            {
                Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.08f, 0.05f);
                Projectile.ai[2]++;

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
                            npc.velocity += pull * PullStrength * (1f - dist / PullRadius) * 5f;
                        }
                    }
                }

                if (Projectile.ai[2] > StationaryLifetime - 60)
                    Projectile.alpha = (int)(255 * (Projectile.ai[2] - (StationaryLifetime - 60)) / 60f);
                if (Projectile.ai[2] >= StationaryLifetime)
                    Projectile.Kill();
            }

            float progress = Math.Min(Projectile.ai[1] / DamageThreshold, 1f);
            Projectile.scale = 1f + (MaxScale + 0.05f) * progress;
            Projectile.width = Projectile.height = (int)(sideLength / 1.5f * Projectile.scale);

            if (Main.rand.NextBool(2))
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(Projectile.width * 0.4f, Projectile.width * 0.7f);
                Vector2 spawnPos = Projectile.Center + angle.ToRotationVector2() * dist;
                Vector2 inward = (Projectile.Center - spawnPos).SafeNormalize(Vector2.Zero);
                Vector2 tangent = new Vector2(-inward.Y, inward.X);
                Dust dust = Dust.NewDustPerfect(spawnPos, DustID.BlueFairy,
                    inward * 2f + tangent * 1.5f, 0, default, Projectile.scale * 0.6f);
                dust.noGravity = true;
                dust.fadeIn = 0.8f;
            }

            if (Main.rand.NextBool(4))
            {
                Dust glow = Dust.NewDustPerfect(
                    Projectile.Center + Main.rand.NextVector2Circular(Projectile.width * 0.3f, Projectile.height * 0.3f),
                    DustID.BlueFairy, Vector2.Zero, 0, default, Projectile.scale * 0.4f);
                glow.noGravity = true;
                glow.velocity *= 0.1f;
            }

            Lighting.AddLight(Projectile.Center, 0.15f * Projectile.scale, 0.4f * Projectile.scale, 0.8f * Projectile.scale);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsStationary)
                Projectile.ai[1] += damageDone;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[ModContent.ProjectileType<NoProj>()].Value;
            Texture2D pixel = TextureAssets.MagicPixel.Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);
            float drawScale = (float)Projectile.width / Projectile.scale;
            float alpha = 1f - Projectile.alpha / 255f;
            float time = Main.GlobalTimeWrappedHourly;

            // STATE: BEGUN (vanilla)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (additive)

            float pulse = 1f + MathF.Sin(time * 5f) * 0.12f;

            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(20, 60, 180) * 0.15f * alpha,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 1.6f * pulse), SpriteEffects.None, 0f);

            sb.Draw(pixel, Projectile.Center - Main.screenPosition, PixelRect,
                new Color(60, 140, 255) * 0.2f * alpha,
                0f, PixelOriginCenter, new Vector2(Projectile.width * 0.8f * pulse), SpriteEffects.None, 0f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero) continue;
                float t = 1f - (float)k / Projectile.oldPos.Length;
                Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;

                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(15, 40, 180) * 0.3f * t * alpha,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * t), SpriteEffects.None, 0f);
                sb.Draw(pixel, trailPos, PixelRect,
                    new Color(60, 140, 255) * 0.5f * t * alpha,
                    0f, PixelOriginCenter, new Vector2(Projectile.width * 0.4f * t), SpriteEffects.None, 0f);
            }

            // STATE: BEGUN (additive)
            sb.End();
            // STATE: ENDED
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            // STATE: BEGUN (normal) EnterShaderRegion needs this

            sb.EnterShaderRegion();
            // STATE: BEGUN (shader)

            Texture2D vortexNoise = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/GreyscaleGradients/Cracks").Value;
            GameShaders.Misc["CalamityMod:DoGPortal"].UseOpacity(0.8f * alpha);
            GameShaders.Misc["CalamityMod:DoGPortal"].UseColor(new Color(20, 80, 200));
            GameShaders.Misc["CalamityMod:DoGPortal"].UseSecondaryColor(new Color(100, 180, 255));
            GameShaders.Misc["CalamityMod:DoGPortal"].Apply();

            float vortexScale = Projectile.width / (float)sideLength * 0.8f;
            for (int i = 0; i < 5; i++)
            {
                float angle = MathHelper.TwoPi * i / 5f + time * MathHelper.TwoPi * 0.6f;
                Color dc = Color.White * alpha * 0.7f;
                dc.A = 0;
                Main.EntitySpriteDraw(vortexNoise, Projectile.Center - Main.screenPosition + angle.ToRotationVector2() * 3f,
                    null, dc, angle + MathHelper.PiOver2, vortexNoise.Size() * 0.5f,
                    vortexScale, SpriteEffects.None, 0);
            }

            sb.ExitShaderRegion();
            // STATE: BEGUN (normal), correct for vanilla to resume

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(soundSlot, out var sound))
                sound.Stop();
            for (int i = 0; i < 25; i++)
            {
                float angle = MathHelper.TwoPi * i / 25f;
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BlueFairy,
                    angle.ToRotationVector2() * Main.rand.NextFloat(4f, 10f), 0, default, Projectile.scale * 0.8f);
                d.noGravity = true;
            }
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy);
                d.velocity *= 3f;
                d.noGravity = true;
                d.scale = 1.8f;
            }
        }
    }
}