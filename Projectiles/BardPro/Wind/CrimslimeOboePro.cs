using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;

using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles.Bard;

using CalamityMod;

using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.BardPro.Wind
{
    public class CrimslimeOboePro : BardProjectile, ILocalizedModType{
        public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		private bool Stuck;
		private int stuckNPC = -1;
		private Vector2 stickOffset;
		private int explodeTimer;
		public const int ExplodeDelay = 2;
		
        public override void SetBardDefaults(){
            Projectile.width = 40;
            Projectile.height = 22;
			Projectile.scale = 0.75f;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
        }
	
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone){
			SpawnImpactDust();
			if (Stuck)
				return;
			Vector2 impactDirection = Vector2.Normalize(Projectile.velocity);
			Stuck = true;
			stuckNPC = target.whoAmI;
			
			float npcSize = (target.width + target.height) * 0.25f;
			float embedDepth = MathHelper.Clamp(npcSize * 0.15f, 6f, 20f);
			Projectile.Center += impactDirection * embedDepth;
			stickOffset = Projectile.Center - target.Center;
		
			Projectile.velocity = Vector2.Zero;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.timeLeft = 180;
			Projectile.aiStyle = 0;
			Projectile.penetrate = -1;
			Projectile.netUpdate = true;
		}

		public override void AI(){
			if (Stuck){
				if (stuckNPC < 0 || !Main.npc[stuckNPC].active){
					Projectile.Kill();
					return;
				}
				NPC target = Main.npc[stuckNPC];
				Projectile.Center = target.Center + stickOffset;
				explodeTimer++;
				if (explodeTimer >= 60 * ExplodeDelay){
					Explode();
				}
				return;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			 
			for (int i = 0; i < 4; i++){
				Vector2 offset = Projectile.velocity * -0.2f;
				int dustIndex = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					117,
					offset.X,
					offset.Y,
					150,
					default,
					1f * Projectile.scale
				);
				Main.dust[dustIndex].noGravity = true;
				Main.dust[dustIndex].velocity *= 0.5f;
			}
			 
			 WindHomingCommon(null, 384f, null, null, false);
		}
		
		private void Explode(){
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			
			Vector2 oldCenter = Projectile.Center;
			int explosionSize = 80;
			Projectile.width = explosionSize;
			Projectile.height = explosionSize;
			Projectile.Center = oldCenter;
			Projectile.Damage();
		
			for (int i = 0; i < 25; i++){
				Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
				Dust dust = Dust.NewDustPerfect(
					Projectile.Center,
					117,
					velocity,
					0,
					default,
					1.5f
				);
				dust.noGravity = true;
			}
			SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			Projectile.Kill();
		}
		
        public override bool OnTileCollide(Vector2 oldVelocity){
			SpawnImpactDust();
			return true;
        }
		
		private void SpawnImpactDust(){
			int dustAmount = (int)(12f * Projectile.scale);
			for (int i = 0; i < dustAmount; i++){
				Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f) * Projectile.scale;
				int dustIndex = Dust.NewDust(
					Projectile.position,
					Projectile.width,
					Projectile.height,
					117,
					velocity.X,
					velocity.Y,
					100,
					default,
					1.2f * Projectile.scale
				);
				Main.dust[dustIndex].noGravity = false;
			}
		}
		
        public override void OnKill(int timeLeft){
            for (int i = 0; i < 8; i++){
                Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, 117 , Main.rand.Next(-2, 3), Main.rand.Next(-2, 3), 0, default, 1.5f).noGravity = true;
            }
        }
    }
}