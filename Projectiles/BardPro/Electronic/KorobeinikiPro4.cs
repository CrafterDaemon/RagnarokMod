﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using System;
using System.Collections.Generic;
namespace RagnarokMod.Projectiles.BardPro.Electronic
{
    public class KorobeinikiPro4 : ModProjectile, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;


        }
        public override void AI()
        {

        }
    }
}