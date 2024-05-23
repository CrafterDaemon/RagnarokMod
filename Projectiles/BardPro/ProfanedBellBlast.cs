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

	public class ProfanedBellBlast : BardProjectile
	{

        public override string Texture => "CalamityMod/Projectiles/Boss/HolyBlast";
        public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}
		
		public override void SetBardDefaults()
		{
			base.Projectile.width = 72;
			base.Projectile.height = 196;
			base.Projectile.penetrate = 1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 240;
			Projectile.scale = 0.33f;
			Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
		}

		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
		}
		public override void AI() 
		{
		
			if (++Projectile.frameCounter >= 5) 
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.frame = 0;
           
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
		
		}
		
		
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 25;
			height = 25;
			return true;
		}
		
		
		  public override bool PreDraw(ref Color lightColor) {
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
			spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			int frameHeight = texture.Height / Main.projFrames[Type];
			int startY = frameHeight * Projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			float offsetX = 20f;
			origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
			return false;
		}
		
		
		public override void OnKill(int timeLeft)
		{
			SoundEngine.PlaySound(HolyBlast.ImpactSound, new Vector2?(base.Projectile.Center), null);
			Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), base.Projectile.Center, Vector2.Zero, ModContent.ProjectileType<BellBlastBoom>(), (int)((float)base.Projectile.damage * 0.75f), 1f, base.Projectile.owner, 0f, 0f, 0f);
			for (int j = 0; j < 10; j++)
			{
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.GemTopaz, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.25f).noGravity = true;
			}
		}
		
	}
}
