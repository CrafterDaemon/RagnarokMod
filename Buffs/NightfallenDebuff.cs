using RagnarokMod.Common.GlobalNPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using CalamityMod.Systems.Collections;
using CalamityMod.DataStructures;

namespace RagnarokMod.Buffs
{
    public class NightfallenDebuff : ModBuff
    {
        public static DebuffData debuffData = new DebuffData
        {
            EnemyLostRegen = 200f,
            DrawAboveNPC = true
        };
        public override void SetStaticDefaults()
        {
            Main.debuff[base.Type] = true;
            Main.pvpBuff[base.Type] = true;
            Main.buffNoSave[base.Type] = false;
            BuffID.Sets.NurseCannotRemoveDebuff[base.Type] = true;

            CalamityBuffSets.DebuffDataset[Type] = debuffData;
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
            player.Hurt(PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} " + Language.GetTextValue("Mods.RagnarokMod.Compat.Darkness"))), 5, 0);
            Dust.NewDust(player.position, player.width, player.height, DustID.Asphalt);
        }
    }
}
