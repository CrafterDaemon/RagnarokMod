using Terraria.ModLoader;
using Terraria;
using ThoriumMod;

namespace RagnarokMod.Utils
{
    public static class NPCHelper
    {
        public static bool IsHostile(this NPC npc, object attacker = null, bool ignoreDontTakeDamage = false)
		{
			return !npc.friendly && npc.lifeMax > 5 && npc.chaseable && (!npc.dontTakeDamage || ignoreDontTakeDamage) && !npc.immortal;
		}
    }
}
