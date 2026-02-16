using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RagnarokMod.Items.HealerItems.Scythes;
using RagnarokMod.Projectiles.HealerPro.Scythes;
using RagnarokMod.Utils;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items.HealerItems;
using ThoriumMod.Items.Terrarium;
using ThoriumMod.Items.Titan;

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
            base.Item.damage = 100;
            scytheSoulCharge = 4;
            base.Item.useTime = 100;
            base.Item.useAnimation = 100;
            base.Item.autoReuse = false;
            base.Item.reuseDelay = 50;
            base.Item.width = 47;
            base.Item.height = 53;
            base.Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            base.Item.rare = ModContent.RarityType<BurnishedAuric>();
            base.Item.shoot = ModContent.ProjectileType<ExecutionerMark05Pro>();

        }


        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            return DrawHelper.DrawItemInWorldScaled(Item, spriteBatch, lightColor, ref rotation, ref scale, 0.75f);
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
