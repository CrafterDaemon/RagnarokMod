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
    public class KorobeinikiPro1 : ModProjectile, ILocalizedModType
    {
        private int x = 10;
        private List<float> rotations = new List<float> { MathHelper.PiOver2, MathHelper.Pi, MathHelper.PiOver2 + MathHelper.Pi, 0f };
        public override void SetStaticDefaults()
        {

            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 56;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;


        }
        public override void AI()
        {

            if (x == 10)
            {

                int r = Main.rand.Next(0, 4);

                Projectile.rotation = rotations[r];
                x = 0;
            }

        }
    }
}