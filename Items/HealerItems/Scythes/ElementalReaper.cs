using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Projectiles.HealerPro.Other;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ElementalReaper : ScytheItem
    {

        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 115;
            scytheSoulCharge = 2;
            base.Item.width = 146;
            base.Item.height = 176;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ItemRarityID.Purple;
            base.Item.shoot = ModContent.ProjectileType<ElementalReaperPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 vel1 = new Vector2(10f, 0f);
            Projectile.NewProjectileDirect(source, position, vel1, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
            Vector2 vel2 = new Vector2(0f, 10f);
            Projectile.NewProjectileDirect(source, position, vel2, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
            Vector2 vel3 = new Vector2(-10f, 0f);
            Projectile.NewProjectileDirect(source, position, vel3, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
            Vector2 vel4 = new Vector2(0f, -10f);
            Projectile.NewProjectileDirect(source, position, vel4, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
            
    
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
