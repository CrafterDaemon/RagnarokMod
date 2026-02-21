using CalamityMod;
using CalamityMod.Cooldowns;
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
    public class HiveMindRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 5400;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<ResourceMaximum>(2);
            Empowerments.Add<ResourceGrabRange>(2);
            Empowerments.Add<ResourceConsumptionChance>(2);
        }

        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            if (Main.myPlayer == bardPlayer.whoAmI)
            {
                // Kill any existing spore clouds from this player
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    if (Main.projectile[i].active
                        && Main.projectile[i].type == ModContent.ProjectileType<InfectedSporeCloud>()
                        && Main.projectile[i].owner == bardPlayer.whoAmI)
                    {
                        Main.projectile[i].Kill();
                    }
                }

                // Spawn fresh spore cloud
                Projectile.NewProjectile(
                    bardPlayer.GetSource_Misc("HiveMindRiff"),
                    bardPlayer.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<InfectedSporeCloud>(),
                    bardPlayer.HeldItem.damage,
                    bardPlayer.HeldItem.knockBack,
                    bardPlayer.whoAmI
                );
            }
        }
    }
}