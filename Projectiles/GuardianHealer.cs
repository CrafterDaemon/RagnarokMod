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
using Terraria.GameContent;

namespace RagnarokMod.Projectiles
{
	public class GuardianHealer : ModProjectile
	{
		public override string Texture => "CalamityMod/NPCs/ProfanedGuardians/ProfanedGuardianHealer";
		public override void SetStaticDefaults()
		{
			Main.projFrames[base.Projectile.type] = 10;
		}

		public override void SetDefaults()
		{
			base.Projectile.width = 228;
			base.Projectile.height = 164;
			base.Projectile.aiStyle = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.ignoreWater = true;
			base.Projectile.timeLeft = 7200;
			base.Projectile.netImportant = true;
			base.Projectile.scale = 0.5f;
		}

		public override void PostDraw(Color lightColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (Projectile.spriteDirection == -1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Texture2D texture = ModContent.Request<Texture2D>(this.Texture + "Glow").Value;
			int frameHeight = texture.Height / Main.projFrames[Type];
			int startY = frameHeight * Projectile.frame;
			Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
			Vector2 origin = sourceRectangle.Size() / 2f;
			float offsetX = 0f;
			origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
			Color drawColor = Projectile.GetAlpha(lightColor);
			Main.EntitySpriteDraw(texture,
				Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
				sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            texture = ModContent.Request<Texture2D>(this.Texture + "Glow2").Value;
            frameHeight = texture.Height / Main.projFrames[Type];
            startY = frameHeight * Projectile.frame;
            sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            origin = sourceRectangle.Size() / 2f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
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
			base.Projectile.direction = -player.direction;
			base.Projectile.spriteDirection = -player.direction;
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

		public int yOffset = -146;


        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            int frameHeight = texture.Height / Main.projFrames[Type];
            int startY = frameHeight * Projectile.frame;
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);
            Vector2 origin = sourceRectangle.Size() / 2f;
            float offsetX = 0f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);
            return false;
        }
    }
}
