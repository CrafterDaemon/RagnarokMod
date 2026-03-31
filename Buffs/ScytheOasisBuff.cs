using RagnarokMod.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RagnarokMod.Buffs
{
    public class ScytheOasisBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 40;

            // Particles
            if (Main.rand.NextBool(2))
            {
                Vector2 dustPos = player.position + new Vector2(Main.rand.NextFloat(player.width), Main.rand.NextFloat(player.height));

                Dust dust = Dust.NewDustDirect(
                    dustPos,
                    0,
                    0,
                    DustID.Grass,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-0.5f, 0.5f),
                    0,
                    Color.LimeGreen
                );

                dust.noGravity = true;
                dust.scale = 1.1f;
                dust.alpha = 0;
            }
        }
    }
}