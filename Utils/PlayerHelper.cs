

using CalamityMod;
using CalamityMod.Balancing;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Utils
{
    public static class PlayerHelper
    {
        private static RagnarokModPlayer localRagnarokModPlayer;
        public static void SetLocalRagnarokModPlayer(RagnarokModPlayer ragnarokmodPlayer)
        {
            //Setting up my very own modplayer
            if (Main.myPlayer == ((Entity)((ModPlayer)ragnarokmodPlayer).Player).whoAmI && !((ModPlayer)ragnarokmodPlayer).Player.isDisplayDollOrInanimate)
            {
                localRagnarokModPlayer = ragnarokmodPlayer;
            }
        }
        public static void Unload()
        {
            localRagnarokModPlayer = null;
        }

        public static RagnarokModPlayer GetRagnarokModPlayer(this Player player)
        {
            //FETCH ME THEIR SOULSSSSS
            if (!Main.gameMenu && ((Entity)player).whoAmI == Main.myPlayer && !player.isDisplayDollOrInanimate && localRagnarokModPlayer != null)
            {
                return localRagnarokModPlayer;
            }
            return player.GetModPlayer<RagnarokModPlayer>();
        }

        public static DamageClass HighestClass(this Player player)
        {
            float num = 1f;
            DamageClass result = DamageClass.Generic;
            float additive = player.GetTotalDamage<MeleeDamageClass>().Additive;
            if (additive > num)
            {
                num = additive;
                result = DamageClass.Melee;
            }

            float additive2 = player.GetTotalDamage<RangedDamageClass>().Additive;
            if (additive2 > num)
            {
                num = additive2;
                result = DamageClass.Ranged;
            }

            float additive3 = player.GetTotalDamage<MagicDamageClass>().Additive;
            if (additive3 > num)
            {
                num = additive3;
                result = DamageClass.Magic;
            }

            float num2 = player.GetTotalDamage<SummonDamageClass>().Additive * 0.75f;
            if (num2 > num)
            {
                num = num2;
                result = DamageClass.Summon;
            }

            if (player.GetTotalDamage<RogueDamageClass>().Additive - player.Calamity().stealthDamage > num)
            {
                result = ModContent.GetInstance<RogueDamageClass>();
            }

            if (player.GetTotalDamage<BardDamage>().Additive > num)
            {
                result = ThoriumDamageBase<BardDamage>.Instance;
            }

            if (player.GetTotalDamage<HealerDamage>().Additive > num)
            {
                result = ThoriumDamageBase<HealerDamage>.Instance;
            }
            return result;
        }
    }
}
