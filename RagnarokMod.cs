using RagnarokMod.Utils;
using Terraria.ModLoader;

namespace RagnarokMod
{
	public class RagnarokMod : Mod
	{
		internal static RagnarokMod mod;

		public RagnarokMod()
		{
			mod = this;
		}

		public override void Load()
		{
			
		}

        public override void Unload()
        {
			PlayerHelper.Unload();
        }
    }
}