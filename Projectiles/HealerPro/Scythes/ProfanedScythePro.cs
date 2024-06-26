using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ProfanedScythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(362, 384);
            dustOffset = new Vector2(-35f, 7f);
            dustCount = 4;
            dustType = 87;
            base.rotationSpeed = 0.25f;
            Projectile.light = 1f;
            fadeOutSpeed = 30;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<HolyInferno>(), 180);
        }
    }
}
