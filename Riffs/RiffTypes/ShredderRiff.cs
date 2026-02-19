using CalamityMod;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.Riffs;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class ShredderRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 54;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<LifeRegeneration>(2);
            Empowerments.Add<AttackSpeed>(2);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {
            target.GetRagnarokModPlayer().shredderLifesteal = true;
        }
        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<ShredderTendrils>(), 0, 0f, target.whoAmI, target.whoAmI);
        }
    }
}