using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items
{
    public class NewLichRequirement : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.Item.ResearchUnlockCount = 0;
            ItemID.Sets.ItemsThatShouldNotBeInInventory[this.Item.type] = true;
        }

        public override void SetDefaults()
        {
            ((Entity)this.Item).width = 20;
            ((Entity)this.Item).height = 20;
            this.Item.maxStack = 1;
            this.Item.value = 0;
            this.Item.rare = ItemRarityID.White;
        }
    }
}
