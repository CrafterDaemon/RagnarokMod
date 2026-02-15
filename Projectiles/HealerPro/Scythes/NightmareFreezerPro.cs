
using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using RagnarokMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;


namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class NightmareFreezerPro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            base.Projectile.Size = new Vector2(204, 182);
            dustOffset = new Vector2(-35f, 7f);
            dustCount = 4;
            dustType = DustID.BorealWood;
            base.rotationSpeed = 0.25f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 6;
            Projectile.light = 1f;
            fadeOutSpeed = 10;
            rotationSpeed = 0.25f;
            scytheCount = 1;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Nightwither>(), 180);
        }
    }
}
