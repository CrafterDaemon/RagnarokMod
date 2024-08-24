using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System.Drawing.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class PhantasmalEdgeBombs : ModProjectile, ILocalizedModType
    {
        public override string Texture => "CalamityMod/Projectiles/Magic/FatesRevealFlame";

        public int timer = 0;
        public int deathCounter = 0;
        public bool fireballReleased = false;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = false;
            Projectile.alpha = 255;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 2000000;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.scale = 1.2f;
        }

        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, 1f, 0.25f, 0.25f);
            int inc = Projectile.frameCounter + 1;
            Projectile.frameCounter = inc;
            if (inc > 4)
            {
                Projectile.frameCounter = 0;
                inc = Projectile.frame + 1;
                Projectile.frame = inc;
                if (inc >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            bool ownerExists = Projectile.TryGetOwner(out var owner);
            if (!fireballReleased && ownerExists && owner.channel) 
            {
                Projectile.velocity = owner.velocity + new Vector2(Main.rand.NextFloat()-0.5f, Main.rand.NextFloat() - 0.5f);
            }
            if (timer <= 45)
            {
                Projectile.alpha -= 5;
            }
            else if (!fireballReleased && ownerExists && !owner.channel)
            {
                fireballReleased = true;
                Projectile.velocity = new Vector2(Main.rand.NextFloat() - 0.5f, Main.rand.NextFloat() - 0.5f);
            }
            if (fireballReleased)
            {
                Projectile.friendly = true;
                NPC npc = Projectile.FindNearestNPCIgnoreTiles(500);
                if (npc != null)
                {
                    Projectile.HomeInOnTarget(npc, 15f, 0.15f);
                }
                if (npc == null && deathCounter >= 120)
                {
                    if (Main.rand.Next(1, 9) == 8)
                    {
                        Projectile.Kill();
                    }
                }
                deathCounter++;
            }
            timer++;
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);

            Projectile.position = Projectile.Center;
            Projectile.width = Projectile.height = 84;
            Projectile.position.X = Projectile.position.X - (float)(Projectile.width / 2);
            Projectile.position.Y = Projectile.position.Y - (float)(Projectile.height / 2);
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.Damage();

            int e;
            for (int i = 0; i < 3; i = e + 1)
            {
                int firespeck = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 100, default, 1.5f);
                Main.dust[firespeck].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                e = i;
            }
            for (int j = 0; j < 10; j = e + 1)
            {
                int morefirespec = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 2.5f);
                Main.dust[morefirespec].position = Projectile.Center + Vector2.UnitY.RotatedByRandom(MathHelper.Pi) * (float)Main.rand.NextDouble() * (float)Projectile.width / 2f;
                Main.dust[morefirespec].noGravity = true;
                Dust dust = Main.dust[morefirespec];
                dust.velocity *= 2f;
                e = j;
            }
            for (int k = 0; k < 5; k = e + 1)
            {
                int finalfirefleck = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 1.5f);
                Main.dust[finalfirefleck].position = Projectile.Center + Vector2.UnitX.RotatedByRandom(MathHelper.Pi).RotatedBy((double)Projectile.velocity.ToRotation(), default) * (float)Projectile.width / 2f;
                Main.dust[finalfirefleck].noGravity = true;
                Dust dust = Main.dust[finalfirefleck];
                dust.velocity *= 2f;
                e = k;
            }
        }
    }
}