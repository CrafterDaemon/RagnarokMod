using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.CalPlayer;
using ThoriumMod;
using ThoriumMod.Utilities;
using RagnarokMod;

namespace RagnarokMod.Items.BardItems
{
    [AutoloadEquip(EquipType.Head)]
	public class AerospecBard : ModItem
	{
		private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
				
        }

        public override void SetDefaults()
        {
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = CalamityGlobalItem.Rarity3BuyPrice;
			base.Item.rare = 3;
			base.Item.defense = 4;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<AerospecBreastplate>() && legs.type == ModContent.ItemType<AerospecLeggings>();
		}

		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		
        public override void UpdateArmorSet(Player player)
        {
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.setBonus = this.GetLocalizedValue("SetBonus");
			player.Calamity().aeroSet = true;
			player.noFallDmg = true;
			player.moveSpeed += 0.07f;
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.06f;
        }

        public override void UpdateEquip(Player player)
        {
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.05f;
			thoriumPlayer.inspirationRegenBonus += 0.05f;	
			thoriumPlayer.bardBuffDuration += 120;
        }
		
        public override void AddRecipes()
        {
				base.CreateRecipe(1).AddIngredient<AerialiteBar>(5).AddIngredient(824, 3)
				.AddIngredient(320, 1)
				.AddTile(305)
				.Register();
        }
    }
}
