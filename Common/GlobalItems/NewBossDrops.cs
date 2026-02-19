using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossTheGrandThunderBird;
using RagnarokMod.Items.Materials;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossBoreanStrider;
using CalamityMod.Items.TreasureBags;
using RagnarokMod.Items.HealerItems.Other;
using RagnarokMod.Items.RevItems;
using RagnarokMod.Utils;
using CalamityMod.NPCs.CalClone;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossViscount;
using RagnarokMod.Items.BardItems.Percussion;
using RagnarokMod.Items.HealerItems.Scythes;
using RagnarokMod.Items.BardItems.String;
using RagnarokMod.Items.HealerItems.Accessories;
using RagnarokMod.Items.BardItems.Accessories;

namespace RagnarokMod.Common.GlobalItems
{
    public class NewBossDrops : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return (
            item.type == ModContent.ItemType<TheGrandThunderBirdTreasureBag>()
            || item.type == ModContent.ItemType<QueenJellyfishTreasureBag>()
            || item.type == ModContent.ItemType<StarScouterTreasureBag>()
            || item.type == ModContent.ItemType<GraniteEnergyStormTreasureBag>()
            || item.type == ModContent.ItemType<BuriedChampionTreasureBag>()
            || item.type == ModContent.ItemType<BoreanStriderTreasureBag>()
            || item.type == ModContent.ItemType<PerforatorBag>()
            || item.type == ModContent.ItemType<CalamitasCloneBag>()
            || item.type == ModContent.ItemType<ProvidenceBag>()
            || item.type == ModContent.ItemType<FallenBeholderTreasureBag>()
            || item.type == ModContent.ItemType<ForgottenOneTreasureBag>()
            || item.type == ModContent.ItemType<PolterghastBag>()
            || item.type == ModContent.ItemType<CalamitasCoffer>()
            || item.type == ModContent.ItemType<OldDukeBag>()
            || item.type == ModContent.ItemType<YharonBag>()
            || item.type == ModContent.ItemType<BrimstoneElementalBag>()
            || item.type == ModContent.ItemType<LeviathanBag>()
            || item.type == ModContent.ItemType<DesertScourgeBag>()
            );
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            //changing lootbag drops
            if (item.type == ModContent.ItemType<TheGrandThunderBirdTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StormFeather>(), 1, 1, 4));
            }
            if (item.type == ModContent.ItemType<QueenJellyfishTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<QueenJelly>(), 1, 4, 16));
            }
            if (item.type == ModContent.ItemType<StarScouterTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>(), 1, 4, 6));
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
            if (item.type == ModContent.ItemType<ProvidenceBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ProfanedBell>(), 3));
            }
            if (item.type == ModContent.ItemType<FallenBeholderTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidseerPearl>()));
            }
            if (item.type == ModContent.ItemType<ForgottenOneTreasureBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EldritchShellFragment>(), 1, 1, 3));
            }
            if (item.type == ModContent.ItemType<PolterghastBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantasmalEdge>()));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Ectambourine>()));
            }
            if (item.type == ModContent.ItemType<CalamitasCoffer>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Lamentation>(), 3));
            }
            if (item.type == ModContent.ItemType<OldDukeBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Virusprayer>(), 3));
            }
            if (item.type == ModContent.ItemType<YharonBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<DragonForce>(), 3));
            }
            if (item.type == ModContent.ItemType<BrimstoneElementalBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<BrimScythe>(), 3));
            }
            if (item.type == ModContent.ItemType<LeviathanBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LeviathanHeart>(), 3));
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SirenScale>(), 3));
            }
            if (item.type == ModContent.ItemType<DesertScourgeBag>())
            {
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ScourgesFrets>(), 3));
            }
        }
    }
}
