using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Projectiles.Boss;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.BardPro.Riff;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Animations;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Utilities;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class DevourerRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 120*60;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<Damage>(4);
            Empowerments.Add<DamageReduction>(4);
            Empowerments.Add<ResourceMaximum>(5);
            Empowerments.Add<ResourceRegen>(5);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {
            target.Calamity().infiniteFlight = true;
            //target.GetThoriumPlayer().bardResource = target.GetThoriumPlayer().bardResourceMax;
        }

        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), bardPlayer.Center, Vector2.Zero, ModContent.ProjectileType<DoGTeleportRift>(), 0, 0);
            Projectile.NewProjectile(bardPlayer.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<DevourerRiffSine>(), 0, 0f, target.whoAmI, target.whoAmI);
        }
    }
}