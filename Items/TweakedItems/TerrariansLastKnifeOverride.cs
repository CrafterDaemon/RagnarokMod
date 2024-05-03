using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Omni;
using ThoriumMod.Items.BossThePrimordials.Slag;
using ThoriumMod.Projectiles;
using ThoriumMod.Rarities;
using ThoriumMod.Utilities;
using ThoriumMod.Items;
using ThoriumMod;
using RagnarokMod;
using RagnarokMod.Utils;

namespace RagnarokMod.Items.TweakedItems
{
	// Reimplements the TerrariansLastKnife with tweaked lifesteal
    [LegacyName(new string[] { "TerrariansKnife" })]
    public class TerrariansLastKnifeOverride : ThoriumItem
    {
		public override string Texture => "ThoriumMod/Items/Donate/TerrariansLastKnife";
      
        public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}

       public override void SetDefaults()
		{
			base.Item.damage = 280;
			base.Item.DamageType = DamageClass.Melee;
			base.Item.width = 40;
			base.Item.height = 40;
			base.Item.scale = 1.75f;
			base.Item.useTime = 8;
			base.Item.useAnimation = 8;
			base.Item.autoReuse = true;
			base.Item.useStyle = 1;
			base.Item.crit = 16;
			base.Item.knockBack = 7f;
			base.Item.value = Item.sellPrice(0, 25, 0, 0);
			base.Item.rare = DonatorRarity.Get(ModContent.RarityType<BloodOrangeRarity>());
			base.Item.UseSound = new SoundStyle?(SoundID.Item1);
			base.Item.shoot = ModContent.ProjectileType<TerrariansLastKnifePro>();
			base.Item.shootSpeed = 16f;
			this.isCheat = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.altFunctionUse != 2 || player.statLife > (int)((float)player.statLifeMax2 * 0.05f);
		}
		
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.altFunctionUse != 2 && target.IsHostile(null, false) && player.statLife < player.statLifeMax2)
			{
				player.HealLife(1 + (int)(Math.Pow(damageDone/30, 0.33)), null, true, true);
				for (int i = 0; i < 15; i++)
				{
					int num = Dust.NewDust(player.position, player.width, player.height, 235, 0f, 0f, 0, default(Color), 1f);
					Main.dust[num].noGravity = true;
					Main.dust[num].velocity *= 0.75f;
					int num2 = Main.rand.Next(-55, 56);
					int num3 = Main.rand.Next(-55, 56);
					Dust dust = Main.dust[num];
					dust.position.X = dust.position.X + (float)num2;
					Dust dust2 = Main.dust[num];
					dust2.position.Y = dust2.position.Y + (float)num3;
					Main.dust[num].velocity.X = -(float)num2 * 0.075f;
					Main.dust[num].velocity.Y = -(float)num3 * 0.075f;
				}
			}
		}
		
		public override float UseSpeedMultiplier(Player player)
		{
			if (player.altFunctionUse != 2)
			{
				return 1f;
			}
			return 0.2f;
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x002AF9E8 File Offset: 0x002ADBE8
		public override void UseAnimation(Player player)
		{
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
			if (player.altFunctionUse != 2)
			{
				thoriumPlayer.lastKnifeGlow = true;
				base.Item.UseSound = new SoundStyle?(SoundID.Item1);
				return;
			}
			thoriumPlayer.lastKnifeGlow = false;
			base.Item.UseSound = new SoundStyle?(SoundID.Item74);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (player.altFunctionUse != 2)
			{
				return false;
			}
			Vector2 vector = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + vector, 0, 0))
			{
				position += vector;
			}
			CombatText.NewText(player.getRect(), CombatTextHelper.DamagedFriendlyReduced, (int)((float)player.statLifeMax2 * 0.05f), false, false);
			player.statLife -= (int)((float)player.statLifeMax2 * 0.05f);
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, (int)((float)damage * 3f), knockback * 0.25f, player.whoAmI, 0f, 0f, 0f);
			return false;
		}

		public override void AddRecipes()
		{
			Recipe donatorRecipe = ThoriumItem.GetDonatorRecipe(() => ThoriumConfigServer.Instance.donatorWeapons.toggleTerrariansLastKnife, base.Type, 1);
			donatorRecipe.AddIngredient(3106, 1);
			donatorRecipe.AddIngredient(ModContent.ItemType<InfernoEssence>(), 3);
			donatorRecipe.AddIngredient(ModContent.ItemType<DeathEssence>(), 3);
			donatorRecipe.AddIngredient(ModContent.ItemType<OceanEssence>(), 3);
			donatorRecipe.AddIngredient(3467, 20);
			donatorRecipe.AddIngredient(3459, 26);
			donatorRecipe.AddIngredient(3456, 26);
			donatorRecipe.AddIngredient(3457, 26);
			donatorRecipe.AddIngredient(3458, 26);
			donatorRecipe.AddTile(412);
			donatorRecipe.Register();
		}
    }
}
