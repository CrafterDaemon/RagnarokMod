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
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.Consumable;
using ThoriumMod.Items.Donate;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.ThrownItems;
using ThoriumMod.Items.Valadium;
using ThoriumMod.Items.Lodestone;
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossBoreanStrider;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossThePrimordials;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Flesh;
using ThoriumMod.Items.Geode;
using ThoriumMod.Utilities;
using ThoriumMod.Buffs;

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
			GetRecipe finder = new();
            finder.LookFor(ModContent.ItemType<ValadiumAxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumBattleAxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumBow>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumBreastPlate>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumFoeBlaster>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumGreaves>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumHammer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumHelmet>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumPickaxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumSlicer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumSpear>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
            finder.LookFor(ModContent.ItemType<ValadiumStaff>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			//Lodestone
			finder.LookFor(ModContent.ItemType<LodeStonePickaxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneGreatAxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneHammer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneClaymore>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneBreaker>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodestoneJavelin>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneBow>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneQuickDraw>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneStaff>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodestoneRadio>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneFaceGuard>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneChestGuard>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<LodeStoneShinGuards>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			// Flesh
			finder.LookFor(ModContent.ItemType<FleshMask>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshBody>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshLegs>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshJackhammer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshPickAxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshDrill>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshAxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshChainSaw>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshHammer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<ToothOfTheConsumer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshMace>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshSkewer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshBow>(), 1);
            foreach (Recipe item in finder.Search()) {
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<BloodBelcher>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<BloodClotStaff>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshHorn>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<FleshWings>(), 1);
            foreach (Recipe item in finder.Search()) {
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<UnfathomableFlesh>(), 3);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			//Geode
			finder.LookFor(ModContent.ItemType<GeodePickaxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<CrystalGeode>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeSaxophone>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<CrystalArrow>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<SawbladeLight>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeGatherer>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeHamaxe>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeHelmet>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeGreaves>(), 1);
            foreach (Recipe item in finder.Search()) {
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GeodeChestplate>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<Xylophone>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<GlitteringScepter>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
			finder.LookFor(ModContent.ItemType<CrystalWish>(), 1);
            foreach (Recipe item in finder.Search()){
                Tileswitcher(item, 134, TileID.Anvils);
            }
        }

        public void Tileswitcher( Recipe recipe ,int tileold, int tilenew) {
			recipe.RemoveTile(tileold);
			recipe.AddTile(tilenew);
		}
    }
}
