using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Riffs;
using RagnarokMod.Projectiles.Riffs;
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
            Empowerments.Add<MovementSpeed>(2);
            Empowerments.Add<AttackSpeed>(2);
            Empowerments.Add<Damage>(2);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {

        }

        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            if (target == bardPlayer)
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), bardPlayer.Center, Vector2.Zero, ModContent.ProjectileType<StellarAurora>(), 0, 0f, bardPlayer.whoAmI, bardPlayer.whoAmI);
        }
    }
}