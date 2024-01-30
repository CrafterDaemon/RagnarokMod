using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro
{
    public class ScoriaDualscythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.Size = new Vector2(120f);
            dustOffset = new Vector2(-33f, 7f);
            dustCount = 4;
            dustType = 236;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 15;
        }
    }
}
