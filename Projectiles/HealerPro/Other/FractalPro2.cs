using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class FractalPro2 : ModProjectile, ILocalizedModType
    {
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
        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            NPC npc = Projectile.FindNearestNPC(750);
            if (npc != null) {
                Projectile.HomeInOnTarget(npc, 15f, 0.15f);
            }
        }
    }
}