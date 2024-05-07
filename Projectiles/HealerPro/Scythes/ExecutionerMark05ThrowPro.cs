using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Projectiles.Scythe;

namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class ExecutionerMark05ThrowPro : ModProjectile, ILocalizedModType
    {
        public bool LockedIn = false;

        public Vector2 boomSpot = Vector2.Zero;
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 150;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.Size = new Vector2(50f, 50f);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 0;
        }
        public override void AI()
        {
            if (Projectile.direction == 1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                Projectile.spriteDirection = Projectile.direction;
            }
            else
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI - MathHelper.ToRadians(45f);
                Projectile.spriteDirection = Projectile.direction;
            }
            Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 312, 0f, 0f, 100, default, 1f);
            dust.scale = 0.1f + Main.rand.Next(5);
            dust.fadeIn = 1.5f + Main.rand.Next(5) * 0.1f;
            dust.noGravity = true;
            dust.position = Projectile.Center + new Vector2(1, 0).RotatedBy(Projectile.rotation - 2.1f, default) * 10f;
            NPC npc = Projectile.FindNearestNPC(500);
            if (npc != null)
            {
                Projectile.HomeInOnTarget(npc, 22f, 0.15f);
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<WapBamBoom>(), default, default);
        }
    }
}
