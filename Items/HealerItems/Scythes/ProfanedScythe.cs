using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Rarities;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ProfanedScythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            Item.damage = 124;
            scytheSoulCharge = 3;
            Item.width = 86;
            Item.height = 90;
            Item.value = Item.sellPrice(0, 28, 0);
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.shoot = ModContent.ProjectileType<ProfanedScythePro>();
        }
		
		public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
        }
    }
}
