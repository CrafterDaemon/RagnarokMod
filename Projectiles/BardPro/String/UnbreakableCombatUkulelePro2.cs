using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace RagnarokMod.Projectiles.BardPro.String
{
    public class UnbreakableCombatUkulelePro2 : BardProjectile, ILocalizedModType
    {
        public override BardInstrumentType InstrumentType => BardInstrumentType.String;

        public override void SetBardDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.aiStyle = -1;

        }
        public override void AI()
        {
            this.Projectile.rotation += this.Projectile.velocity.X / Math.Abs(this.Projectile.velocity.X) * 0.3f;
            Player player = Main.player[this.Projectile.owner];
            if ((double)this.Projectile.ai[0] == 0.0)
            {
                ++this.Projectile.ai[1];
                if ((double)this.Projectile.ai[1] < 40.0)
                    return;
                this.Projectile.ai[0] = 1f;
                this.Projectile.ai[1] = 0.0f;
                this.Projectile.netUpdate = true;
            }
            else
            {
                this.Projectile.tileCollide = false;
                this.Projectile.extraUpdates = 1;
                float num1 = 8f;
                float num2 = 4f;
                Vector2 vector2 = new Vector2(((Entity)this.Projectile).position.X + (float)((Entity)this.Projectile).width * 0.75f, ((Entity)this.Projectile).position.Y + (float)((Entity)this.Projectile).height * 0.75f);
                float num3 = ((Entity)player).position.X + (float)(((Entity)player).width / 5) - vector2.X;
                float num4 = ((Entity)player).position.Y + (float)(((Entity)player).height / 5) - vector2.Y;
                float num5 = (float)Math.Sqrt((double)num3 * (double)num3 + (double)num4 * (double)num4);
                if ((double)num5 > 3000.0)
                    this.Projectile.Kill();
                if ((double)num5 > 0.0)
                    num5 = num1 / num5;
                float num6 = num3 * num5;
                float num7 = num4 * num5;
                if ((double)((Entity)this.Projectile).velocity.X < (double)num6)
                {
                    ((Entity)this.Projectile).velocity.X = ((Entity)this.Projectile).velocity.X + num2;
                    if ((double)((Entity)this.Projectile).velocity.X < 0.0 && (double)num6 > 0.0)
                        ((Entity)this.Projectile).velocity.X = ((Entity)this.Projectile).velocity.X + num2;
                }
                else if ((double)((Entity)this.Projectile).velocity.X > (double)num6)
                {
                    ((Entity)this.Projectile).velocity.X = ((Entity)this.Projectile).velocity.X - num2;
                    if ((double)((Entity)this.Projectile).velocity.X > 0.0 && (double)num6 < 0.0)
                        ((Entity)this.Projectile).velocity.X = ((Entity)this.Projectile).velocity.X - num2;
                }
                if ((double)((Entity)this.Projectile).velocity.Y < (double)num7)
                {
                    ((Entity)this.Projectile).velocity.Y = ((Entity)this.Projectile).velocity.Y + num2;
                    if ((double)((Entity)this.Projectile).velocity.Y < 0.0 && (double)num7 > 0.0)
                        ((Entity)this.Projectile).velocity.Y = ((Entity)this.Projectile).velocity.Y + num2;
                }
                else if ((double)((Entity)this.Projectile).velocity.Y > (double)num7)
                {
                    ((Entity)this.Projectile).velocity.Y = ((Entity)this.Projectile).velocity.Y - num2;
                    if ((double)((Entity)this.Projectile).velocity.Y > 0.0 && (double)num7 < 0.0)
                        ((Entity)this.Projectile).velocity.Y = ((Entity)this.Projectile).velocity.Y - num2;
                }
                if (Main.myPlayer != this.Projectile.owner || !this.Projectile.getRect().Intersects(player.getRect()))
                    return;
                this.Projectile.Kill();

            }
        }
        public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<UnbreakableCombatUkulelePro3>(), Projectile.damage, default);
            this.Projectile.Kill();
        }

    }
}