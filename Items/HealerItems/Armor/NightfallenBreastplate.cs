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
	[AutoloadEquip(EquipType.Body)]
	public class NightfallenBreastplate : ModItem
	{
		public override void SetDefaults()
		{
			base.Item.width = 38;
			base.Item.height = 20;
			base.Item.value = 115055;
			base.Item.rare = ModContent.RarityType<PureGreen>();
			base.Item.defense = 28;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<NightfallenHelmet>() && legs.type == ModContent.ItemType<NightfallenGreaves>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = this.GetLocalizedValue("SetBonus");
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			thoriumPlayer.healBonus += 2;
		}

		public override void UpdateEquip(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.35f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.8f;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 12f;
		
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<Lumenyl>(10).AddIngredient<ExodiumCluster>(200).AddIngredient<RuinousSoul>(10)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
