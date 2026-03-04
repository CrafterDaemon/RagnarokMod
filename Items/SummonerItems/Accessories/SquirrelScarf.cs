using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.ID;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BardItems;
using ThoriumMod;
using ThoriumMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod;
using CalamityMod.Items;

namespace RagnarokMod.Items.SummonerItems.Accessories
{
    public class SquirrelScarf : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityWhiteBuyPrice;
            Item.rare = ItemRarityID.White;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxTurrets += 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ItemID.Acorn, 5);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
