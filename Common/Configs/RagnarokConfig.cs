using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class BossProgressionConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("BossProgressionChanges")]

        [DefaultValue(true)]
        [ReloadRequired]
        public bool GrandFlareGun;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DesertMedallion;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DecapoditaSprout;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool MarbleGranite;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool OverloadedSludge;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool StarCaller;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool CryoKey;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool MechBosses;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool Lich;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool CharredIdol;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DeathWhistle;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool DoomSayerCoin;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool RuneOfKos;
    }
}