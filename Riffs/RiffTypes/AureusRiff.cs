using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class AureusRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 180 * 60;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<FlightTime>(3);
            Empowerments.Add<DamageReduction>(3);
			Empowerments.Add<LifeRegeneration>(3);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {
            target.GetAttackSpeed(DamageClass.Generic) += 0.15f;
            target.GetDamage(DamageClass.Generic) += 0.1f;
            target.moveSpeed += 0.1f;
            for (int i = 0; i < 24; i++){
				float angle = MathHelper.TwoPi * i / 24f + Main.GameUpdateCount * 0.05f;
				Vector2 pos = target.Center + Vector2.UnitX.RotatedBy(angle) * 60f;
			
				int dustType = (i % 2 == 0)
					? ModContent.DustType<AstralOrange>()
					: ModContent.DustType<AstralBlue>();
			
				Dust dust = Dust.NewDustPerfect(pos, dustType);
				dust.noGravity = true;
				dust.velocity = Vector2.Zero;
			}
        }
    }
}