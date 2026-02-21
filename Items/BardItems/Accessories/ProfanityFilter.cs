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
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;

namespace RagnarokMod.Items.BardItems.Accessories
{
    public class ProfanityFilter : BardItem
    {
        public override void SetBardDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.rare = ModContent.RarityType<Turquoise>();
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            if (player.HeldItem.ModItem is BardItem bardItem && bardItem.InstrumentType == BardInstrumentType.Electronic)
            {
                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) -= 0.1f;
                player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.5f;
                thoriumPlayer.bardBuffDuration -= 60;
            }
            else
                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.15f;
            player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 10f;
            thoriumPlayer.bardResourceMax2 += 4;
            thoriumPlayer.accAutoTuner = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient<AutoTuner>(1)
                .AddIngredient<UnholyEssence>(10)
                .AddTile<ProfanedCrucible>()
                .Register();
        }
    }
}
