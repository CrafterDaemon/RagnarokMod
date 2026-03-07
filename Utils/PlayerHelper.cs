

using System.Reflection;
using System;
using CalamityMod;
using CalamityMod.Balancing;
using Terraria;
using Terraria.ID;
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

        public static void ClearAllEmpowerments(Player player)
        {
            try
            {
                var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();
                if (thoriumPlayer == null) return;

                var empField = typeof(ThoriumPlayer)
                    .GetField("Empowerments", BindingFlags.Instance | BindingFlags.NonPublic);
                var data = empField?.GetValue(thoriumPlayer);
                if (data == null) return;

                var clearMI = typeof(ThoriumMod.Empowerments.EmpowermentLoader)
                    .GetMethod("UpdateDeadEmpowerments", BindingFlags.Static | BindingFlags.NonPublic);
                if (clearMI != null)
                    clearMI.Invoke(null, new object[] { data });

                // Nudge the cache right away (optional but helps UI/state feel instant)
                var updateMI = data.GetType().GetMethod("Update", BindingFlags.Instance | BindingFlags.Public);
                updateMI?.Invoke(data, null);
            }
            catch (Exception e)
            {
                if (Main.netMode != NetmodeID.Server)
                    Main.NewText($"[RagnarokMod:PlayerHelper] Failed to clear empowerments: {e.Message}");
            }
        }
    }
}
