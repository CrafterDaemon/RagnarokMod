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
            return ( item.defense > 0 );
        }
		
		public override void UpdateEquip(Item item, Player player) 
		{
			if(item.defense > 0)
			{
				foreach (var compareditem in rogue_armor_add_tech_points) 
					{
						if ( item.type == calamity.Find<ModItem>(compareditem.Key).Type) 
						{
							player.GetThoriumPlayer().techPointsMax += compareditem.Value;
							break;
						}
					}
				if(item.type == calamity.Find<ModItem>("BloodflareHeadRogue").Type) 
				{
					player.GetThoriumPlayer().techRechargeBonus = 15;
				}
				else if(item.type == calamity.Find<ModItem>("GodSlayerHeadRogue").Type) 
					{
					player.GetThoriumPlayer().techRechargeBonus = 20;
				}
				else if(item.type == calamity.Find<ModItem>("AuricTeslaPlumedHelm").Type) 
				{
					player.GetThoriumPlayer().techRechargeBonus = 30;
				}
			}
			
			// Catalyst
			if(ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod)) 
				{
					if(item.type == CatalystMod.Find<ModItem>("IntergelacticHeadRogue").Type) 
					{
						player.GetThoriumPlayer().techRechargeBonus = 15;
						player.GetThoriumPlayer().techPointsMax += 2;
					}
				}		
		}
		
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if(item.defense > 0)
			{
				foreach (var compareditem in rogue_armor_add_tech_points) 
					{
						if ( item.type == calamity.Find<ModItem>(compareditem.Key).Type) 
						{
							tooltips[3].Text = tooltips[3].Text + "\nIncreases your maximum technique points by " + compareditem.Value;
							break;
						}
					}
				if(item.type == calamity.Find<ModItem>("BloodflareHeadRogue").Type) 
				{
					tooltips[3].Text = tooltips[3].Text + " and recharge by 15%";
				}
				else if(item.type == calamity.Find<ModItem>("GodSlayerHeadRogue").Type) 
				{
					tooltips[3].Text = tooltips[3].Text + " and recharge by 20%";
				}
				else if(item.type == calamity.Find<ModItem>("AuricTeslaPlumedHelm").Type) 
				{
					tooltips[3].Text = tooltips[3].Text + " and recharge by 30%";
				}
				// Catalyst
				if(ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod)) 
				{
					if(item.type == CatalystMod.Find<ModItem>("IntergelacticHeadRogue").Type) 
					{
						tooltips[3].Text = tooltips[3].Text + "\nIncreases your maximum technique points by and recharge by 15%";
					}	
				}
			} 
		}
		
		private static Dictionary<string,int> rogue_armor_add_tech_points = new Dictionary<string,int>
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









