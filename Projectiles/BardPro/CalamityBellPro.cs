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

namespace RagnarokMod.Projectiles.BardPro
{
	public class CalamityBellPro : BardProjectile
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
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.alpha = 255;
			base.Projectile.aiStyle = -1;
			base.Projectile.penetrate = 1;
			base.Projectile.friendly = true;
			base.Projectile.timeLeft = 240;
			this.forwardRotation = true;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			if (base.Projectile.ai[1] > 0f)
			{
				hitbox.Inflate(8, 8);
			}
		}

		/*
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (base.Projectile.ai[1] > 0f)
			{
				target.AddBuff(44, 300, false);
				for (int i = 0; i < 15; i++)
				{
					Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, 92, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.25f).noGravity = true;
				}
			}
		}

		*/

		
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
