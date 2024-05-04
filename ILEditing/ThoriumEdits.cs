using CalamityMod.Items.Materials;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria.ModLoader;

namespace RagnarokMod.ILEditing
{
    public class ThoriumEdits : ModSystem
    {
        private static Mod Thorium => ModLoader.GetMod("ThoriumMod");
        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Thorium != null)
                {
                    IL.ThoriumMod.Items.Donate.TerrariansLastKnife.OnHitNPC += NewLifestealMath;
                    IL.ThoriumMod.Tiles.AncientPhylactery.RightClick += HavocPhylactory;
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
            }
        }
        private void HavocPhylactory(ILContext il)
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
    }
}
