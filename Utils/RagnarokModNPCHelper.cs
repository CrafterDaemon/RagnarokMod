using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;

namespace RagnarokMod.Utils
{
	internal static class RagnarokModNPCHelper
	{
		public static bool RagnarokModIsHostile(this NPC npc, object attacker = null, bool ignoreDontTakeDamage = false)
		{
			return !npc.friendly && npc.lifeMax > 5 && npc.chaseable && (!npc.dontTakeDamage || ignoreDontTakeDamage) && !npc.immortal;
		}
	}
}
