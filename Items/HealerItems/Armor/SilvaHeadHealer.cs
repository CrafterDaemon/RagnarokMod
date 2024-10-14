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
using CalamityMod.Items.Armor.Silva;
using CalamityMod.Items.Placeables;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class SilvaHeadHealer : ThoriumItem
	{
		public override string LocalizationCategory
		{
			get
			{
				return "Items.Armor.PostMoonLord";
			}
		}

		public override void SetDefaults()
		{
			base.Item.width = 24;
			base.Item.height = 22;
			base.Item.value = CalamityGlobalItem.RarityDarkBlueBuyPrice;
			base.Item.defense = 23;
			base.Item.rare = ModContent.RarityType<DarkBlue>();
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<SilvaArmor>() && legs.type == ModContent.ItemType<SilvaLeggings>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			CalamityPlayer calamityPlayer = player.Calamity();
			calamityPlayer.silvaSet = true;
			//calamityPlayer.silvaMage = true;
			player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<SilvaArmor>("CommonSetBonus");
			player.GetRagnarokModPlayer().silvaHealer = true;
		}

		
		public override void UpdateEquip(Player player)
		{
			player.manaCost *= 0.75f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.7f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.25f;
			thoriumPlayer.healBonus += 9;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 20f;
			player.statManaMax2 += 80;
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<PlantyMush>(6).AddIngredient<EffulgentFeather>(5)
				.AddIngredient<AscendantSpiritEssence>(2)
				.AddTile<CosmicAnvil>()
				.Register();
		}
	}
}
