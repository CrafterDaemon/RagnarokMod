using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using RagnarokMod.Items.Materials;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class GodSlayersReap : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            scytheSoulCharge = 6;
            Item.autoReuse = false;
            Item.channel = true;
            Item.damage = 2100;
            Item.width = 86;
            Item.height = 122;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.rare = ModContent.RarityType<CosmicPurple>();
            Item.shoot = ModContent.ProjectileType<GodSlayersReapPro1>();
        }
        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<CosmiliteBar>(12)
            .AddTile<CosmicAnvil>()
            .Register();
        }
    }
}
