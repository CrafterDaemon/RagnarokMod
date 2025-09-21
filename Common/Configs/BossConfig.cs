using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
	public enum ThoriumBossRework_selection_mode
		{
			Off,
			Auto,
			Ragnarok,
			ThoriumBossRework
		}
	
    public class BossConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

		[Header("BossChanges")]
	   
		[DefaultValue(true)]
		[Label("Boss Stat Tweak")]
		[Tooltip("Select, if Thorium Bosses receive stat tweaks (Attack Damage, Health, Defense Damage, Damage Reduction) for Calamity scaling")]
		public bool bossstatstweak;
	   
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Thorium Boss Rush")]
		[Tooltip("Select, which bossrush mode should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode bossrush;
	   
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Grand Thunderbird AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode bird;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Queen Jellyfish AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode jelly;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Viscount AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode viscount;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Granite Enegy Storm AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode granite;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Buried Champion AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode champion;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Star Scouter AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode scouter;
		
		[DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
		[Label("Borean Strider AI")]
		[Tooltip("Select, which AI rework should be used. Off for vanilla behaviour. Auto uses Ragnarok AI if no other rework is detected")]
        public ThoriumBossRework_selection_mode strider;
    }
}