using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class AuricGreatscythe : ScytheItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 450;
            scytheSoulCharge = 1;
            base.Item.width = 52;
            base.Item.height = 48;
            base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            base.Item.rare = ModContent.RarityType<BurnishedAuric>();
            base.Item.shoot = ModContent.ProjectileType<AuricGreatscythePro>();
            Item.reuseDelay = 8;
            Item.channel = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;

            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // Slow, heavy swing into lightning bolt
                Item.channel = false;
                Item.useTime = 40;
                Item.useAnimation = 40;
                Item.reuseDelay = 20;
                Item.UseSound = SoundID.Item71;
            }
            else
            {
                // Normal channeled scythe swing
                Item.channel = true;
                Item.useTime = 20;
                Item.useAnimation = 20;
                Item.reuseDelay = 8;
                Item.UseSound = null;
            }
            return base.CanUseItem(player);
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<AuricGreatscythePro>();

            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<AuricGreatscytheSwing>();
                damage = (int)(damage * 20f);
                knockback *= 2f;
                // Normalize the bolt direction and pass it through velocity
                // The swing projectile will extract this for the bolt launch
                velocity = velocity.SafeNormalize(Vector2.UnitX);
            }
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.75f);
        }
        public override void AddRecipes()
        {
        }
    }
}
