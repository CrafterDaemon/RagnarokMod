using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class ItemBalancerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

       [Header("ItemBalanceChanges")]
		
		[DefaultValue(true)]
        //[ReloadRequired]
        public bool OmegaCore; 
    }
}