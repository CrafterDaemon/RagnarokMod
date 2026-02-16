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
    public class PhantasmalEdge : ScytheItem, ILocalizedModType
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 320;
            scytheSoulCharge = 4;
            base.Item.width = 76;
            base.Item.height = 74;
            base.Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            base.Item.rare = ModContent.RarityType<PureGreen>();
            base.Item.shoot = ModContent.ProjectileType<PhantasmalEdgePro1>();
            Item.reuseDelay = 8;
            Item.channel = true;
            Item.autoReuse = false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void HoldItem(Player player)
        {
            player.Calamity().rightClickListener = true;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            type = ModContent.ProjectileType<PhantasmalEdgePro1>();

            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<PhantasmalEdgeHook>();
                velocity *= 150;
            }
        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.5f);
        }
        public override void AddRecipes()
        {
        }
    }
}
