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
using CalamityMod.Items;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Rarities;

namespace RagnarokMod.Items.HealerItems.Other
{
	public class Virusprayer : ThoriumItem
	{
		int numProjectiles = 0;

		public override void SetDefaults()
		{
			base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
			base.Item.damage = 284;
			this.isHealer = true;
			this.healDisplay = true;
			this.radiantLifeCost = 8;
			base.Item.width = 56;
			base.Item.mana = 10;
			base.Item.height = 40;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
			base.Item.rare = ModContent.RarityType<PureGreen>();
			base.Item.UseSound = new SoundStyle?(SoundID.Item8);
			base.Item.shoot = ModContent.ProjectileType<VirusprayerPro1>();
			base.Item.shootSpeed = 20f;

		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override Vector2? HoldoutOffset() {
			return new Vector2(-6f, -2f);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			numProjectiles = Main.rand.Next(3, 6);
			for (int i = 0; i < numProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
				newVelocity *= Main.rand.NextFloat(0.75f, 1.3f);
				Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
			}
			numProjectiles = Main.rand.Next(1, 3);
			for (int i = 0; i < numProjectiles; i++)
			{
				Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(10));
				newVelocity *= Main.rand.NextFloat(1.1f, 1.5f);
				Projectile.NewProjectileDirect(source, position, newVelocity, ModContent.ProjectileType<VirusprayerPro2>(), damage, knockback, player.whoAmI);
			}
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
			target.AddBuff(ModContent.BuffType<SulphuricPoisoning>(), 150);
		}
	}
}
