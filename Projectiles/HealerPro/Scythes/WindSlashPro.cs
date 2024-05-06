using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class WindSlashPro : ModProjectile, ILocalizedModType
    {
        public int counter = 0;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 100;
            Projectile.alpha = 60;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.Size = new Vector2(50f, 70f);
        }
        public override void AI()
        {
            Projectile.velocity *= 0.98f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            counter += 1;
            Projectile.alpha += 2;
            if (counter > 5)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, 0f, 0f, 100, Color.White, 0.5f);
                counter = 0;
            }

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
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }

    }
}
