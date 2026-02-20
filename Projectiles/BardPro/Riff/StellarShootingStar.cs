using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

namespace RagnarokMod.Projectiles.BardPro.Riffs
{
    public class StellarShootingStar : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_0";

        private Color astralRed = new Color(237, 93, 83);
        private Color astralCyan = new Color(66, 189, 181);

        // ai[0] = target NPC index
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            // Home toward target
            int targetIndex = (int)Projectile.ai[0];
            if (targetIndex >= 0 && targetIndex < Main.maxNPCs)
            {
                NPC target = Main.npc[targetIndex];
                if (target.active && !target.friendly)
                {
                    Vector2 toTarget = target.Center - Projectile.Center;
                    float distance = toTarget.Length();

                    if (distance > 50f)
                    {
                        toTarget.Normalize();
                        Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * 16f, 0.08f);
                    }
                }
            }

            // Trail particles - more frequent and visible
            if (Main.rand.NextBool())
            {
                Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted,
                    -Projectile.velocity * 0.4f, 0, color, Main.rand.NextFloat(1.6f, 3f));
                star.noGravity = true;
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Star burst on impact
            for (int i = 0; i < 24; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, velocity, 0, color, Main.rand.NextFloat(1f, 2f));
                star.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Small burst on timeout
            for (int i = 0; i < 16; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(12f, 12f);
                Color color = Main.rand.NextBool() ? astralRed : astralCyan;
                Dust star = Dust.NewDustPerfect(Projectile.Center, DustID.TintableDustLighted, velocity, 0, color, Main.rand.NextFloat(0.8f, 1.5f));
                star.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            // Use a round texture instead of pixel
            Texture2D glowTexture = TextureAssets.Extra[ExtrasID.ThePerfectGlow].Value; // Falling star sparkle

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            // Alternating color star - much brighter
            Color starColor = (Main.GameUpdateCount % 20 < 10) ? astralRed : astralCyan;

            // Draw star with bloom
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = glowTexture.Size() / 2f;

            // Outer glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.5f, 0f, origin, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.5f, MathHelper.PiOver2, origin, 1.5f, SpriteEffects.None, 0f);

            // Mid glow
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.8f, 0f, origin, 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, starColor * 0.8f, MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);

            // Bright core
            spriteBatch.Draw(glowTexture, drawPos, null, Color.White, 0f, origin, 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(glowTexture, drawPos, null, Color.White, MathHelper.PiOver2, origin, 0.5f, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}