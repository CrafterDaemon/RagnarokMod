using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Mono.Cecil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using RagnarokMod.Common.Configs;
using System;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Buffs.Bard;

namespace RagnarokMod.ILEditing
{
    public class ThoriumEdits : ModSystem
    {
        private static Mod Thorium => ModLoader.GetMod("ThoriumMod");
        private static Assembly ThoriumAssembly = Thorium.GetType().Assembly;
        private static Type tlk = null;
        private static MethodInfo tlkonhit = null;
        private static ILHook tlkhook = null;
        private static Type phyl = null;
        private static MethodInfo phylrc = null;
        private static ILHook phylhook = null;
        private static Type midi = null;
        private static MethodInfo midihit = null;
        private static ILHook midihook = null;
        private static Type solo = null;
        private static MethodInfo solobuff = null;
        private static ILHook solohook = null; 
        private static Type rhap = null;
        private static MethodInfo rhapbuff = null;
        private static ILHook rhaphook = null;
        private static Type bardcap = null;
        private static MethodInfo insplimit = null;
        private static ILHook insphook = null;
        private static Type gscale = null;
        private static MethodInfo gscaleup = null;
        private static ILHook gscalehook = null;
        private static Type depthaura = null;
        private static MethodInfo depthaurabuff = null;
        private static ILHook depthaurahook = null;
        private static Type ddh = null;
        private static MethodInfo ddhbuff = null;
        private static ILHook ddhhook = null;
		private static Type bardempowermentbar = null;
        private static MethodInfo getstartcoordinates = null;
        private static ILHook bardempowermentbarhook = null;
		private static Type nsm = null;
        private static MethodInfo nsmbuff = null;
        private static ILHook nsmhook = null;

        public int maxInsp = 50;
        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Thorium != null)
                {
                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "TerrariansLastKnife")
                        {
                            tlk = type;
                        }
                    }
                    tlkonhit = tlk.GetMethod("OnHitNPC", BindingFlags.Public | BindingFlags.Instance);
                    tlkhook = new ILHook(tlkonhit, NewLifestealMath);
                    tlkhook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "AncientPhylactery")
                        {
                            phyl = type;
                        }
                    }
                    phylrc = phyl.GetMethod("RightClick", BindingFlags.Public | BindingFlags.Instance);
                    phylhook = new ILHook(phylrc, HavocPhylactory);
                    phylhook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "BlackMIDIPro")
                        {
                            midi = type;
                        }
                    }
                    midihit = midi.GetMethod("BardOnHitNPC", BindingFlags.Public | BindingFlags.Instance);
                    midihook = new ILHook(midihit, BlackMidiTweak);
                    midihook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "SoloistsHatSetBuff")
                        {
                            solo = type;
                        }
                    }
                    solobuff = solo.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly, [typeof(Player), typeof(int).MakeByRefType()]);
                    solohook = new ILHook(solobuff, SoloistSetNerf);
                    solohook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "InspiratorsHelmet")
                        {
                            rhap = type;
                        }
                    }
                    rhapbuff = rhap.GetMethod("ModifyEmpowerment", BindingFlags.Public | BindingFlags.Instance);
                    rhaphook = new ILHook(rhapbuff, RhapsodistSetNerf);
                    rhaphook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "ThoriumPlayer")
                        {
                            bardcap = type;
                        }
                    }
                    insplimit = bardcap.GetMethod("PostUpdateEquips", BindingFlags.Public | BindingFlags.Instance);
                    insphook = new ILHook(insplimit, removeBardResourceCaps);
                    insphook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "GoldenScaleBuff")
                        {
                            gscale = type;
                        }
                    }
                    gscaleup = gscale.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    gscalehook = new ILHook(gscaleup, tweakGoldenScaleBuff);
                    gscalehook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "DepthDiverAura")
                        {
                            depthaura = type;
                        }
                    }
                    depthaurabuff = depthaura.GetMethod("Update", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    depthaurahook = new ILHook(depthaurabuff, tweakDepthBreath);
                    depthaurahook.Apply();

                    foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "DepthDiverHelmet")
                        {
                            ddh = type;
                        }
                    }
                    ddhbuff = ddh.GetMethod("UpdateEquip", BindingFlags.Public | BindingFlags.Instance);
                    ddhhook = new ILHook(ddhbuff, tweakDepthDiverHelmet);
                    ddhhook.Apply();
					
					foreach (Type type in ThoriumAssembly.GetTypes())
					{
						if (type.Name == "BardEmpowermentBar")
						{
                            bardempowermentbar = type;
						}
					}
					getstartcoordinates = bardempowermentbar.GetMethod("GetStartCoordinates", BindingFlags.NonPublic | BindingFlags.Instance);
					bardempowermentbarhook = new ILHook(getstartcoordinates, updateEmpowermentBar);	
					bardempowermentbarhook.Apply();    
					
					foreach (Type type in ThoriumAssembly.GetTypes())
                    {
                        if (type.Name == "NagaSkinMask")
                        {
                            nsm = type;
                        }
                    }
                    nsmbuff = nsm.GetMethod("UpdateEquip", BindingFlags.Public | BindingFlags.Instance);
                    nsmhook = new ILHook(nsmbuff, tweakNagaSkinMask);
                    nsmhook.Apply();
					
                    ZZZtoLoadAfterThoirumEditsBardWheel.GetMaxInsp(maxInsp);
                    loadCaught = true;
                    break;
                }
            }
        }
        public override void OnModUnload()
        {
            if (Thorium != null)
            {
                tlkhook.Dispose();
                phylhook.Dispose();
                midihook.Dispose();
                solohook.Dispose();
                rhaphook.Dispose();
                insphook.Dispose();
                gscalehook.Dispose();
                depthaurahook.Dispose();
                ddhhook.Dispose();
				bardempowermentbarhook.Dispose();
				nsmhook.Dispose();
            }
        }
        private void HavocPhylactory(ILContext il)
        {
            if (ModContent.GetInstance<BossProgressionConfig>().Lich)
            {
                var c = new ILCursor(il);

                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1725)))
                {
                    return;
                }
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, ModContent.ItemType<EssenceofHavoc>());
                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(30)))
                {
                    return;
                }
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 3);
            }
        }
        private void NewLifestealMath(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchDiv()))
            {
                return;
            }


            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, 30);
            c.Index++;
            c.Emit(OpCodes.Conv_R8);
            c.Emit(OpCodes.Ldc_R8, 0.5);
            c.Emit(OpCodes.Call, typeof(Math).GetMethod("Pow"));
            c.Emit(OpCodes.Conv_I4);
        }

        private void BlackMidiTweak(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchConvI4()))
            {
                return;
            }

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldarg, 3);
            c.EmitDelegate<Func<int, int>>(damageDone => (int)Math.Sqrt((double)(damageDone / 20)));
        }

        private void SoloistSetNerf(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(0.35f)))
            {
                return;
            }

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, 0.15f);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(0.75f)))
            {
                return;
            }

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, 0.3f);
        }
		
		private void RhapsodistSetNerf(ILContext il)
        {
            var c = new ILCursor(il);

            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(600)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, 300);
        }

        private void removeBardResourceCaps(ILContext il)
        {
            var c = new ILCursor(il);

            for (int i = 0; i < 8; i++)
            {
                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(40)))
                {
                    return;
                }
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, maxInsp);
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(40)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, maxInsp);
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(60)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, maxInsp + 20);
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(60)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, maxInsp + 20);
        }
		
        private void tweakGoldenScaleBuff(ILContext il) 
        {
				var c = new ILCursor(il);
				c.EmitRet();
        }
		
		private void tweakDepthBreath(ILContext il) 
        {
				var c = new ILCursor(il);
				c.EmitRet();
        }
		
		private void tweakDepthDiverHelmet(ILContext il) 
		{
			var c = new ILCursor(il);
			c.EmitRet();
		}
		
		 private void updateEmpowermentBar(ILContext il)
        {
            var c = new ILCursor(il);
			
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(32f)))
            {
                return;
            }
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(32f)))
            {
                return;
            }

			c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, (float)ModContent.GetInstance<UIConfig>().BardEmpowermentBarOffsetX);
			
			
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(76)))
            {
                return;
            }

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, (int)ModContent.GetInstance<UIConfig>().BardEmpowermentBarOffsetY);
        }
		
		private void tweakNagaSkinMask(ILContext il) 
		{
			var c = new ILCursor(il);
			c.EmitRet();
		}
    }
}
