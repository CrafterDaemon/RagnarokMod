using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Materials
{
    public class StrangeAlienMotherBoard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.33f;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(silver: 10);
            Item.rare = ItemRarityID.Orange;
        }
    }
}
