using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Empowerments;
using ThoriumMod.Projectiles.Bard;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.BardPro;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using RagnarokMod.Projectiles.HealerPro;
using CalamityMod.Items.Materials;


namespace RagnarokMod.Items.BardItems
{
	public class Steampipes : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		
		public override void SetStaticDefaults()
		{
			this.Empowerments.AddInfo<ResourceRegen>(3, 0);
			this.Empowerments.AddInfo<ResourceMaximum>(2, 0);
        }

		public override void SetBardDefaults()
		{
			Item.damage = 165;
			base.InspirationCost = 1;
			base.Item.width = 56;
			base.Item.height = 50;
			base.Item.useTime = 14;
			base.Item.useAnimation = 14;
			base.Item.useStyle = ItemUseStyleID.Shoot;
			base.Item.noMelee = true;
			base.Item.autoReuse = true;
			base.Item.knockBack = 2f;
			Item.value = CalamityGlobalItem.Rarity7BuyPrice;
			Item.rare = ItemRarityID.Lime;
			base.Item.shoot = ModContent.ProjectileType<SteampipesPro>();
			base.Item.shootSpeed = 13f;

		}	
		public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<ScoriaBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<Bagpipe>(), 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();

        }
	}
}