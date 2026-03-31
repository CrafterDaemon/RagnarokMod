using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public class AprilChaos : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("AprilChaos")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool MasterToggle { get; set; }

        private int _radmage;
        [Header("RadiantMage")]
        [ReloadRequired]
        [DefaultValue(0)]
        [Range(-1, 1)]
        [Increment(1)]
        [Slider]
        [DrawTicks]
        public int RadMage
        {
            get => MasterToggle? _radmage : 0;
            set => _radmage = value;
        }

        private int _melsum;
        [Header("MeleeSummoner")]
        [ReloadRequired]
        [DefaultValue(0)]
        [Range(-1, 1)]
        [Increment(1)]
        [Slider]
        [DrawTicks]
        public int MelSum
        {
            get => MasterToggle? _melsum : 0;
            set => _melsum = value;
        }

        private int _rogran;
        [Header("RogueRanger")]
        [ReloadRequired]
        [DefaultValue(0)]
        [Range(-1, 1)]
        [Increment(1)]
        [Slider]
        [DrawTicks]
        public int RogRan
        {
            get => MasterToggle? _rogran : 0;
            set => _rogran = value;
        }

        private bool _obbard;
        [Header("ObnoxiousBard")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool ObBard
        {
            get => MasterToggle && _obbard;
            set => _obbard = value;
        }
    }
}