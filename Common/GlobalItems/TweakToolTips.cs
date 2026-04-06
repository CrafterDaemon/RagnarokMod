using Microsoft.Xna.Framework;
using RagnarokMod.ChatTags;
using RagnarokMod.ILEditing;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using ThoriumMod.Buffs;
using ThoriumMod.NPCs;

namespace RagnarokMod.Common.GlobalItems
{
    public class TweakToolTips : GlobalItem
    {
        private static Mod calamity = ModLoader.GetMod("CalamityMod");

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            int lastTooltipIndex = -1;
            for (int i = 0; i < tooltips.Count; i++)
                if (tooltips[i].Name.StartsWith("Tooltip"))
                    lastTooltipIndex = i;

            var buffIdsInTooltip = new HashSet<int>();
            foreach (var tooltip in tooltips)
            {
                var snippets = ChatManager.ParseMessage(tooltip.Text, Color.White);
                foreach (var snippet in snippets)
                {
                    if (snippet is RagnarokBuffTagHandler.Snippet buffSnippet)
                        buffIdsInTooltip.Add(buffSnippet.BuffId);
                }
            }

            if (buffIdsInTooltip.Count == 0 || lastTooltipIndex == -1)
                return;

            bool foundDebuff = false;
            bool showHint = false;

            foreach (int buffId in buffIdsInTooltip)
            {
                string tooltipKey = buffId < BuffID.Count
                    ? $"Mods.Terraria.Buffs.{BuffID.Search.GetName(buffId)}.ItemTooltip"
                    : $"Mods.{BuffLoader.GetBuff(buffId).Mod.Name}.Buffs.{BuffLoader.GetBuff(buffId).Name}.ItemTooltip";

                if (!Language.Exists(tooltipKey) || string.IsNullOrWhiteSpace(Language.GetTextValue(tooltipKey)))
                    continue;

                foundDebuff = true;

                if (!PlayerInput.Triggers.Current.SmartCursor)
                {
                    showHint = true;
                    break;
                }

                tooltips.Add(
                    new TooltipLine(Mod, "RagnarokMod:AltExpand" + buffId, $"[rbuff:{buffId}]\n{Language.GetTextValue(tooltipKey)}"));
            }

            if (showHint)
            {
                var key = PlayerInput.CurrentProfile.InputModes[InputMode.Keyboard]
                    .KeyStatus["SmartCursor"].First().ToString();
                var hint = new TooltipLine(Mod, "RagnarokMod:AltHint", $"Hold {key} to see buff information");
                hint.OverrideColor = new Color(170, 170, 170);
                tooltips.Add(hint);
            }
            else if (foundDebuff)
            {
                foreach (var t in tooltips)
                    if (t.Name.Contains("Tooltip") && !t.Name.Contains("AltExpand"))
                        t.Hide();
            }
            if (item.type == calamity.Find<ModItem>("TheCommunity").Type)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("Power"))
                    {
                        tooltips[i].Text = System.Text.RegularExpressions.Regex.Replace(
                        tooltips[i].Text,
                        @"\d+% Power",
                        $"{Math.Round(CalamityEdits.calculateCommunityPower() * 100)}" + Language.GetTextValue("Mods.RagnarokMod.Compat.Power")
                        );
                    }
                }
            }
			
            if (item.type == calamity.Find<ModItem>("EldritchSoulArtifact").Type)
            {
				for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("Boosts"))
                    {
                        tooltips[i].Text += "\nIncreases maximum inspiration by 2 and heal bonus by 2";
                    }
                }
			}
        }
    }
}