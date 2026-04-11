using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
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
            Projectile.velocity *= 0.99f;

            if (Main.player[Projectile.owner].GetRagnarokModPlayer().sirenVisualHidden)
                Projectile.alpha = 0;
            else
                Projectile.alpha = 255;
        }

        public override void OnKill(int timeLeft)
        {
            if (!Main.player[Projectile.owner].GetRagnarokModPlayer().sirenVisualHidden)
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
                drawColor * (Projectile.alpha / 255f),
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
