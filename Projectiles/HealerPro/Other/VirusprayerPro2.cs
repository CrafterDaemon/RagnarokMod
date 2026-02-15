using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class VirusprayerPro2 : ModProjectile, ILocalizedModType
    {
        private int mult = 0;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.alpha = 0;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            mult++;
            Projectile.velocity *= 0.97f;
            Projectile.alpha += 1 + (mult / 55);
            if (Projectile.timeLeft >= 120)
            {
                Projectile.velocity.Y -= 0.1f;
            }
            if (Projectile.timeLeft <= 119)
            {
                Projectile.velocity.Y *= 0.995f;
            }
            if (Projectile.alpha >= 255)
            {
                Dust.NewDust(Projectile.position, 32, 32, DustID.CorruptGibs, default, default, 128, default, 1.25f);
                Dust.NewDust(Projectile.position, 32, 32, DustID.CorruptGibs, default, default, 128, default, 1.25f);
                Dust.NewDust(Projectile.position, 32, 32, DustID.CorruptGibs, default, default, 128, default, 1.25f);
                Dust.NewDust(Projectile.position, 32, 32, DustID.CorruptGibs, default, default, 128, default, 1.25f);
                Projectile.Kill();
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
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 1.5f;
        }

    }
}