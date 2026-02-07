using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod;
using RagnarokMod.Utils;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.ExtraJumps;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using CalamityMod.Tiles.FurnitureStatigel;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
	public class StatigelHeadHealer : ThoriumItem
	{
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
			Item.rare = ItemRarityID.LightRed;
			Item.defense = 5;
        }

        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = this.GetLocalizedValue("SetBonus");
			player.Calamity().statigelSet = true;
			player.GetJumpState<StatigelJump>().Enable();
			Player.jumpHeight += 5;
			player.jumpSpeedBoost += 0.6f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.statManaMax2 += 20;
			player.manaCost *= 0.95f;
			thoriumPlayer.healBonus += 2;	
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
		}

        public override void UpdateEquip(Player player)
        {
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(DamageClass.Generic) -= 0.1f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.2f;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 7f;
        }
		
        public override void AddRecipes()
        {
			base.CreateRecipe(1).AddIngredient<PurifiedGel>(5).AddIngredient<BlightedGel>(5)
				.AddTile<StaticRefiner>()
				.Register();
        }
    }
}
