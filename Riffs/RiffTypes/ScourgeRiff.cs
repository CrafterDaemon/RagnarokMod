using CalamityMod;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.Riffs;
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
        public override int CooldownTicks => 5400;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<LifeRegeneration>(2);
            Empowerments.Add<InvincibilityFrames>(2);
            Empowerments.Add<Damage>(1);
            Empowerments.Add<AttackSpeed>(1);
            Empowerments.Add<MovementSpeed>(1);
        }
        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ScourgesFretsWorm>(), 0, 0f, target.whoAmI, target.whoAmI);
        }
    }
}