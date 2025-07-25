using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Systems;
using CalamityMod;
using Ragnarok.Items;
using RagnarokMod.Items;
using RagnarokMod.Items.BardItems;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RagnarokMod.Items.BardItems.Armor;
using RagnarokMod.Items.HealerItems.Armor;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.ModSystems
{
    public class OtherModsCompat : ModSystem
    {
        public override void PostAddRecipes()
        {
			if(ModLoader.TryGetMod("CalamityBardHealer", out Mod CalamityBardHealer))				
			{
				if(ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Off) 
				{
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AerospecBard>(),"AerospecHeadphones");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AerospecHealer>(),"AerospecBiretta");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AuricTeslaFrilledHelmet>(),"AuricTeslaFeatheredHeadwear");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<AuricTeslaHealerHead>(),"AuricTeslaValkyrieVisage");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<BloodflareHeadHealer>(),"BloodflareRitualistMask");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<BloodflareHeadBard>(),"BloodflareSirenSkull");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<DaedalusHeadHealer>(),"DaedalusCowl");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<DaedalusHeadBard>(),"DaedalusHat");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<GodSlayerHeadBard>(),"GodSlayerDeathsingerCowl");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<SilvaHeadHealer>(),"SilvaGuardianHelmet");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<StatigelHeadBard>(),"StatigelEarrings");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<StatigelHeadHealer>(),"StatigelFoxMask");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<TarragonShroud>(),"TarragonChapeau");
					ExchangeRecipe(CalamityBardHealer, ModContent.ItemType<TarragonCowl>(),"TarragonParagonCrown");
				}
				else if (ModContent.GetInstance<ModCompatConfig>().item_deduplication_mode == CalamityBardHealer_selection_mode.Ragnarok) 
				{
					RemoveOtherModRecipe(CalamityBardHealer,"AerospecHeadphones");
					RemoveOtherModRecipe(CalamityBardHealer,"AerospecBiretta");
					RemoveOtherModRecipe(CalamityBardHealer,"AugmentedAuricTeslaFeatheredHeadwear");
					RemoveOtherModRecipe(CalamityBardHealer,"AugmentedAuricTeslaValkyrieVisage");
					RemoveOtherModRecipe(CalamityBardHealer,"AuricTeslaFeatheredHeadwear");
					RemoveOtherModRecipe(CalamityBardHealer,"AuricTeslaValkyrieVisage");
					RemoveOtherModRecipe(CalamityBardHealer,"BloodflareRitualistMask");
					RemoveOtherModRecipe(CalamityBardHealer,"BloodflareSirenSkull");
					RemoveOtherModRecipe(CalamityBardHealer,"DaedalusCowl");
					RemoveOtherModRecipe(CalamityBardHealer,"DaedalusHat");
					RemoveOtherModRecipe(CalamityBardHealer,"GodSlayerDeathsingerCowl");
					RemoveOtherModRecipe(CalamityBardHealer,"IntergelacticCloche");
					RemoveOtherModRecipe(CalamityBardHealer,"IntergelacticProtectorHelm");
					RemoveOtherModRecipe(CalamityBardHealer,"SilvaGuardianHelmet");
					RemoveOtherModRecipe(CalamityBardHealer,"StatigelEarrings");
					RemoveOtherModRecipe(CalamityBardHealer,"StatigelFoxMask");
					RemoveOtherModRecipe(CalamityBardHealer,"TarragonChapeau");
					RemoveOtherModRecipe(CalamityBardHealer,"TarragonParagonCrown");
				}
				else 
				{
					RemoveOwnRecipe(ModContent.ItemType<AerospecBard>());
					RemoveOwnRecipe(ModContent.ItemType<AerospecHealer>());
					RemoveOwnRecipe(ModContent.ItemType<AuricTeslaFrilledHelmet>());
					RemoveOwnRecipe(ModContent.ItemType<AuricTeslaHealerHead>());
					RemoveOwnRecipe(ModContent.ItemType<BloodflareHeadHealer>());
					RemoveOwnRecipe(ModContent.ItemType<BloodflareHeadBard>());
					RemoveOwnRecipe(ModContent.ItemType<DaedalusHeadHealer>());
					RemoveOwnRecipe(ModContent.ItemType<DaedalusHeadBard>());
					RemoveOwnRecipe(ModContent.ItemType<GodSlayerHeadBard>());
					RemoveOwnRecipe(ModContent.ItemType<SilvaHeadHealer>());
					RemoveOwnRecipe(ModContent.ItemType<StatigelHeadBard>());
					RemoveOwnRecipe(ModContent.ItemType<StatigelHeadHealer>());
					RemoveOwnRecipe(ModContent.ItemType<TarragonShroud>());
					RemoveOwnRecipe(ModContent.ItemType<TarragonCowl>());
				}
			}
        }

		public void ExchangeRecipe(Mod othermod, int ritemtype, string oitemname) 
		{
			Recipe recipe_ragnarok_to_othermod = Recipe.Create(othermod.Find<ModItem>(oitemname).Type, 1);
			recipe_ragnarok_to_othermod.AddIngredient(ritemtype, 1).AddTile(26); 
			recipe_ragnarok_to_othermod.Register();
			
			Recipe recipe_othermod_to_ragnarok = Recipe.Create(ritemtype, 1);
			recipe_othermod_to_ragnarok.AddIngredient(othermod.Find<ModItem>(oitemname).Type, 1).AddTile(26);
			recipe_othermod_to_ragnarok.Register();
		}
		
		public void RemoveOwnRecipe(int ritemtype) 
		{
			GetRecipe finder = new();
			finder.LookFor(ritemtype, 1);
			foreach (Recipe item in finder.Search())
			{
				RecipeHelper helper = new(item);
				helper.Disable();
			}
		}
		
		public void RemoveOtherModRecipe(Mod othermod, string oitemname) 
		{
			GetRecipe finder = new();
			finder.LookFor(othermod.Find<ModItem>(oitemname).Type, 1);
			foreach (Recipe item in finder.Search())
			{
				RecipeHelper helper = new(item);
				helper.Disable();
			}
		}
		
        public void Tileswitcher( Recipe recipe ,int tileold, int tilenew) 
		{
			recipe.RemoveTile(tileold);
			recipe.AddTile(tilenew);
		}
    }
}
