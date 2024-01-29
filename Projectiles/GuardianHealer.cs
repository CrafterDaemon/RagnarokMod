using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using RagnarokMod.Utils;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Projectiles;

namespace RagnarokMod.Projectiles
{
	public class GuardianHealer : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[base.Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			base.Projectile.width = 57;
			base.Projectile.height = 39;
			base.Projectile.aiStyle = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.timeLeft = 7200;
			base.Projectile.netImportant = true;
		}

		public override void PostDraw(Color lightColor)
		{
			ProjectileExtras.DrawLikeVanilla(base.Projectile, Color.White * base.Projectile.Opacity, ModContent.Request<Texture2D>(this.Texture + "_Effect").Value, default(Vector2), 
			null, default(Vector2), 0f, 0f);
		}

		public override void AI()
		{
			Player player = Main.player[base.Projectile.owner];
			Lighting.AddLight(base.Projectile.Center, 0.1f, 0.4f, 0.1f);
			if (!player.active || player.dead || !player.GetRagnarokModPlayer().tarraHealer)
			{
				base.Projectile.Kill();
				return;
			}
			base.Projectile.Center = player.Center;
			Projectile projectile = base.Projectile;
			projectile.position.Y = projectile.position.Y + (float)this.yOffset;
			base.Projectile.gfxOffY = player.gfxOffY;
			base.Projectile.direction = player.direction;
			base.Projectile.spriteDirection = player.direction;
			if (!this.shift)
			{
				base.Projectile.alpha += 2;
			}
			else
			{
				base.Projectile.alpha -= 2;
			}
			if (base.Projectile.alpha > 75 && !this.shift)
			{
				this.shift = true;
			}
			if (base.Projectile.alpha <= 0)
			{
				this.shift = false;
			}
			base.Projectile.ai[1] += 1f;
			if (base.Projectile.ai[1] >= 120f)
			{
				base.Projectile.ai[1] = 0f;
				IEntitySource source = base.Projectile.GetSource_FromThis(null);
				if (Main.myPlayer == base.Projectile.owner)
				{
					for (int i = 0; i < 255; i++)
					{
						Player target = Main.player[i];
						if (target.active && !target.dead && target != player && target.statLife < target.statLifeMax2 && base.Projectile.DistanceSQ(target.Center) < 4000000f && Collision.CanHit(base.Projectile.position, base.Projectile.width, base.Projectile.height, target.position, target.width, target.height))
						{
							Vector2 vector = target.Center - base.Projectile.Center;
							float speed = 15f;
							float mag = vector.Length();
							if (mag > speed)
							{
								mag = speed / mag;
								vector *= mag;
							}
							int direct = 2 + 12 * player.direction;
							Projectile.NewProjectile(source, base.Projectile.Center.X + (float)direct, base.Projectile.Center.Y + 8f, vector.X, vector.Y, ModContent.ProjectileType<LifeEssenceBeam>(), 0, 0f, base.Projectile.owner, 0f, 0f, 0f);
						}
					}
				}
			}
		}

		public override void PostAI()
		{
			base.Projectile.frameCounter++;
			if (base.Projectile.frameCounter >= 5)
			{
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
			if (base.Projectile.frame >= Main.projFrames[base.Projectile.type])
			{
				base.Projectile.frame = 0;
			}
		}

		public bool shift;

		public int yOffset = -46;
	}
}
