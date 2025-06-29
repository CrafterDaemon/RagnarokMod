using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class ClassBalancerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ClassBalanceChanges")]
		
		[DefaultValue(1f)]
		[BackgroundColor(192, 0, 192, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float HealerDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(192, 0, 192, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float BardDamageModifier { get; set; }
    }
}