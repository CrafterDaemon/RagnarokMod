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
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantBlue : ModProjectile
    {
        // ai[0]: state 0 = following cursor, 1 = stationary
        // ai[1]: cumulative damage dealt
        // ai[2]: stationary timer
        private SlotId soundSlot;
        private int trail = 8;
        private float DamageThreshold = 20000f;
        private const float MaxScale = 2f;
        private const float PullRadius = 200f;
        private const float PullStrength = 0.15f;
        private const int StationaryLifetime = 300; // 5 seconds
        private const int sideLength = 80; 
        private const int MaxFollowTime = 600;
        private int followTimer;
        private int healTimer;
        private bool IsStationary => Projectile.ai[0] == 1;
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
                        {
                            Projectile.ThoriumHeal(2, radius: 1000, specificPlayer: searchPlayer.whoAmI);
                        }
                    }
                }
            }
                    if (!SoundEngine.TryGetActiveSound(soundSlot, out _))
                soundSlot = SoundEngine.PlaySound(RagnarokModSounds.Blue, Projectile.Center);
            else
                SoundEngine.TryGetActiveSound(soundSlot, out var s).ToString();
            if (++Projectile.frameCounter >= 6) // speed of animation, lower = faster
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }
            if (!IsStationary)
            {
                // Lerp toward cursor
                Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.08f, 0.15f);

                // Pull nearby NPCs
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.boss)
                            continue;

                        float dist = Vector2.Distance(npc.Center, Projectile.Center);
                        if (dist < PullRadius && dist > 0)
                        {
                            Vector2 pull = (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero);
                            float strength = PullStrength * (1f - dist / PullRadius);
                            npc.velocity += pull * strength * 5f;
                        }
                    }
                }

                // Check damage threshold
                if (++followTimer >= MaxFollowTime)
                {
                    Projectile.Kill();
                }
                else if (Projectile.ai[1] >= DamageThreshold)
                {
                    Projectile.ai[0] = 1;
                    followTimer = 0;
                }
                Projectile.alpha = (int)(255 * (followTimer / (float)MaxFollowTime));
            }
            else
            {
                Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.08f, 0.05f);
                Projectile.ai[2]++;

                // Pull nearby NPCs
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    for (int i = 0; i < Main.maxNPCs; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (!npc.active || npc.friendly || npc.dontTakeDamage || npc.boss)
                            continue;

                        float dist = Vector2.Distance(npc.Center, Projectile.Center);
                        if (dist < PullRadius && dist > 0)
                        {
                            Vector2 pull = (Projectile.Center - npc.Center).SafeNormalize(Vector2.Zero);
                            float strength = PullStrength * (1f - dist / PullRadius);
                            npc.velocity += pull * strength * 5f;
                        }
                    }
                }
                // Fade out near end
                if (Projectile.ai[2] > StationaryLifetime - 60)
                    Projectile.alpha = (int)(255 * (Projectile.ai[2] - (StationaryLifetime - 60)) / 60f);

                if (Projectile.ai[2] >= StationaryLifetime)
                    Projectile.Kill();
            }

            // Scale grows with damage dealt, capped at MaxScale
            float progress = Math.Min(Projectile.ai[1] / DamageThreshold, 1f);
            Projectile.scale = 1f + (MaxScale - 1f) * progress;
            Projectile.width = Projectile.height = (int)(sideLength/1.5f * Projectile.scale);

            // Blue dust trail
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale = Projectile.scale * 0.8f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!IsStationary)
                Projectile.ai[1] += damageDone;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            Rectangle sourceRect = new Rectangle(0, Projectile.frame * frameHeight, texture.Width, frameHeight);
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

            // Scale sprite to match hitbox size
            float drawScale = (float)Projectile.width / texture.Width * Projectile.scale;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                float trailProgress = 1f - (float)k / Projectile.oldPos.Length;
                Color trailColor = new Color(255, 80, 80) * MathF.Pow(trailProgress, 2f);
                float trailScale = drawScale * trailProgress;
                Vector2 trailPos = Projectile.oldPos[k] + new Vector2(Projectile.width, Projectile.height) / 2f - Main.screenPosition;
                Main.EntitySpriteDraw(texture, trailPos, sourceRect, trailColor, Projectile.oldRot[k], origin, trailScale, SpriteEffects.None);
            }
            Color color = new Color(50, 150, 255) * (1f - Projectile.alpha / 255f);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRect, color, Projectile.rotation, origin, drawScale, SpriteEffects.None);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            if (SoundEngine.TryGetActiveSound(soundSlot, out var sound))
                sound.Stop();
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy);
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
        }
    }
}
