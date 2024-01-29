using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod.Dusts;
using ThoriumMod.Utilities;
using RagnarokMod.Utils;
using RagnarokMod.Buffs;

namespace RagnarokMod.Projectiles
{
	public class GuardianHealerBeam : ModProjectile
	{
		public override void SetDefaults()
		{
			base.Projectile.width = 6;
			base.Projectile.height = 6;
			base.Projectile.aiStyle = -1;
			base.Projectile.penetrate = 1;
			base.Projectile.timeLeft = 180;
			base.Projectile.extraUpdates = 100;
		}
		
		public override string Texture
		{
			get
			{
				return "RagnarokMod/Projectiles/NoProj";
			}
		}

		public override void AI()
		{
			Player player = Main.player[base.Projectile.owner];
			if (base.Projectile.localAI[0] == 0f)
			{
				base.Projectile.localAI[0] = 1f;
				SoundEngine.PlaySound(SoundID.Item24, new Vector2?(base.Projectile.Center), null);
			}
			int num454;
			for (int num452 = 0; num452 < 4; num452 = num454 + 1)
			{
				Vector2 vector36 = base.Projectile.position;
				vector36 -= base.Projectile.velocity * ((float)num452 * 0.25f);
				int num453 = Dust.NewDust(vector36, 1, 1, 74, 0f, 0f, 0, default(Color), 1f);
				Main.dust[num453].position = vector36;
				Main.dust[num453].noGravity = true;
				Main.dust[num453].velocity *= 0.4f;
				num454 = num452;
			}
			int num457;
			for (int num455 = 0; num455 < 4; num455 = num457 + 1)
			{
				Vector2 vector37 = base.Projectile.position;
				vector37 -= base.Projectile.velocity * ((float)num455 * 0.25f);
				int num456 = Dust.NewDust(vector37, 1, 1, ModContent.DustType<HolyFireDust>(), 0f, 0f, 0, default(Color), 0.5f);
				Main.dust[num456].position = vector37;
				Main.dust[num456].noGravity = true;
				Main.dust[num456].velocity *= 0.4f;
				num457 = num455;
			}
			base.Projectile.ThoriumHeal(5, 30f, true, true, delegate(Player player, Player target, ref int healAmount, ref int selfHealAmount)
			{
				base.Projectile.velocity = Vector2.Zero;
			}, null, -1, true, false, true);
			for (int i = 0; i < 255; i++)
			{
				Player bufftarget = Main.player[i];
				int buff = ModContent.BuffType<GuardianHealerBeamBuff>();
				if (bufftarget.active && !bufftarget.dead && bufftarget != player && !bufftarget.HasBuff(buff) && base.Projectile.DistanceSQ(bufftarget.Center) < 1600f)
				{
					bufftarget.AddBuff(buff, 1200, true, false);
				}
			}
			
		}
	}
}
