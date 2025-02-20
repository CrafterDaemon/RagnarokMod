using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using CalamityMod.Items.Materials;
using CalamityMod.Items;

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
            base.Item.damage = 145;
            scytheSoulCharge = 2;
            base.Item.width = 80;
            base.Item.height = 76;
            base.Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            base.Item.rare = ItemRarityID.Red;
            base.Item.shoot = ModContent.ProjectileType<AstralRipperPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            
            float num = velocity.Length();
            for (int i = 0; i < 2; i++)
            {
                float fallspeedmult = 50;
                float projSpeed = num * Main.rand.NextFloat(0.7f, 1.4f);
                float x = Main.MouseWorld.X + Main.rand.NextFloat(0f - 50f, 50f);
                float y = Main.MouseWorld.Y - Main.rand.NextFloat(650f, 850f);
                
                Vector2 vector = new Vector2(x, y);
                Vector2 vel = Main.MouseWorld - vector;
                vel.X += Main.rand.NextFloat(0f - 130f, 130f);
                vel.Y += Main.rand.NextFloat(0f - 260f, 260f);
                float n = vel.Length();
                n = projSpeed/n;
                vel.X *= n;
                vel.Y *= n*fallspeedmult;
                Projectile.NewProjectileDirect(source, vector, vel, ModContent.ProjectileType<AstralRipperStarPro>(), damage, knockback, player.whoAmI);
            }
    
			return false;
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
