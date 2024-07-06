using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class ScytheAirBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.2f;
            if (player.wingTimeMax > 0)
            {
                player.wingTimeMax += (int)(player.wingTimeMax * 0.2f);
            }
        }
    }
}