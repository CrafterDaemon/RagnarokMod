using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
	public class VictideHeadHealer : ModItem
	{
		private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
			ArmorIDs.Head.Sets.PreventBeardDraw[base.Item.headSlot] = true;
            //DisplayName.SetDefault("Victide Hood");
            //Tooltip.SetDefault("5% increased radiant damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideGreaves>();
		}

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Enemies are less likely to target you\n+3 life regen and 10% increased radiant but 10% decreased non-radiant damage while submerged in liquid\nWhen using any weapon you have a 10% chance to throw a returning seashell projectile\nThis seashell does true damage and does not benefit from any damage class\nProvides increased underwater mobility and slightly reduces breath loss in the abyss";
			player.Calamity().victideSet = true;
			player.ignoreWater = true;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, false))
			{
				player.GetDamage(DamageClass.Generic) -= 0.10f;
				player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.2f;
				player.lifeRegen += 3;
			}
			player.aggro -= 200;
        }

        public override void UpdateEquip(Player player)
        {
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.05f;
        }
		
        public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<SeaRemains>(), 3);
			recipe.AddTile(16);
			recipe.Register();
        }
    }
}
