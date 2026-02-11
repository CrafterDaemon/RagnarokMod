using RagnarokMod.Common.GlobalNPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class NightfallenDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[base.Type] = true;
            Main.pvpBuff[base.Type] = true;
            Main.buffNoSave[base.Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[base.Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            // Damage logic
            Dust.NewDust(npc.position, npc.width, npc.height, DustID.Asphalt);
            npc.GetGlobalNPC<RagnarokGlobalNPC>().debuffNightfallen = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Damage logic for Players
                player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} fell into darkness."), 5, 0);
                Dust.NewDust(player.position, player.width, player.height, DustID.Asphalt);
        }
    }
}
