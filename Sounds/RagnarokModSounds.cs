using Terraria.Audio;

namespace RagnarokMod.Sounds
{
    public static class RagnarokModSounds
    {
        //it's quiet. too quiet...
        public const string PathPfx = "RagnarokMod/Sounds";

        public static readonly SoundStyle bonk = new("RagnarokMod/Sounds/bonk") { Volume = 0.5f, MaxInstances = 0};
		public static readonly SoundStyle calamitybell = new("RagnarokMod/Sounds/CalamityBell") { Volume = 0.75f, MaxInstances = 0};
        public static readonly SoundStyle TSC = new("RagnarokMod/Sounds/TSC") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSL = new("RagnarokMod/Sounds/TSL") { Volume = 0.75f, MaxInstances = 0 };
        public static readonly SoundStyle TSS = new("RagnarokMod/Sounds/TSS") { Volume = 0.75f, MaxInstances = 0 };
    }
}
