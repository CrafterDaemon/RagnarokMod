using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Utilities;
using ThoriumMod;

namespace RagnarokMod.Buffs
{
    public class AntarcticReinforcementBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[base.Type] = false;
            Main.pvpBuff[base.Type] = true;
            Main.buffNoSave[base.Type] = true;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
        {
            rare = 3;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 8;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.healBonus += 2;
        }
    }
}
