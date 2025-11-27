using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod;
using ThoriumMod;
using RagnarokMod.ILEditing;

namespace RagnarokMod.Common.GlobalItems
{
    public class TweakToolTips : GlobalItem
    {
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
		public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
			return item.type == calamity.Find<ModItem>("TheCommunity").Type;
        }
		
		public override void ModifyTooltips (Item item, List< TooltipLine > tooltips) 
		{
			if(item.type == calamity.Find<ModItem>("TheCommunity").Type) 
				{
					for (int i = 0; i < tooltips.Count; i++)
					{
						if (tooltips[i].Text.Contains("Power"))
						{
								tooltips[i].Text = System.Text.RegularExpressions.Regex.Replace(
								tooltips[i].Text,
								@"\d+% Power",
								$"{CalamityEdits.calculateCommunityPower()}% Power"
								);
						}
					}
				}
		}	
	}
}