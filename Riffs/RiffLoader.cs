using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Utilities;

namespace RagnarokMod.Riffs
{
    public class RiffLoader : ModSystem
    {
        public class Cooldown : CooldownHandler
        {
            public static new string ID => "RiffCooldown";
            public override bool ShouldDisplay => true;
            public override LocalizedText DisplayName => Language.GetText("Mods.RagnarokMod.Riffs.Cooldown");
            public override string Texture => "RagnarokMod/Items/BardItems/Consumable/InspirationEssence";
            public override Color OutlineColor => new Color(220, 50, 50);
            public override Color CooldownStartColor => new Color(200, 30, 30);
            public override Color CooldownEndColor => new Color(170, 10, 10);
        }
        private static Dictionary<byte, Riff> Riffs = new();

        public static void AddRiff(Riff riff)
        {
            riff.RiffType = (byte)Riffs.Count;
            Riffs[riff.RiffType] = riff;
        }

        public static Riff GetRiff(byte type) => Riffs.TryGetValue(type, out var riff) ? riff : null;

        public static byte RiffType<T>() where T : Riff
        {
            foreach (var kvp in Riffs)
                if (kvp.Value is T)
                    return kvp.Key;
            throw new Exception($"Riff type {typeof(T).Name} is not registered.");
        }

        public override void OnModUnload()
        {
            Riffs.Clear();
        }

        // Call this from RagnarokModPlayer.PostUpdateEquips
        public static void UpdateRiffs(Player player)
        {
            var ragnarokPlayer = player.GetRagnarokModPlayer();
            if (ragnarokPlayer.fretPlaying)
            {
                if (ragnarokPlayer.savedMusicVolume < 0f)
                    ragnarokPlayer.savedMusicVolume = Main.musicVolume;
            }
            if (!ragnarokPlayer.fretPlaying)
            {
                // Restore volume every frame until back to normal
                if (ragnarokPlayer.savedMusicVolume >= 0f)
                {
                    Main.musicVolume = MathHelper.Lerp(Main.musicVolume, ragnarokPlayer.savedMusicVolume, 0.1f);
                    if (Math.Abs(Main.musicVolume - ragnarokPlayer.savedMusicVolume) < 0.01f)
                    {
                        Main.musicVolume = ragnarokPlayer.savedMusicVolume;
                        ragnarokPlayer.savedMusicVolume = -1f;
                    }
                }
                return;
            }

            // Fade down while riff is active
            Main.musicVolume = MathHelper.Lerp(Main.musicVolume, 0.1f, 0.1f);
            if (!ragnarokPlayer.fretPlaying)
                return;

            Riff activeRiff = GetRiff(ragnarokPlayer.activeRiffType);
            if (activeRiff == null)
                return;

            ThoriumPlayer bardThorium = player.GetThoriumPlayer();
            short duration = (short)((300 + bardThorium.bardBuffDuration) * bardThorium.bardBuffDurationX);

            void ApplyToTarget(Player target)
            {
                activeRiff.Update(player, target);

                ThoriumPlayer targetThorium = target.GetThoriumPlayer();
                foreach ((byte type, byte level) in activeRiff.Empowerments)
                    EmpowermentHelper.ApplyEmpowerment(bardThorium, targetThorium, type, level, duration);
            }

            foreach (Player target in MiscHelper.GetPlayersInRange(player, activeRiff.Range))
                ApplyToTarget(target);

            ApplyToTarget(player); // apply to self
        }
    }
}