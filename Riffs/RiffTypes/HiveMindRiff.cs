using Terraria;

namespace RagnarokMod.Riffs.RiffTypes
{
    public class HiveMindRiff : Riff
    {
        public override float Range => 1200f;
        public override int CooldownTicks => 90 * 60;
        public override void SetStaticDefaults()
        {
        }
        public override void SafeOnStart(Player bardPlayer, Player target)
        {
            
        }
    }
}