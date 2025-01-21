using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Buffs
{
    public class AuricSurge : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // Cannot be cleared by right-clicking it, or overflowing buff limit cap
            Main.pvpBuff[Type] = true; // Allows it to affect players in PvP.
            Main.buffNoSave[Type] = false; // Buff persists when reloading the world
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            // Damage logic
            if (Main.rand.NextBool(4)) // Deals damage every 4 ticks (~0.25 seconds).
            {
                npc.SimpleStrikeNPC(50, 0);
                Dust.NewDust(npc.position, npc.width, npc.height, DustID.Electric);
            }
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Damage logic for Players
            if (Main.rand.NextBool(4)) // Deals damage every 15 ticks (~0.25 seconds).
            {
                player.Hurt(PlayerDeathReason.ByCustomReason($"{player.name} was smited."), 10, 0);
                Dust.NewDust(player.position, player.width, player.height, DustID.Electric);
            }
        }
    }
}