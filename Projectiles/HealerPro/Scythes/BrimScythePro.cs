using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;


namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class BrimScythePro : ScythePro2
    {
        public override void SafeSetStaticDefaults()
        { }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            rotationSpeed = 0.25f;
            scytheCount = 2;
            Projectile.Size = new Vector2(335, 335);

            /*
            dustCount = 2;
            dustType = 60;
            Projectile.light = 1f;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            dustOffset = new Vector2(-280f, 40f);
            */
        }

        public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex)
        {
            dust.scale = 2.0f;
            dust.noLight = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // Fade with projectile alpha
            lightColor *= MathHelper.Lerp(1f, 0f, Projectile.alpha / 255f);

            // Main scythe texture
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation + MathHelper.PiOver4 * Projectile.spriteDirection,
                texture.Size() / 2f,
                Projectile.scale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                0f
            );

            // afterimage
            Color scytheColor = Color.Red *
                                MathHelper.Lerp(0.15f, 0f, Projectile.alpha / 255f);
            scytheColor.A = 0;

            Texture2D slashTexture = (Texture2D)ModContent.Request<Texture2D>("RagnarokMod/Effects/Assets/Slash_3");
            float slashScale = 4.35f;

            // Two forward-facing afterimages
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            // Two mirrored afterimages (rotated by 180)
            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            Main.EntitySpriteDraw(slashTexture, Projectile.Center - Main.screenPosition, null, scytheColor,
                Projectile.rotation + MathHelper.Pi, slashTexture.Size() / 2f, Projectile.scale * slashScale,
                Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

            return false;
        }
    }
}
