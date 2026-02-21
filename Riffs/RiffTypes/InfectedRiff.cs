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
    public class InfectedRiff : Riff
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
        }
    }
}