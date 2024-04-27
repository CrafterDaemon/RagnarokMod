using CalamityMod.Items.Accessories;
using CalamityMod.Items.Accessories.Wings;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Systems;
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
            ModContent.GetInstance<FaceMelter>().CreateRecipe()
                .AddIngredient<FaceMelterOverride>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
            ModContent.GetInstance<FaceMelterOverride>().CreateRecipe()
                .AddIngredient<FaceMelter>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
            ModContent.GetInstance<AnahitasArpeggio>().CreateRecipe()
                .AddIngredient<AnahitasArpeggioOverride>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
            ModContent.GetInstance<AnahitasArpeggioOverride>().CreateRecipe()
                .AddIngredient<AnahitasArpeggio>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
            ModContent.GetInstance<BelchingSaxophone>().CreateRecipe()
                .AddIngredient<BelchingSaxophoneOverride>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();
            ModContent.GetInstance<BelchingSaxophoneOverride>().CreateRecipe()
                .AddIngredient<BelchingSaxophone>(1)
                .AddCondition(Condition.InGraveyard)
                .AddTile(TileID.Anvils)
                .Register();

            //initialize the recipe fetcher
            GetRecipe finder = new();
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
        }

    }
}
