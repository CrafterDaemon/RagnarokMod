using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using CalamityMod;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using CalamityMod.Items;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class JellySlicer : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 47;
            scytheSoulCharge = 2;
            base.Item.width = 48;
            base.Item.height = 44;
            base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<GelScythePro1>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            Vector2 velocity2 = player.Center - Main.MouseWorld;
            Vector2 vel = velocity2.SafeNormalize(Vector2.UnitX);
            float multiplier = -14f;

            Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<GelScythePro2>(), damage, knockback, player.whoAmI);



            return false;

        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<PurifiedGel>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
