using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using ThoriumMod.Items.HealerItems;
using Terraria.ID;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using ThoriumMod;
using CalamityMod.Items;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class BrimScythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            scytheSoulCharge = 1;
            isHealer = true;
            base.Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            base.Item.noMelee = true;
            base.Item.noUseGraphic = true;
            base.Item.autoReuse = true;
            base.Item.useTime = 20;
            base.Item.useAnimation = 20;
            base.Item.maxStack = 1;
            base.Item.knockBack = 6.5f;
            base.Item.useStyle = 1;
            base.Item.UseSound = SoundID.Item1;
            base.Item.shootSpeed = 0.1f;
            base.Item.damage = 80;
            base.Item.width = 144;
            base.Item.height = 164;
			base.Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
			base.Item.rare = 5;
            base.Item.shoot = ModContent.ProjectileType<BrimScythePro>();
        }
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			 Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<BrimScytheFirePro>(), (int)(1.25f * damage), knockback, player.whoAmI);
			 return true;
		}
    }
}
