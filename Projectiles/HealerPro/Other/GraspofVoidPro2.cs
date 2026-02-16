using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using ThoriumMod.Utilities;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using RagnarokMod.Dusts;
using Terraria.Utilities;
using Microsoft.CodeAnalysis.Editing;


namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class GraspofVoidPro2 : ModProjectile, ILocalizedModType
    {

        private int spacer = 0;
        public override void SetDefaults()
        {
            this.Projectile.DamageType = (DamageClass)ThoriumDamageBase<HealerDamage>.Instance;
            ((Entity)this.Projectile).width = 34;
            ((Entity)this.Projectile).height = 34;
            this.Projectile.aiStyle = -1;
            this.Projectile.friendly = true;
            this.Projectile.penetrate = -1;
            this.Projectile.timeLeft = 40;
            this.Projectile.extraUpdates = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 600, false);
        }
        public override void AI()
        {
            spacer++;
            ((Entity)this.Projectile).velocity.Y += Main.rand.NextFloat(-1.25f, 1.25f);
            ((Entity)this.Projectile).velocity.X += Main.rand.NextFloat(-1.25f, 1.25f);
            Vector2 center = ((Entity)this.Projectile).Center;
            this.Projectile.scale = 1f - this.Projectile.localAI[0];
            ((Entity)this.Projectile).width = (int)(20.0 * (double)this.Projectile.scale);
            ((Entity)this.Projectile).height = ((Entity)this.Projectile).width;
            ((Entity)this.Projectile).position.X = center.X - (float)(((Entity)this.Projectile).width / 2);
            ((Entity)this.Projectile).position.Y = center.Y - (float)(((Entity)this.Projectile).height / 2);
            if ((double)this.Projectile.localAI[0] < 0.1)
                this.Projectile.localAI[0] += 0.01f;
            else
                this.Projectile.localAI[0] += 0.025f;
            if ((double)this.Projectile.localAI[0] >= 0.949999988079071)
                this.Projectile.Kill();
            if ((double)((Entity)this.Projectile).velocity.Length() > 16.0)
            {
                ((Entity)this.Projectile).velocity.Normalize();
                Projectile projectile = this.Projectile;
                ((Entity)projectile).velocity = ((Entity)projectile).velocity * 16f;
            }
            if ((double)this.Projectile.scale >= 1.0)
                return;
            if (spacer == 3)
            {
                for (int index1 = 0; (double)index1 < (double)this.Projectile.scale; ++index1)
                {
                    int index2 = Dust.NewDust(new Vector2(((Entity)this.Projectile).position.X, ((Entity)this.Projectile).position.Y), ((Entity)this.Projectile).width, ((Entity)this.Projectile).height, ModContent.DustType<TemplateDust>(), ((Entity)this.Projectile).velocity.X, ((Entity)this.Projectile).velocity.Y, 100, new Color(10, 10, 10), 0.75f);
                    Main.dust[index2].position = (Main.dust[index2].position + ((Entity)this.Projectile).Center) / 2f;
                    Main.dust[index2].noGravity = true;
                    Main.dust[index2].velocity *= 0.1f;
                    Main.dust[index2].velocity -= ((Entity)this.Projectile).velocity * (1.3f - this.Projectile.scale);
                    Main.dust[index2].fadeIn = (float)(100 + this.Projectile.owner);
                    Main.dust[index2].scale += this.Projectile.scale * 0.5f;
                }
                spacer = 0;
            }
        }
    }
}