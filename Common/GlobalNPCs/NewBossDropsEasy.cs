using CalamityMod;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.TreasureBags;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.NPCs.BrimstoneElemental;
using CalamityMod.NPCs.CalClone;
using CalamityMod.NPCs.DesertScourge;
using CalamityMod.NPCs.Leviathan;
using CalamityMod.NPCs.OldDuke;
using CalamityMod.NPCs.Perforator;
using CalamityMod.NPCs.Polterghast;
using CalamityMod.NPCs.Providence;
using CalamityMod.NPCs.SulphurousSea;
using CalamityMod.NPCs.SupremeCalamitas;
using CalamityMod.NPCs.Yharon;
using CalamityMod.World;
using Ragnarok.Items;
using RagnarokMod.Common.Configs;
using RagnarokMod.Items.BardItems.Accessories;
using RagnarokMod.Items.BardItems.Percussion;
using RagnarokMod.Items.BardItems.String;
using RagnarokMod.Items.HealerItems.Accessories;
using RagnarokMod.Items.HealerItems.Other;
using RagnarokMod.Items.HealerItems.Scythes;
using RagnarokMod.Items.Materials;
using RagnarokMod.Items.RevItems;
using RagnarokMod.Utils;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.NPCs.BossBoreanStrider;
using ThoriumMod.NPCs.BossBuriedChampion;
using ThoriumMod.NPCs.BossFallenBeholder;
using ThoriumMod.NPCs.BossForgottenOne;
using ThoriumMod.NPCs.BossGraniteEnergyStorm;
using ThoriumMod.NPCs.BossQueenJellyfish;
using ThoriumMod.NPCs.BossStarScouter;
using ThoriumMod.NPCs.BossTheGrandThunderBird;
using ThoriumMod.NPCs.BossViscount;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class NewBossDropsEasy : GlobalNPC
    {
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return (
            npc.type == ModContent.NPCType<TheGrandThunderBird>()
            || npc.type == ModContent.NPCType<Viscount>()
            || npc.type == ModContent.NPCType<QueenJellyfish>()
            || npc.type == ModContent.NPCType<StarScouter>()
            || npc.type == ModContent.NPCType<BelchingCoral>()
            || npc.type == ModContent.NPCType<GraniteEnergyStorm>()
            || npc.type == ModContent.NPCType<BuriedChampion>()
            || npc.type == ModContent.NPCType<BoreanStrider>()
            || npc.type == ModContent.NPCType<FallenBeholder>()
            || npc.type == ModContent.NPCType<PerforatorHive>()
            || npc.type == ModContent.NPCType<CalamitasClone>()
            || npc.type == ModContent.NPCType<Providence>()
            || npc.type == ModContent.NPCType<ForgottenOne>()
            || npc.type == ModContent.NPCType<AstrumDeusHead>()
            || npc.type == ModContent.NPCType<Polterghast>()
            || npc.type == ModContent.NPCType<OldDuke>()
            || npc.type == ModContent.NPCType<SupremeCalamitas>()
            || npc.type == ModContent.NPCType<Yharon>()
            || npc.type == ModContent.NPCType<BrimstoneElemental>()
            || npc.type == ModContent.NPCType<DesertScourgeHead>()
            || npc.type == ModContent.NPCType<Anahita>()
            || npc.type == ModContent.NPCType<Leviathan>()
            );
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // Thorium Bosses
            if (npc.type == ModContent.NPCType<TheGrandThunderBird>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StormFeather>()));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<QueenJellyfish>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<QueenJelly>(), 1, 4, 16));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<StarScouter>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>(), 1, 4, 6));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<GraniteEnergyStorm>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EmpoweredGranite>(), 1, 3, 6));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<BuriedChampion>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EnchantedMarble>(), 1, 3, 6));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<BoreanStrider>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<StriderFang>(), 1, 1, 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<FallenBeholder>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VoidseerPearl>()));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<ForgottenOne>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EldritchShellFragment>(), 1, 1, 3));
                npcLoot.Add(notExpert);
            }

            // Calamity Bosses
            if (npc.type == ModContent.NPCType<PerforatorHive>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<EquivalentExchange>(), 4));
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Shredder>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<CalamitasClone>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<CalamityBell>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<Polterghast>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<PhantasmalEdge>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<OldDuke>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Virusprayer>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<SupremeCalamitas>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Lamentation>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<Yharon>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DragonForce>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<BrimstoneElemental>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BrimScythe>(), 4));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<DesertScourgeHead>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ScourgesFrets>(), 4));
                npcLoot.Add(notExpert);
            }

            // Special cases
            if (npc.type == ModContent.NPCType<Viscount>())
            {
                npcLoot.AddIf(() => CalamityWorld.revenge, ModContent.ItemType<GoldenBatDroppings>(), 1, 1, 1, true, null);
            }

            if (npc.type == ModContent.NPCType<BelchingCoral>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());

                if (ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Ragnarok)
                {
                    notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophoneOverride>(), 10));
                    if (ModLoader.TryGetMod("CalamityBardHealer", out Mod CalamityBardHealer))
                    {
                        try
                        {
                            npcLoot.Remove(ItemDropRule.Common(CalamityBardHealer.Find<ModItem>("BelchingSaxophone").Type));
                        }
                        catch
                        {
                            Mod ragnarok = ModContent.GetInstance<RagnarokMod>();
                            ragnarok.Logger.Error("Ragnarok Error: Failed to remove CalamityBardHealer BelchingSaxophone from npcLoot");
                        }
                    }
                }
                else if (ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Off)
                {
                    notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophoneOverride>(), 10));
                }
                npcLoot.Remove(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophone>(), 10));
                npcLoot.Add(notExpert);
            }

            if (npc.type == ModContent.NPCType<AstrumDeusHead>())
            {
                LeadingConditionRule lastWorm = npcLoot.DefineConditionalDropSet((DropAttemptInfo info) => !AstrumDeusHead.ShouldNotDropThings(info.npc));
                lastWorm.OnSuccess(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<CelestialFragment>(), 1, 16, 24, 20, 32));
                lastWorm.OnSuccess(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<ShootingStarFragment>(), 1, 16, 24, 20, 32));
                lastWorm.OnSuccess(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<WhiteDwarfFragment>(), 1, 16, 24, 20, 32));
            }

            if (npc.type == ModContent.NPCType<Providence>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ProfanedBell>(), 4));
                npcLoot.Add(notExpert);

                if (ModContent.GetInstance<BossProgressionConfig>().RuneOfKos)
                {
                    IItemDropRule itemDrop = ItemDropRule.Common(ModContent.ItemType<MarkofProvidence>());
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

            if (npc.type == ModContent.NPCType<Leviathan>())
            {
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                LeadingConditionRule lastAlive = (LeadingConditionRule)notExpert.OnSuccess(npcLoot.DefineConditionalDropSet(Leviathan.LastAnLStanding));
                lastAlive.OnSuccess(ItemDropRule.Common(ModContent.ItemType<LeviathanHeart>(), 4));
                lastAlive.OnSuccess(ItemDropRule.Common(ModContent.ItemType<SirenScale>(), 4));
                npcLoot.Add(notExpert);
            }
        }
    }
}