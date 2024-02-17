using Terraria.Audio;

namespace RagnarokMod.Sounds
{
    public static class RagnarokModSounds
    {
        //it's quiet. too quiet...
        public const string PathPfx = "RagnarokMod/Sounds";

        public static readonly SoundStyle bonk = new("RagnarokMod/Sounds/bonk") { Volume = 0.5f, MaxInstances = 0};
		 public static readonly SoundStyle calamitybell = new("RagnarokMod/Sounds/CalamityBell") { Volume = 0.75f, MaxInstances = 0};
    }
}
