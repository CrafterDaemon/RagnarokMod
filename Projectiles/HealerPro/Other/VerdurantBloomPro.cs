

using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
#nullable disable
namespace RagnarokMod.Projectiles.HealerPro.Other
{
  public class VerdurantBloomPro : ModProjectile, ILocalizedModType
  {
    
    public override void SetDefaults()
    {
      this.Projectile.DamageType = (DamageClass) ThoriumDamageBase<HealerDamage>.Instance;
      ((Entity) this.Projectile).width = 18;
      ((Entity) this.Projectile).height = 14;
      this.Projectile.aiStyle = -1;
      this.Projectile.friendly = true;
      this.Projectile.penetrate = 1;
      this.Projectile.timeLeft = 150;
    }

    public override Color? GetAlpha(Color lightColor)
    {
      return new Color?(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 150) * (1f * this.Projectile.Opacity));
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
      target.AddBuff(ModContent.BuffType<HolyFlames>(), 180, false);
    }

    public override void AI()
    {
      Projectile projectile = this.Projectile;
      ((Entity) projectile).velocity = ((Entity) projectile).velocity * 0.97f;
      Projectile.rotation = Projectile.velocity.ToRotation();
    }

    public override void OnKill(int timeLeft)
    {
      for (int index1 = 0; index1 < 5; ++index1)
      {
        int index2 = Dust.NewDust(((Entity) this.Projectile).position, ((Entity) this.Projectile).width, ((Entity) this.Projectile).height, 54, (float) Main.rand.Next(-2, 2), (float) Main.rand.Next(-2, 2), 0, new Color(255, 173, 17), 1f);
        Main.dust[index2].noGravity = true;
      }
    }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
          modifiers.FinalDamage *= 1.5f;
        }
    }
}
