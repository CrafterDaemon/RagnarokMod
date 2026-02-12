using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using RagnarokMod.Dusts;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;


namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class BrimScythePro : ScythePro
    {
        public override void SafeSetStaticDefaults()
        {}

        public override void SafeSetDefaults(){
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            rotationSpeed = 0.25f;
            scytheCount = 2;
            base.Projectile.Size = new Vector2(280,320);
        }
    }
}
