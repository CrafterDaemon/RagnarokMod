/*
using System.Collections.Generic;
using RagnarokMod.Common.Configs;
using RagnarokMod.Sounds;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace RagnarokMod.Items.BardItems
{
    /// <summary>
    /// Shared weighted-random riff sound picker for all riff instrument bases.
    /// Reads weights from the AprilChaos config at call time so changes take effect
    /// immediately without a reload.
    /// </summary>
    internal static class ObnoxiousBardHelper
    {
        // Maps config dictionary keys to their actual SoundStyle.
        // Add new entries here when adding new joke riffs.
        private static readonly Dictionary<string, SoundStyle> SoundMap = new()
        {
            ["RotJD"] = RagnarokModSounds.rotjdriff,
            ["CalClone"] = RagnarokModSounds.calcloneriff,
            ["Slime"] = RagnarokModSounds.slimeriff,
            ["Aureus"] = RagnarokModSounds.aureusriff,
            ["Shredder"] = RagnarokModSounds.shredderriff,
            ["HiveMind"] = RagnarokModSounds.HiveMindRiff,
            ["Devourer"] = RagnarokModSounds.devourerriff,
            ["Frets"] = RagnarokModSounds.fretsriff,
            ["NFL"] = RagnarokModSounds.nfl,
            ["LavaChicken"] = RagnarokModSounds.lavachicken,
            ["Cortisol"] = RagnarokModSounds.cortisol,
            ["CrabRave"] = RagnarokModSounds.crabrave,
            ["Meow"] = RagnarokModSounds.meow,
            ["Outro"] = RagnarokModSounds.outro,
            ["Sans"] = RagnarokModSounds.sans,
            ["Tycoon"] = RagnarokModSounds.tycoon,
            ["WorldRevolving"] = RagnarokModSounds.worldrevolving,
            ["RickRoll"] = RagnarokModSounds.rickroll,
            ["Werehog"] = RagnarokModSounds.werehog,
            ["Slide"] = RagnarokModSounds.slide,
            ["Pizza"] = RagnarokModSounds.pizza,
            ["Driftveil"] = RagnarokModSounds.driftveil,
            ["Malo"] = RagnarokModSounds.malo,
            ["Giorno"] = RagnarokModSounds.giorno,
            ["Slaughter"] = RagnarokModSounds.slaughter,
            ["Coconut"] = RagnarokModSounds.coconut,
            ["Cheese"] = RagnarokModSounds.cheese,
            ["Piggies"] = RagnarokModSounds.piggies,
            ["Coda"] = RagnarokModSounds.coda,
            ["Whisper"] = RagnarokModSounds.whisper,
            ["Homer"] = RagnarokModSounds.homer,
            ["Surprise"] = RagnarokModSounds.surprise,
            ["NumberOne"] = RagnarokModSounds.numberone,
            ["Gas"] = RagnarokModSounds.gas,
            ["Rockefeller"] = RagnarokModSounds.rockefeller,
            ["Nineties"] = RagnarokModSounds.nineties,
            ["Scatman"] = RagnarokModSounds.scatman,
            ["Oiia"] = RagnarokModSounds.oiia,
            ["Alia"] = RagnarokModSounds.alia,
            ["BadApple"] = RagnarokModSounds.badapple,
            ["Delirious"] = RagnarokModSounds.delirious,
            ["Kingdom"] = RagnarokModSounds.kingdom,
            ["HollowPurple"] = RagnarokModSounds.hollowpurple,
            ["Shrine"] = RagnarokModSounds.shrine,
            ["TF2"] = RagnarokModSounds.tf2,
        };

        internal static SoundStyle GetRandomRiffSound(SoundStyle ownRiff)
        {
            var config = ModContent.GetInstance<AprilChaos>();

            // Build the weighted list at call time from config
            var pool = new List<(SoundStyle sound, int weight)>
            {
                (ownRiff, config.OwnRiffWeight)
            };

            foreach (var (key, weight) in config.RiffWeights)
            {
                if (weight > 0 && SoundMap.TryGetValue(key, out SoundStyle sound))
                    pool.Add((sound, weight));
            }

            int total = 0;
            foreach (var (_, w) in pool) total += w;
            if (total <= 0) return ownRiff;

            int roll = Main.rand.Next(total);
            int cumulative = 0;
            foreach (var (sound, w) in pool)
            {
                cumulative += w;
                if (roll < cumulative)
                    return sound;
            }

            return ownRiff;
        }
    }

    public abstract partial class BigRiffInstrumentBase
    {
        private static SoundStyle GetRandomRiffSound(SoundStyle riff)
            => ObnoxiousBardHelper.GetRandomRiffSound(riff);
    }

    public abstract partial class RiffInstrumentBase
    {
        private static SoundStyle GetRandomRiffSound(SoundStyle riff)
            => ObnoxiousBardHelper.GetRandomRiffSound(riff);
    }
   
}
*/