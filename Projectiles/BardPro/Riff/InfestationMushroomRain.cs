using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Riff
{
    public class InfestationMushroomRain : BardProjectile
    {
        public override string Texture => "CalamityMod/NPCs/Crabulon/CrabShroom";
        public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 16;
            Projectile.timeLeft = 300;
            Projectile.alpha = 50;
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
            // Gentle float downward with slight sway
            Projectile.velocity.Y += 0.04f;
            if (Projectile.velocity.Y > 4f)
                Projectile.velocity.Y = 4f;

            Projectile.velocity.X *= 0.99f;

            // Slow rotation
            Projectile.rotation += Projectile.velocity.X * 0.03f;

            // Spore trail
            if (Main.rand.NextBool(4))
            {
                Dust d = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(8f, 8f),
                    DustID.GlowingMushroom, Vector2.Zero, 0, default, 0.6f);
                d.noGravity = true;
                d.velocity *= 0.2f;
            }

            // Green glow
            Lighting.AddLight(Projectile.Center, 0.15f, 0.4f, 0.15f);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GlowingMushroom, velocity, 0, default, Main.rand.NextFloat(0.8f, 1.2f));
                d.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return true;
        }
    }
}
