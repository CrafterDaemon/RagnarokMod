using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items;

namespace RagnarokMod.Common.GlobalItems
{
    public class MulticlassersEmblem : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation) => item.type == ModContent.ItemType<NinjaEmblem>();

        public override void SetDefaults(Item item)
        {
            if (item.ModItem is ThoriumItem thoriumItem)
            {
               thoriumItem.isThrower = false;
            }
        }

        //change to all class item
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Throwing) -= 0.15f;
            player.GetDamage(DamageClass.Generic) += 0.12f;
            player.GetCritChance(DamageClass.Generic) += 3;
        }
    }
}
