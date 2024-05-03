using CalamityMod.Items.Materials;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ModLoader;

namespace RagnarokMod.ILEditing
{
    public class ThoriumEdits : ModSystem
    {
        private Mod Thorium => ModLoader.GetMod("ThoriumMod");

        public override void Load()
        {
            IL.ThoriumMod.Tiles.AncientPhylactery.RightClick += HavocPhylactory;
        }
        public override void Unload()
        {
            IL.ThoriumMod.Tiles.AncientPhylactery.RightClick -= HavocPhylactory;
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
    }
}
