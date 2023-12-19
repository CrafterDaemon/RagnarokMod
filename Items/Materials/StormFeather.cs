using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Materials
{
    public class StormFeather : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.75f;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 1);
            Item.rare = ItemRarityID.Blue;
        }
    }
}
