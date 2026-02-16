using CalamityMod.Items.Materials;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Items.Lodestone;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.Geode;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Donate;

namespace RagnarokMod.Common.ModSystems
{
    public class ThoriumOverrides : ModSystem
    {
        public override void AddRecipes()
        {
            Recipe recipe_bloodorb = Recipe.Create(ModContent.ItemType<BloodOrb>(), 1);
            recipe_bloodorb.AddIngredient(ModContent.ItemType<UnholyShards>(), 4);
            recipe_bloodorb.Register();

            Recipe recipe_unholyshard = Recipe.Create(ModContent.ItemType<UnholyShards>(), 1);
            recipe_unholyshard.AddIngredient(ModContent.ItemType<BloodOrb>(), 2);
            recipe_unholyshard.AddTile(TileID.Anvils);
            recipe_unholyshard.Register();

            // Tweak recipes for hardmode ore rework
            // All of these items use tile 134 by default; switch them to Anvils
            GetRecipe finder = new();

            // Valadium
            int[] valadiumItems = {
                ModContent.ItemType<ValadiumAxe>(),
                ModContent.ItemType<ValadiumBattleAxe>(),
                ModContent.ItemType<ValadiumBow>(),
                ModContent.ItemType<ValadiumBreastPlate>(),
                ModContent.ItemType<ValadiumFoeBlaster>(),
                ModContent.ItemType<ValadiumGreaves>(),
                ModContent.ItemType<ValadiumHammer>(),
                ModContent.ItemType<ValadiumHelmet>(),
                ModContent.ItemType<ValadiumPickaxe>(),
                ModContent.ItemType<ValadiumSlicer>(),
                ModContent.ItemType<ValadiumSpear>(),
                ModContent.ItemType<ValadiumStaff>(),
            };

            // Lodestone
            int[] lodestoneItems = {
                ModContent.ItemType<LodeStonePickaxe>(),
                ModContent.ItemType<LodeStoneGreatAxe>(),
                ModContent.ItemType<LodeStoneHammer>(),
                ModContent.ItemType<LodeStoneClaymore>(),
                ModContent.ItemType<LodeStoneBreaker>(),
                ModContent.ItemType<LodestoneJavelin>(),
                ModContent.ItemType<LodeStoneBow>(),
                ModContent.ItemType<LodeStoneQuickDraw>(),
                ModContent.ItemType<LodeStoneStaff>(),
                ModContent.ItemType<LodestoneRadio>(),
                ModContent.ItemType<LodeStoneFaceGuard>(),
                ModContent.ItemType<LodeStoneChestGuard>(),
                ModContent.ItemType<LodeStoneShinGuards>(),
            };

            // Flesh
            int[] fleshItems = {
                ModContent.ItemType<FleshMask>(),
                ModContent.ItemType<FleshBody>(),
                ModContent.ItemType<FleshLegs>(),
                ModContent.ItemType<FleshJackhammer>(),
                ModContent.ItemType<FleshPickAxe>(),
                ModContent.ItemType<FleshDrill>(),
                ModContent.ItemType<FleshAxe>(),
                ModContent.ItemType<FleshChainSaw>(),
                ModContent.ItemType<FleshHammer>(),
                ModContent.ItemType<ToothOfTheConsumer>(),
                ModContent.ItemType<FleshMace>(),
                ModContent.ItemType<FleshSkewer>(),
                ModContent.ItemType<FleshBow>(),
                ModContent.ItemType<BloodBelcher>(),
                ModContent.ItemType<BloodClotStaff>(),
                ModContent.ItemType<FleshHorn>(),
                ModContent.ItemType<FleshWings>(),
            };

            // Geode
            int[] geodeItems = {
                ModContent.ItemType<GeodePickaxe>(),
                ModContent.ItemType<CrystalGeode>(),
                ModContent.ItemType<GeodeSaxophone>(),
                ModContent.ItemType<CrystalArrow>(),
                ModContent.ItemType<SawbladeLight>(),
                ModContent.ItemType<GeodeGatherer>(),
                ModContent.ItemType<GeodeHamaxe>(),
                ModContent.ItemType<GeodeHelmet>(),
                ModContent.ItemType<GeodeGreaves>(),
                ModContent.ItemType<GeodeChestplate>(),
                ModContent.ItemType<Xylophone>(),
                ModContent.ItemType<GlitteringScepter>(),
                ModContent.ItemType<CrystalWish>(),
            };

            RecipeTileHelper.SwitchTilesForItems(finder, valadiumItems, 1, 134, TileID.Anvils);
            RecipeTileHelper.SwitchTilesForItems(finder, lodestoneItems, 1, 134, TileID.Anvils);
            RecipeTileHelper.SwitchTilesForItems(finder, fleshItems, 1, 134, TileID.Anvils);
            RecipeTileHelper.SwitchTilesForItems(finder, geodeItems, 1, 134, TileID.Anvils);

            // UnfathomableFlesh uses stack 3
            RecipeTileHelper.SwitchTilesForItems(finder, new[] { ModContent.ItemType<UnfathomableFlesh>() }, 3, 134, TileID.Anvils);
        }
    }
}
