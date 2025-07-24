using CalamityMod;
using CalamityMod.NPCs.AquaticScourge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Buffs;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class BossDebuffImmunities : GlobalNPC
    {
		public override bool AppliesToEntity(NPC npc, bool lateInstantiation)
        {
            return ( npc.type == ModContent.NPCType<AquaticScourgeHead>());
        }
		
        public override void SetDefaults(NPC npc)
        {
            if (npc == ModContent.GetModNPC(ModContent.NPCType<AquaticScourgeHead>()).NPC)
            {
                NPCID.Sets.SpecificDebuffImmunity[npc.type][ModContent.BuffType<Stunned>()] = true;
            }  
        }
    }
}
