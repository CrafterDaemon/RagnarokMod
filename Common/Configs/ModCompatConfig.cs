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
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float UnofficialBardDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float UnofficialHealerDamageModifier { get; set; }

    }
}