using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Buffs
{
    public class LeviathanHeartBubbleCorrupted : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 10% damage bonus
            player.GetDamage(DamageClass.Generic) += 0.10f;

            // 5% damage vulnerability
            player.endurance -= 0.05f;

            // Spawn purple visual if not present
            LeviathanHeartBubble.EnsureBubbleProjectile(player, isCorrupted: true);
        }
    }
}
