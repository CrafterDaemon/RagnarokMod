using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using RagnarokMod.Items.Materials;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossTheGrandThunderBird;

namespace RagnarokMod
{
    public class BossProgressionFix : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe.Create(ModContent.ItemType<GrandFlareGun>())
                .AddRecipeGroup(RecipeGroupID.IronBar, 5)
                .AddIngredient<StormlionMandible>()
                .AddTile(TileID.Anvils)
                .AddCondition(Condition.ZenithWorld)
                .Register();

            Recipe.Create(ModContent.ItemType<StormFlare>())
                .AddRecipeGroup(RecipeGroupID.IronBar, 1)
                .AddIngredient<StormlionMandible>()
                .AddIngredient(ItemID.FallenStar, 1)
                .AddTile(TileID.Anvils)
                .AddCondition(Condition.ZenithWorld)
                .Register();

            GetRecipe getter = new GetRecipe();
            getter.LookFor(ModContent.ItemType<JellyfishResonator>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new RecipeHelper(item);
                helper.Add(ModContent.ItemType<PearlShard>(),1);
            }
            getter.LookFor(ModContent.ItemType<DesertMedallion>(), 1);
            foreach (Recipe item in getter.Search())
            {
                RecipeHelper helper = new RecipeHelper(item);
                helper.Add(ModContent.ItemType<StormFeather>(), 2);
            }

        }

        public override void PostAddRecipes()
        {
        }
    }
}
