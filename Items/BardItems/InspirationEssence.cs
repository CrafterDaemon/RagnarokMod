using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.CalPlayer;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using RagnarokMod;

namespace RagnarokMod.Items.BardItems
{
  public class InspirationEssence : InspirationConsumableBase
  {
    public override int InspirationBase => 40;

    public override void SetStaticDefaults()
    {
      ItemID.Sets.ShimmerTransformToItem[this.Type] = ModContent.ItemType<InspirationGem>();
      base.SetStaticDefaults();
    }

    public override void SetBardDefaults()
    {
      base.SetBardDefaults();
      this.Item.value = Item.sellPrice(0, 10, 0, 0);
      this.Item.rare = 11;
    }

    public virtual void AddRecipes()
    {
      Recipe recipe = this.CreateRecipe(1);
      recipe.AddIngredient(ModContent.ItemType<OceanEssence>(), 1);
      recipe.AddIngredient(ModContent.ItemType<DeathEssence>(), 1);
      recipe.AddIngredient(ModContent.ItemType<InfernoEssence>(), 1);
      recipe.AddIngredient(ModContent.ItemType<InspirationFragment>(), 1);
      recipe.AddTile(412);
      recipe.Register();
    }
  }
}