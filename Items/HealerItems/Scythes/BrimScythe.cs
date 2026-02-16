using CalamityMod.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

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
            base.Item.useStyle = ItemUseStyleID.Swing;
            base.Item.UseSound = SoundID.Item1;
            base.Item.shootSpeed = 0.1f;
            base.Item.damage = 40;
            base.Item.width = 72;
            base.Item.height = 82;
            base.Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            base.Item.rare = ItemRarityID.Pink;
            base.Item.shoot = ModContent.ProjectileType<BrimScythePro>();
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<BrimScytheFirePro>(), (int)(1.25f * damage), knockback, player.whoAmI);
            return true;
        }
    }
}
