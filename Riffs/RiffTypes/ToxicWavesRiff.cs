using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Riff;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class ToxicWavesRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 5400;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<Defense>(3);
            Empowerments.Add<LifeRegeneration>(3);
            Empowerments.Add<DamageReduction>(3);
        }

        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ToxicWavesVortex>(), 0, 0f, target.whoAmI, target.whoAmI);
        }
    }
}
