using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CalamityMod.Rarities;
using RagnarokMod.Tiles.Paintings;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace RagnarokMod.Items.Placeables.Paintings
{
    public class FateoftheGods : ModItem
    {
        public override string Texture => "RagnarokMod/icon";
        
        private readonly string[] donors = { "FireFlameJoe", "TheUnknownNerd", "HayWire799", "Aeolus" };
        private readonly string[] artists = { "Ochette", "Kyou", "Delly", "e³", "Bence", "TheStormQueen", "Imonthatgudkush" };
        private readonly string[] programmers = { "CrafterDaemon", "Patrick1234", "Frogleader", "Akira", "WardrobeHummus", "Ropro0923" };
        private readonly string[] otherDevelopers = { "TelosRyu", "Deeno", "Moon_Ditch" };

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
            Item.value = Item.buyPrice(0, 2, 0, 0);
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.createTile = ModContent.TileType<FateoftheGodsTile>();
            Item.Calamity().donorItem = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (!Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))
                return;

            string tooltip = "";

            // Donors
            tooltip += $"\n[c/96FF96:{Language.GetTextValue("Mods.RagnarokMod.Items.FateoftheGods.Donors")}]:\n";
            tooltip += string.Join(", ", donors);

            // Artists
            tooltip += $"\n[c/FFFF0A:{Language.GetTextValue("Mods.RagnarokMod.Items.FateoftheGods.Artists")}]:\n";
            tooltip += string.Join(", ", artists);

            // Programmers
            tooltip += $"\n[c/FF2864:{Language.GetTextValue("Mods.RagnarokMod.Items.FateoftheGods.Programmers")}]:\n";
            tooltip += string.Join(", ", programmers);

            // Other Developers
            tooltip += $"\n[c/9696FF:{Language.GetTextValue("Mods.RagnarokMod.Items.FateoftheGods.OtherDevelopers")}]:\n";
            tooltip += string.Join(", ", otherDevelopers);

            TooltipLine line = tooltips.FirstOrDefault(t => t.Mod == "Terraria" && t.Name == "Tooltip2");
            if (line != null)
                line.Text = tooltip;
        }
    }
}