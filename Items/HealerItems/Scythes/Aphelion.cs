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
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class Aphelion : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 1280;
            scytheSoulCharge = 5;
            base.Item.width = 224;
            base.Item.height = 282;
            base.Item.value = CalamityGlobalItem.RarityHotPinkBuyPrice;
            base.Item.rare = ModContent.RarityType<BurnishedAuric>();
            base.Item.shoot = ModContent.ProjectileType<AphelionPro>();
            base.Item.channel = true; // hold to channel
        }

        public override bool AltFunctionUse(Player player) => false;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.type == type && p.owner == player.whoAmI)
                    return false;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor,
            ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }
    }
}