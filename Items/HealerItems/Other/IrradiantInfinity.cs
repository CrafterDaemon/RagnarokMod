using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Sounds;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

namespace RagnarokMod.Items.HealerItems.Other
{
    public class IrradiantInfinity : ThoriumItem
    {
        public override void SetStaticDefaults()
        {
            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(6, 8)); // 6 ticks per frame, 4 frames
            ItemID.Sets.AnimatesAsSoul[Type] = true; // loops the animation
        }
        public override void SetDefaults()
        {
            isHealer = true;
            Item.damage = 1550;
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;
            Item.knockBack = 2f;
            Item.rare = ItemRarityID.Pink;
            Item.shoot = ModContent.ProjectileType<IrradiantRed>();
            Item.shootSpeed = 18f;
            Item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
            Item.UseSound = RagnarokModSounds.none;
            Item.noUseGraphic = true;
            radiantLifeCost = 16;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Block Blue spawn if Blue or Purple already exists
                foreach (Projectile p in Main.projectile)
                {
                    if (p.active && p.owner == player.whoAmI &&
                        (p.type == ModContent.ProjectileType<IrradiantBlue>() ||
                         p.type == ModContent.ProjectileType<IrradiantPurple>()))
                        return false;
                }

                Item.UseSound = RagnarokModSounds.none;
                Item.shoot = ModContent.ProjectileType<IrradiantBlue>();
                Item.shootSpeed = 0f;
                Item.mana = 200;
            }
            else
            {
                Item.UseSound = RagnarokModSounds.Red;
                Item.shoot = ModContent.ProjectileType<IrradiantRed>();
                Item.shootSpeed = 18f;
                Item.mana = 10;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                // Spawn Blue at player center, with velocity zero. it will seek cursor itself
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, ModContent.ProjectileType<IrradiantBlue>(), damage, knockback, player.whoAmI);
                return false;
            }
            return true;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);

            spriteBatch.Draw(texture, position, frame, new Color(255, 80, 80) * 0.6f, 0f, origin, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = TextureAssets.Item[Type].Value;
            Rectangle frame = Main.itemAnimations[Type] != null
                ? Main.itemAnimations[Type].GetFrame(texture)
                : texture.Bounds;
            Vector2 origin = frame.Size() / 2f;

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, new Color(255, 80, 80) * 0.6f, rotation, origin, scale, SpriteEffects.None, 0f);

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RuinousSoul>(), 15)
                .AddIngredient(ModContent.ItemType<Necroplasm>(), 15)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
