using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace RagnarokMod.Buffs
{
    public class ScytheSandBuff : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.05f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.05f;

            // Particles
            if (Main.rand.NextBool(2))
            {
                Vector2 dustPos = player.position + new Vector2(Main.rand.NextFloat(player.width), Main.rand.NextFloat(player.height));

                Dust dust = Dust.NewDustDirect(
                    dustPos,
                    0,
                    0,
                    DustID.Sand,
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