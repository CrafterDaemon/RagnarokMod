using CalamityMod;
using CalamityMod.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Buffs;
using RagnarokMod.Projectiles;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ElementalReaper : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 230;
            scytheSoulCharge = 2;
            base.Item.width = 73;
            base.Item.height = 88;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ItemRarityID.Purple;
            base.Item.shoot = ModContent.ProjectileType<ElementalReaperPro>();
            base.Item.shootSpeed = 0.1f;
        }
        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            RagnarokModPlayer ragnarokplayer = player.GetRagnarokModPlayer();
            if (player.altFunctionUse == 2 && ragnarokplayer.elementalReaperCD == 0)
            {
                Projectile.NewProjectileDirect(source, position, Vector2.Zero, ModContent.ProjectileType<ElementalBuff>(), damage, knockback, player.whoAmI);
                player.AddBuff(ModContent.BuffType<ElementalScytheCooldown>(), 1200);
                ragnarokplayer.elementalReaperCD = 1200;
            }
            else if (player.altFunctionUse != 2)
            {
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
                Vector2 vel1 = new Vector2(10f, 0f);
                Projectile.NewProjectileDirect(source, position, vel1, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
                Vector2 vel2 = new Vector2(0f, 10f);
                Projectile.NewProjectileDirect(source, position, vel2, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
                Vector2 vel3 = new Vector2(-10f, 0f);
                Projectile.NewProjectileDirect(source, position, vel3, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
                Vector2 vel4 = new Vector2(0f, -10f);
                Projectile.NewProjectileDirect(source, position, vel4, ModContent.ProjectileType<ElementalReaperPro2>(), damage, knockback, player.whoAmI);
            }


            return false;
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TerraScythe>());
            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddIngredient(ModContent.ItemType<LifeAlloy>(), 5);
            recipe.AddIngredient(ModContent.ItemType<GalacticaSingularity>(), 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
