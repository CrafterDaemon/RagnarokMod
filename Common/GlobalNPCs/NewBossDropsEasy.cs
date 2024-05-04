using CalamityMod.Items.Weapons.Magic;
using CalamityMod;
using CalamityMod.NPCs.SulphurousSea;
using RagnarokMod.Items.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using ThoriumMod.NPCs.BossStarScouter;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using Ragnarok.Items;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.NPCs.BossBoreanStrider;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.ThrownItems;
using CalamityMod.NPCs.Perforator;
using RagnarokMod.Items.HealerItems;
using RagnarokMod.Items.BardItems;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using ThoriumMod.NPCs.BossFallenBeholder;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossForgottenOne;
using CalamityMod.NPCs.Providence;
using CalamityMod.Items.SummonItems;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class NewBossDropsEasy : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //changing non-lootbag drops
            if (npc.type == ModContent.NPCType<TheGrandThunderBird>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StormFeather>()));
            }
            if (npc.type == ModContent.NPCType<QueenJellyfish>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QueenJelly>(), 1, 4, 16));
            }
            if (npc.type == ModContent.NPCType<StarScouter>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>()));
            }
            if (npc.type == ModContent.NPCType<BelchingCoral>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Remove(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophone>(), 10));
                npcLoot.Add(ModContent.ItemType<BelchingSaxophoneOverride>(), 10);
            }
            if (npc.type == ModContent.NPCType<GraniteEnergyStorm>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmpoweredGranite>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BuriedChampion>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedMarble>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BoreanStrider>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StriderFang>(), 1, 1, 4));
            }
            if (npc.type == ModContent.NPCType<FallenBeholder>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidseerPearl>()));
            }
            if (npc.type == ModContent.NPCType<PerforatorHive>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<EquivalentExchange>(), 4, 3));
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CalamityBell>(), 4, 3));
            }
            if (npc.type == ModContent.NPCType<ForgottenOne>() && Condition.InClassicMode.IsMet())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EldritchShellFragment>(), 1, 1, 3));
            }
            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
            {
                LeadingConditionRule lastWorm = npcLoot.DefineConditionalDropSet((DropAttemptInfo info) => !AstrumDeusHead.ShouldNotDropThings(info.npc));
                lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<CelestialFragment>(), 1, 16, 24, 20, 32), false);
                lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<ShootingStarFragment>(), 1, 16, 24, 20, 32), false);
                lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<WhiteDwarfFragment>(), 1, 16, 24, 20, 32), false);
            }
            if (npc.type == ModContent.NPCType<Providence>())
            {
                IItemDropRule itemDrop = ItemDropRule.Common(ModContent.ItemType<RuneofKos>());
                bool waitingForChange = true;
                while (waitingForChange && DownedBossSystem.downedProvidence)
                {
                    foreach (var item in npcLoot.Get())
                    {
                        if (item == itemDrop) 
                        { 
                            npcLoot.Remove(item);
                            waitingForChange = false; 
                            break;
                        }
                    }
                }
            }
        }
    }
}
