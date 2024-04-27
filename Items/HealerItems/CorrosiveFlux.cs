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
using RagnarokMod.Projectiles.HealerPro;
using CalamityMod.Items.Placeables;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;

namespace RagnarokMod.Items.HealerItems
{
	public class CorrosiveFlux : ThoriumItem
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
			base.Item.damage = 24;
			this.isHealer = true;
			this.healDisplay = true;
			this.radiantLifeCost = 3;
			base.Item.width = 30;
			base.Item.mana = 8;
			base.Item.height = 30;
			base.Item.useTime = 30;
			base.Item.useAnimation = 30;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.value = Item.sellPrice(0, 0, 80, 0);
			base.Item.rare = 2;
			base.Item.UseSound = new SoundStyle?(SoundID.Item8);
			base.Item.shoot = ModContent.ProjectileType<CorrosiveFluxPro>();
			base.Item.shootSpeed = 12f;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(-20));
			Projectile.NewProjectile(source, position.X, position.Y, newVelocity.X, newVelocity.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			Vector2 newVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(0));
			Projectile.NewProjectile(source, position.X, position.Y, newVelocity2.X, newVelocity2.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			Vector2 newVelocity3 = velocity.RotatedBy(MathHelper.ToRadians(20));
			Projectile.NewProjectile(source, position.X, position.Y, newVelocity3.X, newVelocity3.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = Recipe.Create(Item.type);
			recipe.AddIngredient(ModContent.ItemType<Acidwood>(), 30);
			recipe.AddIngredient(ModContent.ItemType<SulphuricScale>(), 12);
			recipe.AddTile(16);
			recipe.Register();
		}
		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			target.AddBuff(ModContent.BuffType<Irradiated>(), 60);
		}
	}
}
