using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Drawing.Text;
using System.Drawing;
using CalamityMod.NPCs.TownNPCs;
using CalamityMod.Items;

namespace RagnarokMod.Items.Materials
{
    public class VoidseerPearl: ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 9999;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
        }
    }
}
