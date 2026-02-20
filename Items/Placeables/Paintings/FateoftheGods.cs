using System.Collections.Generic;
using System.Linq;
using CalamityMod;
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
            Item.rare = ItemRarityID.White;
            Item.createTile = ModContent.TileType<FateoftheGodsTile>();
            Item.Calamity().donorItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                return;

            string tooltip = "";

            int namesPerLine = 8;
            for (int i = 0; i < devList.Count; i++)
            {
                tooltip += devList[i];

                if (i == devList.Count - 1)
                    break;

                if (i % namesPerLine == 0 && i != 0)
                    tooltip += "\n";
                else
                    tooltip += ", ";
            }
            tooltip += "\n";

            TooltipLine line = tooltips.FirstOrDefault(t => t.Mod == "Terraria" && t.Name == "Tooltip2");
            if (line != null)
                line.Text = tooltip;
        }

        public static IList<string> devList = new List<string>()
        {
            //Donors
            "fireflamejoe", 
            "HayWire799",
            "aeolus",

            //Artists
            "Ochette", 
            "Kyou", 
            "Delly", 
            "e³", 
            "bence", 
            "TheStormQueen", 
            "imonthatgudkush",

            //Programmers
            "CrafterDaemon", 
            "Patrick1234", 
            "frogleader", 
            "Akira", 
            "WardrobeHummus", 
            "Ropro0923",

            //Other Developers
            "TelosRyu", 
            "Deeno", 
            "moon_ditch"
        };
    }
}
