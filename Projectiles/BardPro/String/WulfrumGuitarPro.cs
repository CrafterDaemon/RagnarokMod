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
    public class WulfrumGuitarPro : BardProjectile, ILocalizedModType{
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public int collide;
        public int collideMax = 1;
		public bool returning = false;
		float storedRotation;
		bool storedRotationSet = false;
		int returnTimer = 0;
        public override void SetBardDefaults(){
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            base.Projectile.aiStyle = ProjAIStyleID.Arrow;
            base.Projectile.alpha = 75;
            base.Projectile.penetrate = -1;
            base.Projectile.timeLeft = 180;
            base.AIType = ProjectileID.Bullet;
            base.DrawOffsetX = -5;
            base.DrawOriginOffsetY = -10;
            this.fadeOutTime = 30;
            this.fadeOutSpeed = 7;
        }

        public override bool OnTileCollide(Vector2 oldVelocity){
            for (int i = 0; i < 10; i++){
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, 49, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f).noGravity = true;
            }
            return base.Projectile.Bounce(oldVelocity, this.collideMax + Main.player[base.Projectile.owner].GetThoriumPlayer().bardBounceBonus, ref this.collide, 1f);
        }
		
		public void ReturnProjectile(){
			if (returning)
			return;
			returning = true;
			Projectile.aiStyle = 0;
			AIType = 0;
			storedRotation = Projectile.rotation;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1; 
			base.Projectile.timeLeft = 300;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
		}

        public override void AI(){
            for (int i = 0; i < 3; i++){
                Dust dust = Dust.NewDustDirect(base.Projectile.position - base.Projectile.velocity, base.Projectile.width, base.Projectile.height, 49, 0f, 0f, 0, default(Color), 1f);
                dust.alpha = 125;
                dust.velocity *= 0.3f;
                dust.noGravity = true;
            }
			if (!returning)
				return;
			Player player = Main.player[Projectile.owner];
			returnTimer++;
			Vector2 toPlayer = player.Center - Projectile.Center;
			if (!storedRotationSet){
				storedRotationSet = true;
			}
			
			if (returnTimer < 20){
				Projectile.velocity = Vector2.Zero;
				Projectile.rotation = storedRotation;
				return;
			}
			float speed = 16f;
			float inertia = 18f;
			Vector2 desiredVelocity = Vector2.Normalize(toPlayer) * speed;
			Projectile.velocity = (Projectile.velocity * (inertia - 1) + desiredVelocity) / inertia;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.Distance(player.Center) < 20f){
			Projectile.Kill();
			}
		}
			
        public override void ModifyDamageHitbox(ref Rectangle hitbox){
            hitbox.Inflate(5, 5);
        }

        public override void OnKill(int timeLeft){
            for (int i = 0; i < 15; i++){
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, 49, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.5f).noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor){
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = texture.Size() / 2f; // True center
            Main.EntitySpriteDraw(
				texture,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );
            return false;
        }
    }
}