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

        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

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


        public override bool PreDraw(ref Color lightColor){
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


        public override void OnKill(int timeLeft){
            SoundEngine.PlaySound(SoundID.Item14, new Vector2?(Projectile.Center), null);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(null), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AuricDamruFlareBombBoom>(), (int)(Projectile.damage * 0.75f), 1f, Projectile.owner, 0f, 0f, 0f);
            for (int j = 0; j < 30; j++){
				
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Main.rand.Next(-15, 15), Main.rand.Next(-15, 15), 0, default, 2.5f).noGravity = true;
            }

        }

    }
}
