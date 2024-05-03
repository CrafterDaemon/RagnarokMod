using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Systems;
using Ragnarok.Items;
using RagnarokMod.Items;
using RagnarokMod.Items.TweakedItems;
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
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Valadium;

namespace RagnarokMod.Common.GlobalItems
{
    public class ThoriumOverrides : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.ModItem is TerrariansLastKnife;
        }
        public override void AddRecipes()
        {
			
			GetRecipe finder = new();
			
			// Remove old version recipe
			finder.LookFor(ModContent.ItemType<TerrariansLastKnife>(), 1);
            foreach (Recipe item in finder.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }
			
			// Create a recipe to switch between versions
            ModContent.GetInstance<TerrariansLastKnife>().CreateRecipe()
                .AddIngredient<TerrariansLastKnifeOverride>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
			ModContent.GetInstance<TerrariansLastKnifeOverride>().CreateRecipe()
                .AddIngredient<TerrariansLastKnife>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();

			
			finder.LookFor(ModContent.ItemType<ValadiumAxe>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumBattleAxe>(), 1);
            foreach (Recipe item in finder.Search())
            {
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumBow>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumBreastPlate>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumFoeBlaster>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumGreaves>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumHammer>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumHelmet>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumPickaxe>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumSlicer>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumSpear>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ValadiumStaff>(), 1);
            foreach (Recipe item in finder.Search())
            {
				Tileswitcher(item, 134, TileID.Anvils);
            }				
        }
   
		public void Tileswitcher( Recipe recipe ,int tileold, int tilenew) 
		{
			recipe.RemoveTile(tileold);
			recipe.AddTile(tilenew);
		}

    }
}
