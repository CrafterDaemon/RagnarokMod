using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
	public enum CalamityBardHealer_selection_mode
		{
			Off,
			Ragnarok,
			CalamityBardHealer
		}
	
    public class ModCompatConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("ModCompat")]
		
        [DefaultValue(CalamityBardHealer_selection_mode.Ragnarok)]
        [ReloadRequired]
		[Label("Calamity Bard & Healer item deduplication")]
		[Tooltip("Select, which item recipes of duplicate items are loaded. Off enables both mod versions.")]
        public CalamityBardHealer_selection_mode item_deduplication_mode;
		
    }
}