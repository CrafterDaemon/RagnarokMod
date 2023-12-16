using Terraria.Audio;

namespace RagnarokMod.Sounds
{
    public static class RagnarokModSounds
    {
        public const string PathPfx = "RagnarokModMod/Sounds";

        public static readonly SoundStyle bonk = new("RagnarokModMod/Sounds/bonk") { Volume = 0.5f, MaxInstances = 0};
    }
}
