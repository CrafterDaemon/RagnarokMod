using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Utilities;
namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class BloodOrb : ThoriumProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 3;
        }
        public override void SetDefaults()
        {
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1800;
        }

        public override void AI()
        {
            int maxDist = 250;

            bool lockedOn = false;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Projectile.TryGetOwner(out Player player);
                Player searchPlayer = Main.player[i];
                if (searchPlayer.active && !searchPlayer.dead && !searchPlayer.hostile && searchPlayer.team == player.team && searchPlayer.team != 0 || searchPlayer == player)
                {
                    double distance = Vector2.Distance(searchPlayer.Center, Projectile.Center);
                    if (distance <= maxDist)
                    {
                        lockedOn = true;
                        if (distance <= 10)
                        {
                            Projectile.ThoriumHeal(Main.rand.Next(1, 3), specificPlayer: i, ignoreHealer: false);
                            Projectile.Kill();
                        }
                        Projectile.HomeInOnTarget(searchPlayer, 15f);
                    }
                    else
                    {
                        lockedOn = false;
                    }
                }
            }
            if (lockedOn == false) { Projectile.velocity *= 0.975f; }
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Redraw the projectile with the color not influenced by light
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale - (0.05f * Math.Max(k - 1, 1)), SpriteEffects.None, 0);
            }

            return true;
        }
    }
}