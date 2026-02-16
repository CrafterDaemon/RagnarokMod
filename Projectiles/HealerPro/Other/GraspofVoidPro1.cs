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

namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class GraspofVoidPro1 : ModProjectile, ILocalizedModType
    {
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 22;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;
            Projectile.light = 0.5f;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }
        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            Lighting.AddLight(((Entity)this.Projectile).Center, 0.3f, 0.45f, 0.1f);
            for (int index3 = 0; index3 < 2; ++index3)
            {
                int index4 = Dust.NewDust(new Vector2(((Entity)this.Projectile).position.X, ((Entity)this.Projectile).position.Y), ((Entity)this.Projectile).width, ((Entity)this.Projectile).height, ModContent.DustType<TemplateDust>(), 0.0f, 0.0f, 100, new Color(0, 0, 0), 1.25f);
                Main.dust[index4].velocity *= 0.2f;
                Main.dust[index4].noGravity = true;
            }
            for (int index5 = 0; index5 < 1; ++index5)
            {
                int index6 = Dust.NewDust(new Vector2(((Entity)this.Projectile).position.X, ((Entity)this.Projectile).position.Y), ((Entity)this.Projectile).width, ((Entity)this.Projectile).height, DustID.ShadowbeamStaff, 0.0f, 0.0f, 100, new Color(), 1f);
                Main.dust[index6].velocity *= 0.2f;
                Main.dust[index6].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundEngine.PlaySound(SoundID.Item107, new Vector2?(((Entity)this.Projectile).Center), (SoundUpdateCallback)null);
            if (Main.myPlayer != this.Projectile.owner)
            {
                return;
            }
            int num1 = ModContent.ProjectileType<GraspofVoidPro2>();
            int num2 = (int)((double)this.Projectile.damage * 0.8);
            for (int index = 0; index < 8; ++index)
            {
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), ((Entity)this.Projectile).Center.X, ((Entity)this.Projectile).Center.Y, (float)Main.rand.Next(-10, 10), (float)Main.rand.Next(-10, 10), num1, num2, 5f, this.Projectile.owner, 0.0f, 0.0f, 0.0f);
            }
        }
    }
}