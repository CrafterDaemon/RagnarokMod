using RagnarokMod.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class ScytheBrimstoneBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetRagnarokModPlayer().brimstoneFlamesOnHit = true;
        }
    }
}