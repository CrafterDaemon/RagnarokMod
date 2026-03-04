using CalamityMod.Items.TreasureBags.MiscGrabBags;
using RagnarokMod.Utils;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Common.GlobalItems{
    public class NewStarterBag : GlobalItem{
        public override bool AppliesToEntity(Item item, bool lateInstantiation){
            return item.type == ModContent.ItemType<StarterBag>();
        }

        public override void ModifyItemLoot(Item item, ItemLoot itemLoot){
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Tambourine>(), 1, 1, 1));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Pill>(), 1, 200, 200));
        }
		
		public override bool IsLoadingEnabled(Mod mod){
            return (!(ModLoader.HasMod("WHummusMultiModBalancing") || ModLoader.HasMod("InfernalEclipseAPI")));
        }
    }
}