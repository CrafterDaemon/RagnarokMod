using RagnarokMod.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class ScytheOasisBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 10;
        }
    }
}