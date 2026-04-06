using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.HealerPro.Other;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.BossMini;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Misc;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Rarities;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Other
{
    [LegacyName("Fractal")]
    public class Asterion : ThoriumItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }


        public override void SetDefaults()
        {
            base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            base.Item.damage = 220;
            this.isHealer = true;
            this.healDisplay = true;
            base.Item.width = 60;
            base.Item.mana = 12;
            base.Item.height = 66;
            base.Item.useTime = 12;
            base.Item.useAnimation = 36;
            base.Item.autoReuse = true;
            base.Item.reuseDelay = 48;
            base.Item.useStyle = ItemUseStyleID.Shoot;
            base.Item.noMelee = true;
            base.Item.knockBack = 5.2f;
            base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            base.Item.rare = ModContent.RarityType<PureGreen>();
            base.Item.UseSound = new SoundStyle?(SoundID.Item8);
            base.Item.shoot = ModContent.ProjectileType<FractalOrb>();
            base.Item.shootSpeed = 8f;
        }

        private static bool ConstellationActive(Player player)
        {
            int type = ModContent.ProjectileType<AsterionConstellation>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    return true;
            }
            return false;
        }
        private static bool ConstellationDismissable(Player player)
        {
            int type = ModContent.ProjectileType<AsterionConstellation>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI && p.ai[1] == 1f)
                    return true;
            }
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.Calamity().StratusStarburst >= 5 || ConstellationActive(player);
        }

        public override void HoldItem(Player player)
        {
            // Keep starburst timer alive while holding the weapon
            player.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(
                player.Calamity().StratusStarburstResetTimer, 600);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                // If constellation is already active, dismiss it
                if (ConstellationDismissable(player))
                {
                    type = ModContent.ProjectileType<AsterionConstellation>();
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.type == type && p.owner == player.whoAmI)
                            p.ai[0] = 1f;
                            p.timeLeft = (int)MathHelper.Min(p.timeLeft, 60);
                    }
                }
                else if(!ConstellationActive(player))
                {
                    Projectile.NewProjectile(source,
                        player.Center + new Vector2(-125, -100), Vector2.Zero,
                        ModContent.ProjectileType<AsterionConstellation>(),
                        (int)(damage * 4f), knockback, player.whoAmI);
                }
                return false;
            }

            // Left-click: fire LyraOrb normally toward the cursor
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
    }
}
