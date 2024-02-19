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
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.BardItems
{
    [AutoloadEquip(EquipType.Head)]
	public class TarragonShroud : BardItem
	{
		private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
           ArmorIDs.Head.Sets.PreventBeardDraw[base.Item.headSlot] = true;
        }

        public override void SetBardDefaults()
        {
            Item.width = 12;
            Item.height = 12;
            Item.value = CalamityGlobalItem.Rarity12BuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.defense = 11;
        }

        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = "Increased heart pickup range\nEnemies have a chance to drop extra hearts on death\nAll buff duration enhancements are prolonged for additional 50%\nSet bonus of Ornate armor";
			CalamityPlayer calamityPlayer = player.Calamity();
			calamityPlayer.tarraSet = true;
			player.GetRagnarokModPlayer().tarraBard = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
			calamity.TryFind<ModItem>("TarragonBreastplate", out ModItem tarragonb);
			calamity.TryFind<ModItem>("TarragonLeggings", out ModItem tarragonl);
			return (body.type == tarragonb.Type) && (legs.type == tarragonl.Type);
        }

        public override void UpdateEquip(Player player)
        {
			player.endurance+=0.05f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.20f;
			player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 15f;
			player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.15f;
			thoriumPlayer.inspirationRegenBonus += 0.25f;
			thoriumPlayer.bardResourceMax2 += 4;
			thoriumPlayer.bardResourceDropBoost += 0.15f;	
        }
		
        public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 7);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();

        }
    }
}
