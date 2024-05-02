using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Scythe;
using CalamityMod;

using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using CalamityMod.Items.Placeables;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro;
using System.Diagnostics.Metrics;

namespace RagnarokMod.Items.HealerItems
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
            base.Item.value = Item.sellPrice(0, 28, 0);
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
                float multiplier = -14f;

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
