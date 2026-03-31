using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RagnarokMod.Sounds;
using Terraria;
using Terraria.Audio;

namespace RagnarokMod.Items.BardItems
{
    public abstract partial class BigRiffInstrumentBase
    {
        private static SoundStyle GetRandomRiffSound(SoundStyle riff)
        {
            // (sound, weight) pairs -- higher weight = more likely
            (SoundStyle sound, int weight)[] riffSounds = {
                (riff, 100),
                (RagnarokModSounds.rotjdriff,   10),
                (RagnarokModSounds.calcloneriff, 10),
                (RagnarokModSounds.slimeriff,   10),
                (RagnarokModSounds.aureusriff, 10),
                (RagnarokModSounds.shredderriff,   10),
                (RagnarokModSounds.HiveMindRiff, 10),
                (RagnarokModSounds.devourerriff,   10),
                (RagnarokModSounds.fretsriff, 10),
                (RagnarokModSounds.nfl, 2),
                (RagnarokModSounds.lavachicken, 2),
                (RagnarokModSounds.cortisol, 2),
                (RagnarokModSounds.crabrave, 2),
                (RagnarokModSounds.meow, 2),
                (RagnarokModSounds.outro, 2),
                (RagnarokModSounds.sans, 2),
                (RagnarokModSounds.tycoon, 2),
                (RagnarokModSounds.worldrevolving, 2),
                (RagnarokModSounds.rickroll, 2)
            };

            int totalWeight = 0;
            foreach (var (_, weight) in riffSounds)
                totalWeight += weight;

            int roll = Main.rand.Next(totalWeight);
            int cumulative = 0;
            foreach (var (sound, weight) in riffSounds)
            {
                cumulative += weight;
                if (roll < cumulative)
                    return sound;
            }

            return riffSounds[0].sound; // fallback
        }
    }

    public abstract partial class RiffInstrumentBase
    {
        private static SoundStyle GetRandomRiffSound(SoundStyle riff)
        {
            // (sound, weight) pairs -- higher weight = more likely
            (SoundStyle sound, int weight)[] riffSounds = {
                (riff, 100),
                (RagnarokModSounds.rotjdriff,   10),
                (RagnarokModSounds.calcloneriff, 10),
                (RagnarokModSounds.slimeriff,   10),
                (RagnarokModSounds.aureusriff, 10),
                (RagnarokModSounds.shredderriff,   10),
                (RagnarokModSounds.HiveMindRiff, 10),
                (RagnarokModSounds.devourerriff,   10),
                (RagnarokModSounds.fretsriff, 10),
                (RagnarokModSounds.nfl, 2),
                (RagnarokModSounds.lavachicken, 2),
                (RagnarokModSounds.cortisol, 2),
                (RagnarokModSounds.crabrave, 2),
                (RagnarokModSounds.meow, 2),
                (RagnarokModSounds.outro, 2),
                (RagnarokModSounds.sans, 2),
                (RagnarokModSounds.tycoon, 2),
                (RagnarokModSounds.worldrevolving, 2),
                (RagnarokModSounds.rickroll, 2)
            };

            int totalWeight = 0;
            foreach (var (_, weight) in riffSounds)
                totalWeight += weight;

            int roll = Main.rand.Next(totalWeight);
            int cumulative = 0;
            foreach (var (sound, weight) in riffSounds)
            {
                cumulative += weight;
                if (roll < cumulative)
                    return sound;
            }

            return riffSounds[0].sound; // fallback
        }
    }
}
