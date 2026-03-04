using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;

namespace RagnarokMod.Items.BardItems.Accessories
{
    public class RedglassMonocle : BardItem
    {
        public override void SetBardDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RagnarokModPlayer>().redglassMonocle = true;
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.25f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<RuinousSoul>(),2)
                .AddIngredient(ModContent.ItemType<TwistingNether>(),1)
                .AddIngredient(ItemID.Ruby, 10)
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}