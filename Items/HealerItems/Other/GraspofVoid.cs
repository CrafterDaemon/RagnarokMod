using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ThoriumMod.Items.Misc;
using ThoriumMod.Rarities;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.BossMini;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Utilities;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Other;
using CalamityMod.Items.Placeables;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Rarities;

namespace RagnarokMod.Items.HealerItems.Other
{
	public class GraspofVoid : ThoriumItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
		}
		

		public override void SetDefaults()
		{
			base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
			base.Item.damage = 420;
			this.isHealer = true;
			this.healDisplay = true;
			this.radiantLifeCost = 4;
			base.Item.width = 44;
			base.Item.mana = 12;
			base.Item.height = 58;
			base.Item.scale = 1.2f;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.autoReuse = true;
			base.Item.value = Item.sellPrice(0, 1, 0, 0);
			base.Item.rare = ModContent.RarityType<PureGreen>();
			base.Item.UseSound = new SoundStyle?(SoundID.Item8);
			base.Item.shoot = ModContent.ProjectileType<GraspofVoidPro1>();
			base.Item.shootSpeed = 20f;
		}
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 6);
			recipe.AddIngredient(ModContent.ItemType<DarkPlasma>(), 6);
			recipe.AddIngredient(ModContent.ItemType<PaganGrasp>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
	}
}
