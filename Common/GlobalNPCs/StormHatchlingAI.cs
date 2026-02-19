using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using RagnarokMod.Common.ModSystems;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class StormHatchlingAI : GlobalNPC
    {
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return npc.type == thorium.Find<ModNPC>("StormHatchling").Type;
        }

        public override bool PreAI(NPC npc)
        {
            if (CalamityGamemodeCheck.isBossrush)
            {
                if (OtherModsCompat.tbr_loaded) // Can be removed as soon as Thorium Rework bossrush is fixed
                {
                    return true;
                }
                if (!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not selected do not change bossrush AIs
                {
                    return true;
                }
                if (!(OtherModsCompat.shouldRagnarokBossAILoad(ModContent.GetInstance<BossConfig>().bird)))
                {
                    return true;
                }
                NPCHelper.BatAI(npc, 0, 2.75f);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}