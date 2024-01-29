using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Projectiles.Scythe;
using Terraria.ID;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro;

namespace RagnarokMod.Items.HealerItems
{
    public class MarbleScythe : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 18;
            scytheSoulCharge = 2;
            base.Item.width = 54;
            base.Item.height = 42;
            base.Item.value = Item.sellPrice(0, 0, 27);
            base.Item.rare = ItemRarityID.Orange;
            base.Item.shoot = ModContent.ProjectileType<MarbleScythePro>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<EnchantedMarble>(8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
