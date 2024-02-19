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

namespace RagnarokMod.Items.HealerItems
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodflareHeadHealer : ThoriumItem
	{
		public string LocalizationCategory
		{
			get
			{
				return "Items.Armor.PostMoonLord";
			}
		}

		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = CalamityGlobalItem.Rarity13BuyPrice;
			base.Item.defense = 23;
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
			//calamityPlayer.bloodflareMage = true;
			player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<BloodflareBodyArmor>("CommonSetBonus");
			player.crimsonRegen = true;
			player.GetRagnarokModPlayer().bloodflareHealer = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.manaCost *= 0.80f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.7f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.2f;
			thoriumPlayer.healBonus += 7;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 20f;
			player.statManaMax2 += 60;
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<BloodstoneCore>(11).AddIngredient<RuinousSoul>(2)
				.AddTile(412)
				.Register();
		}
	}
}
