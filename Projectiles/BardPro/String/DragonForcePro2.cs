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

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class DragonForcePro2 : ModProjectile, ILocalizedModType
    {
        public int timer = 0;
        public int dustTimer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 36;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
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
                Dust.NewDust(Projectile.Center, 0, 0, DustID.Torch);
            }

            Projectile.rotation = Projectile.velocity.ToRotation();

            NPC npc = Projectile.FindNearestNPC(800);
            if (npc != null)
            {
                timer = 0;
                Projectile.HomeInOnTarget(npc, 20f, 0.15f);
            }

            if (timer >= 36 && npc == null) { Projectile.Kill(); }
        }
    }
}