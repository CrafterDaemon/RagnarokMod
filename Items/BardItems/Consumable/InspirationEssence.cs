using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Items.BossThePrimordials.Aqua;
using ThoriumMod.Items.BossThePrimordials.Slag;
using ThoriumMod.Items.BossThePrimordials.Omni;
using CalamityMod.Items;

namespace RagnarokMod.Items.BardItems.Consumable
{
    public class InspirationEssence : InspirationConsumableBase
    {
        public override int InspirationBase => 40;

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<InspirationGem>();
            base.SetStaticDefaults();
        }

        public override void SetBardDefaults()
        {
            base.SetBardDefaults();
            Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
            Item.rare = ItemRarityID.Purple;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<OceanEssence>(), 1);
            recipe.AddIngredient(ModContent.ItemType<DeathEssence>(), 1);
            recipe.AddIngredient(ModContent.ItemType<InfernoEssence>(), 1);
            recipe.AddIngredient(ModContent.ItemType<InspirationFragment>(), 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}