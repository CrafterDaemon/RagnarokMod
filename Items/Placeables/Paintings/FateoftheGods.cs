using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CalamityMod.Rarities;
using RagnarokMod.Tiles.Paintings;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Items.Placeables.Paintings
{
    public class FateoftheGods : ModItem
    {
        public override string Texture => "RagnarokMod/icon";
        public override void SetDefaults()
        {
            Item.width = Item.height = 80;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.value = Item.buyPrice(0, 2, 0, 0); ;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.createTile = ModContent.TileType<FateoftheGodsTile>();
            Item.Calamity().donorItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                return;

            string tooltip = "";

            for (int i = 0; i < devList.Count; i++)
            {
                tooltip += devList[i];

                if (i == devList.Count - 1)
                    break;
            }

            TooltipLine line = tooltips.FirstOrDefault(t => t.Mod == "Terraria" && t.Name == "Tooltip2");
            if (line != null)
                line.Text = tooltip;
        }

        public IList<string> devList = new List<string>()
        {
            "\n[c/96FF96:Donors]:\n",
            "FireFlameJoe, ",
            "TheUnknownNerd, ",
            "HayWire799, ",
            "Aeolus",

            "\n[c/FFFF0A:Artists]:\n",
            "Ochette, ",
            "Kyou, ",
            "Delly, ",
            "e³, ",
            "Bence, ",
            "TheStormQueen, ",
            "Imonthatgudkush",

            "\n[c/FF2864:Programmers]:\n",
            "CrafterDaemon, ",
            "Patrick1234, ",
            "Frogleader, ",
            "Akira, ",
            "WardrobeHummus, ",
            "Ropro0923",

            "\n[c/9696FF:Other Developers]:\n",
            "TelosRyu, ",
            "Deeno, ",
            "Moon_Ditch"
        };
    }
}
