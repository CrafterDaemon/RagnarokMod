using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;

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

            // Particles
            if (Main.rand.NextBool(2))
            {
                Vector2 dustPos = player.position + new Vector2(Main.rand.NextFloat(player.width), Main.rand.NextFloat(player.height));

                Dust dust = Dust.NewDustDirect(
                    dustPos,
                    0,
                    0,
                    DustID.Cloud,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-0.5f, 0.5f)
                );

                dust.noGravity = true;
                dust.scale = 1.1f;
                dust.alpha = 0;
            }
        }
    }
}