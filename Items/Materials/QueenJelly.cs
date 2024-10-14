using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using CalamityMod.Items;

namespace RagnarokMod.Items.Materials
{
    public class QueenJelly : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.FoodParticleColors[Item.type] = new Color[3] {
                new Color(255, 0, 255),
                new Color(175, 0, 175),
                new Color(255, 80, 210)
            };

            ItemID.Sets.IsFood[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed2, 5 * 60 * 60);
            Item.width = 64;
            Item.height = 64;
            Item.maxStack = 9999;
            Item.value = CalamityGlobalItem.RarityLightRedBuyPrice;
            Item.rare = ItemRarityID.LightRed;
            Item.potion = true;
        }
        public override void OnConsumeItem(Player player)
        {
            player.Heal(50);
        }

        public override void AddRecipes()
        {
            CreateRecipe(4)
                .AddIngredient<QueenJelly>(1)
                .AddIngredient(ItemID.BottledHoney, 3)
                .AddIngredient(ItemID.PinkGel, 9)
                .AddTile(TileID.Bottles)
                .Register();
        }
    }
}
