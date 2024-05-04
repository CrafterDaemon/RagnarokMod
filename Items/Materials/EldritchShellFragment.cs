using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Materials
{
    public class EldritchShellFragment : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.33f;
            Item.maxStack = 9999;
            Item.value = 333333;
            Item.rare = ItemRarityID.Yellow;
        }
    }
}
