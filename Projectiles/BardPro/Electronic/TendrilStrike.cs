using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Sounds;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Electronic
{
    public class TendrilStrike : BardProjectile, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 64;
            Projectile.scale = 1.5f;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 8;
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
            if (Projectile.ai[0] >= 8)
            {
                Projectile.ai[1]++;
                Projectile.ai[0] = 0;
                if (Projectile.ai[1] == 2)
                {
                    Projectile.friendly = true;
                }
            }
            if (Projectile.ai[1] >= 9)
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
            Projectile.position.X += 100 * player.direction;
            Projectile.position.Y -= 20;
        }

        public void AdjustPlayerItemFrameValues(Player player)
        {
            player.bodyFrame.Y = player.bodyFrame.Height;
            Projectile.spriteDirection = player.direction;
            player.heldProj = Projectile.whoAmI;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            int sheetwidth = 366;
            int sheethight = 204;
            int frames = 3;
            int width = sheetwidth / frames;
            int height = sheethight / frames;

            Vector2 drawCenter = Projectile.Center;
            Rectangle frameRectangle = new Rectangle(Projectile.frame / frames * width, Projectile.frame % frames * height, width, height);

            Texture2D tendril = ModContent.Request<Texture2D>(Texture).Value;

            Main.spriteBatch.Draw(tendril,
                                  drawCenter - Main.screenPosition,
                                  frameRectangle,
                                  Projectile.GetAlpha(lightColor),
                                  Projectile.rotation,
                                  frameRectangle.Size() / 2,
                                  Projectile.scale,
                                  Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                                  0f);
            return false;
        }
    }
}