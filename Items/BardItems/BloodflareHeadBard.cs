using System;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod;
using RagnarokMod.Utils;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using CalamityMod.Items.Armor.Bloodflare;
using CalamityMod.Items.Placeables;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Items.BardItems
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodflareHeadBard : BardItem
	{
		public string LocalizationCategory
		{
			get
			{
				return "Items.Armor.PostMoonLord";
			}
		}

		public override void SetBardDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			base.Item.defense = 25;
			base.Item.rare = ModContent.RarityType<PureGreen>();
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<BloodflareBodyArmor>() && legs.type == ModContent.ItemType<BloodflareCuisses>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			CalamityPlayer calamityPlayer = player.Calamity();
			calamityPlayer.bloodflareSet = true;
			player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<BloodflareBodyArmor>("CommonSetBonus");
			player.crimsonRegen = true;
			player.GetRagnarokModPlayer().bloodflareBard = true;
		}

		public override void UpdateEquip(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
			player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 12f;
			thoriumPlayer.inspirationRegenBonus += 0.15f;
			thoriumPlayer.bardResourceMax2 += 6;
			thoriumPlayer.bardResourceDropBoost += 0.15f;	
		}
		
		public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
		{	
			empPool.Add<AttackSpeed>(4);
			empPool.Add<Damage>(4);
			empPool.Add<MovementSpeed>(4);
			empPool.Add<LifeRegeneration>(4);
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<BloodstoneCore>(11).AddIngredient<RuinousSoul>(2)
				.AddTile(412)
				.Register();
		}
	}
}
