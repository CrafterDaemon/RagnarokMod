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
    [AutoloadEquip(EquipType.Legs)]
    public class NightfallenGreaves : ThoriumItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 22;
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
            player.GetDamage(DamageClass.Generic) -= 0.2f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.4f;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 12f;
            player.manaCost -= 0.2f;
            player.moveSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient<Lumenyl>(7).AddIngredient<ExodiumCluster>(150).AddIngredient<RuinousSoul>(7)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
