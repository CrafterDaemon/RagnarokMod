using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class WindSlashPro : ModProjectile, ILocalizedModType
    {
        public int counter = 0;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.light = 0.5f;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120;
            Projectile.alpha = 50;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.Size = new Vector2(50f, 70f);
            
        }
        public override void AI() {
			Projectile.velocity *= 0.97f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            counter += 1;
            if (counter > 5) {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.SilverFlame, 0f, 0f, 100, Color.White, 0.5f);
                counter = 0;
            }
            
		}
        
    }
}
