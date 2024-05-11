using System;
using Microsoft.Xna.Framework;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Armor;
using CalamityMod.Items;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Armor.GodSlayer;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using RagnarokMod;
using RagnarokMod.Utils;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;

namespace RagnarokMod.Items.BardItems
{
	[AutoloadEquip(new EquipType[] { 0 })]
	public class GodSlayerHeadBard : BardItem
	{
		public override void SetBardDefaults()
		{
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = 145050;
			base.Item.defense = 22;
			base.Item.rare = ModContent.RarityType<DarkBlue>();
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<GodSlayerChestplate>() && legs.type == ModContent.ItemType<GodSlayerLeggings>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateArmorSet(Player player)
		{
			CalamityPlayer calamityPlayer = player.Calamity();
			
			string hotkey = CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString();
			player.setBonus = ILocalizedModTypeExtensions.GetLocalizedValue(this, "SetBonus") + "\n" + CalamityUtils.GetTextFromModItem<GodSlayerChestplate>("CommonSetBonus").Format(new object[]
			{
				hotkey,
				GodslayerArmorDash.GodslayerCooldown
			});
			if (calamityPlayer.godSlayerDashHotKeyPressed || (player.dashDelay != 0 && calamityPlayer.LastUsedDashID == GodslayerArmorDash.ID))
			{
				calamityPlayer.DeferredDashID = GodslayerArmorDash.ID;
				player.dash = 0;
			}
			
			calamityPlayer.godSlayer = true;
			player.GetRagnarokModPlayer().godslayerBard = true;
		}
		
		public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
		{
			empPool.Clear();
			switch(player.GetRagnarokModPlayer().godslayerBardcurrentemp) 
			{
				case 1:
				empPool.Add<AttackSpeed>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 2:
				empPool.Add<CriticalStrikeChance>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 3:
				empPool.Add<Damage>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 4:
				empPool.Add<FlatDamage>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 5:
				empPool.Add<AquaticAbility>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 6:
				empPool.Add<FlightTime>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 7:
				empPool.Add<JumpHeight>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 8:
				empPool.Add<MovementSpeed>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 9:
				empPool.Add<DamageReduction>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 10:
				empPool.Add<Defense>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 11:
				empPool.Add<InvincibilityFrames>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 12:
				empPool.Add<LifeRegeneration>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 13:
				empPool.Add<ResourceMaximum>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 14:
				empPool.Add<ResourceConsumptionChance>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 15:
				empPool.Add<ResourceGrabRange>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 16:
				empPool.Add<ResourceRegen>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				case 17:
				empPool.Add<EmpowermentProlongation>((byte)player.GetRagnarokModPlayer().godslayerBardcurrentemplevel);
				break;
				default:
				break;
			}
		}

		public override void UpdateEquip(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.25f;
			player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 15f;
			thoriumPlayer.inspirationRegenBonus += 0.15f;
			thoriumPlayer.bardResourceMax2 += 6;
			thoriumPlayer.bardResourceDropBoost += 0.15f;	
		}

		public override void AddRecipes()
		{
			base.CreateRecipe(1).AddIngredient<CosmiliteBar>(7).AddIngredient<AscendantSpiritEssence>(2)
				.AddTile<CosmicAnvil>()
				.Register();
		}
	}
}
