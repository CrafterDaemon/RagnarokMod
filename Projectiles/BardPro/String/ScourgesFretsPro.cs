using CalamityMod;
using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Utils;
using RagnarokMod.Riffs;
using RagnarokMod.Riffs.RiffTypes;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;
using ThoriumMod.Utilities;

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
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
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
            if (owner.GetRagnarokModPlayer().activeRiffType == RiffLoader.RiffType<ScourgeRiff>())
            {
                Projectile.width = 60;
                Projectile.height = 60;
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

        public override bool PreDraw(ref Color lightColor)
        {
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