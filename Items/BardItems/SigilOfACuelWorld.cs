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

namespace RagnarokMod.Items.BardItems
{
	public class SigilOfACruelWorld : BardItem
	{
		public override void SetBardDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 24;
			base.Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
			base.Item.rare = 8;
			base.Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.15f;
			player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.08f;
			thoriumPlayer.inspirationRegenBonus += 0.08f;
			thoriumPlayer.bardResourceMax2 += 3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = base.CreateRecipe(1);
			recipe.AddIngredient(935, 1);
			recipe.AddIngredient(ModContent.ItemType<BandKit>(), 1);
			recipe.AddIngredient(ModContent.ItemType<ScoriaBar>(), 5);
			recipe.AddIngredient(ModContent.ItemType<AshesofCalamity>(), 15);
			recipe.AddTile(134);
			recipe.Register();
		}
	}
}