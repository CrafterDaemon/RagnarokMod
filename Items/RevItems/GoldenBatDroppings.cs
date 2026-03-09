using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using ThoriumMod;
using ThoriumMod.Utilities;
using CalamityMod.Items;
using CalamityMod;

namespace RagnarokMod.Items.RevItems
{
    public class GoldenBatDroppings : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

            Item.Calamity().revengeanceItem = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.statCoinDrops += 0.3f;
            player.GetRagnarokModPlayer().batpoop = true;
        }
    }
}
