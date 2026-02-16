using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class NightmareFreezer : ScytheItem
    {
        public int counter = 0;
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 320;
            scytheSoulCharge = 3;
            base.Item.width = 55;
            base.Item.height = 47;
            base.Item.useTime = 20;

            base.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            base.Item.rare = ModContent.RarityType<CosmicPurple>();
            base.Item.shoot = ModContent.ProjectileType<NightmareFreezerPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 velocity2 = player.Center - Main.MouseWorld;
            Vector2 vel = velocity2.SafeNormalize(Vector2.UnitX);
            float multiplier = -20f;
            counter++;

            Vector2 vel2 = vel.RotatedBy(MathHelper.ToRadians(8));
            Vector2 vel3 = vel.RotatedBy(MathHelper.ToRadians(-8));
            Vector2 vel4 = vel.RotatedBy(MathHelper.ToRadians(4));
            Vector2 vel5 = vel.RotatedBy(MathHelper.ToRadians(-4));
            if (counter == 1)
            {
                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel2 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel3 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel4 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel5 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
            }
            if (counter == 2)
            {
                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel2 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel3 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel4 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel5 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                counter = 0;
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
            recipe.AddIngredient(ModContent.ItemType<EndothermicEnergy>(), 10);
            recipe.AddIngredient(ModContent.ItemType<NightmareFuel>(), 10);
            recipe.AddTile<CosmicAnvil>();
            recipe.Register();
        }
    }
}
