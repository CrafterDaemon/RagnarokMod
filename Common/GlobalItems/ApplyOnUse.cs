using RagnarokMod.Buffs;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Common.GlobalItems
{
    public class ApplyOnUse : GlobalItem
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation)
        {
            return entity.ModItem is ThoriumItem item && item.radiantLifeCost > 0;
        }

        public override bool? UseItem(Item item, Player player)
        {
            if (player.GetRagnarokModPlayer().leviathanHeart && player.GetThoriumPlayer().darkAura)
                player.AddBuff(ModContent.BuffType<LeviathanHeartBubbleCorrupted>(), 300);
            return base.UseItem(item, player);
        }
    }
}
