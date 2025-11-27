using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Items;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using System.Security.Cryptography.X509Certificates;
using CalamityMod.Buffs.DamageOverTime;
using Terraria.DataStructures;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class DragonForcePro1 : BardProjectile, ILocalizedModType
    {
        private int dustTimer = 0;
        public override void SetBardDefaults()
        {
            Projectile.width = 72;
            Projectile.height = 94;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            
        }

        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = (int)(Projectile.damage * 1.25f);
        }

        public override void AI()
        {
            dustTimer++;
            if (dustTimer == 3)
            {
                dustTimer = 0;
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.localAI[0] == 1)
                return; // prevent infinite recursion

            Projectile.localAI[0] = 1;

            Vector2 launchVelocity1 = Projectile.velocity;
            launchVelocity1 = Vector2.Normalize(launchVelocity1) * 25f;
            Vector2 launchVelocity2 = launchVelocity1.RotatedBy(MathHelper.ToRadians(90));
            Vector2 launchVelocity3 = launchVelocity1.RotatedBy(MathHelper.ToRadians(180));
            Vector2 launchVelocity4 = launchVelocity1.RotatedBy(MathHelper.ToRadians(270));
            Vector2 launchVelocity5 = launchVelocity1.RotatedBy(MathHelper.ToRadians(360));
            target.AddBuff(ModContent.BuffType<Dragonfire>(), 300, false);
                
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity2, ModContent.ProjectileType<DragonForcePro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity3, ModContent.ProjectileType<DragonForcePro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity4, ModContent.ProjectileType<DragonForcePro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, launchVelocity5, ModContent.ProjectileType<DragonForcePro2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, 0, 1);
            Projectile.Kill();
        }    
    }
}