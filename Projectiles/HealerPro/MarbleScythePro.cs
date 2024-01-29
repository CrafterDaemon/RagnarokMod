using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro
{
    public class MarbleScythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(126f);
            dustOffset = new Vector2(-28f, 6f);
            dustCount = 4;
            dustType = 236;
        }
    }
}
