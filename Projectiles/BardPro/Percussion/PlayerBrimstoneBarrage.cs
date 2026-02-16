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
using CalamityMod.NPCs.CalClone;
using Terraria.DataStructures;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class PlayerBrimstoneBarrage : BardProjectile
    {
        public override string Texture => "CalamityMod/Projectiles/Boss/BrimstoneBarrage"; // Use original texture if you want same look

        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetBardDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 90;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.scale = 1f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
            Projectile.usesIDStaticNPCImmunity = false;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Default pierce
            Projectile.penetrate = 1;

            // If spawned by Gigablast
            if (Projectile.ai[0] == 1f)
            {
                Projectile.penetrate = 2;
            }
        }


        public override void AI()
        {
            // Animate frames: advance frame every few ticks
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6) // change 6 to a smaller number for faster animation
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0; // loop back to first frame
                }
            }

            // Add light and dust effects
            Lighting.AddLight(Projectile.Center, 0.9f, 0f, 0f);

            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center, 60);
                d.noGravity = true;
                d.velocity = Projectile.velocity * 0.2f;
                d.scale = 0.8f;
            }
        }


        public override void Kill(int timeLeft)
        {
            // Explosion dust on death
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, 60);
                d.noGravity = true;
                d.velocity = velocity;
                d.scale = 1f;
            }
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle sourceRect = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);

            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

            float rotation = Projectile.rotation + MathHelper.ToRadians(90);

            // Draw the sprite centered on Projectile.Center, rotated properly
            Main.spriteBatch.Draw(texture,
                                 Projectile.Center - Main.screenPosition,
                                 sourceRect,
                                 lightColor * Projectile.Opacity,
                                 rotation,
                                 origin,
                                 Projectile.scale,
                                 SpriteEffects.None,
                                 0f);

            return false; // prevent vanilla draw
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 120);
        }
    }
}
