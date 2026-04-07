using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using ThoriumMod;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class AstralRipper : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 135;
            scytheSoulCharge = 2;
            base.Item.width = 40;
            base.Item.height = 38;
            base.Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            base.Item.rare = ItemRarityID.Cyan;
            base.Item.shoot = ModContent.ProjectileType<AstralRipperPro>();
            base.Item.shootSpeed = 0.1f;
            base.Item.channel = true; // hold to channel
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 16;
                Item.useAnimation = 16;
                Item.channel = false;
                Item.noMelee = true;
            }
            else
            {
                Item.useTime = 30;
                Item.useAnimation = 30;
                Item.channel = true;
                Item.noMelee = false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                // Only one charge swing at a time
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.owner == player.whoAmI
                        && p.type == ModContent.ProjectileType<AstralRipperChargeSwing>())
                        return false;
                }
                Projectile.NewProjectile(source, position, velocity,
                    ModContent.ProjectileType<AstralRipperChargeSwing>(),
                    damage * 3, knockback * 1.5f, player.whoAmI);
                return false;
            }

            // Left-click: only spawn the scythe if one isn't already active
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    return false;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}