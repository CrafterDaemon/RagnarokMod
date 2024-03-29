using System;
using RagnarokMod.Utils;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Armor.Daedalus;
using CalamityMod.Rarities;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.BardItems
{
	[AutoloadEquip(EquipType.Head)]
	public class DaedalusHeadBard : BardItem
	{
		public override void SetBardDefaults()
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
			player.GetRagnarokModPlayer().daedalusBard = true;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.08f;
		}

		public override void UpdateEquip(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.12f;
			player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 5f;
			thoriumPlayer.bardResourceMax2 += 2;
		}

		public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
		{
			empPool.Add<AttackSpeed>(1);
			empPool.Add<Damage>(1);
		}
		
		public override void ModifyEmpowerment(ThoriumPlayer player, ThoriumPlayer target, byte type, ref byte level, ref short duration)
		{
			duration += 600;
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<CryonicBar>(7).AddIngredient<EssenceofEleum>(1)
				.AddTile(134)
				.Register();
		}
	}
}
