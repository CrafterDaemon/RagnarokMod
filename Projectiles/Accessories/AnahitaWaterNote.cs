using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Projectiles.Accessories
{
    public class AnahitaWaterNote : ModProjectile
    {
        // Use one of Anahita's existing water projectile textures from Calamity
        public override string Texture => "CalamityMod/Projectiles/Magic/AnahitasArpeggioNote";
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 16;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Generic;
            Projectile.scale = 0.8f;
        }

        public override void AI()
        {
            // Slight drag
            Projectile.velocity *= 0.99f;

            // Leave water dust trail
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(
                    Projectile.position, Projectile.width, Projectile.height,
                    Terraria.ID.DustID.Water,
                    Projectile.velocity.X * 0.2f,
                    Projectile.velocity.Y * 0.2f,
                    100, default, 0.8f);
                dust.noGravity = true;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // Small water splash on death
            for (int i = 0; i < 8; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                Dust dust = Dust.NewDustDirect(
                    Projectile.Center, 4, 4,
                    Terraria.ID.DustID.Water,
                    speed.X, speed.Y, 100, default, 1f);
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var tex = ModContent.Request<Texture2D>(Texture).Value;

            // Teal tint to match Anahita's water theme
            Color drawColor = Color.Lerp(lightColor, new Color(100, 220, 255), 0.6f);

            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                drawColor,
                Projectile.rotation,
                tex.Size() / 2f,
                Projectile.scale,
                SpriteEffects.None,
                0f
            );

            return false;
        }
    }
}
