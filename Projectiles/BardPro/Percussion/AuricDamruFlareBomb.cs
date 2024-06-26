using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;
namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class AuricDamruFlareBomb : BardProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 5;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 66;
            Projectile.penetrate = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 240;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
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
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 25;
            height = 25;
            return true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate(-10, -5);
        }


        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = TextureAssets.Projectile[Type].Value;

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If image isn't centered or symmetrical you can specify origin of the sprite
            // (0,0) for the upper-left corner
            float offsetX = 20f;
            origin.X = Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX;

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }


        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, new Vector2?(Projectile.Center), null);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AuricDamruFlareBombBoom>(), (int)(Projectile.damage * 0.75f), 1f, Projectile.owner, 0f, 0f, 0f);
            for (int j = 0; j < 10; j++)
            {
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), 0, default, 1.25f).noGravity = true;
            }

        }

    }
}
