using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using System.Security.Cryptography.X509Certificates;

namespace RagnarokMod.Projectiles.HealerPro
{
    public class FractalPro1 : ModProjectile, ILocalizedModType
    {
        public int timerthing = 0;
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 5;
            Projectile.timeLeft = 161;
            Projectile.ignoreWater = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            
        }
        public override void AI() {
            timerthing += 1;
            Projectile.rotation += 0.5f;
            if (timerthing >= 40) {
                timerthing = 0;
                Vector2 launchVelocity1 = Projectile.velocity;
                launchVelocity1 = Vector2.Normalize(launchVelocity1) * 17f;
                Vector2 launchVelocity2 = launchVelocity1.RotatedBy(MathHelper.ToRadians(72));
                Vector2 launchVelocity3 = launchVelocity1.RotatedBy(MathHelper.ToRadians(144));
                Vector2 launchVelocity4 = launchVelocity1.RotatedBy(MathHelper.ToRadians(216));
                Vector2 launchVelocity5 = launchVelocity1.RotatedBy(MathHelper.ToRadians(288));
                
                
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity1, ModContent.ProjectileType<FractalPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity2, ModContent.ProjectileType<FractalPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity3, ModContent.ProjectileType<FractalPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity4, ModContent.ProjectileType<FractalPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity5, ModContent.ProjectileType<FractalPro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);

            }
        }
    }
}