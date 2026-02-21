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
        public bool bossstatstweak;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode bossrush;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode bird;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode jelly;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode viscount;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode granite;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode champion;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode scouter;

        [DefaultValue(ThoriumBossRework_selection_mode.Auto)]
        [ReloadRequired]
        public ThoriumBossRework_selection_mode strider;
    }
}