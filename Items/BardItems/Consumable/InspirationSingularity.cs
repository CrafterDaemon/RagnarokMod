using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BardItems;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Rarities;

namespace RagnarokMod.Items.BardItems.Consumable
{
    public class InspirationSingularity : InspirationConsumableBase
    {
        public override int InspirationBase => 50;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InspirationGem>();
            base.SetStaticDefaults();
        }

        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ModContent.RarityType<BurnishedAuric>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<AshesofAnnihilation>(), 10);
            recipe.AddIngredient(ModContent.ItemType<InspirationEssence>(), 1);
            recipe.AddTile(ModContent.TileType<DraedonsForge>());
            recipe.Register();
        }
    }
}