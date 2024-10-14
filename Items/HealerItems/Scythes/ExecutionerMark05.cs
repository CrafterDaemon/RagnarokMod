using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using ThoriumMod.Items.HealerItems;
using CalamityMod.Rarities;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Items.HealerItems.Scythes;
using CalamityMod.Items.Materials;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items.Titan;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items;

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
            base.Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            base.Item.rare = ModContent.RarityType<Turquoise>();
            base.Item.shoot = ModContent.ProjectileType<ExecutionerMark05Pro>();

        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MiracleMatter>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ProfanedScythe>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TerrariumHolyScythe>(), 1);
			recipe.AddIngredient(ModContent.ItemType<RealitySlasher>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TitanScythe>(), 1);
            recipe.AddTile<DraedonsForge>();
            recipe.Register();
        }
    }
}
