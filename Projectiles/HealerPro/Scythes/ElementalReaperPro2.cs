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

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ElementalReaperPro2 : ModProjectile, ILocalizedModType
    {
        public int dustTimer = 0;
        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 250;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {
            dustTimer++;
            if (dustTimer == 1)
            {
                dustTimer = 0;
                int rainbow = Dust.NewDust(Projectile.position, 2, 2, DustID.RainbowTorch, 0f, 0f, 150, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), 1.3f);
                Main.dust[rainbow].noGravity = true;
                Main.dust[rainbow].velocity *= 0f;
            }

            float direction = Projectile.velocity.ToRotation();
            direction += MathHelper.PiOver2;
            Projectile.rotation = direction;

            NPC npc = Projectile.FindNearestNPC(600);
            if (npc != null)
            {
                Projectile.HomeInOnTarget(npc, 15f, 0.12f);
            }
            
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			    modifiers.FinalDamage *= 0.8f;
            }
    }
}