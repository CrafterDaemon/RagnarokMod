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
            ((Entity)((ModProjectile)this).Projectile).Size = new();
        }
    }
}
