using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using CalamityMod.Items;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using RagnarokMod.Dusts;
namespace RagnarokMod.Projectiles.BardPro.String
{
    public class UnbreakableCombatUkulelePro1 : ModProjectile, ILocalizedModType
    {
        int counter = 0;
        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = ThoriumDamageBase<BardDamage>.Instance;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 200;
            Projectile.ignoreWater = true;

        }
        public override void AI()
        {
            counter++;

            float direction = Projectile.velocity.ToRotation();
            direction += MathHelper.PiOver2;
            Projectile.rotation = direction;
            if (counter == 5) {
                Dust.NewDust(Projectile.Center, 25, 25, ModContent.DustType<TemplateDust>(), 0, 0, 255, new Color(236,164,172));
                counter = 0;
            }
        }
    }
}