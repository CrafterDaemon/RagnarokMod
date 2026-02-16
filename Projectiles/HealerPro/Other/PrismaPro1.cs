using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class PrismaPro1 : ModProjectile, ILocalizedModType
    {
        private int timer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {
            timer++;
            if (timer == 4)
            {
                timer = 0;
                Dust.NewDust(Projectile.Center, 0, 0, DustID.MoonBoulder);
            }
            Projectile.velocity *= 0.975f;
            Projectile.rotation += 0.5f;
            if (Projectile.velocity.Length() < 0.5f)
            {
                Vector2 launchVelocity1 = Projectile.velocity;
                launchVelocity1 = Vector2.Normalize(launchVelocity1) * 15f;
                Vector2 launchVelocity2 = launchVelocity1.RotatedBy(MathHelper.ToRadians(72));
                Vector2 launchVelocity3 = launchVelocity1.RotatedBy(MathHelper.ToRadians(144));
                Vector2 launchVelocity4 = launchVelocity1.RotatedBy(MathHelper.ToRadians(216));
                Vector2 launchVelocity5 = launchVelocity1.RotatedBy(MathHelper.ToRadians(288));


                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity1, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity2, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity3, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity4, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity5, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.Kill();
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            Vector2 launchVelocity1 = Projectile.velocity;
            launchVelocity1 = Vector2.Normalize(launchVelocity1) * 15f;
            Vector2 launchVelocity2 = launchVelocity1.RotatedBy(MathHelper.ToRadians(72));
            Vector2 launchVelocity3 = launchVelocity1.RotatedBy(MathHelper.ToRadians(144));
            Vector2 launchVelocity4 = launchVelocity1.RotatedBy(MathHelper.ToRadians(216));
            Vector2 launchVelocity5 = launchVelocity1.RotatedBy(MathHelper.ToRadians(288));


            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity1, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity2, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity3, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity4, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity5, ModContent.ProjectileType<PrismaPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.Kill();
        }
    }
}