using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class AuricGreatscytheBolt : ModProjectile
    {
        private const int TrailLength = 16;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Type] = TrailLength;
            ProjectileID.Sets.TrailingMode[Type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 180;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.extraUpdates = 2;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            // Fade in quickly
            if (Projectile.alpha > 0)
                Projectile.alpha = Math.Max(Projectile.alpha - 40, 0);

            // Bright electric lighting
            Lighting.AddLight(Projectile.Center, 0.9f, 0.9f, 1f);

            // Slight velocity wobble for organic lightning feel
            Projectile.velocity = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(2f));

            // Heavy electric dust trail
            for (int i = 0; i < 4; i++)
            {
                Vector2 dustPos = Projectile.Center + Main.rand.NextVector2Circular(12f, 12f);
                Dust dust = Dust.NewDustDirect(dustPos, 1, 1, DustID.Electric, 0f, 0f, 100, default, 1.4f);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.2f + Main.rand.NextVector2Circular(1.5f, 1.5f);
            }

            // Side sparks for width
            if (Main.rand.NextBool(2))
            {
                Vector2 perpendicular = Projectile.velocity.SafeNormalize(Vector2.UnitX).RotatedBy(MathHelper.PiOver2);
                float offset = Main.rand.NextFloat(-20f, 20f);
                Vector2 sparkPos = Projectile.Center + perpendicular * offset;
                Dust spark = Dust.NewDustDirect(sparkPos, 1, 1, DustID.Electric, 0f, 0f, 150, default, 0.8f);
                spark.noGravity = true;
                spark.velocity = perpendicular * offset * 0.05f;
            }

            // Rotation follows velocity
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;

            // Draw jagged lightning segments between trail positions
            DrawLightningTrail(spriteBatch);

            // Draw glowing orb at head of bolt
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Vector2 drawOrigin = texture.Size() / 2f;
            float opacity = 1f - (Projectile.alpha / 255f);

            // Glow layers
            for (int layer = 0; layer < 3; layer++)
            {
                float scale = Projectile.scale * (1f + layer * 0.5f);
                Color glowColor = new Color(180, 220, 255) * (0.4f / (layer + 1)) * opacity;
                Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, glowColor, Projectile.rotation, drawOrigin, scale, SpriteEffects.None);
            }

            // Core bright draw
            Color coreColor = Color.White * opacity;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, coreColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None);

            return false;
        }

        private void DrawLightningTrail(SpriteBatch spriteBatch)
        {
            float opacity = 1f - (Projectile.alpha / 255f);
            if (opacity <= 0f)
                return;

            for (int k = 0; k < Projectile.oldPos.Length - 1; k++)
            {
                if (Projectile.oldPos[k] == Vector2.Zero || Projectile.oldPos[k + 1] == Vector2.Zero)
                    continue;

                Vector2 start = Projectile.oldPos[k] + Projectile.Size / 2f - Main.screenPosition;
                Vector2 end = Projectile.oldPos[k + 1] + Projectile.Size / 2f - Main.screenPosition;

                // Add jagged offset to middle for lightning look
                Vector2 mid = (start + end) / 2f;
                Vector2 perp = (end - start).SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.PiOver2);
                float jag = Main.rand.NextFloat(-8f, 8f);
                mid += perp * jag;

                float trailProgress = 1f - (float)k / Projectile.oldPos.Length;
                float thickness = 3f * trailProgress;
                Color trailColor = new Color(180, 220, 255) * trailProgress * 0.7f * opacity;
                Color coreTrailColor = Color.White * trailProgress * 0.5f * opacity;

                // Draw two segments through the jagged midpoint
                DrawLine(spriteBatch, start, mid, trailColor, thickness + 2f);
                DrawLine(spriteBatch, mid, end, trailColor, thickness + 2f);
                // Bright core
                DrawLine(spriteBatch, start, mid, coreTrailColor, thickness);
                DrawLine(spriteBatch, mid, end, coreTrailColor, thickness);
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        {
            Vector2 edge = end - start;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            float length = edge.Length();

            spriteBatch.Draw(
                TextureAssets.MagicPixel.Value,
                start,
                new Rectangle(0, 0, 1, 1),
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0f
            );
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Electric burst on hit
            SoundEngine.PlaySound(SoundID.Item94, Projectile.Center);

            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Electric, 0f, 0f, 100, default, 1.5f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(6f, 6f);
            }

            target.AddBuff(ModContent.BuffType<AuricRebuke>(), 60, false);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<AuricRebuke>(), 60, false);
        }

        public override void OnKill(int timeLeft)
        {
            // Final electric burst
            SoundEngine.PlaySound(SoundID.Item93, Projectile.Center);

            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(15f), 30, 30, DustID.Electric, 0f, 0f, 100, default, 1.8f);
                dust.noGravity = true;
                dust.velocity = Main.rand.NextVector2Circular(8f, 8f);
            }

            // Lingering sparks
            for (int i = 0; i < 8; i++)
            {
                Dust spark = Dust.NewDustDirect(Projectile.Center - new Vector2(10f), 20, 20, DustID.Electric, 0f, 0f, 200, default, 0.6f);
                spark.velocity = Main.rand.NextVector2Circular(3f, 3f);
                spark.fadeIn = 1.2f;
            }
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White * (1f - Projectile.alpha / 255f);
        }
    }
}
