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
		private static Mod Calamity => ModLoader.GetMod("CalamityMod");


        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Calamity != null)
                {
					// Does not work and breaks the function
					//IL.CalamityMod.CalPlayer.CalamityPlayer.UpdateRogueStealth += RogueUseTimeFix;
					
					
                    loadCaught = true;
                    break;
                }	
            }
        }
        public override void OnModUnload()
        {
            if (Calamity != null)
            {
				//IL.CalamityMod.CalPlayer.CalamityPlayer.UpdateRogueStealth -= RogueUseTimeFix;
            }
        }
   
		private void RogueUseTimeFix(ILContext il)
		{
			var c = new ILCursor(il);
			 if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcR4(0f)))
            {
                return;
            }
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_R4, 20f);
			
			/*
			if (!c.TryGotoNext(MoveType.After, i =>
			i.OpCode == OpCodes.Ldfld && i.Operand.ToString().Contains("animationCheck") ||
			i.OpCode == OpCodes.Ldsfld && i.Operand.ToString().Contains("animationCheck")))
			{
				Main.NewText("Hat nicht geschmeckt");
				return;
			}

			// Now that we're after the likely allocation, insert Main.NewText(animationCheck) call
			c.Emit(OpCodes.Call, "Main::NewText");
			*/	
		}
	}
}
