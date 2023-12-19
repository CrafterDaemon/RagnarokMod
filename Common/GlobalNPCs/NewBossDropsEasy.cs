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
using CalamityMod.NPCs.Leviathan;

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
            
        }
    }
}
