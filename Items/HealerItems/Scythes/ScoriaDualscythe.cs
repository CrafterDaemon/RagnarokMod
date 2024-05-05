using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.HealerPro.Scythes;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ScoriaDualscythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 90;
            scytheSoulCharge = 3;
            base.Item.width = 114;
            base.Item.height = 116;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ScoriaDualscythePro>();
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ScoriaBar>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
