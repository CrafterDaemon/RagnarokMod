using Terraria;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using ThoriumMod;
using ThoriumMod.Utilities;
using Terraria.ID;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AerospecHealer : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.defense = 4;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AerospecBreastplate>() && legs.type == ModContent.ItemType<AerospecLeggings>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.Calamity().aeroSet = true;
            player.noFallDmg = true;
            player.moveSpeed += 0.1f;
            thoriumPlayer.healBonus += 2;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.1f;
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.1f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.2f;
            player.statManaMax2 += 20;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient<AerialiteBar>(5)
            .AddIngredient(ItemID.SunplateBlock, 3)
            .AddIngredient(ItemID.Feather, 1)
            .AddTile(TileID.Anvils)
            .Register();
        }
    }
}
