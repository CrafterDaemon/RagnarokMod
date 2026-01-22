using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class ClassBalancerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ClassBalanceChanges")]
		
		[DefaultValue(1f)]
		[BackgroundColor(255, 0, 0, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float BardDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(255, 160, 0, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float MeleeDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(255, 255, 0, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float HealerDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(0, 255, 0, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float RogueDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(0, 255, 255, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float RangedDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(0, 192, 255, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float SummonDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(255, 0, 180, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float MagicDamageModifier { get; set; }
		
		[DefaultValue(1f)]
		[BackgroundColor(218, 165, 32, 170)]
		[Range(0f, 3f)]
		[Increment(0.05f)]
		[DrawTicks]
		public float HealingBonusModifier { get; set; }
    }
}