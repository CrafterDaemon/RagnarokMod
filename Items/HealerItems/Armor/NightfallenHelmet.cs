using RagnarokMod.Utils;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
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
    [AutoloadEquip(EquipType.Head)]
    public class NightfallenHelmet : ThoriumItem
    {
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;

        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.rare = ModContent.RarityType<PureGreen>();
            Item.defense = 18;
            isDarkHealer = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<NightfallenBreastplate>() && legs.type == ModContent.ItemType<NightfallenGreaves>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.healBonus += 4;
            thoriumPlayer.darkAura = true;
            player.GetRagnarokModPlayer().nightfallen = true;
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.33f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.66f;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 16f;
            player.statManaMax2 += 60;
            player.manaCost -= 0.2f;

        }

        public override void AddRecipes()
        {
            base.CreateRecipe(1).AddIngredient<Lumenyl>(5).AddIngredient<ExodiumCluster>(100).AddIngredient<RuinousSoul>(5)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
