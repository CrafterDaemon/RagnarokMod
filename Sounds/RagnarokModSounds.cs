using Terraria.Audio;

namespace RagnarokMod.Sounds
{
    public static class RagnarokModSounds
    {
        //it's quiet. too quiet...
        public const string PathPfx = "RagnarokMod/Sounds";

        public static readonly SoundStyle bonk = new("RagnarokMod/Sounds/bonk") { Volume = 0.5f, MaxInstances = 0 };
        public static readonly SoundStyle calamitybell = new("RagnarokMod/Sounds/CalamityBell") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSC = new("RagnarokMod/Sounds/TSC") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSL = new("RagnarokMod/Sounds/TSL") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSS = new("RagnarokMod/Sounds/TSS") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle Steampipes = new("RagnarokMod/Sounds/Steampipes") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle RadioDemon = new("RagnarokMod/Sounds/radiodemon") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle HookLaunch = new("RagnarokMod/Sounds/HookLaunch") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle HookRetract = new("RagnarokMod/Sounds/HookRetract") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle fretsriff = new("RagnarokMod/Sounds/fretsriff") { Volume = 0.6f, MaxInstances = 1, SoundLimitBehavior = SoundLimitBehavior.IgnoreNew };
        public static readonly SoundStyle scourgesfrets = new("RagnarokMod/Sounds/scourgesfrets") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle none = new("RagnarokMod/Sounds/empty") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle Red = new("RagnarokMod/Sounds/Red") { Volume = 0.6f, MaxInstances = 0 };
        public static readonly SoundStyle Blue = new("RagnarokMod/Sounds/Blue") { Volume = 1f, MaxInstances = 0 };
        public static readonly SoundStyle PurpleCharge = new("RagnarokMod/Sounds/PurpleCharge") { Volume = 1f, MaxInstances = 0 };
        public static readonly SoundStyle PurpleExplode = new("RagnarokMod/Sounds/PurpleExplode") { Volume = 1f, MaxInstances = 0 };
    }
}
