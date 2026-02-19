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
        public override BardInstrumentType InstrumentType{
            get{
                return BardInstrumentType.String;
            }
        }
		public override void SetStaticDefaults(){
			Main.projFrames[base.Type] = 4;
			ProjectileID.Sets.TrailCacheLength[base.Type] = 5;
			ProjectileID.Sets.TrailingMode[base.Type] = 0;
		}
        public override void SetBardDefaults() {
            Projectile.width = 20;
            Projectile.height = 20;
			Projectile.scale = 2f;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = true;
			base.Projectile.alpha = 255;
			base.Projectile.penetrate = 0;
			base.Projectile.timeLeft = 180;
        }

        public override void OnSpawn(IEntitySource source){
			DrawOffsetX = -10;
            DrawOriginOffsetY = -20;
            Projectile.position.Y -= Projectile.height / 2;
        }
		
		private Vector2 SafeDirectionTo(Vector2 from, Vector2 to, Vector2 fallback){
            Vector2 direction = to - from;
            if (direction == Vector2.Zero)
                return fallback;
            return Vector2.Normalize(direction);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) {
            for (int i = 0; i < 10; i++){
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, ModContent.DustType<AstralOrange>(), (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f).noGravity = true;
            }
			return true;
        }

        public override void AI(){
			if (base.Projectile.ai[0] == 1f){
				base.Projectile.extraUpdates = 2;
				float maxVelocity = 3f;
				if (base.Projectile.velocity.Length() < maxVelocity){
					base.Projectile.velocity *= 1.015f;
					if (base.Projectile.velocity.Length() > maxVelocity){
						base.Projectile.velocity.Normalize();
						base.Projectile.velocity *= maxVelocity;
					}
				}
			}
			else if (Vector2.Distance(new Vector2(base.Projectile.ai[0], base.Projectile.ai[1]), base.Projectile.Center) < 80f){
				base.Projectile.tileCollide = true;
			}
			base.Projectile.frameCounter++;
			if (base.Projectile.frameCounter > 4){
				base.Projectile.frame++;
				base.Projectile.frameCounter = 0;
			}
			if (base.Projectile.frame > 3){
				base.Projectile.frame = 0;
			}
			if (base.Projectile.alpha > 0){
				base.Projectile.alpha -= 25;
			}
			if (base.Projectile.alpha < 0){
				base.Projectile.alpha = 0;
			}
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + 1.5707964f;
			
			 // Homing
            float inertia = 90f;
            float homeSpeed = 20f;
            float minDist = 40f;

			NPC target = Projectile.FindNearestNPC(800f);
            if (target != null && Projectile.Center.Distance(target.Center) > minDist){
                Vector2 direction = SafeDirectionTo(Projectile.Center, target.Center, Vector2.UnitY);
                Projectile.velocity = (Projectile.velocity * (inertia - 1f) + direction * homeSpeed) / inertia;
            }
		}

        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Inflate(5, 5);
        }
		
		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
		}
		
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++){
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, ModContent.DustType<AstralOrange>(), (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.5f).noGravity = true;
            }
        }
    }
}