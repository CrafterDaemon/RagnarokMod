﻿using CalamityMod.Items.Materials;
using Microsoft.CodeAnalysis.CSharp;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace RagnarokMod.ILEditing
{
    public class ZZZtoLoadAfterThoirumEditsBardWheel : ModSystem
    {
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        private static Assembly ThoriumAssembly = thorium.GetType().Assembly;
        private static Type bardwheel = null;
        private static MethodInfo drawself = null;
        private static ILHook _hook = null;
        private static int maxInsp = 0;
        private static bool CaughtInspChange = false;
        public override void OnModLoad()
        {
            while (!CaughtInspChange)
            {
                if (maxInsp != 0)
                {
                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "BardWheel")
                        {
                            bardwheel = type;
                        }
                    }
                    drawself = bardwheel.GetMethod("DrawSelf", BindingFlags.NonPublic | BindingFlags.Instance);
                    _hook = new ILHook(drawself, updateBardWheel);

                    CaughtInspChange = true;
                }
            }
        }

        private static void updateBardWheel(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(40)))
            {
                return;
            }

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, maxInsp);
        }

        public static void GetMaxInsp(int insp)
        {
            maxInsp = insp;
        }
    }
}