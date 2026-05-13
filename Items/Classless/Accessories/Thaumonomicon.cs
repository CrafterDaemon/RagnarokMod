using RagnarokMod.Utils;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Classless.Accessories
{
    public class Thaumonomicon : ModItem
    {
        public override bool IsLoadingEnabled(Mod mod) => false;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = Item.sellPrice(gold: 5);
            Item.rare = Terraria.ID.ItemRarityID.LightRed;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RagnarokModPlayer>().HasThaumonomicon = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(Terraria.ID.ItemID.Book)
                .AddIngredient(Terraria.ID.ItemID.CrystalShard, 3)
                .AddTile(Terraria.ID.TileID.Bookcases)
                .Register();
        }
    }
}