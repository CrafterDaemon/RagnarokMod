using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items;

namespace RagnarokMod.Items.Materials
{
    public class StrangeAlienMotherBoard : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 9999;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
        }
    }
}
