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

namespace RagnarokMod.Projectiles.BardPro
{
	public class BellBlastBoom : BardProjectile
	{
		public override void SetBardDefaults()
		{
			base.Projectile.width = 100;
			base.Projectile.height = 100;
			base.Projectile.penetrate = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 1;
			Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
		}
		
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
			
		}
		
	}
}
