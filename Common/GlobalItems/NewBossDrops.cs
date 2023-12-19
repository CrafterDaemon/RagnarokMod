

using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using System.Linq;
using ThoriumMod.Items.BossTheGrandThunderBird;
using RagnarokMod.Items.Materials;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.Sandstone;
using ThoriumMod.NPCs.BossTheGrandThunderBird;

namespace RagnarokMod.Common.GlobalItems
{
    public class NewBossDrops : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            //changing lootbag drops
            if (item.type == ModContent.ItemType<TheGrandThunderBirdTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StormFeather>(), 1, 1, 4));
            }
            if (item.type == ModContent.ItemType<StarScouterTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>()));
            }
        }
    }
}
