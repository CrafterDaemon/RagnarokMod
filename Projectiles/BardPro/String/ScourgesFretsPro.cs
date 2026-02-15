using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Items;
using CalamityMod;
using Terraria.DataStructures;
using RagnarokMod.Utils;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class ScourgesFretsPro : BardProjectile, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType
        {
            get
            {
                return BardInstrumentType.String;
            }
        }
        public override void SetBardDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            base.Projectile.width = 20;
            base.Projectile.height = 20;
            base.Projectile.aiStyle = ProjAIStyleID.Arrow;
            base.Projectile.alpha = 75;
            base.Projectile.penetrate = -1;
            base.Projectile.timeLeft = 120;
            base.AIType = ProjectileID.Bullet;
            base.DrawOffsetX = -5;
            base.DrawOriginOffsetY = -10;
            this.fadeOutTime = 30;
            this.fadeOutSpeed = 7;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Player owner = Main.player[Projectile.owner];
            if (owner.GetRagnarokModPlayer().fretPlaying)
            {
                Projectile.width = 40;
                Projectile.height = 40;
                Projectile.scale = 2f;
                DrawOffsetX = -10;
                DrawOriginOffsetY = -20;
            }
            Projectile.position.Y -= Projectile.height / 2;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.Sand, (float)Main.rand.Next(-3, 3), (float)Main.rand.Next(-3, 3), 0, default(Color), 1.25f).noGravity = true;
            }
            return base.Projectile.Bounce(oldVelocity, this.collideMax + Main.player[base.Projectile.owner].GetThoriumPlayer().bardBounceBonus, ref this.collide, 1f);

        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(base.Projectile.position - base.Projectile.velocity, base.Projectile.width, base.Projectile.height, DustID.Sand, 0f, 0f, 0, default(Color), 1f);
                dust.alpha = 125;
                dust.velocity *= 0.3f;
                dust.noGravity = true;
            }
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Inflate(5, 5);
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDustDirect(base.Projectile.position, base.Projectile.width, base.Projectile.height, DustID.Sand, (float)Main.rand.Next(-4, 5), (float)Main.rand.Next(-4, 5), 0, default(Color), 1.5f).noGravity = true;
            }
        }

        public int collide;
        public int collideMax = 1;
    }
}