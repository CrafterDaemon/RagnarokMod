using Terraria.ModLoader;
using Terraria;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace RagnarokMod.Items.HealerItems.Scythes
{
    public class ExecutionerMark05 : ScytheItem
    {
        public override void SetStaticDefaults()
        {
            SetStaticDefaultsToScythe();
        }

        public override void SetDefaults()
        {
            SetDefaultsToScythe();
            base.Item.damage = 220;
            scytheSoulCharge = 1;
            base.Item.useTime = 100;
            base.Item.useAnimation = 100;
            base.Item.autoReuse = false;
            base.Item.reuseDelay = 50;
            base.Item.width = 94;
            base.Item.height = 106;
            base.Item.value = Item.sellPrice(0, 28, 0);
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ExecutionerMark05Pro>();

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
