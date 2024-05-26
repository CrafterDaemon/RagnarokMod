using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BardItems;
using ThoriumMod;
using ThoriumMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace RagnarokMod.Items.BardItems
{
	public class UniversalHeadset : BardItem
	{
		public override void SetBardDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
			base.Item.rare = ModContent.RarityType<DarkBlue>();
			base.Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
			player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 7f;
			player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
			thoriumPlayer.inspirationRegenBonus += 0.1f;
			thoriumPlayer.bardResourceMax2 += 5;
			thoriumPlayer.accHeadset = true;
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<SigilOfACruelWorld>(1)
			.AddIngredient<Headset>(1)
			.AddIngredient(3467, 8)
			.AddIngredient<GalacticaSingularity>(4)
			.AddIngredient<AscendantSpiritEssence>(4)
			.AddTile<CosmicAnvil>()
			.Register();
		}
	}
}
