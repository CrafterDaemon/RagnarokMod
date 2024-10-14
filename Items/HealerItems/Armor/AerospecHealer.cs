using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using ThoriumMod;
using ThoriumMod.Utilities;
using Terraria.ID;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
	public class AerospecHealer : ModItem
	{
		private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
				
        }

        public override void SetDefaults()
        {
			base.Item.width = 18;
			base.Item.height = 18;
			base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
			base.Item.rare = ItemRarityID.Orange;
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
			player.moveSpeed += 0.1f;
			thoriumPlayer.healBonus += 2;
        }

        public override void UpdateEquip(Player player)
        {
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(DamageClass.Generic) -= 0.2f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.4f;
			player.statManaMax2 += 20;
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
