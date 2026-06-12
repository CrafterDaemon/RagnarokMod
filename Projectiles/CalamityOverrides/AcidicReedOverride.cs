using CalamityMod;
using CalamityMod.Projectiles.Magic;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using Terraria.ID;

namespace RagnarokMod.Projectiles.CalamityOverrides
{
    public class AcidicReedOverride : BardProjectile, ILocalizedModType{
        public override string Texture => "CalamityMod/Projectiles/Magic/AcidicReed";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;

        public override void SetBardDefaults(){
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.friendly = true;
			base.Projectile.timeLeft = 600;
			base.Projectile.tileCollide = true;
			base.Projectile.ignoreWater = false;
			base.Projectile.penetrate = 1;
		}

		public override void AI(){
			if (base.Projectile.ai[0] == 1f){
				SoundEngine.PlaySound(AcidicReed.SaxSound, new Vector2?(base.Projectile.Center), null);
				base.Projectile.ai[0] = 0f;
			}
			if (base.Projectile.velocity.Y < 10f){
				Projectile projectile = base.Projectile;
				projectile.velocity.Y = projectile.velocity.Y + 0.25f;
			}
			base.Projectile.rotation = Terraria.Utils.ToRotation(base.Projectile.velocity);
			WindHomingCommon(null, 384f, null, null, false);
		}

		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone){
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180, false);
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info){
			target.AddBuff(ModContent.BuffType<Irradiated>(), 180, true, false);
		}

		public override void OnKill(int timeLeft){
			for (int i = 0; i <= 2; i++){
				int idx = Dust.NewDust(base.Projectile.position, 8, 8, DustID.CursedTorch, 0f, 0f, 0, default(Color), 0.75f);
				Main.dust[idx].noGravity = true;
				Main.dust[idx].velocity *= 3f;
				idx = Dust.NewDust(base.Projectile.position, 8, 8, DustID.CursedTorch, 0f, 0f, 0, default(Color), 0.75f);
				Main.dust[idx].noGravity = true;
				Main.dust[idx].velocity *= 3f;
			}
		}
		public static readonly SoundStyle SaxSound = new SoundStyle("CalamityMod/Sounds/Item/Saxophone/Sax", 6, 0);
        
    }
}
