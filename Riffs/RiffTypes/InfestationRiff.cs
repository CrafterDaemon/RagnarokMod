using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Riff;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class InfestationRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 5400;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<DamageReduction>(1);
            Empowerments.Add<JumpHeight>(2);
        }

        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), target.Center, Vector2.Zero,
                ModContent.ProjectileType<InfestationMushroomHat>(), 0, 0f, target.whoAmI, target.whoAmI);
        }
    }
}