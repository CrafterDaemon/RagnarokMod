using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using RagnarokMod.Dusts;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles.Scythe;


namespace RagnarokMod.Projectiles.HealerPro.Scythes
{
    public class BrimScytheFirePro : ScythePro
    {
        public override string Texture => "RagnarokMod/Projectiles/NoProj";

        public override void SafeSetDefaults()
        {
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ownerHitCheck = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 26;
            Projectile.knockBack = 0f;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 40;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            rotationSpeed = 0.25f;
            base.Projectile.Size = new Vector2(450, 500);
            dustCount = 2;
            dustType = 60;
            Projectile.light = 1f;
            fadeOutTime = 10;
            fadeOutSpeed = 30;
            dustOffset = new Vector2(-135f, 40f);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return true;
        }


        public override void ModifyDust(Dust dust, Vector2 position, int scytheIndex)
        {
            dust.scale = 3.0f;
            dust.noLight = false;
        }

        public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300, false);
            if (target.IsHostile(null, false))
            {
                if (target.life <= 0 && !target.active && Main.player[base.Projectile.owner].statLife <= (int)(0.25f * Main.player[base.Projectile.owner].statLifeMax2))
                {
                    Main.player[base.Projectile.owner].HealLife(3 + (int)(target.lifeMax * 0.05f), null, true, true);
                }
            }
        }
    }
}
