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

using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class CorroslimeBassPro : BardProjectile, ILocalizedModType{
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public static int MaxSplits = 2;
		public static int ProjperSplit = 3;
		public int SplitCount = 0;
		public int collide;
        public int collideMax = 2;
        
        public override void SetBardDefaults(){
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 420;
        }

        public override bool OnTileCollide(Vector2 oldVelocity){
			SpawnImpactDust();
			return base.Projectile.Bounce(oldVelocity, this.collideMax + Main.player[base.Projectile.owner].GetThoriumPlayer().bardBounceBonus, ref this.collide, 1f);
        }
		
		public void SplitProjectile(){
			if (SplitCount >= MaxSplits)
			return;

			float totalSpread = 40f;
			float startAngle = -totalSpread / 2f;
			for (int i = 0; i < ProjperSplit; i++){
				float angleOffset;
				if (ProjperSplit == 1){
					angleOffset = 0f;
				}
				else{
					angleOffset = MathHelper.ToRadians(startAngle + (totalSpread / (ProjperSplit - 1)) * i);
				}
				Vector2 newVelocity = Projectile.velocity.RotatedBy(angleOffset);
	
				Projectile newProj = Projectile.NewProjectileDirect(
					Projectile.GetSource_FromThis(),
					Projectile.Center,
					newVelocity,
					Projectile.type,
					Projectile.damage / 2,
					Projectile.knockBack,
					Projectile.owner
				);
				newProj.scale = Projectile.scale * 0.75f;
				if (newProj.ModProjectile is CorroslimeBassPro child){
					child.SplitCount = this.SplitCount + 1;
				}
			}
			Projectile.Kill();
		}
		
		private void SpawnImpactDust(){
			int dustAmount = (int)(12f * Projectile.scale);
			for (int i = 0; i < dustAmount; i++){
				Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f) * Projectile.scale;
				int dustIndex = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					14,
					velocity.X,
					velocity.Y,
					100,
					default,
					1.2f * Projectile.scale
				);
				Main.dust[dustIndex].noGravity = false;
			}
		}
		
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone){
			SpawnImpactDust();
		}

        public override void AI(){
			float gravity = 0.25f;
			float maxFallSpeed = 10f;
			Projectile.velocity.Y += gravity;
			if (Projectile.velocity.Y > maxFallSpeed)
			Projectile.velocity.Y = maxFallSpeed;
			
			Projectile.rotation += 0.2f;
			int dustAmount = (int)(2f * Projectile.scale);
			for (int i = 0; i < dustAmount; i++){
				Vector2 offset = Projectile.velocity * -0.2f;
				int dustIndex = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					14,
					offset.X,
					offset.Y,
					150,
					default,
					1f * Projectile.scale
				);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 0.5f;
			}
		}

        public override void OnKill(int timeLeft){
            for (int i = 0; i < 8; i++){
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 14, Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 0, default, 1.5f).noGravity = true;
            }
        }
    }
}