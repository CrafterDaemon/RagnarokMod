using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class ItemBalancerConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("ItemDamageMultiplier")]
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
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float UnofficialGenericDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float RagnarokBardDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float RagnarokHealerDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float RagnarokGenericDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumBardDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 160, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumMeleeDamageModifier { get; set; }

		[DefaultValue(1f)]
        [BackgroundColor(255, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumHealerDamageModifier { get; set; }
		
        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumRogueDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumRangedDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 192, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumSummonDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 180, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumMagicDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float ThoriumGenericDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 160, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamityMeleeDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamityRogueDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamityRangedDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 192, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamitySummonDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 180, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamityMagicDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float CalamityGenericDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 160, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaMeleeDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaRogueDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaRangedDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 192, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaSummonDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 180, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaMagicDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float VanillaGenericDamageModifier { get; set; }
		
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherBardDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 160, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherMeleeDamageModifier { get; set; }

		[DefaultValue(1f)]
        [BackgroundColor(255, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherHealerDamageModifier { get; set; }
		
        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 0, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherRogueDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 255, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherRangedDamageModifier { get; set; }

        [DefaultValue(1f)]
        [BackgroundColor(0, 192, 255, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherSummonDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(255, 0, 180, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherMagicDamageModifier { get; set; }
		
		[DefaultValue(1f)]
        [BackgroundColor(0, 40, 40, 40)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherGenericDamageModifier { get; set; }
		
		[DefaultValue(1f)]
         [BackgroundColor(255, 0, 80, 170)]
        [Range(0f, 3f)]
        [Increment(0.05f)]
        [DrawTicks]
        public float OtherUnknownClassDamageModifier { get; set; }
		
        [Header("SpecificItems")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool genericweaponchanges;

        [DefaultValue(true)]
        public bool OmegaCore;
    }
}