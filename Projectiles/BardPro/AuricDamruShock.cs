using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod;

namespace RagnarokMod.Projectiles.BardPro
{
	public class AuricDamruShock : BardProjectile
	{
		public override BardInstrumentType InstrumentType
		{
			get
			{
				return BardInstrumentType.Percussion;
			}
		}

		public override void SetBardDefaults()
		{
			base.Projectile.ownerHitCheck = true;
			base.Projectile.width = 700;
			base.Projectile.height = 700;
			base.Projectile.aiStyle = -1;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 10;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[base.Projectile.owner];
			Vector2 vector = player.Center - Main.screenPosition;
			vector.Y += player.gfxOffY;
			Texture2D value = TextureAssets.Projectile[base.Projectile.type].Value;
			Vector2 vector2 = Terraria.Utils.Size(value) / 2f;
			Main.EntitySpriteDraw(value, vector, null, new Color(255, 255, 255, 0) * (0.02f * (float)base.Projectile.timeLeft), base.Projectile.rotation, vector2, 1.7f, 0, 0f);
			return false;
		}

		public override void BardModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.HitDirectionOverride = new int?((target.Center.X < Main.player[base.Projectile.owner].Center.X) ? (-1) : 1);
		}

		public override bool? CanCutTiles()
		{
			return new bool?(false);
		}
	}
}
