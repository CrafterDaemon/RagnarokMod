using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Projectiles.Boss;

namespace RagnarokMod.Projectiles.BardPro.Percussion
{
    public class LamentationBoom : BardProjectile
    {
		public override string Texture => "RagnarokMod/Projectiles/BardPro/Percussion/AOE_Blast";
        public override void SetBardDefaults()
        {
            Projectile.width = 110;
            Projectile.height = 110;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 1;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
        }

        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240, false);

        }

    }
}
