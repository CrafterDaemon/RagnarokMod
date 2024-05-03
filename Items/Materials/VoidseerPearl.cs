using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Drawing.Text;
using System.Drawing;
using CalamityMod.NPCs.TownNPCs;

namespace RagnarokMod.Items.Materials
{
    public class VoidseerPearl: ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 9999;
            Item.value = Item.sellPrice(gold: 15);
            Item.rare = ItemRarityID.LightRed;
        }
    }
}
