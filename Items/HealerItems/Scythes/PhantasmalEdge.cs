using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.HealerItems;
using CalamityMod;
using CalamityMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Projectiles.HealerPro.Other;
using RagnarokMod.Projectiles;
using CalamityMod.Items;

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
            base.Item.damage = 220;
            scytheSoulCharge = 5;
            base.Item.width = 152;
            base.Item.height = 148;
            base.Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            base.Item.rare = ItemRarityID.Red;
            base.Item.shoot = ModContent.ProjectileType<PhantasmalEdgePro1>();
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
        public override void AddRecipes()
        {
        }
    }
}
