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
                        {
                            Projectile.ThoriumHeal(4, radius: 1000, specificPlayer: searchPlayer.whoAmI);
                        }
                    }
                }
            }
            if (++Projectile.frameCounter >= 6) // speed of animation, lower = faster
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }
            // Follow cursor
            Vector2 target = Main.player[Projectile.owner].Center + (Main.MouseWorld - Main.player[Projectile.owner].Center);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, (target - Projectile.Center) * 0.1f, 0.12f);

            // Pull nearby NPCs more aggressively than Blue
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
                        float strength = 0.2f * (1f - dist / PullRadius);
                        npc.velocity += pull * strength * 5f;
                    }
                }
            }

            // Unstable pulsing scale
            float pulse = 1f + System.MathF.Sin(Main.GameUpdateCount * 0.15f) * 0.1f;
            Projectile.scale = pulse;

            // Purple/void dust
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
                dust.noGravity = true;
                dust.velocity *= 0.5f;
                dust.scale = 1.4f;
            }
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.BlueFairy);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale = 1.2f;
            }

            // Explode when lifetime expires (timeLeft hits 0 triggers Kill)
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

            Color color = new Color(255, 80, 80);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRect, color, Projectile.rotation, origin, drawScale, SpriteEffects.None);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Spawn explosion
            if (Main.myPlayer == Projectile.owner)
            {
                SoundEngine.PlaySound(RagnarokModSounds.PurpleExplode, Projectile.Center);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<IrradiantExplosion>(),
                    Projectile.damage * 3,
                    10f,
                    Projectile.owner
                );
            }
        }
    }
}
