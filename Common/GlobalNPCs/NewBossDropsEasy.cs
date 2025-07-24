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
using RagnarokMod.Items.HealerItems.Other;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.SupremeCalamitas;
using ThoriumMod.NPCs.BossFallenBeholder;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossForgottenOne;
using ThoriumMod.NPCs.BossViscount;
using CalamityMod.NPCs.Providence;
using CalamityMod.Items.SummonItems;
using CalamityMod.World;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;
using RagnarokMod.Items.RevItems;
using RagnarokMod.Items.BardItems.Percussion;
using CalamityMod.NPCs.Polterghast;
using RagnarokMod.Items.HealerItems.Scythes;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class NewBossDropsEasy : GlobalNPC
    {
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return (
			npc.type == ModContent.NPCType<TheGrandThunderBird>()
			||npc.type == ModContent.NPCType<Viscount>()
            ||npc.type == ModContent.NPCType<QueenJellyfish>() 
            ||npc.type == ModContent.NPCType<StarScouter>()
            ||npc.type == ModContent.NPCType<BelchingCoral>() 
            ||npc.type == ModContent.NPCType<GraniteEnergyStorm>()
            ||npc.type == ModContent.NPCType<BuriedChampion>()
            ||npc.type == ModContent.NPCType<BoreanStrider>()  
            ||npc.type == ModContent.NPCType<FallenBeholder>()
            ||npc.type == ModContent.NPCType<PerforatorHive>()
            ||npc.type == ModContent.NPCType<CalamitasClone>() 
            ||npc.type == ModContent.NPCType<Providence>()   
            ||npc.type == ModContent.NPCType<ForgottenOne>() 
            ||npc.type == ModContent.NPCType<AstrumDeusHead>()
            ||npc.type == ModContent.NPCType<Polterghast>()
			||npc.type == ModContent.NPCType<SupremeCalamitas>() );
        }
		
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //changing non-lootbag drops
            if (npc.type == ModContent.NPCType<TheGrandThunderBird>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StormFeather>()));
            }
			 if (npc.type == ModContent.NPCType<Viscount>())
            {
				npcLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<GoldenBatDroppings>(), 1, 1, 1, true, null);
            }
            if (npc.type == ModContent.NPCType<QueenJellyfish>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<QueenJelly>(), 1, 4, 16));
            }
            if (npc.type == ModContent.NPCType<StarScouter>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>()));
            }
            if (npc.type == ModContent.NPCType<BelchingCoral>() && !Main.expertMode)
            {
                npcLoot.Remove(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophone>(), 10));
                npcLoot.Add(ModContent.ItemType<BelchingSaxophoneOverride>(), 10);
            }
            if (npc.type == ModContent.NPCType<GraniteEnergyStorm>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmpoweredGranite>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BuriedChampion>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedMarble>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BoreanStrider>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StriderFang>(), 1, 1, 4));
            }
            if (npc.type == ModContent.NPCType<FallenBeholder>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidseerPearl>()));
            }
            if (npc.type == ModContent.NPCType<PerforatorHive>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<EquivalentExchange>(), 4, 0));
            }
            if (npc.type == ModContent.NPCType<CalamitasClone>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<CalamityBell>(), 4, 0));
            }
            if (npc.type == ModContent.NPCType<Providence>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<ProfanedBell>(), 4, 0));
            }
            if (npc.type == ModContent.NPCType<ForgottenOne>() && !Main.expertMode)
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
            if (ModContent.GetInstance<BossProgressionConfig>().RuneOfKos)
            {
                if (npc.type == ModContent.NPCType<Providence>())
                {
               
                    // Remove Rune of Kos
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
            if (npc.type == ModContent.NPCType<Polterghast>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<PhantasmalEdge>(), 4, 0));
            }
			if (npc.type == ModContent.NPCType<SupremeCalamitas>() && !Main.expertMode)
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<Lamentation>(), 4, 0));
            }
        }
    }
}
