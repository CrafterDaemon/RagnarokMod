using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using RagnarokMod.Items.Materials;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.NPCItems;

namespace RagnarokMod
{
    public class BossProgressionFix : ModSystem
    {
        public override void AddRecipes()
        {
            GetRecipe getter = new();
            getter.LookFor(ModContent.ItemType<GrandFlareGun>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }
            getter.LookFor(ModContent.ItemType<StormFlare>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }

            Recipe.Create(ModContent.ItemType<GrandFlareGun>())
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient<StormlionMandible>()
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ModContent.ItemType<StormFlare>())
                .AddRecipeGroup(RecipeGroupID.IronBar, 1)
                .AddIngredient<StormlionMandible>()
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .Register();

            getter.LookFor(ModContent.ItemType<JellyfishResonator>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<PearlShard>(),1);
            }
            getter.LookFor(ModContent.ItemType<DesertMedallion>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StormFeather>(), 2);
            }
            getter.LookFor(ModContent.ItemType<DecapoditaSprout>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StormFeather>(), 3);
                helper.Add(ModContent.ItemType<PearlShard>(), 1);
            }
            getter.LookFor(ModContent.ItemType<OverloadedSludge>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<EmpoweredGranite>(), 5);
                helper.Add(ModContent.ItemType<EnchantedMarble>(), 5);
            }
            getter.LookFor(ModContent.ItemType<StarCaller>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Disable();
            }
            Recipe.Create(ModContent.ItemType<StarCaller>())
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient<StrangeAlienTech>()
                .AddIngredient<BloodSample>(5)
                .AddTile(TileID.Anvils)
                .Register();

            Recipe.Create(ModContent.ItemType<StarCaller>())
                .AddIngredient(ItemID.MeteoriteBar, 10)
                .AddIngredient<StrangeAlienTech>()
                .AddIngredient<RottenMatter>(5)
                .AddTile(TileID.Anvils)
                .Register();
            getter.LookFor(ModContent.ItemType<CryoKey>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StriderFang>(), 5);
            }
            getter.LookFor(ItemID.MechanicalEye, 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StrangeAlienMotherBoard>(), 1);
            }
            getter.LookFor(ItemID.MechanicalSkull, 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StrangeAlienMotherBoard>(), 1);
            }
            getter.LookFor(ItemID.MechanicalWorm, 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new(item);
                helper.Add(ModContent.ItemType<StrangeAlienMotherBoard>(), 1);
            }
        }

        public override void PostAddRecipes()
        {
        }
    }
}
