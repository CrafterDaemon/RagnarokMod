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
            base.Item.damage = 464;
            scytheSoulCharge = 3;
            base.Item.width = 86;
            base.Item.height = 90;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ProfanedScythePro>();
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
