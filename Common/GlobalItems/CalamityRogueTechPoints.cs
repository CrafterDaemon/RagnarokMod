using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Common.GlobalItems
{
    public class CalamityRogueTechPoints : GlobalItem
    {
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        private static Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return (item.defense > 0);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (item.defense > 0)
            {
                foreach (var compareditem in rogue_armor_add_tech_points)
                {
                    if (item.type == calamity.Find<ModItem>(compareditem.Key).Type)
                    {
                        player.GetThoriumPlayer().techPointsMax += compareditem.Value;
                        break;
                    }
                }
                if (item.type == calamity.Find<ModItem>("BloodflareHeadRogue").Type)
                {
                    player.GetThoriumPlayer().techRechargeBonus = 15;
                }
                else if (item.type == calamity.Find<ModItem>("GodSlayerHeadRogue").Type)
                {
                    player.GetThoriumPlayer().techRechargeBonus = 20;
                }
                else if (item.type == calamity.Find<ModItem>("AuricTeslaPlumedHelm").Type)
                {
                    player.GetThoriumPlayer().techRechargeBonus = 30;
                }
            }

            // Catalyst
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                if (item.type == CatalystMod.Find<ModItem>("IntergelacticHeadRogue").Type)
                {
                    player.GetThoriumPlayer().techRechargeBonus = 15;
                    player.GetThoriumPlayer().techPointsMax += 2;
                }
            }
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.defense > 0)
            {
                string tooltipText = "";

                foreach (var compareditem in rogue_armor_add_tech_points)
                {
                    if (item.type == calamity.Find<ModItem>(compareditem.Key).Type)
                    {
                        tooltipText += Language.GetTextValue("Mods.RagnarokMod.Compat.TechPoints") + " " + compareditem.Value;
                        break;
                    }
                }

                if (item.type == calamity.Find<ModItem>("BloodflareHeadRogue").Type)
                {
                    tooltipText += " " + Language.GetTextValue("Mods.RagnarokMod.Compat.Recharge") + " 15%";
                }
                else if (item.type == calamity.Find<ModItem>("GodSlayerHeadRogue").Type)
                {
                    tooltipText += " " + Language.GetTextValue("Mods.RagnarokMod.Compat.Recharge") + " 20%";
                }
                else if (item.type == calamity.Find<ModItem>("AuricTeslaPlumedHelm").Type)
                {
                    tooltipText += " " + Language.GetTextValue("Mods.RagnarokMod.Compat.Recharge") + " 30%";
                }

                // Catalyst
                if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
                {
                    if (item.type == CatalystMod.Find<ModItem>("IntergelacticHeadRogue").Type)
                    {
                        tooltipText = Language.GetTextValue("Mods.RagnarokMod.Compat.TechPoints") + " 2 " +
                                      Language.GetTextValue("Mods.RagnarokMod.Compat.Recharge") + " 15%";
                    }
                }

                if (!string.IsNullOrEmpty(tooltipText))
                {
                    int index = tooltips.FindLastIndex(x => x.Mod == "Terraria" && x.Name.StartsWith("Tooltip"));
                    if (index == -1) index = tooltips.Count - 1;

                    tooltips.Insert(index + 1, new TooltipLine(Mod, "TechPoints", tooltipText));
                }
            }
        }

        private static Dictionary<string, int> rogue_armor_add_tech_points = new Dictionary<string, int>
        {
            {"TitanHeartMask", 1},
            {"ForbiddenCirclet", 1},
            {"UmbraphileHood", 1},
            {"HydrothermicHeadRogue", 2},
            {"EmpyreanMask", 2},
            {"TarragonHeadRogue", 2},
            {"BloodflareHeadRogue", 2},
            {"GodSlayerHeadRogue", 2},
            {"AuricTeslaPlumedHelm", 2}
        };
    }
}









