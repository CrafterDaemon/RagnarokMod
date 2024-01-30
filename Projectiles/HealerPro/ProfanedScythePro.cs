using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro
{
    public class ProfanedScythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(160f);
            dustOffset = new Vector2(-35f, 7f);
            dustCount = 4;
            dustType = 236;
        }
    }
}
