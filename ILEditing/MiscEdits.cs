using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RagnarokMod.Items;
using RagnarokMod.Sounds;
using ReLogic.Utilities;
using System;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.ILEditing
{
    public class MiscEdits : ModSystem
    {
        private static int scareCooldown = 0;
        public delegate void RefAction<T>(ref T value);

        public override void OnModLoad()
        {
            On_SoundEngine.PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback += PlaySoundHook;
        }

        private SlotId PlaySoundHook(On_SoundEngine.orig_PlaySound_refSoundStyle_Nullable1_SoundUpdateCallback orig, ref SoundStyle style, Vector2? position, SoundUpdateCallback callback)
        {
            Player player = Main.LocalPlayer;

            if (player.active && scareCooldown <= 0)
            {
                foreach (Item item in player.inventory)
                {
                    if (item.type == ModContent.ItemType<PrimalTerror>() &&
                        item.favorited &&
                        Main.rand.NextBool(10000))
                    {
                        SoundStyle scareStyle =
                            new SoundStyle("CalamityMod/Sounds/Custom/Scare");

                        scareCooldown = 600;

                        return orig(ref scareStyle, position, callback);
                    }
                }
            }

            return orig(ref style, position, callback);
        }

    }
}