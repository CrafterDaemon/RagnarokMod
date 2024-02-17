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
using CalamityMod.NPCs.CalClone;

namespace RagnarokMod.Projectiles.BardPro
{
	public class CalamityBellPro2 : BardProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}

		public override BardInstrumentType InstrumentType
		{
			get
			{
				return BardInstrumentType.Percussion;
			}
		}

		public override void SetBardDefaults()
		{
			base.Projectile.width = 26;
			base.Projectile.height = 13;
			base.Projectile.alpha = 255;
			base.Projectile.aiStyle = -1;
			base.Projectile.penetrate = 1;
			base.Projectile.friendly = true;
			base.Projectile.timeLeft = 240;
			this.forwardRotation = true;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
				hitbox.Inflate(16, 16);
		}
		
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 240, false);
			if(target.type == ModContent.NPCType<CalamitasClone>() || target.type == ModContent.NPCType<Cataclysm>() || target.type == ModContent.NPCType<Catastrophe>()) 
			{
				damageDone = (int)(damageDone * 1.5f);
			}
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(Color.White * (0.25f + 0.01f * (float)base.Projectile.timeLeft));
		}


		public override void AI()
		{
			int dustType = 90;
			int numDusts = 3;
			for (int k = 0; k < numDusts; k++)
			{
				Vector2 offset = base.Projectile.velocity / (float)numDusts * (float)k;
				Dust.NewDustPerfect(base.Projectile.Center - offset, dustType, new Vector2?(Vector2.Zero), 0, default(Color), 1f).noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			for (int j = 0; j < 10; j++)
			{
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, 90, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.25f).noGravity = true;
			}
		}
	}
}
