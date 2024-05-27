using Terraria.ModLoader;
using Terraria;

namespace RagnarokMod.Utils
{
    public class CalamityGamemodeCheck
    {
        public static bool isDeath
		{
            get {
                if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity)) {
                    return (bool)Calamity.Call("GetDifficultyActive", "death");
                }
                return false;
            }
        }
		
		public static bool isRevengeance
        {
            get {
                if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity)) {
                    return (bool)Calamity.Call("GetDifficultyActive", "revengeance");
                }
                return false;
            }
        }
		
		public static bool isBossrush
        {
            get {
                if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity)) {
                    return (bool)Calamity.Call("GetDifficultyActive", "bossrush");
                }
                return false;
            }
        }
    }
}
