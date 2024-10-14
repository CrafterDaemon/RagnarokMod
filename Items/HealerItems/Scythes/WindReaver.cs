using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using CalamityMod.Items;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class WindReaver : ScytheItem
    {
        public int counter = 0;

        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 36;
            scytheSoulCharge = 2;
            base.Item.width = 80;
            base.Item.height = 76;
            base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<WindReaverPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (counter == 2) {
                counter = 0;
                Vector2 velocity2 = player.Center - Main.MouseWorld;
                Vector2 vel = velocity2.SafeNormalize(Vector2.UnitX);
                float multiplier = -20f;

                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<WindSlashPro>(), damage, knockback, player.whoAmI);
            }
            counter += 1;
            
    
			return false;
        
		}
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SunplateBlock, 8);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
