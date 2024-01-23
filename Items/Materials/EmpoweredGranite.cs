using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Materials
{
    public class EmpoweredGranite : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.33f;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 5);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
