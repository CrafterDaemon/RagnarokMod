using System.Collections.Generic;
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

        private bool _radmage;
        [Header("RadiantMage")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool RadMage
        {
            get => MasterToggle && _radmage;
            set => _radmage = value;
        }

        private bool _melsum;
        [Header("MeleeSummoner")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool MelSum
        {
            get => MasterToggle && _melsum;
            set => _melsum = value;
        }

        private bool _rogran;
        [Header("RogueRanger")]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool RogRan
        {
            get => MasterToggle && _rogran;
            set => _rogran = value;
        }

        private bool _obbard;
        [Header("ObnoxiousBard")]
        [DefaultValue(false)]
        public bool ObBard
        {
            get => MasterToggle && _obbard;
            set => _obbard = value;
        }

        // Weight for playing the instrument's actual correct riff sound.
        // Set to 0 to never play the real riff, higher = more likely.
        [DefaultValue(155)]
        [Range(0, 200)]
        public int OwnRiffWeight { get; set; } = 155;

        // Per-track weights for joke riff sounds. Set any entry to 0 to disable it.
        // Keys match the sound names listed in ObnoxiousBard.cs.
        public Dictionary<string, int> RiffWeights { get; set; } = new()
        {
            ["RotJD"] = 1,
            ["CalClone"] = 1,
            ["Slime"] = 1,
            ["Aureus"] = 1,
            ["Shredder"] = 1,
            ["HiveMind"] = 1,
            ["Devourer"] = 1,
            ["Frets"] = 1,
            ["NFL"] = 1,
            ["LavaChicken"] = 1,
            ["Cortisol"] = 1,
            ["CrabRave"] = 1,
            ["Meow"] = 1,
            ["Outro"] = 1,
            ["Sans"] = 1,
            ["Tycoon"] = 1,
            ["WorldRevolving"] = 1,
            ["RickRoll"] = 1,
            ["Werehog"] = 1,
            ["Slide"] = 1,
            ["Pizza"] = 1,
            ["Driftveil"] = 1,
            ["Malo"] = 1,
            ["Giorno"] = 1,
            ["Slaughter"] = 1,
            ["Coconut"] = 1,
            ["Cheese"] = 1,
            ["Piggies"] = 1,
            ["Coda"] = 1,
            ["Whisper"] = 1,
            ["Homer"] = 1,
            ["Surprise"] = 1,
            ["NumberOne"] = 1,
            ["Gas"] = 1,
            ["Rockefeller"] = 1,
            ["Nineties"] = 1,
            ["Scatman"] = 1,
            ["Oiia"] = 1,
            ["Alia"] = 1,
            ["BadApple"] = 1,
            ["Delirious"] = 1,
            ["Kingdom"] = 1,
            ["HollowPurple"] = 1,
            ["Shrine"] = 1,
            ["TF2"] = 1,
        };
    }
}