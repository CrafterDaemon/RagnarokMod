using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Core.Handlers.HoverItemHandler;
using ThoriumMod.Tiles;

namespace RagnarokMod.Items
{
    /*
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
    */

    public class HavocPhylacteryAdjustment : GlobalTile
    {
        Player localPlayer = Main.LocalPlayer;
        public override void MouseOver(int i, int j, int type)
        {
            if (type == ModContent.TileType<AncientPhylactery>())
            {
                if (AncientPhylactery.DownedAllMechBosses && !Main.IsItDay())
                {
                    localPlayer.noThrow = 2;
                    HoverItemSystem.QueueHoverItem(ModContent.ItemType<EssenceofHavoc>(), 3);
                    return;
                }
            }
        }
    }
}
