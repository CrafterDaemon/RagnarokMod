using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class UIConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("UITweaks")]
		
		[DefaultValue(32)]
		[ReloadRequired]
		[BackgroundColor(192, 0, 192, 170)]
		[Range(0, 4000)]
		[Increment(1)]
		[DrawTicks]
		public int BardEmpowermentBarOffsetX { get; set; }
		
		[DefaultValue(130)]
		[ReloadRequired]
		[BackgroundColor(192, 0, 192, 170)]
		[Range(0, 4000)]
		[Increment(1)]
		[DrawTicks]
		public int BardEmpowermentBarOffsetY { get; set; }
    }
}