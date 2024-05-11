using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.CalPlayer;
using ThoriumMod;
using RagnarokMod;

namespace RagnarokMod.Items.BardItems
{
    [AutoloadEquip(EquipType.Head)]
	public class VictideHeadBard : ModItem
	{
		private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Victide Hat");
            //Tooltip.SetDefault("5% increased symphonic damage");
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 2400;
            Item.rare = 2;
            Item.defense = 2;
        }
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideGreaves>();
		}

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Enemies are less likely to target you\n+3 life regen and 10% increased symphonic damage while submerged in liquid\nWhen using any weapon you have a 10% chance to throw a returning seashell projectile\nThis seashell does true damage and does not benefit from any damage class\nProvides increased underwater mobility and slightly reduces breath loss in the abyss";
			player.Calamity().victideSet = true;
			player.ignoreWater = true;
			if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, false))
			{
				player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
				player.lifeRegen += 3;
			}
			player.aggro -= 200;
        }

        public override void UpdateEquip(Player player)
        {
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.05f;
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
