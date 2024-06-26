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
namespace RagnarokMod.Projectiles.BardPro.Wind
{
    public class SteampipesPro : ModProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {

            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {

            if (++Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;

            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            Dust.NewDust(Projectile.Center, 0, 0, 55);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}