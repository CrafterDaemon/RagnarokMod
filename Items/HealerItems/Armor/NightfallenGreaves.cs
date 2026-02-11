using System;
using RagnarokMod.Utils;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Armor.Daedalus;
using CalamityMod.Rarities;
using ThoriumMod;
using ThoriumMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Placeables.Ores;

namespace RagnarokMod.Items.HealerItems.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class NightfallenGreaves : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 26;
			base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
			base.Item.rare = ModContent.RarityType<PureGreen>();
			base.Item.defense = 22;
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}

		public override void UpdateEquip(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.25f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.0f;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 8f;
			player.manaCost *= 0.8f;
		
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<Lumenyl>(7).AddIngredient<ExodiumCluster>(150).AddIngredient<RuinousSoul>(7)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
