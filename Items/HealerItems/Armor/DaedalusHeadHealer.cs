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

namespace RagnarokMod.Items.HealerItems.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class DaedalusHeadHealer : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = CalamityGlobalItem.Rarity5BuyPrice;
			base.Item.rare = 5;
			base.Item.defense = 6;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<DaedalusBreastplate>() && legs.type == ModContent.ItemType<DaedalusLeggings>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = this.GetLocalizedValue("SetBonus");
			player.GetRagnarokModPlayer().daedalusHealer = true;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			thoriumPlayer.healBonus += 3;
		}

		public override void UpdateEquip(Player player)
		{
			player.manaCost *= 0.9f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.15f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.3f;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 7f;
			player.statManaMax2 += 40;
		
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<CryonicBar>(7).AddIngredient<EssenceofEleum>(1)
				.AddTile(134)
				.Register();
		}
	}
}
