using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.HealerItems.Other;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class LucentGemini : ScytheItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 410;
            scytheSoulCharge = 3;
            base.Item.width = 76;
            base.Item.height = 74;
            base.Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            base.Item.rare = ItemRarityID.Cyan;
            base.Item.shoot = ModContent.ProjectileType<LucentGeminiPro>();
            Item.reuseDelay = 8;
            Item.channel = true;
            Item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return player.Calamity().StratusStarburst >= 5 || ConstellationActive(player);
        }

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;

            // Keep starburst timer alive while holding
            player.Calamity().StratusStarburstResetTimer = (int)MathHelper.Max(
                player.Calamity().StratusStarburstResetTimer, 600);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                int constellationType = ModContent.ProjectileType<GeminiConstellation>();

                // If constellation is already active, dismiss it
                if (ConstellationDismissable(player))
                {
                    for (int i = 0; i < Main.maxProjectiles; i++)
                    {
                        Projectile p = Main.projectile[i];
                        if (p.active && p.type == constellationType && p.owner == player.whoAmI)
                        {
                            p.ai[0] = 1f;
                            p.timeLeft = (int)MathHelper.Min(p.timeLeft, 60);
                        }
                    }
                }
                else if (!ConstellationActive(player))
                {
                    // Spawn constellation above/behind player
                    Projectile.NewProjectile(source,
                        player.Center + new Vector2(-125, -100), Vector2.Zero,
                        constellationType,
                        (int)(damage * 2f), knockback, player.whoAmI);
                }
                return false;
            }

            // Left-click: normal spin
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<LucentGeminiPro>();
        }

        private static bool ConstellationActive(Player player)
        {
            int type = ModContent.ProjectileType<GeminiConstellation>();
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
            int type = ModContent.ProjectileType<GeminiConstellation>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI && p.ai[1] == 1f)
                    return true;
            }
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 8);
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
            recipe.AddIngredient(ModContent.ItemType<AstralRipper>(), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
