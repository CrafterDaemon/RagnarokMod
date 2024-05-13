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
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.BardPro;
using RagnarokMod.Sounds;
using ThoriumMod.Items.BardItems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RagnarokMod.Items.BardItems
{
	public class ProfanedBell : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		
		public override void SetStaticDefaults()
		{
			this.Empowerments.AddInfo<MovementSpeed>(2, 0);
			this.Empowerments.AddInfo<FlightTime>(3, 0);
            this.Empowerments.AddInfo<JumpHeight>(2, 0);
        }

		public override void SetBardDefaults()
		{
			Item.damage = 380;
			base.InspirationCost = 3;
			base.Item.width = 65;
			base.Item.height = 65;
			base.Item.useTime = 27;
			base.Item.useAnimation = 27;
			base.Item.useStyle = ItemUseStyleID.Swing;
			base.Item.noMelee = true;
			base.Item.autoReuse = true;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(0, 25, 0);
			base.Item.rare = ModContent.RarityType<Turquoise>();
			Item.UseSound = Sounds.RagnarokModSounds.calamitybell;
			base.Item.shoot = ModContent.ProjectileType<ProfanedBellBlast>();
			base.Item.shootSpeed = 10f;

		}
		
		public override void AddRecipes()
        {
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 4);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.Register();
        }
	}
}