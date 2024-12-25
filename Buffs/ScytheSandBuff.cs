using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Buffs
{
    public class ScytheSandBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.05f;
        }
    }
}