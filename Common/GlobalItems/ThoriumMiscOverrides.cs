using ThoriumMod.Items.ZRemoved;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using RagnarokMod.Items;
using Microsoft.Xna.Framework.Graphics;

namespace RagnarokMod.Common.GlobalItems
{
    public class ThoriumMiscOverrides : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.ModItem is LichRequirement;
        }
        public override void SetStaticDefaults()
        {
            TextureAssets.Item[ModContent.ItemType<LichRequirement>()] = ModContent.Request<Texture2D>("RagnarokMod/Items/NewLichRequirement");
        }
    }
}
