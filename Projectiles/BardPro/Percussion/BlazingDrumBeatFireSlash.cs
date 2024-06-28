using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Sounds;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    //Calamity Fire Slash, with 100% less scythe.
    public class BlazingDrumBeatFireSlash : BardProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 11;
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 300;
            Projectile.height = 398;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (player is null || player.dead)
                Projectile.Kill();

            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;

            AdjustPlayerPositionValues(player);

            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 4)
            {
                Projectile.ai[1]++;
                Projectile.ai[0] = 0;
                if (Projectile.ai[1] == 5)
                {
                    Projectile.friendly = true;
                }
            }
            if (Projectile.ai[1] >= 11)
            {
                Projectile.Kill();
                return;
            }
            Projectile.frame = (int)Projectile.ai[1];

            AdjustPlayerItemFrameValues(player);
        }
        public void AdjustPlayerPositionValues(Player player)
        {
            Projectile.Center = player.Center;
            Projectile.position.X += 60 * player.direction;
            Projectile.position.Y -= 30;
        }

        public void AdjustPlayerItemFrameValues(Player player)
        {
            if (Projectile.ai[1] < 5)
            {
                player.bodyFrame.Y = player.bodyFrame.Height;
            }
            else if (Projectile.ai[1] < 8)
            {
                player.bodyFrame.Y = 3 * player.bodyFrame.Height;
            }
            else
            {
                player.bodyFrame.Y = 4 * player.bodyFrame.Height;
            }

            Projectile.spriteDirection = player.direction;
            player.heldProj = Projectile.whoAmI;
        }
        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 300);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int width = 296 / 4;
            int height = 276 / 2;

            Vector2 drawCenter = Projectile.Center;
            Rectangle frameRectangle = new Rectangle(Projectile.frame / 2 * width, Projectile.frame % 2 * height, width, height);

            Texture2D scytheTexture = ModContent.Request<Texture2D>(Texture).Value;
            Texture2D glowTexture = ModContent.Request<Texture2D>("RagnarokMod/Projectiles/BardPro/Percussion/BlazingDrumBeatFireSlash_Glow").Value;

            Main.spriteBatch.Draw(scytheTexture,
                                  drawCenter - Main.screenPosition,
                                  frameRectangle,
                                  Projectile.GetAlpha(lightColor),
                                  Projectile.rotation,
                                  frameRectangle.Size() / 2,
                                  Projectile.scale,
                                  Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                  0f);
            Main.spriteBatch.Draw(glowTexture,
                                  drawCenter - Main.screenPosition,
                                  frameRectangle,
                                  Projectile.GetAlpha(Color.White),
                                  Projectile.rotation,
                                  frameRectangle.Size() / 2,
                                  Projectile.scale,
                                  Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                  0f);
            return false;
        }
    }
}