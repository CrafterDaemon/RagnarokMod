using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.SummonItems;
using CalamityMod.Items.TreasureBags.MiscGrabBags;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RagnarokMod.Items.Materials;
using RagnarokMod.Utils;
using RagnarokMod.Common.Configs;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossLich;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.BossThePrimordials;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Omni;
using ThoriumMod.Items.BossThePrimordials.Slag;
using ThoriumMod.Items.BossViscount;
using ThoriumMod.Items.Bronze;
using ThoriumMod.Items.Granite;
using ThoriumMod.Items.NPCItems;
using ThoriumMod.Tiles;

namespace RagnarokMod
{
    public class BossProgressionFix : ModSystem
    {
        public override void AddRecipes()
        {
            GetRecipe getter = new();
            if (ModContent.GetInstance<BossProgressionConfig>().GrandFlareGun)
            {
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
            }

            if (ModContent.GetInstance<BossProgressionConfig>().DesertMedallion)
            {
                getter.LookFor(ModContent.ItemType<DesertMedallion>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<StormFeather>(), 2);
                }
            }
            if (ModContent.GetInstance<BossProgressionConfig>().DecapoditaSprout)
            {
                getter.LookFor(ModContent.ItemType<DecapoditaSprout>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<StormFeather>(), 3);
                    helper.Add(ModContent.ItemType<PearlShard>(), 1);
                    helper.Add(ModContent.ItemType<QueenJelly>(), 3);
                }
            }

            if (ModContent.GetInstance<BossProgressionConfig>().MarbleGranite)
            {
                getter.LookFor(ModContent.ItemType<UnstableCore>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Disable();
                }
                Recipe.Create(ModContent.ItemType<UnstableCore>())
                    .AddIngredient(ItemID.GraniteBlock, 25)
                    .AddIngredient<GraniteEnergyCore>(4)
                    .AddIngredient<BloodSample>(5)
                    .AddIngredient<DraculaFang>(100)
                    .AddTile(TileID.Anvils)
                    .Register();
                Recipe.Create(ModContent.ItemType<UnstableCore>())
                    .AddIngredient(ItemID.GraniteBlock, 25)
                    .AddIngredient<GraniteEnergyCore>(4)
                    .AddIngredient<RottenMatter>(5)
                    .AddIngredient<DraculaFang>(100)
                    .AddTile(TileID.Anvils)
                    .Register();
                getter.LookFor(ModContent.ItemType<AncientBlade>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Disable();
                }
                Recipe.Create(ModContent.ItemType<AncientBlade>())
                    .AddIngredient(ItemID.MarbleBlock, 25)
                    .AddIngredient<BronzeAlloyFragments>(4)
                    .AddIngredient<BloodSample>(5)
                    .AddIngredient<DraculaFang>(100)
                    .AddTile(TileID.Anvils)
                    .Register();
                Recipe.Create(ModContent.ItemType<AncientBlade>())
                    .AddIngredient(ItemID.MarbleBlock, 25)
                    .AddIngredient<BronzeAlloyFragments>(4)
                    .AddIngredient<RottenMatter>(5)
                    .AddIngredient<DraculaFang>(100)
                    .AddTile(TileID.Anvils)
                    .Register();
            }
            if (ModContent.GetInstance<BossProgressionConfig>().OverloadedSludge)
            {
                getter.LookFor(ModContent.ItemType<OverloadedSludge>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<EmpoweredGranite>(), 5);
                    helper.Add(ModContent.ItemType<EnchantedMarble>(), 5);
                }
            }

            if (ModContent.GetInstance<BossProgressionConfig>().StarCaller)
            {
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
            }
            if (ModContent.GetInstance<BossProgressionConfig>().CryoKey)
            {
                getter.LookFor(ModContent.ItemType<CryoKey>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<StriderFang>(), 5);
                }
            }

            if (ModContent.GetInstance<BossProgressionConfig>().MechBosses)
            {
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

            if (ModContent.GetInstance<BossProgressionConfig>().CharredIdol)
            {
                getter.LookFor(ModContent.ItemType<CharredIdol>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<VoidseerPearl>(), 1);
                }
            }
            if (ModContent.GetInstance<BossProgressionConfig>().Seafood)
            {
                getter.LookFor(ModContent.ItemType<Seafood>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<QueenJelly>(), 5);
                }
            }
            if (ModContent.GetInstance<BossProgressionConfig>().EyeOfDesolation)
            {
                getter.LookFor(ModContent.ItemType<EyeofDesolation>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<VoidseerPearl>(), 1);
                }
            }
            if (ModContent.GetInstance<BossProgressionConfig>().DeathWhistle)
            {
                getter.LookFor(ModContent.ItemType<DeathWhistle>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<CursedCloth>(), 50);
                }
            }
            if (ModContent.GetInstance<BossProgressionConfig>().DoomSayerCoin)
            {
                getter.LookFor(ModContent.ItemType<DoomSayersCoin>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Add(ModContent.ItemType<EldritchShellFragment>(), 3);
                    helper.Add(ModContent.ItemType<AshesofCalamity>(), 10);
                }
            }

            if (ModContent.GetInstance<BossProgressionConfig>().RuneOfKos)
            {
                getter.LookFor(ModContent.ItemType<RuneofKos>(), 1);
                foreach (Recipe item in getter.Search())
                {
                    RecipeHelper helper = new(item);
                    helper.Disable();
                }
                Recipe.Create(ModContent.ItemType<RuneofKos>())
                    .AddIngredient(ModContent.ItemType<DivineGeode>(), 50)
                    .AddIngredient(ModContent.ItemType<InfernoEssence>(), 10)
                    .AddIngredient(ModContent.ItemType<OceanEssence>(), 10)
                    .AddIngredient(ModContent.ItemType<DeathEssence>(), 10)
                    .AddTile(TileID.LunarCraftingStation)
                    .Register();
            }
        }

        public override void PostAddRecipes()
        {
        }
    }
}
