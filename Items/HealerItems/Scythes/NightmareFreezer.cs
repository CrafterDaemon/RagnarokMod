using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using System.CommandLine.Help;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;

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
            base.Item.width = 110;
            base.Item.height = 94;
            base.Item.useTime = 20;

            base.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
            base.Item.rare = ModContent.RarityType<CosmicPurple>();
            base.Item.shoot = ModContent.ProjectileType<NightmareFreezerPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 velocity2 = player.Center - Main.MouseWorld;
            Vector2 vel = velocity2.SafeNormalize(Vector2.UnitX);
            float multiplier = -20f;
            counter++;
            
            Vector2 vel2 = vel.RotatedBy(MathHelper.ToRadians(8));
            Vector2 vel3 = vel.RotatedBy(MathHelper.ToRadians(-8));
            Vector2 vel4 = vel.RotatedBy(MathHelper.ToRadians(4));
            Vector2 vel5 = vel.RotatedBy(MathHelper.ToRadians(-4));
            if (counter == 1) {
                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel2 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel3 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel4 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
		        Projectile.NewProjectileDirect(source, position, vel5 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
            }
            if (counter == 2) {
                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel2 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel3 * multiplier, ModContent.ProjectileType<NightmareFreezerPro3>(), damage, knockback, player.whoAmI);
                Projectile.NewProjectileDirect(source, position, vel4 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
		        Projectile.NewProjectileDirect(source, position, vel5 * multiplier, ModContent.ProjectileType<NightmareFreezerPro2>(), damage, knockback, player.whoAmI);
                counter = 0;            
            }
            
    
			return false;
        
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
