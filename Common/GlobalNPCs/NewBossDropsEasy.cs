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
using CalamityMod.NPCs.AstrumDeus;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class NewBossDropsEasy : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            //changing non-lootbag drops
            if (npc.type == ModContent.NPCType<TheGrandThunderBird>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StormFeather>()));
            }
            if (npc.type == ModContent.NPCType<StarScouter>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StrangeAlienMotherBoard>()));
            }
            if (npc.type == ModContent.NPCType<BelchingCoral>())
            {
                npcLoot.Remove(ItemDropRule.Common(ModContent.ItemType<BelchingSaxophone>(), 10));
                npcLoot.Add(ModContent.ItemType<BelchingSaxophoneOverride>(), 10);
            }
            if (npc.type == ModContent.NPCType<GraniteEnergyStorm>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmpoweredGranite>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BuriedChampion>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EnchantedMarble>(), 1, 1, 6));
            }
            if (npc.type == ModContent.NPCType<BoreanStrider>())
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<StriderFang>(), 1, 1, 4));
            }
            if (npc.type == ModContent.NPCType<PerforatorHive>())
            {
                npcLoot.Add(ItemDropRule.NormalvsExpert(ModContent.ItemType<EquivalentExchange>(), 3, -1));
            }
			if (npc.type == ModContent.NPCType<AstrumDeusHead>()) 
			{
				LeadingConditionRule lastWorm = npcLoot.DefineConditionalDropSet((DropAttemptInfo info) => !AstrumDeusHead.ShouldNotDropThings(info.npc));
				lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<CelestialFragment>(), 1, 16, 24, 20, 32), false);
				lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<ShootingStarFragment>(), 1, 16, 24, 20, 32), false);
				lastWorm.Add(DropHelper.NormalVsExpertQuantity(ModContent.ItemType<WhiteDwarfFragment>(), 1, 16, 24, 20, 32), false);
			}
        }
    }
}
