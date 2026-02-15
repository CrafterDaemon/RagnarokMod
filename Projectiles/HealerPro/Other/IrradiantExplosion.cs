using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class IrradiantExplosion : ModProjectile
    {
        private int trail = 8;
        private const int Lifetime = 45;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = trail;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.oldPos = new Vector2[trail];
            Projectile.oldRot = new float[trail];
            Projectile.width = 300;
            Projectile.height = 300;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.penetrate = -1;
            Projectile.timeLeft = Lifetime;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = Lifetime; // hit once only
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;

            // Expanding burst of dust
            float progress = 1f - (float)Projectile.timeLeft / Lifetime;
            Projectile.scale = progress * 6f;

            for (int i = 0; i < 5; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float dist = Main.rand.NextFloat(150f) * progress;
                Vector2 pos = Projectile.Center + angle.ToRotationVector2() * dist;


                // Death burst
                    Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(Projectile.width/2, Projectile.height)/2, Projectile.width*2, Projectile.height*2, DustID.PurpleTorch);
                    dust.velocity *= 4f;
                    dust.noGravity = true;
                    dust.scale = 2f;
                    Dust dust2 = Dust.NewDustDirect(Projectile.position - new Vector2(Projectile.width/2, Projectile.height/2), Projectile.width * 2, Projectile.height*2, DustID.BlueFairy);
                    dust2.velocity *= 3f;
                    dust2.noGravity = true;
                    dust2.scale = 1.8f;
            }
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
            float progress = 1f - (float)Projectile.timeLeft / Lifetime;
            float alpha = 1f - progress; // fades out as it expands
            Color color = new Color(180, 50, 255) * alpha;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, sourceRect, color, Projectile.rotation, origin, drawScale, SpriteEffects.None);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }
    }
}
