using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.BardItems.Accessories;
using RagnarokMod.Utils;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.Accessories
{
    public class MiniAnahitaCompanion : ModProjectile
    {
        public override string Texture => "CalamityMod/NPCs/Leviathan/Anahita";

        // How many frames in the spritesheet
        private const int FrameCount = 6;
        private const int FrameSpeed = 6; // ticks per frame

        // Horizontal offset from player center
        private const float OffsetX = 48f;

        // Fade speed
        private const float FadeSpeed = 0.04f;

        // Half-second cooldown in ticks
        private const int FireCooldown = 30;

        // ai[0] current X offset (lerped for smooth side-switching)
        // ai[1] fire cooldown timer
        private ref float CurrentOffsetX => ref Projectile.ai[0];
        private ref float FireTimer => ref Projectile.ai[1];

        private int frameCounter = 0;
        private int currentFrame = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = FrameCount;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 56;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 9999;
            Projectile.scale = 0.5f; // mini
            Projectile.Opacity = 0.8f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            RagnarokModPlayer ragPlayer = player.GetRagnarokModPlayer();

            // Kill if player is dead, inactive, or no longer has the accessory
            if (!player.active || player.dead || !ragPlayer.sirenScale)
            {
                Projectile.Kill();
                return;
            }

            bool isPlayingString = ragPlayer.stringInstrumentUsed;

            // Determine target offset behind the player based on facing direction
            float targetOffsetX = -player.direction * OffsetX;

            // Smoothly lerp current offset toward target
            CurrentOffsetX = MathHelper.Lerp(CurrentOffsetX, targetOffsetX, 0.08f);

            // Position beside player
            Projectile.Center = player.Center + new Vector2(CurrentOffsetX, -20f);
            Projectile.velocity = Vector2.Zero;


            Projectile.spriteDirection = -player.direction;


            // Animate frames
            frameCounter++;
            if (frameCounter >= FrameSpeed)
            {
                frameCounter = 0;
                currentFrame = (currentFrame + 1) % FrameCount;
            }
            Projectile.frame = currentFrame;

            // Fire cooldown tick
            if (FireTimer > 0)
                FireTimer--;

            // Fire water note if instrument was used and cooldown elapsed
            if (isPlayingString && FireTimer <= 0 && Projectile.owner == Main.myPlayer)
            {
                Vector2 toMouse = Main.MouseWorld - Projectile.Center;
                SoundStyle sound = new("CalamityMod/Sounds/Item/HarpEnd");
                sound.Pitch = (toMouse.Distance(Vector2.Zero) * 1.5f / Main.screenHeight - 1);
                sound.Volume = 0.5f;
                sound.MaxInstances = 9;
                SoundEngine.PlaySound(sound, Projectile.Center);
                FireTimer = FireCooldown;
                ragPlayer.stringInstrumentUsed = false;

                if (toMouse != Vector2.Zero)
                {
                    toMouse.Normalize();
                    toMouse *= 8f;

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        toMouse,
                        ModContent.ProjectileType<AnahitaWaterNote>(),
                        Projectile.damage,
                        2f,
                        Projectile.owner
                    );
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.Opacity <= 0f)
                return false;

            var tex = ModContent.Request<Texture2D>(Texture).Value;
            int frameHeight = tex.Height / FrameCount;
            Rectangle sourceRect = new Rectangle(0, currentFrame * frameHeight, tex.Width, frameHeight);

            SpriteEffects effects = Projectile.spriteDirection == -1
                ? SpriteEffects.FlipHorizontally
                : SpriteEffects.None;

            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                sourceRect,
                Color.White * Projectile.Opacity,
                Projectile.rotation,
                new Vector2(tex.Width / 2f, frameHeight / 2f),
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }
    }
}
