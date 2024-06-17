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
using ThoriumMod.Items.ArcaneArmor;
using CalamityMod.Rarities;
using ThoriumMod.Items.Donate;


namespace RagnarokMod.Items.HealerItems.Other
{
	public class VerdurantBloom : ThoriumItem
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
    	this.Item.DamageType = (DamageClass) ThoriumDamageBase<HealerDamage>.Instance;
    	this.Item.mana = 10;
    	this.Item.damage = 165;
    	this.healType = HealType.Ally;
    	this.healAmount = 5;
    	this.healDisplay = true;
    	this.isHealer = true;
    	this.Item.width = 34;
        this.Item.height = 42;
    	this.Item.useTime = 22;
    	this.Item.useAnimation = 22;
    	this.Item.noUseGraphic = true;
    	this.Item.useStyle = 1;
    	this.Item.noMelee = true;
    	this.Item.knockBack = 4f;
    	this.Item.value = Item.sellPrice(0, 16, 40, 0);
    	this.Item.rare = ModContent.RarityType<Turquoise>();
    	this.Item.UseSound = new SoundStyle?(SoundID.Item32);
    	this.Item.autoReuse = true;
    	this.Item.shoot = ModContent.ProjectileType<VerdurantBloomPro>();
    	this.Item.shootSpeed = 22f;
    }

    public override bool Shoot(
      Player player,
      EntitySource_ItemUse_WithAmmo source,
    	Vector2 position,
    	Vector2 velocity,
    	int type,
    	int damage,
    	float knockback)
    {
    	Vector2 newVelocity = velocity.RotatedBy(MathHelper.ToRadians(-21));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity.X, newVelocity.Y, ModContent.ProjectileType<VerdurantBloomPro2>(), damage, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity2 = velocity.RotatedBy(MathHelper.ToRadians(-14));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity2.X, newVelocity2.Y, ModContent.ProjectileType<VerdurantBloomPro3>(), 0, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity3 = velocity.RotatedBy(MathHelper.ToRadians(-7));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity3.X, newVelocity3.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity4 = velocity.RotatedBy(MathHelper.ToRadians(0));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity4.X, newVelocity4.Y, ModContent.ProjectileType<VerdurantBloomPro2>(), damage, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity5 = velocity.RotatedBy(MathHelper.ToRadians(7));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity5.X, newVelocity5.Y, type, damage, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity6 = velocity.RotatedBy(MathHelper.ToRadians(14));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity6.X, newVelocity6.Y, ModContent.ProjectileType<VerdurantBloomPro3>(), 0, knockback, player.whoAmI, 0f, 0f);
		Vector2 newVelocity7 = velocity.RotatedBy(MathHelper.ToRadians(21));
		Projectile.NewProjectile(source, position.X, position.Y, newVelocity7.X, newVelocity7.Y, ModContent.ProjectileType<VerdurantBloomPro2>(), damage, knockback, player.whoAmI, 0f, 0f);
		return false;
    }
	public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<BalanceBloom>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
	}
}
