using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using IL.ThoriumMod;
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

namespace RagnarokMod.ILEditing
{
    public class ThoriumEdits : ModSystem
    {
        private static Mod Thorium => ModLoader.GetMod("ThoriumMod");

        public int maxInsp = 50;
        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Thorium != null)
                {
                    IL.ThoriumMod.Items.Donate.TerrariansLastKnife.OnHitNPC += NewLifestealMath;
                    IL.ThoriumMod.Tiles.AncientPhylactery.RightClick += HavocPhylactory;
                    IL.ThoriumMod.Projectiles.Bard.BlackMIDIPro.BardOnHitNPC += BlackMidiTweak;
                    IL.ThoriumMod.Buffs.Bard.SoloistsHatSetBuff.Update += SoloistSetNerf;
					IL.ThoriumMod.Items.BossThePrimordials.Rhapsodist.InspiratorsHelmet.ModifyEmpowerment += RhapsodistSetNerf;
                    IL.ThoriumMod.ThoriumPlayer.PostUpdateEquips += removeBardResourceCaps;
					//IL.ThoriumMod.Buffs.Mount.GoldenScaleBuff.Update += tweakGoldenScaleBuff;
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
                IL.ThoriumMod.Items.Donate.TerrariansLastKnife.OnHitNPC -= NewLifestealMath;
                IL.ThoriumMod.Tiles.AncientPhylactery.RightClick -= HavocPhylactory;
                IL.ThoriumMod.Projectiles.Bard.BlackMIDIPro.BardOnHitNPC -= BlackMidiTweak;
                IL.ThoriumMod.Buffs.Bard.SoloistsHatSetBuff.Update -= SoloistSetNerf;
				IL.ThoriumMod.Items.BossThePrimordials.Rhapsodist.InspiratorsHelmet.ModifyEmpowerment -= RhapsodistSetNerf;
                IL.ThoriumMod.ThoriumPlayer.PostUpdateEquips -= removeBardResourceCaps;
				//IL.ThoriumMod.Buffs.Mount.GoldenScaleBuff.Update -= tweakGoldenScaleBuff;
            }
        }
        private void HavocPhylactory(ILContext il)
        {
            if (ModContent.GetInstance<BossProgressionConfig>().Lich)
            {
                var c = new ILCursor(il);

                if (!c.TryGotoNext(i => i.MatchLdcI4(1725)))
                {
                    return;
                }

                c.Index++;
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, ModContent.ItemType<EssenceofHavoc>());
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
			
			if (!c.TryGotoNext(MoveType.After, i => i.OpCode == OpCodes.Ldarg_0
                                      && i.Next?.OpCode == OpCodes.Ldfld
                                      && ((FieldReference)i.Next.Operand).Name == "breath"
                                      && i.Next.Next?.OpCode == OpCodes.Ldarg_0
                                      && i.Next.Next.Next?.OpCode == OpCodes.Ldfld
                                      && ((FieldReference)i.Next.Next.Next.Operand).Name == "breathMax"
                                      && i.Next.Next.Next.Next?.OpCode == OpCodes.Add
                                      && i.Next.Next.Next.Next.Next?.OpCode == OpCodes.Ldc_I4_3
                                      && i.Next.Next.Next.Next.Next.Next?.OpCode == OpCodes.Add
                                      && i.Next.Next.Next.Next.Next.Next.Next?.OpCode == OpCodes.Stfld))
			{
				return;
			}
			c.Remove();
			c.Emit(OpCodes.Ldarg_0);
			c.Emit(OpCodes.Dup);
			c.Emit(OpCodes.Ldfld, typeof(Player).GetField("breath"));
			c.Emit(OpCodes.Ldc_I4_1);
			c.Emit(OpCodes.Add);
			c.Emit(OpCodes.Stfld, typeof(Player).GetField("breath"));
		}
    }
}
