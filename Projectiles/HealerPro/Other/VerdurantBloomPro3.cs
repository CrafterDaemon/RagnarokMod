// Decompiled with JetBrains decompiler
// Type: ThoriumMod.Projectiles.Healer.BalanceBloomPro
// Assembly: ThoriumMod, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D385B2C-ED5D-4D9C-B022-312790A283EB
// Assembly location: C:\Users\daemo\Desktop\tModUnpacker\ThoriumMod\ThoriumMod.dll

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
#nullable disable
using ThoriumMod.Utilities;
using ThoriumMod;
namespace RagnarokMod.Projectiles.HealerPro.Other
{
    public class VerdurantBloomPro3 : ModProjectile, ILocalizedModType
  {
    public override void SetDefaults()
    {
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
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.velocity *= 0.97f;

            if (!Projectile.TryGetOwner(out Player owner))
                return;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player p = Main.player[i];

                if (!p.active || p.dead)
                    continue;

                if (p.whoAmI == owner.whoAmI)
                    continue;

                if (owner.team == 0 || p.team != owner.team)
                    continue;

                if (Projectile.Hitbox.Intersects(p.Hitbox))
                {
                    Projectile.ThoriumHeal(2, specificPlayer: p.whoAmI, ignoreHealer: false);
                    Projectile.Kill();
                    break;
                }
            }
        }

    }
}
