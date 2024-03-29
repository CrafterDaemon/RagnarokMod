﻿using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossTheGrandThunderBird;
using RagnarokMod.Items.Materials;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossBoreanStrider;
using CalamityMod.Items.TreasureBags;
using RagnarokMod.Items.HealerItems;
using RagnarokMod.Items.BardItems;
using CalamityMod.NPCs.CalClone;

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
            if (item.type == ModContent.ItemType<GraniteEnergyStormTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmpoweredGranite>(), 1, 1, 10));
            }
            if (item.type == ModContent.ItemType<BuriedChampionTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedMarble>(), 1, 1, 10));
            }
            if (item.type == ModContent.ItemType<BoreanStriderTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StriderFang>(), 1, 1, 10));
            }
            if (item.type == ModContent.ItemType<PerforatorBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EquivalentExchange>(), 3));
            }
			if (item.type == ModContent.ItemType<CalamitasCloneBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<CalamityBell>(), 3));
            }
        }
    }
}
