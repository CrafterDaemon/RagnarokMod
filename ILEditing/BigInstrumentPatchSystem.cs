using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using RagnarokMod.Common.Configs;
using RagnarokMod.Items.BardItems;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BardItems;

namespace RagnarokMod.ILEdits
{
    public class BigInstrumentPatchSystem : ModSystem
    {
        public static float DefaultDamageDecreaseOnFail = 0.5f;
        public static bool ShouldPlayMissSound = true;
        public static Dictionary<int, bool> MissSoundOverrides = new();
        public static SoundStyle MissSoundStyle = SoundID.Item16;

        private static ILHook damageDecreaseHook = null;
        private static ILHook soundGateHook = null;
        private static ILHook soundStyleHook = null;
        private static ILHook clampHook = null;

        public override void OnModLoad()
        {
            var bigInstrumentType = typeof(BigInstrumentItemBase);
            var bardShootMethod = bigInstrumentType.GetMethod("BardShoot", BindingFlags.Public | BindingFlags.Instance);

            damageDecreaseHook = new ILHook(
                bigInstrumentType.GetMethod("get_DamageDecreaseOnFail", BindingFlags.Public | BindingFlags.Instance),
                PatchDamageDecreaseOnFail);
            damageDecreaseHook.Apply();
            Mod.Logger.Info("[BigInstrumentPatch] damageDecreaseHook applied");

            soundGateHook = new ILHook(bardShootMethod, PatchSoundGate);
            soundGateHook.Apply();
            Mod.Logger.Info("[BigInstrumentPatch] soundGateHook applied");

            soundStyleHook = new ILHook(bardShootMethod, PatchSoundStyle);
            soundStyleHook.Apply();
            Mod.Logger.Info("[BigInstrumentPatch] soundStyleHook applied");

            clampHook = new ILHook(bardShootMethod, PatchClamp);
            clampHook.Apply();
            Mod.Logger.Info("[BigInstrumentPatch] clampHook applied");
        }

        public override void Unload()
        {
            damageDecreaseHook?.Dispose();
            soundGateHook?.Dispose();
            soundStyleHook?.Dispose();
            clampHook?.Dispose();
        }

        private void PatchDamageDecreaseOnFail(ILContext il)
        {
            var c = new ILCursor(il);
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(0.75f)))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] Could not find ldc.r4 0.75 in get_DamageDecreaseOnFail");
                return;
            }
            c.Emit(OpCodes.Pop);
            c.EmitDelegate<Func<float>>(() => DefaultDamageDecreaseOnFail);
        }

        private void PatchSoundGate(ILContext il)
        {
            var c = new ILCursor(il);

            // Mark the label at IL_00ae after PlaySound AND its pop
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchCall(out var mr) && mr.Name == "PlaySound",
                i => i.MatchPop()))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchSoundGate: Could not find PlaySound+pop");
                return;
            }
            ILLabel afterSound = c.MarkLabel(); // now pointing at IL_00ae: ldloc.0

            // Go back and insert gate before ldsflda SoundID::Item16
            c.Index = 0;
            if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdsflda(out var fr) && fr.Name == "Item16"))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchSoundGate: Could not find ldsflda SoundID::Item16");
                return;
            }

            c.EmitDelegate<Func<bool>>(() =>
            {
                var item = Main.LocalPlayer.HeldItem;
                int type = item.type;

                if (item.ModItem is BigRiffInstrumentBase riffInstrument
                    && Main.LocalPlayer.GetRagnarokModPlayer().activeRiffType == riffInstrument.RiffType
                    && !ClientConfig.Instance.MissSoundDuringRiff)
                    return false;

                return MissSoundOverrides.TryGetValue(type, out bool val) ? val : ShouldPlayMissSound;
            });
            c.Emit(OpCodes.Brfalse, afterSound);
        }

        private void PatchSoundStyle(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdsflda(out var fr) && fr.Name == "Item16"))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchSoundStyle: Could not find ldsflda SoundID::Item16");
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldsflda,
                typeof(BigInstrumentPatchSystem).GetField(nameof(MissSoundStyle),
                    BindingFlags.Public | BindingFlags.Static));
        }

        private void PatchClamp(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchLdcI4(10),
                i => i.MatchDiv(),
                i => i.MatchLdcI4(1),
                i => i.MatchAdd()))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchClamp: Could not find clamp entry sequence");
                return;
            }

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1)))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchClamp: Could not find clamp min");
                return;
            }
            c.Emit(OpCodes.Pop);
            c.EmitDelegate<Func<int>>(() =>
            {
                if (Main.LocalPlayer.HeldItem.ModItem is BigInstrumentItemBase instrument)
                {
                    var emp = instrument.Empowerments;
                    if (emp != null && emp.Count > 0 && emp[0].Count > 0)
                        return emp[0][0].level;
                }
                return 1;
            });

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(3)))
            {
                Mod.Logger.Warn("[BigInstrumentPatch] PatchClamp: Could not find clamp max");
                return;
            }
            c.Emit(OpCodes.Pop);
            c.EmitDelegate<Func<int>>(() =>
            {
                if (Main.LocalPlayer.HeldItem.ModItem is BigInstrumentItemBase instrument)
                {
                    var emp = instrument.Empowerments;
                    if (emp != null && emp.Count > 0 && emp[0].Count > 0)
                        return emp[0][0].level + 2;
                }
                return 3;
            });
        }
    }
}