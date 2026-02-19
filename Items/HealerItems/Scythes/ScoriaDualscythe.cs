using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ScoriaDualscythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 80;
            scytheSoulCharge = 3;
            base.Item.width = 57;
            base.Item.height = 58;
            base.Item.value = CalamityGlobalItem.RarityYellowBuyPrice;
            base.Item.rare = ItemRarityID.Yellow;
            base.Item.shoot = ModContent.ProjectileType<ScoriaDualscythePro>();

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public float ThrowDistance = 75f;

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.useTime = 44;        // slower right click
                Item.useAnimation = 44;
            }
            else
            {
                Item.useTime = 22;        // normal left click
                Item.useAnimation = 22;
            }

            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (Main.myPlayer == player.whoAmI)
            {
                int projIndex = -1;

                if (player.altFunctionUse == 2)
                {
                    Vector2 throwVel = Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * -ThrowDistance;

                    projIndex = Projectile.NewProjectile(
                        source,
                        position,
                        throwVel,
                        type,
                        damage + damage / 5,
                        knockback,
                        player.whoAmI,
                        (Main.rand.Next(2, 5) + 1) * 0.1f, // ai[0]
                        player.itemTime // ai[1]
                    );

                    if (projIndex >= 0)
                        NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, projIndex);

                    return false;
                }
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ScoriaBar>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
