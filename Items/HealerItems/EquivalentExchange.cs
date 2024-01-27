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

namespace RagnarokMod.Items.HealerItems
{
	public class EquivalentExchange : ThoriumItem
	{
		/*
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Equivalent Exchange");
			base.Tooltip.SetDefault("Sends out a bolt of life stealing blood\nRight click to send out a medium ranged healing pulse\nThis spell will heal twice");
		}
		*/

		public override void SetDefaults()
		{
			base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
			base.Item.damage = 18;
			this.isHealer = true;
			this.healDisplay = true;
			this.radiantLifeCost = 2;
			this.healType = HealType.LifeSteal;
			this.healAmount = 4;
			base.Item.width = 30;
			base.Item.mana = 6;
			base.Item.height = 30;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.value = Item.sellPrice(0, 0, 80, 0);
			base.Item.rare = 3;
			base.Item.UseSound = new SoundStyle?(SoundID.Item8);
			base.Item.shoot = ModContent.ProjectileType<BloodTransfusionPro>();
			base.Item.shootSpeed = 9f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		
		/*
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (Main.HoverItem != base.Item && player.altFunctionUse == 2)
			{
				mult = 0.5f;
			}
		}
		*/
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse == 2)
			{
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.5f, velocity.Y * 1.5f, ModContent.ProjectileType<TheGoodBookPro>(), 0, 0f, player.whoAmI, 0f, 0f);
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X * 1.5f, velocity.Y * 1.5f, ModContent.ProjectileType<TheGoodBookPro>(), 0, 0f, player.whoAmI, 0f, 0f);
			} 
			else 
			{
				Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			}
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<LeechBolt>());
			recipe.AddIngredient(ModContent.ItemType<TheGoodBook>());
			//recipe.AddIngredient(ModContent.ItemType<RecoveryWand>());
			recipe.AddIngredient(ItemID.CrimtaneBar, 4);
			recipe.AddIngredient(ItemID.Vertebrae, 4);
			recipe.AddIngredient(ModContent.ItemType<BloodSample>(), 12);
			recipe.AddTile(16);
			recipe.Register();
		}
	}
}
