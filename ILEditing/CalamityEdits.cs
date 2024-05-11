using CalamityMod.Items.Materials;
using IL.CalamityMod;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace RagnarokMod.ILEditing
{
    public class CalamityEdits : ModSystem
    {
		private static Mod ModCalamity = ModLoader.GetMod("CalamityMod");
        private static Assembly calamityAssembly = ModCalamity.GetType().Assembly;
        private static Type calPlayer = null;
        private static MethodInfo updateRogueStealth = null;
        private static ILHook _hook = null;
        private static bool loaded = false;

        public override void OnModLoad()
        {
            while (!loaded)
            {
                foreach (Type type in calamityAssembly.GetTypes())
                {
                    if (type.Name == "CalamityPlayer")
                    {
                        calPlayer = type;
                        Console.WriteLine("Caught Class!");
                    }
                    if (calPlayer != null)
                    {
                        updateRogueStealth = calPlayer.GetMethod("UpdateRogueStealth", BindingFlags.Public | BindingFlags.IgnoreReturn | BindingFlags.Instance);
                        Console.WriteLine("Caught Method!");
                        if (updateRogueStealth != null)
                        {
                            _hook = new ILHook(updateRogueStealth, RogueUseTimeFix);
                            loaded = true;
                            break;
                        }
                    }
                }
            }
        }

   
		private void RogueUseTimeFix(ILContext il)
        {
            Console.WriteLine("Hooked!");
            var c = new ILCursor(il);
			 if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(0f)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, 20f);
			
			if (!c.TryGotoNext(MoveType.After, i =>
			i.OpCode == OpCodes.Ldfld && i.Operand.ToString().Contains("animationCheck") ||
			i.OpCode == OpCodes.Ldsfld && i.Operand.ToString().Contains("animationCheck")))
			{
				Main.NewText("Hat nicht geschmeckt");
				return;
			}

			// Now that we're after the likely allocation, insert Main.NewText(animationCheck) call
			c.Emit(OpCodes.Call, "Main::NewText");
		}
	}
}
