using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using RagnarokMod.Utils;
using System.Threading;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class FractalPro2 : ModProjectile, ILocalizedModType
    {
        public int timer = 0;
        public int dustTimer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {
            timer++;
            dustTimer++;
            if (dustTimer == 3)
            {
                dustTimer = 0;
                Dust.NewDust(Projectile.Center, 0, 0, 56);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC npc = Projectile.FindNearestNPCIgnoreTiles(2000);
            if (npc != null)
            {
                timer = 0;
                Projectile.HomeInOnTarget(npc, 15f, 0.15f);
            }

            if (timer >= 36 && npc == null) { Projectile.Kill(); }
        }
    }
}