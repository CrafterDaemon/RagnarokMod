using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RagnarokMod.Buffs
{
    public class ScytheEarthBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 10;

            // Particles
            if (Main.rand.NextBool(2))
            {
                Vector2 dustPos = player.position + new Vector2(Main.rand.NextFloat(player.width), Main.rand.NextFloat(player.height));

                int dustType = Main.rand.NextBool() ? DustID.Dirt : DustID.Stone;

                Dust dust = Dust.NewDustDirect(
                    dustPos,
                    0,
                    0,
                    dustType,
                    Main.rand.NextFloat(-1f, 1f),
                    Main.rand.NextFloat(-0.5f, 0.5f)
                );

                dust.noGravity = true;
                dust.scale = 1.3f;
                dust.alpha = 0;
            }
        }
    }
}