using CalamityMod.Dusts;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.ModSupport.ModSupportModules;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class EctambourinePro : TambourinePro
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Percussion/Ectambourine";
        public override void SetBardDefaults()
        {
            base.Projectile.aiStyle = -1;
            base.AIType = ProjectileID.WoodenBoomerang;
            base.Projectile.friendly = true;
            base.Projectile.timeLeft = 240;
            Projectile.width = 38;
            Projectile.height = 40;
            Projectile.penetrate = 3;
            Projectile.tileCollide = false;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 10;
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            float returnTime = 30f;
            float maxDistance = 800f;
            Player owner = Main.player[Projectile.owner];
            float distToOwner = Vector2.Distance(Projectile.Center, owner.Center);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(base.Projectile.position - base.Projectile.velocity, base.Projectile.width, base.Projectile.height, DustID.RedTorch, 0f, 0f, 200, Color.White, 2f);
                dust.velocity *= 0.2f;
                dust.noGravity = true;
            }

            if (Projectile.ai[0] > returnTime || distToOwner > maxDistance || Projectile.penetrate <= 2)
            {
                Projectile.penetrate = -1;
                Vector2 direction = owner.Center - Projectile.Center;
                direction.Normalize();
                Vector2 targetVelocity = direction * 16f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVelocity, 0.1f);


                if (distToOwner < 50f)
                    Projectile.Kill();
            }
            Projectile.rotation += 0.4f;
        }
    }
    public class EctambourineProJingle : ShadeWoodTambourinePro2
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Percussion/Ectambourine";
        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Projectile.width = 38;
            Projectile.height = 40;
            fadeOutTime = 30;
        }
    }
}