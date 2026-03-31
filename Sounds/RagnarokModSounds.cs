using Terraria.Audio;

namespace RagnarokMod.Sounds
{
    public static class RagnarokModSounds
    {
        //it's quiet. too quiet...
        public const string PathPfx = "RagnarokMod/Sounds";

        //sounds
        public static readonly SoundStyle TSC = new("RagnarokMod/Sounds/TSC") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSL = new("RagnarokMod/Sounds/TSL") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSS = new("RagnarokMod/Sounds/TSS") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle HookLaunch = new("RagnarokMod/Sounds/HookLaunch") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle HookRetract = new("RagnarokMod/Sounds/HookRetract") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle none = new("RagnarokMod/Sounds/empty") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle Red = new("RagnarokMod/Sounds/Red") { Volume = 0.6f, MaxInstances = 0 };
        public static readonly SoundStyle Blue = new("RagnarokMod/Sounds/Blue") { Volume = 1f, MaxInstances = 0 };
        public static readonly SoundStyle PurpleCharge = new("RagnarokMod/Sounds/PurpleCharge") { Volume = 1f, MaxInstances = 0 };
        public static readonly SoundStyle PurpleExplode = new("RagnarokMod/Sounds/PurpleExplode") { Volume = 1f, MaxInstances = 0 };

        //instruments
        public static readonly SoundStyle bonk = new("RagnarokMod/Sounds/bonk") { Volume = 0.5f, MaxInstances = 0 };
        public static readonly SoundStyle calamitybell = new("RagnarokMod/Sounds/CalamityBell") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle Steampipes = new("RagnarokMod/Sounds/Steampipes") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle RadioDemon = new("RagnarokMod/Sounds/radiodemon") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle scourgesfrets = new("RagnarokMod/Sounds/scourgesfrets") { Volume = 0.5f, MaxInstances = 0 };
        public static readonly SoundStyle corroslimebass = new("RagnarokMod/Sounds/corroslimebass") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle crimslimeoboe = new("RagnarokMod/Sounds/crimslimeoboe") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle stringsofoblivion = new("RagnarokMod/Sounds/stringsofoblivion") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle devourersine = new("RagnarokMod/Sounds/devourersine") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest };
        public static readonly SoundStyle shredder = new("RagnarokMod/Sounds/shredder") { Volume = 0.5f, MaxInstances = 0 };
        public static readonly SoundStyle stellarriff = new("RagnarokMod/Sounds/stellarriff") { Volume = 0.5f, MaxInstances = 0 };
        public static readonly SoundStyle dragonforce = new("RagnarokMod/Sounds/dragonforce") { Volume = 0.2f, MaxInstances = 0 };

        //riffs
        public static readonly SoundStyle fretsriff = new("RagnarokMod/Sounds/Music/Riffs/fretsriff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle devourerriff = new("RagnarokMod/Sounds/Music/Riffs/devourerriff") { Volume = 0.4f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle HiveMindRiff = new("RagnarokMod/Sounds/Music/Riffs/HiveMindRiff") { Volume = 0.4f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle shredderriff = new("RagnarokMod/Sounds/Music/Riffs/shredderriff") { Volume = 0.4f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle aureusriff = new("RagnarokMod/Sounds/Music/Riffs/aureusriff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle rotjdriff = new("RagnarokMod/Sounds/Music/Riffs/rotjdriff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle slimeriff = new("RagnarokMod/Sounds/Music/Riffs/SlimeRiff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle calcloneriff = new("RagnarokMod/Sounds/Music/Riffs/CalamitasCloneRiff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };

        //memes
        public static readonly SoundStyle nfl = new("RagnarokMod/Sounds/Music/Riffs/Memes/nfl") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle lavachicken = new("RagnarokMod/Sounds/Music/Riffs/Memes/lavachicken") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle cortisol = new("RagnarokMod/Sounds/Music/Riffs/Memes/cortisol") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle crabrave = new("RagnarokMod/Sounds/Music/Riffs/Memes/crabrave") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle meow = new("RagnarokMod/Sounds/Music/Riffs/Memes/meow") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle outro = new("RagnarokMod/Sounds/Music/Riffs/Memes/outro") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle sans = new("RagnarokMod/Sounds/Music/Riffs/Memes/sans") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle tycoon = new("RagnarokMod/Sounds/Music/Riffs/Memes/tycoon") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle worldrevolving = new("RagnarokMod/Sounds/Music/Riffs/Memes/worldrevolving") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle rickroll = new("RagnarokMod/Sounds/Music/Riffs/Memes/rickroll") { Volume = 1f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
    }
}
