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
            Projectile.width = 25;
            Projectile.height = 25;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
			base.Projectile.ignoreWater = true;
			base.Projectile.tileCollide = true;
			base.Projectile.alpha = 255;
			base.Projectile.penetrate = 1;
			base.Projectile.timeLeft = 180;
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
			if (Projectile.alpha > 0){
				Projectile.alpha -= 25;
				if (Projectile.alpha < 0) {
					Projectile.alpha = 0;
				}
			}
			 
			if (base.Projectile.ai[0] == 1f){
				base.Projectile.extraUpdates = 2;
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
			base.Projectile.rotation = (float)Math.Atan2((double)base.Projectile.velocity.Y, (double)base.Projectile.velocity.X) + MathHelper.PiOver2;
			
			Projectile.localAI[0]++;
			float homingDelay = 20f;
			if (Projectile.localAI[0] > homingDelay){
				float inertia = 16f;
				float homeSpeed = 9f;
				float minDist = 40f;
				
				Player player = Main.player[Projectile.owner];
				if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>()){ 
					homeSpeed = 11f;
					inertia = 12f;
				}
				NPC target = Projectile.FindNearestNPC(800f);
				if (target != null && Projectile.Distance(target.Center) > minDist){
					Vector2 desiredVelocity = Projectile.DirectionTo(target.Center) * homeSpeed;
					Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;
				}
			}
		}
		
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone){
			// Prevent children from splitting again
			if (Projectile.ai[2] == 1f) {
				return;
			}

			int numberProjectiles = 2;
			float speed = 8f;
			Player player = Main.player[Projectile.owner];
			if (player.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<AureusRiff>()){ 
				numberProjectiles = 4;
			}
			
			for (int i = 0; i < numberProjectiles; i++){
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

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color?(new Color(255, 255, 255, base.Projectile.alpha));
		}
		
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 8; i++){
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, ModContent.DustType<AstralOrange>(), (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.5f).noGravity = true;
				Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, ModContent.DustType<AstralBlue>(), (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.5f).noGravity = true;
            }
        }
    }
}