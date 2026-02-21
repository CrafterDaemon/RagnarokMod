using CalamityMod;
using CalamityMod.Cooldowns;
using CalamityMod.Dusts;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.BardPro.Riffs;
using RagnarokMod.Projectiles.Riffs;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class DragonRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 300 * 60;

        public override void SetStaticDefaults()
        {
            Empowerments.Add<FlatDamage>(10);
            Empowerments.Add<Damage>(8);
            Empowerments.Add<AttackSpeed>(8);
            Empowerments.Add<CriticalStrikeChance>(8);
            Empowerments.Add<FlightTime>(6);
            Empowerments.Add<ResourceMaximum>(6);
        }
        public override void SafeUpdate(Player bardPlayer, Player target)
        {
            if (target != bardPlayer)
                return;

            // === Falling embers from sky ===
            if (Main.rand.NextBool(2))
            {
                Vector2 spawnPos = bardPlayer.Center + new Vector2(
                    Main.rand.NextFloat(-Main.screenWidth / 2, Main.screenWidth / 2),
                    -Main.screenHeight / 2
                );
                Vector2 vel = new Vector2(Main.rand.NextFloat(-0.8f, 0.8f), Main.rand.NextFloat(1.5f, 4f));

                Dust ember = Dust.NewDustPerfect(spawnPos, DustID.AmberBolt, vel,
                    80, default, Main.rand.NextFloat(0.8f, 1.2f));
                ember.noGravity = true;
                ember.fadeIn = 0.5f;
            }

            // Occasional larger falling ember with gravity
            if (Main.rand.NextBool(6))
            {
                Vector2 spawnPos = bardPlayer.Center + new Vector2(
                    Main.rand.NextFloat(-Main.screenWidth / 2, Main.screenWidth / 2),
                    -Main.screenHeight / 2
                );
                Dust bigEmber = Dust.NewDustPerfect(spawnPos, DustID.AmberBolt,
                    new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(1f, 2.5f)),
                    0, default, Main.rand.NextFloat(2f, 3f));
                bigEmber.noGravity = false;
            }
        }
    }
}