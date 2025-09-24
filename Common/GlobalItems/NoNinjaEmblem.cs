using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items;
using ThoriumMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class NoNinjaEmblem : GlobalItem
    {
        //only ninja emblem
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ModContent.ItemType<NinjaEmblem>();
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

        }

        //change to all class item
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            ref StatModifier damage = ref player.GetDamage(DamageClass.Throwing);
            damage -= 0.15f;
            ref StatModifier alldamage = ref player.GetDamage(DamageClass.Generic);
            alldamage += 0.08f;
            player.GetAttackSpeed(DamageClass.Generic) += 0.05f;
            player.GetCritChance(DamageClass.Generic) += 5;
        }
    }
}
