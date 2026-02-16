using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;
using Terraria.ID;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ScoriaDualscythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {
        }

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.2f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 5;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            rotationSpeed = 0.2f;
            scytheCount = 4;
            Projectile.Size = new Vector2(120f);
            dustOffset = new Vector2(-33f, 6f);
            dustCount = 4;
            dustType = 303;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff((BuffID.OnFire3), 180);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff((BuffID.OnFire3), 180);
            target.AddBuff(ModContent.BuffType<CrushDepth>(), 180);
        }
    }
}
