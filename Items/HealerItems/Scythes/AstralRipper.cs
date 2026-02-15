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
    public class AstralRipper : ScytheItem
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
            base.Item.width = 40;
            base.Item.height = 38;
            base.Item.value = CalamityGlobalItem.RarityCyanBuyPrice;
            base.Item.rare = ItemRarityID.Cyan;
            base.Item.shoot = ModContent.ProjectileType<AstralRipperPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

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
                n = projSpeed / n;
                vel.X *= n;
                vel.Y *= n * fallspeedmult;
                Projectile.NewProjectileDirect(source, vector, vel, ModContent.ProjectileType<AstralRipperStarPro>(), damage, knockback, player.whoAmI);
            }

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
            recipe.AddIngredient(ModContent.ItemType<AstralBar>(), 10);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
