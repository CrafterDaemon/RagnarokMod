using Microsoft.Xna.Framework;
using RagnarokMod.ILEdits;
using System.ComponentModel;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.Config;

namespace RagnarokMod.Common.Configs
{
    public enum MissSoundPreset
    {
        Default,      // Item16
        Softer,       // Item106
        Percussion,   // Item89
        Bounce,   // Item150
        Meow,   // Item57
        Clang,   // Custom
        Custom
    }
    public class ClientConfig : ModConfig
    {
        public static ClientConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("WelcomeText")]

        [DefaultValue(true)]
        public bool StartText { get; set; }

        [Header("Audio")]

        [Range(0f, 1f)]
        [Increment(0.05f)]
        [DefaultValue(1f)]
        [DrawTicks]
        [Slider]
        public float RiffMusicVolume { get; set; }

        [DefaultValue(0)]
        [Range(-1,1)]
        [Increment(1)]
        [DrawTicks]
        [Slider]
        public int PlaySoundDuringRiff { get; set; }

        [DefaultValue(false)]
        public bool MissSoundDuringRiff { get; set; }

        [DefaultValue(MissSoundPreset.Default)]
        public MissSoundPreset MissSoundPreset { get; set; }

        [DefaultValue("")]
        public string CustomMissSoundPath { get; set; }

        [DefaultValue(0.5f)]
        [Range(0f, 1f)]
        [Increment(0.05f)]
        [DrawTicks]
        [Slider]
        public float MissSoundVolume { get; set; }
        public SoundStyle Resolve()
        {
            if (MissSoundPreset == MissSoundPreset.Custom)
            {
                if (!string.IsNullOrWhiteSpace(CustomMissSoundPath))
                {
                    try { return new SoundStyle(CustomMissSoundPath) { Volume = MissSoundVolume }; }
                    catch
                    {
                        Mod.Logger.Warn($"[RagnarokConfig] Invalid CustomMissSoundPath: '{CustomMissSoundPath}', falling back to default.");
                        Main.NewText($"[RagnarokMod] Invalid miss sound path '{CustomMissSoundPath}', falling back to default.", Color.OrangeRed);
                    }
                }
            }

            return MissSoundPreset switch
            {
                MissSoundPreset.Softer => SoundID.Item106.WithVolumeScale(MissSoundVolume),
                MissSoundPreset.Percussion => SoundID.Item89.WithVolumeScale(MissSoundVolume),
                MissSoundPreset.Bounce => SoundID.Item150.WithVolumeScale(MissSoundVolume),
                MissSoundPreset.Meow => SoundID.Item57.WithVolumeScale(MissSoundVolume),
                MissSoundPreset.Clang => new SoundStyle("RagnarokMod/Sounds/pipe") {Volume = MissSoundVolume},
                _ => SoundID.Item16.WithVolumeScale(MissSoundVolume)
            };
        }

        public override void OnChanged()
        {
            BigInstrumentPatchSystem.MissSoundStyle = Resolve();
        }

        [Header("Visual")]
        [Range(0f, 1f)]
        [DefaultValue(1f)]
        [Increment(0.01f)]
        [DrawTicks]
        [Slider]
        public float RiffScreenEffectIntensity { get; set; } = 100f;

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