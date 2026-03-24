using System;
using RagnarokMod.Utils;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Items.Armor.Daedalus;
using CalamityMod.Rarities;
using ThoriumMod;
using ThoriumMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using CalamityMod.Items.Placeables.Ores;
using ThoriumMod.Items;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class NightfallenBreastplate : ThoriumItem
    {
        public override void SetDefaults()
        {
            Item.width = 38;
            Item.height = 20;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 28;
            isHealer = true;
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.25f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.5f;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 12f;
            player.manaCost -= 0.2f;
        }

        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient<Lumenyl>(10).AddIngredient<ExodiumCluster>(200).AddIngredient<RuinousSoul>(10)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
