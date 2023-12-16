using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items;
using ThoriumMod;
using CalamityMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class ChangeThrowerWeaponToRogue : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.DamageType == DamageClass.Throwing;
        }
        //get ThoriumItem from Item
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
                thisThoriumItem.isThrower = false;
            }
            item.DamageType = ModContent.GetInstance<RogueDamageClass>();
        }
    }
}
