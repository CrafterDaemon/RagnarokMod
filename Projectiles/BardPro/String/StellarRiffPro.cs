using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;

using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles.Bard;

using CalamityMod;
using CalamityMod.Dusts;

using RagnarokMod.Utils;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;

namespace RagnarokMod.Projectiles.BardPro.String
{
	public class StellarRiffPro : BardProjectile, ILocalizedModType
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public int spawnTime = 0;
		public int spawnFreq = 60;
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
			ProjectileID.Sets.TrailCacheLength[Type] = 5;
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}
		public override void SetBardDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;
			Projectile.scale = 1.25f;
			Projectile.friendly = true;
			Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.penetrate = 1;
			Projectile.timeLeft = 180;
		}


		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			for (int i = 0; i < 10; i++)
			{
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Main.rand.Next(-3, 3), Main.rand.Next(-3, 3), 0, default, 1.25f).noGravity = true;
			}
			return true;
		}

		public override void AI()
		{
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0)
				{
					Projectile.alpha = 0;
				}
			}

			if (Projectile.ai[0] == 1f)
			{
				Projectile.extraUpdates = 2;
			}
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 4)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame > 3)
			{
				Projectile.frame = 0;
			}
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 25;
			}
			if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.PiOver2;
			if (spawnTime < spawnFreq)
				spawnTime++;
			else
			{
				spawnTime = 0;
				if (Projectile.ai[2] != 1f) SpawnChild();
			}
				Projectile.localAI[0]++;
			float homingDelay = 20f;
			if (Projectile.localAI[0] > homingDelay)
			{
				float inertia = 16f;
				float homeSpeed = 9f;
				float minDist = 40f;

				Player player = Main.player[Projectile.owner];
				if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>())
				{
					homeSpeed = 11f;
					inertia = 12f;
				}
				NPC target = Projectile.FindNearestNPCIgnoreTiles(800f);
				if (target != null && Projectile.Distance(target.Center) > minDist)
				{
					Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * homeSpeed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;
				}
			}
		}

		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			// Prevent children from splitting again
			if (Projectile.ai[2] == 1f)
			{
				return;
			}
			SpawnChild();
		}

		private void SpawnChild()
		{
			float speed = 8f;
			for (int i = 0; i < RiffProjCount(); i++)
			{
				float angle = MathHelper.ToRadians(90f * i);
				Vector2 velocity = angle.ToRotationVector2() * speed;

				Projectile newProj = Projectile.NewProjectileDirect(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					velocity,
					Projectile.type,
					Projectile.damage / 4,
					Projectile.knockBack,
					Projectile.owner
				);
				newProj.scale = 0.6f;
				newProj.ai[2] = 1f;
				newProj.timeLeft = 300;
			}
		}

		private int RiffProjCount()
        {
            Player player = Main.player[Projectile.owner];
			if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>())
			{
				return 4;
			}
			else return 2;
        }

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, Projectile.alpha));
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 16; i++)
			{
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralOrange>(), Main.rand.Next(-8, 10), Main.rand.Next(-8, 10), 0, default, 1.5f).noGravity = true;
				Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<AstralBlue>(), Main.rand.Next(-4, 5), Main.rand.Next(-4, 5), 0, default, 1.5f).noGravity = true;
			}
		}
    }
}