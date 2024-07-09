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
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossBoreanStrider;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossThePrimordials;
using ThoriumMod.Utilities;
using ThoriumMod.Buffs;

namespace RagnarokMod.Common.GlobalItems
{
    public class ThoriumOverrides : GlobalItem
    {
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        public override void AddRecipes()
        {
            GetRecipe finder = new();

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

        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<StormFlare>() || item.type == ModContent.ItemType<JellyfishResonator>() || item.type == ModContent.ItemType<UnstableCore>() ||
			item.type == ModContent.ItemType<AncientBlade>() || item.type == ModContent.ItemType<StarCaller>() || item.type == ModContent.ItemType<StriderTear>() ||
			item.type == ModContent.ItemType<VoidLens>() || item.type == ModContent.ItemType<AbyssalShadow2>() || item.type == ModContent.ItemType<DoomSayersCoin>())
			{
				item.consumable = false;
			}
        }

        public void Tileswitcher( Recipe recipe ,int tileold, int tilenew) 
		{
			recipe.RemoveTile(tileold);
			recipe.AddTile(tilenew);
		}
    }
}
