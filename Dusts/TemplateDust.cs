using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Dusts
{
    public class TemplateDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true; // Makes the dust have no gravity.
            dust.noLight = false; // Makes the dust emit no light.
        }
    }
}
