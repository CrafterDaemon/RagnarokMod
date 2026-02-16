using CalamityMod;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class ScourgeRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 3600;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<LifeRegeneration>(2);
            Empowerments.Add<InvincibilityFrames>(2);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {
            target.GetAttackSpeed(DamageClass.Generic) += 0.05f;
            target.GetDamage(DamageClass.Generic) += 0.05f;
            target.moveSpeed += 0.1f;
            for (int i = 0; i < 24; i++)
            {
                float angle = MathHelper.TwoPi * i / 24f + Main.GameUpdateCount * 0.05f;
                Vector2 pos = target.Center + Vector2.UnitX.RotatedBy(angle) * 60f;
                Dust dust = Dust.NewDustPerfect(pos, DustID.Sand);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
            }
        }

        public override void SafeOnStart(Player bardPlayer)
        {
        }
    }
}