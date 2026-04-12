using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class FungalSpore : BardProjectile
    {
        public override string Texture => "CalamityMod/NPCs/Crabulon/CrabShroom";
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.timeLeft = 180;
            Projectile.alpha = 80;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 3)
            {
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
                Projectile.frameCounter = 0;
            }
            // Decelerate and float
            Projectile.velocity *= 0.96f;

            // Gentle upward drift
            Projectile.velocity.Y -= 0.02f;
            if (Projectile.velocity.Y < -0.5f)
                Projectile.velocity.Y = -0.5f;

            // Slow rotation
            Projectile.rotation += 0.02f;

            // Fade over time
            if (Projectile.timeLeft < 40)
            {
                Projectile.alpha += 6;
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }

            // Spore dust
            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(6f, 6f),
                    DustID.GlowingMushroom, Vector2.Zero, 100, default, 0.5f);
                d.noGravity = true;
                d.velocity *= 0.1f;
            }

            // Faint glow
            float lightAmt = 1f - Projectile.alpha / 255f;
            Lighting.AddLight(Projectile.Center, 0.1f * lightAmt, 0.3f * lightAmt, 0.1f * lightAmt);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(2f, 2f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GlowingMushroom, velocity, 0, default, 0.7f);
                d.noGravity = true;
            }
        }
    }
}
