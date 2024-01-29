using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Systems;
using Ragnarok.Items;
using RagnarokMod.Items;
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

namespace RagnarokMod.Common.GlobalItems
{
    public class CalamityOverrides : GlobalItem
    {
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            //fetch me their SOULS
            return item.ModItem is FaceMelter or AnahitasArpeggio or BelchingSaxophone;
        }
        public override void AddRecipes()
        {
            //initialize the recipe fetcher
            GetRecipe finder = new();
            //go locate FaceMelter recipe
            finder.LookFor(ModContent.ItemType<FaceMelter>(), 1);

            //Disable original face melter recipe
            foreach (Recipe item in finder.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }

            foreach (Recipe item in finder.SearchByIngredient())
            {
                RecipeHelper helper = new(item);
                helper.Remove(ModContent.ItemType<FaceMelter>());
                helper.Add(ModContent.ItemType<FaceMelterOverride>(), 1);
            }
            foreach (Recipe item in finder.SearchByIngredient())
            {
                RecipeHelper helper = new(item);
                helper.Remove(ModContent.ItemType<AnahitasArpeggio>());
                helper.Add(ModContent.ItemType<AnahitasArpeggioOverride>(), 1);
            }
            foreach (Recipe item in finder.SearchByIngredient())
            {
                RecipeHelper helper = new(item);
                helper.Remove(ModContent.ItemType<BelchingSaxophone>());
                helper.Add(ModContent.ItemType<BelchingSaxophoneOverride>(), 1);
            }

            finder.LookFor(ModContent.ItemType<TerrariumParticleSprinters>(), 1);
            foreach (Recipe item in finder.Search())
            {
                RecipeHelper helper = new(item);
                helper.Remove(ItemID.TerrasparkBoots);
            }
            finder.LookFor(ModContent.ItemType<TracersCelestial>(), 1);
            foreach (Recipe item in finder.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<TerrariumParticleSprinters>(), 1);
            }

            //ew multiple leather recipes
            finder.LookFor(ItemID.Leather, 1);
            foreach (Recipe item in finder.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }
            //replacement recipes
            SimpleRecipeHandler handler = new();
            handler.SimpleRecipe(ItemID.Leather, ItemID.RottenChunk, 2, TileID.WorkBenches);
            handler.SimpleRecipe(ItemID.Leather, ItemID.Vertebrae, 2, TileID.WorkBenches);
            //thorium potions
            handler.SimpleRecipe(ModContent.ItemType<AquaPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<ArcanePotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<ArtilleryPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<AssassinPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<BloodPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<BouncingFlamePotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<ConflagrationPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<CreativityPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<EarwormPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<FrenzyPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<GlowingPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<HolyPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<HydrationPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<InspirationReachPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<KineticPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
            handler.SimpleRecipe(ModContent.ItemType<WarmongerPotion>(), ItemID.BottledWater, 1, ModContent.ItemType<BloodOrb>(), 10, TileID.AlchemyTable);
        }
        public override void UpdateInventory(Item item, Player player)
        {
            //no non-bard instruments
            if (item.ModItem is FaceMelter)
            {
                item.SetDefaults(ModContent.ItemType<FaceMelterOverride>());
            }
            if (item.ModItem is BelchingSaxophone)
            {
                item.SetDefaults(ModContent.ItemType<BelchingSaxophoneOverride>());
            }
            if (item.ModItem is AnahitasArpeggio)
            {
                item.SetDefaults(ModContent.ItemType<AnahitasArpeggioOverride>());
            }
        }

    }
}
