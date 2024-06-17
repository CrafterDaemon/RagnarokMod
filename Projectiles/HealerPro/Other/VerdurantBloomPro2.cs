// Decompiled with JetBrains decompiler
// Type: ThoriumMod.Projectiles.Healer.BalanceBloomPro
// Assembly: ThoriumMod, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D385B2C-ED5D-4D9C-B022-312790A283EB
// Assembly location: C:\Users\daemo\Desktop\tModUnpacker\ThoriumMod\ThoriumMod.dll

using CalamityMod.Buffs.DamageOverTime;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
#nullable disable
using ThoriumMod;
namespace RagnarokMod.Projectiles.HealerPro.Other
{
  public class VerdurantBloomPro2 : ModProjectile, ILocalizedModType
  {
    public int timer = 0;
    public int dustTimer = 0;
    public override void SetDefaults()
    {
      this.Projectile.DamageType = (DamageClass) ThoriumDamageBase<HealerDamage>.Instance;
      ((Entity) this.Projectile).width = 18;
      ((Entity) this.Projectile).height = 14;
      this.Projectile.aiStyle = -1;
      this.Projectile.friendly = true;
      this.Projectile.penetrate = 1;
      this.Projectile.timeLeft = 250;
    }

    public override Color? GetAlpha(Color lightColor)
    {
      return new Color?(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 150) * (1f * this.Projectile.Opacity));
    }

    public override void AI()
    {
      timer++;
      dustTimer++;
      if (dustTimer == 3)
      {
        dustTimer = 0;
        Dust.NewDust(Projectile.Center, 0, 0, 56);
      }
      Projectile.rotation = Projectile.velocity.ToRotation();
      NPC npc = Projectile.FindNearestNPC(500);
      if (npc != null)
      {
        timer = 0;
        Projectile.HomeInOnTarget(npc, 15f, 0.15f);
      }
      if (timer >= 100 && npc == null) { Projectile.Kill(); }      
    }

    public override void OnKill(int timeLeft)
    {
      for (int index1 = 0; index1 < 5; ++index1)
      {
        int index2 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 54, (float) Main.rand.Next(-2, 2), (float) Main.rand.Next(-2, 2), 0, new Color(80, 160, 80), 1f);
        Main.dust[index2].noGravity = true;
      }
    }
  }
}
