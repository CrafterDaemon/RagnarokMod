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

namespace RagnarokMod.Items.BardItems
{
	public class CalamityBell : BigInstrumentItemBase
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;

		public override void SafeSetStaticDefaults()
		{
			this.Empowerments.AddInfo<LifeRegeneration>(1, 0);
			this.Empowerments.AddInfo<FlightTime>(1, 0);
		}

		public override void SafeSetBardDefaults()
		{
			Item.damage = 115;
			base.InspirationCost = 3;
			base.Item.width = 40;
			base.Item.height = 40;
			base.Item.useTime = 50;
			base.Item.useAnimation = 50;
			base.Item.useStyle = 1;
			base.Item.noMelee = true;
			base.Item.autoReuse = true;
			base.Item.knockBack = 2f;
			Item.value = CalamityGlobalItem.Rarity7BuyPrice;
			Item.rare = 7;
			Item.UseSound = Sounds.RagnarokModSounds.calamitybell;
			base.Item.shoot = ModContent.ProjectileType<CalamityBellPro>();
			base.Item.shootSpeed = 11f;

		}
	
		public override void Shoot_OnSuccess(Player player)
		{
			int numDusts = 40;
			for (int i = 0; i < numDusts; i++)
			{
				Vector2 offset = Terraria.Utils.RotatedBy(-Terraria.Utils.RotatedBy(Vector2.UnitY, (double)((float)i * 6.2831855f / (float)numDusts), default(Vector2)), (double)Terraria.Utils.ToRotation(player.velocity), default(Vector2));
				Dust dust = Dust.NewDustPerfect(player.Center + offset * 15f, 205, new Vector2?(offset * 6f), 0, default(Color), 1f);
				dust.scale = 1.65f;
				dust.noGravity = true;
			}
		}
		
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float bardHit = 1f + (float)success * 0.02f;
			if(success >= 15) 
			{
				Projectile.NewProjectile(source, position, velocity * bardHit, ModContent.ProjectileType<CalamityBellPro2>(), (int)(damage * 1.3f) , knockback, player.whoAmI, (float)(level - 1), 0f, 0f);
			}
			else 
			{
				Projectile.NewProjectile(source, position, velocity * bardHit, type, damage, knockback, player.whoAmI, (float)(level - 1), 0f, 0f);
			}
			
		}
					
	}
}