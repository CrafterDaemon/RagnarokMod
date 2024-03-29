using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Scythe;
using Terraria.ID;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro;

namespace RagnarokMod.Items.HealerItems
{
    public class AstralScythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 145;
            scytheSoulCharge = 2;
            base.Item.width = 60;
            base.Item.height = 64;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ItemRarityID.Red;
            base.Item.shoot = ModContent.ProjectileType<AstralScythePro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            
            float num = velocity.Length();
            for (int i = 0; i < 2; i++)
            {
                float fallspeedmult = 50;
                float projSpeed = num * Main.rand.NextFloat(0.7f, 1.4f);
                float x = Main.MouseWorld.X + Main.rand.NextFloat(0f - 50f, 50f);
                float y = Main.MouseWorld.Y - Main.rand.NextFloat(650f, 850f);
                Vector2 vector = new Vector2(x, y);
                Vector2 vel = Main.MouseWorld - vector;
                vel.X += Main.rand.NextFloat(0f - 130f, 130f);
                vel.Y += Main.rand.NextFloat(0f - 260f, 260f);
                float n = vel.Length();
                n = projSpeed/n;
                vel.X *= n;
                vel.Y *= n*fallspeedmult;
                Projectile.NewProjectileDirect(source, vector, vel, ModContent.ProjectileType<AstralScytheStarPro>(), damage, knockback, player.whoAmI);
            }

			return false;
		}
    }
}
