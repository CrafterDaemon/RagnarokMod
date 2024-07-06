using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class ScytheEarthBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;
        }
    }
}