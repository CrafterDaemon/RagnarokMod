

using Terraria.ModLoader;
using Terraria;
using ThoriumMod;

namespace RagnarokMod.Utils
{
    public static class PlayerHelper
    {
        private static RagnarokModPlayer localRagnarokModPlayer;
        public static void SetLocalRagnarokModPlayer(RagnarokModPlayer ragnarokmodPlayer)
        {
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
            if (!Main.gameMenu && ((Entity)player).whoAmI == Main.myPlayer && !player.isDisplayDollOrInanimate && localRagnarokModPlayer != null)
            {
                return localRagnarokModPlayer;
            }
            return player.GetModPlayer<RagnarokModPlayer>();
        }
    }
}
