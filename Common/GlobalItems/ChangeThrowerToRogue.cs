using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalItems
{
    public class ChangeThrowerToRogue : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            ThoriumItem thisThoriumItem = getThoriumItem(item);
            if (thisThoriumItem != null)
            {
                return thisThoriumItem.isThrower;
            }
            else return false;
        }
        public ThoriumItem getThoriumItem(Item item)
        {
            if (item.ModItem is ThoriumItem thoriumItem)
            {
                return thoriumItem;
            }
            else return null;
        }
        //remove Thorium Thrower Class from item for easier repurposing
        public override void SetDefaults(Item item)
        {
            ThoriumItem thisThoriumItem = getThoriumItem(item);
            if (thisThoriumItem != null)
            {
                item.DamageType = ModContent.GetInstance<ThoriumRogueClass>();
                thisThoriumItem.isThrower = false;
            }
        }
    }
}
