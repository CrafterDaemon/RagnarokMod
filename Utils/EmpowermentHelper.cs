using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Utils
{
    public static class EmpowermentHelper
    {
        private static MethodInfo applyEmpowermentMethod;

        private static FieldInfo empowermentsField;

        public static void Load()
        {
            empowermentsField = typeof(ThoriumPlayer)
                .GetField("Empowerments", BindingFlags.NonPublic | BindingFlags.Instance);

            if (empowermentsField == null)
                ModContent.GetInstance<RagnarokMod>().Logger.Warn("EmpowermentHelper: failed to find Empowerments field");

            applyEmpowermentMethod = typeof(EmpowermentLoader).GetMethod(
                "ApplyEmpowerment",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                new Type[] { typeof(ThoriumPlayer), typeof(ThoriumPlayer), typeof(byte), typeof(byte), typeof(short) },
                null
            );

            if (applyEmpowermentMethod == null)
                ModContent.GetInstance<RagnarokMod>().Logger.Warn("EmpowermentHelper: failed to find ApplyEmpowerment method");
        }

        public static void Unload()
        {
            empowermentsField = null;
            applyEmpowermentMethod = null;
        }

        public static void ApplyEmpowerment(ThoriumPlayer bard, ThoriumPlayer target, byte type, byte level, short duration)
        {
            applyEmpowermentMethod?.Invoke(null, new object[] { bard, target, type, level, duration });
        }

        public static EmpowermentData GetEmpowermentData(ThoriumPlayer thoriumPlayer)
            => (EmpowermentData)empowermentsField.GetValue(thoriumPlayer);

        public static void ModifyActiveEmpowerment(ThoriumPlayer thoriumPlayer, byte type, byte? levelBoost = null, short? durationBoost = null)
        {
            var data = GetEmpowermentData(thoriumPlayer);
            if (!data.Timers.TryGetValue(type, out var timer) || timer.level == 0)
                return;

            if (levelBoost.HasValue)
            {
                byte maxLevel = EmpowermentLoader.GetEmpowerment(type)?.MaxLevel ?? 10;
                timer.level = (byte)Math.Min(timer.level + levelBoost.Value, maxLevel);
            }

            if (durationBoost.HasValue)
                timer.timer += durationBoost.Value;
        }
    }
}