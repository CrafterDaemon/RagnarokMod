using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class IrradiantRed : ModProjectile
    {
        private int trail = 8;
        private const float CollisionRadius = 60f;
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
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.knockBack = 20f;
        }

        public override void AI()
        {
            if (++Projectile.frameCounter >= 6) // speed of animation, lower = faster
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Type];
            }
            // Check for collision with Blue
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (!other.active || other.whoAmI == Projectile.whoAmI)
                    continue;
                if (other.type != ModContent.ProjectileType<IrradiantBlue>())
                    continue;
                if (other.owner != Projectile.owner)
                    continue;
                if (other.ai[0] != 1) // must be stationary
                    continue;
                float dist = Vector2.Distance(Projectile.Center, other.Center);
                if (dist <= CollisionRadius + other.scale * 20f)
                {
                    // Spawn Purple at midpoint
                    Vector2 spawnPos = (Projectile.Center + other.Center) / 2f;
                    // Store cursor direction in ai slots via velocity direction
                    Vector2 cursorDir = (Main.MouseWorld - spawnPos).SafeNormalize(Vector2.UnitX);
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            spawnPos,
                            cursorDir,
                            ModContent.ProjectileType<IrradiantPurple>(),
                            Projectile.damage + other.damage,
                            Projectile.knockBack,
                            Projectile.owner
                        );
                        SoundEngine.PlaySound(RagnarokModSounds.PurpleCharge, spawnPos);
                    }
                    other.Kill();
                    Projectile.Kill();
                    return;
                }
            }

            // Red dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
                dust.scale = 1.2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ThoriumHeal(5, radius: 1000, specificPlayer: Projectile.owner);
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
            for (int i = 0; i < 15; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
                dust.velocity *= 2f;
                dust.noGravity = true;
                dust.scale = 1.5f;
            }
        }
    }
}
