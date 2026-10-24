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
        public CalamityBardHealer_selection_mode item_deduplication_mode;

    }
}