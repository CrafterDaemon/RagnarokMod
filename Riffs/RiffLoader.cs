using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using RagnarokMod.Common.Configs;
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

        public static void UpdateRiffs(Player player)
        {
            var ragnarokPlayer = player.GetRagnarokModPlayer();

            // Fade out music while riff is active (only if riff volume > 0)
            if (ragnarokPlayer.fretPlaying && ModContent.GetInstance<ClientConfig>().RiffMusicVolume > 0f)
            {
                if (Main.musicFade[Main.curMusic] > 0.1f)
                    Main.musicFade[Main.curMusic] = Math.Max(0.1f, Main.musicFade[Main.curMusic] - 0.02f);
            }
            else
            {
                // Restore music fade
                if (Main.musicFade[Main.curMusic] < 1f)
                    Main.musicFade[Main.curMusic] = Math.Min(1f, Main.musicFade[Main.curMusic] + 0.02f);
            }

            if (!ragnarokPlayer.fretPlaying)
            {
                // Call OnEnd for all tracked targets when riff stops
                if (ragnarokPlayer.activeRiffTargets.Count > 0)
                {
                    Riff riff = GetRiff(ragnarokPlayer.activeRiffType);
                    if (riff != null)
                    {
                        foreach (int targetIndex in ragnarokPlayer.activeRiffTargets)
                        {
                            if (targetIndex >= 0 && targetIndex < Main.maxPlayers)
                                riff.OnEnd(player, Main.player[targetIndex]);
                        }
                    }
                    ragnarokPlayer.activeRiffTargets.Clear();
                }

                return;
            }

            Riff activeRiff = GetRiff(ragnarokPlayer.activeRiffType);
            if (activeRiff == null)
                return;

            ThoriumPlayer bardThorium = player.GetThoriumPlayer();
            short duration = (short)((300 + bardThorium.bardBuffDuration) * bardThorium.bardBuffDurationX);

            HashSet<int> currentTargets = new HashSet<int>();

            void ApplyToTarget(Player target)
            {
                currentTargets.Add(target.whoAmI);

                // Call OnStart for new targets
                if (!ragnarokPlayer.activeRiffTargets.Contains(target.whoAmI))
                {
                    activeRiff.OnStart(player, target);
                }

                // Update all targets
                activeRiff.Update(player, target);

                // Apply empowerments
                ThoriumPlayer targetThorium = target.GetThoriumPlayer();
                foreach ((byte type, byte level) in activeRiff.Empowerments)
                    EmpowermentHelper.ApplyEmpowerment(bardThorium, targetThorium, type, level, duration);
            }

            foreach (Player target in MiscHelper.GetPlayersInRange(player, activeRiff.Range))
                ApplyToTarget(target);
            ApplyToTarget(player); // apply to self

            // Call OnEnd for targets that left range
            foreach (int targetIndex in ragnarokPlayer.activeRiffTargets)
            {
                if (!currentTargets.Contains(targetIndex))
                {
                    if (targetIndex >= 0 && targetIndex < Main.maxPlayers)
                        activeRiff.OnEnd(player, Main.player[targetIndex]);
                }
            }

            ragnarokPlayer.activeRiffTargets = currentTargets;
        }
    }
}