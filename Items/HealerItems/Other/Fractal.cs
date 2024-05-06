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
using CalamityMod.Items.Placeables.Ores;

namespace RagnarokMod.Items.HealerItems.Other
{
	public class Fractal : ThoriumItem
	{
		public override void SetStaticDefaults()
		{
			Item.staff[Type] = true;
		}
		

		public override void SetDefaults()
		{
			base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
			base.Item.damage = 24;
			this.isHealer = true;
			this.healDisplay = true;
			base.Item.width = 60;
			base.Item.mana = 12;
			base.Item.height = 66;
			base.Item.useTime = 48;
			base.Item.useAnimation = 48;
			base.Item.autoReuse = true;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.value = Item.sellPrice(0, 1, 0, 0);
			base.Item.rare = 2;
			base.Item.UseSound = new SoundStyle?(SoundID.Item8);
			base.Item.shoot = ModContent.ProjectileType<FractalOrb>();
			base.Item.shootSpeed = 6f;
		}
		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ExodiumCluster>(), 12);
			recipe.AddIngredient(ModContent.ItemType<Lumenyl>(), 8);
			recipe.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
			recipe.AddIngredient(ModContent.ItemType<Prisma>(), 1);
            recipe.AddTile(412);
            recipe.Register();
        }
	}
}
