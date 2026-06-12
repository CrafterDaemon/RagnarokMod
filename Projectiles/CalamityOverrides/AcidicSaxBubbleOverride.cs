using CalamityMod;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using RagnarokMod.Utils;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.CalamityOverrides{
    public class AcidicSaxBubbleOverride : BardProjectile, ILocalizedModType{
        public override string Texture => "CalamityMod/Projectiles/Magic/AcidicSaxBubble";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetStaticDefaults(){
			Main.projFrames[base.Type] = 7;
		}

		public override void SetBardDefaults(){
			base.Projectile.width = 30;
			base.Projectile.height = 30;
			base.Projectile.friendly = true;
			base.Projectile.ignoreWater = true;
			base.Projectile.penetrate = -1;
			base.Projectile.alpha = 255;
			base.Projectile.usesIDStaticNPCImmunity = true;
			base.Projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI(){
			base.Projectile.frameCounter++;
			if (base.Projectile.frameCounter > 6){
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
			if (base.Projectile.frame > 6){
				base.Projectile.frame = 0;
			}
			if (base.Projectile.owner == Main.myPlayer)
			{
				if (this.counter >= 120f){
					this.counter = 0f;
					Vector2 mistRandDirection;
					mistRandDirection = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					mistRandDirection.Normalize();
					mistRandDirection *= (float)Main.rand.Next(50, 401) * 0.01f;
					int damage = (int)Main.player[base.Projectile.owner].GetTotalDamage<MagicDamageClass>().ApplyTo(32f);
					Projectile.NewProjectile(base.Projectile.GetSource_FromThis(null), base.Projectile.Center.X, base.Projectile.Center.Y, mistRandDirection.X, mistRandDirection.Y, ModContent.ProjectileType<AcidicSaxMist>(), damage, 1f, base.Projectile.owner, 0f, 0f, 0f);
				}
				else{
					this.counter += 1f;
				}
			}
			if (base.Projectile.ai[0] == 0f){
				base.Projectile.ai[1] += 1f;
				if (base.Projectile.ai[1] >= 6f){
					if (base.Projectile.alpha > 0){
						base.Projectile.alpha -= 20;
					}
					if (base.Projectile.alpha < 80){
						base.Projectile.alpha = 80;
					}
				}
				if (base.Projectile.ai[1] >= 45f){
					base.Projectile.ai[1] = 45f;
					if (this.counter2 < 1f){
						this.counter2 += 0.002f;
						base.Projectile.scale += 0.002f;
						base.Projectile.width = (int)(30f * base.Projectile.scale);
						base.Projectile.height = (int)(30f * base.Projectile.scale);
					}
					else{
						base.Projectile.width = 60;
						base.Projectile.height = 60;
					}
					if (base.Projectile.wet){
						if (base.Projectile.velocity.Y > 0f){
							base.Projectile.velocity.Y = base.Projectile.velocity.Y * 0.98f;
						}
						if (base.Projectile.velocity.Y > -1f){
							base.Projectile.velocity.Y = base.Projectile.velocity.Y - 0.2f;
						}
					}
					else if (base.Projectile.velocity.Y > -2f){
						base.Projectile.velocity.Y = base.Projectile.velocity.Y - 0.05f;
					}
				}
				this.killCounter++;
				if (this.killCounter >= 200){
					base.Projectile.Kill();
				}
			}
			WindHomingCommon(null, 384f, null, null, false);
			base.Projectile.StickyProjAI(15, false);
		}

		public override void BardModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers){
			base.Projectile.ModifyHitNPCSticky(3);
		}

		public override bool? CanDamage(){
			if (base.Projectile.ai[0] != 1f){
				return base.CanDamage();
			}
			return new bool?(false);
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox){
			if (targetHitbox.Width > 8 && targetHitbox.Height > 8){
				targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			}
			return null;
		}

		public override bool PreDraw(ref Color lightColor){
			Texture2D texture2D13 = TextureAssets.Projectile[base.Type].Value;
			int framing = TextureAssets.Projectile[base.Type].Value.Height / Main.projFrames[base.Type];
			int y6 = framing * base.Projectile.frame;
			Main.spriteBatch.Draw(texture2D13, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle?(new Rectangle(0, y6, texture2D13.Width, framing)), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2((float)texture2D13.Width / 2f, (float)framing / 2f), base.Projectile.scale, 0, 0f);
			return false;
		}

		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone){
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180, false);
			if (base.Projectile.ai[2] == 1f){
				target.AddBuff(BuffID.Poisoned, 180, false);
			}
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info){
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180, true, false);
			if (base.Projectile.ai[2] == 1f){
				target.AddBuff(BuffID.Poisoned, 180, true, false);
			}
		}

		public override void OnKill(int timeLeft){
			base.Projectile.position = base.Projectile.Center;
			base.Projectile.width = (base.Projectile.height = 64);
			base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
			base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
			SoundEngine.PlaySound(SoundID.Item54, new Vector2?(base.Projectile.Center), null);
			int inc;
			for (int i = 0; i < 25; i = inc + 1){
				int toxicDust = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.CursedTorch, 0f, 0f, 0, default(Color), 1f);
				Main.dust[toxicDust].position = (Main.dust[toxicDust].position + base.Projectile.position) / 2f;
				Main.dust[toxicDust].velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				Main.dust[toxicDust].velocity.Normalize();
				Main.dust[toxicDust].velocity *= (float)Main.rand.Next(1, 30) * 0.1f;
				Main.dust[toxicDust].alpha = base.Projectile.alpha;
				inc = i;
			}
			base.Projectile.maxPenetrate = -1;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			base.Projectile.Damage();
		}
		public float counter;
		public float counter2;
		public int killCounter;
    }
}
