using Microsoft.Xna.Framework;
using RagnarokMod.Projectiles.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Buffs
{
    public class LeviathanHeartBubble : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 5% damage reduction
            player.endurance += 0.05f;

            // Spawn teal visual if not present
            EnsureBubbleProjectile(player, isCorrupted: false);
        }

        internal static void EnsureBubbleProjectile(Player player, bool isCorrupted)
        {
            if (player.whoAmI != Main.myPlayer)
                return;

            float corruptedFlag = isCorrupted ? 1f : 0f;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == player.whoAmI
                    && p.ModProjectile is LeviathanHeartBubbleProj
                    && p.ai[1] == corruptedFlag)
                    return; // already exists
            }

            Projectile.NewProjectile(
                player.GetSource_Buff(0),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<LeviathanHeartBubbleProj>(),
                0, 0f,
                player.whoAmI,
                0f,          
                corruptedFlag
            );
        }
    }
}
