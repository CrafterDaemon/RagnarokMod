using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using RagnarokMod;
using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.TweakedProjectiles
{
	public class BlackMIDIProOverride : BardProjectile
	{
		public override string Texture => "ThoriumMod/Projectiles/Bard/BlackMIDIPro";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
		}

		public override BardInstrumentType InstrumentType
		{
			get
			{
				return BardInstrumentType.Wind;
			}
		}

		public override void SetBardDefaults()
		{
			base.Projectile.width = 6;
			base.Projectile.height = 6;
			base.Projectile.alpha = 255;
			base.Projectile.aiStyle = 1;
			base.Projectile.penetrate = 1;
			base.Projectile.friendly = true;
			base.Projectile.extraUpdates = 2;
			base.AIType = 14;
			base.Projectile.timeLeft = 120;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, 50) * 0.75f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D value = TextureAssets.Projectile[base.Projectile.type].Value;
			Vector2 vector = new Vector2((float)value.Width * 0.5f, (float)value.Height * 0.5f);


			for (int i = base.Projectile.oldPos.Length - 1; i > 0; i--)
			{
				Vector2 vector2 = base.Projectile.oldPos[i] - Main.screenPosition + vector + new Vector2(0f, base.Projectile.gfxOffY);
				Color color = base.Projectile.GetAlpha(new Color(255, 255, 255, 50) * 0.75f) * ((float)(base.Projectile.oldPos.Length - i) / (float)base.Projectile.oldPos.Length);
				Main.EntitySpriteDraw(value, vector2, null, color, base.Projectile.rotation, vector, base.Projectile.scale, 0, 0f);
			}
			return true;
		}

		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (target.IsHostile(null, false))
			{
				Main.player[base.Projectile.owner].HealLife((int)(Math.Sqrt(damageDone / 20)), null, true, true);
			}
		}

		public override void AI()
		{
			Lighting.AddLight(base.Projectile.Center, 0.4f, 0.3f, 0.1f);
		}

		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, 87, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.25f).noGravity = true;
			}
		}
	}
}
