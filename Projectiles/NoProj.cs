using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles
{
    public class NoProj : BardProjectile
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.Other;

        public override void SetBardDefaults()
        {
            ((Entity)((ModProjectile)this).Projectile).width = 0;
            ((Entity)((ModProjectile)this).Projectile).height = 0;
            ((ModProjectile)this).Projectile.aiStyle = 0;
            ((ModProjectile)this).Projectile.scale = 0f;
            ((ModProjectile)this).Projectile.friendly = true;
            ((ModProjectile)this).Projectile.penetrate = 1;
            ((ModProjectile)this).Projectile.timeLeft = 0;
            ((ModProjectile)this).AIType = 0;
        }
    }
}
