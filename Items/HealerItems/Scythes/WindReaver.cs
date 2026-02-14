using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class WindReaver : ScytheItem
    {
        public int counter = 0;

        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 36;
            scytheSoulCharge = 2;
            base.Item.width = 40;
            base.Item.height = 38;
            base.Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<WindReaverPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            if (counter == 2)
            {
                counter = 0;
                Vector2 velocity2 = player.Center - Main.MouseWorld;
                Vector2 vel = velocity2.SafeNormalize(Vector2.UnitX);
                float multiplier = -20f;

                Projectile.NewProjectileDirect(source, position, vel * multiplier, ModContent.ProjectileType<WindSlashPro>(), damage, knockback, player.whoAmI);
            }
            counter += 1;


            return false;

        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            scale = 0.5f;
            var texture = TextureAssets.Item[Item.type].Value;
            float scaledHeight = texture.Height * scale;
            var position = Item.TopLeft - Main.screenPosition;
            position.Y += Item.height - scaledHeight;
            spriteBatch.Draw(texture, position, null, lightColor, rotation, Vector2.Zero, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SunplateBlock, 8);
			recipe.AddIngredient(ModContent.ItemType<AerialiteBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
