using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Aerospec;
using CalamityMod.Items.Materials;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.BardItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class AerospecBard : ModItem
    {
        public override void SetStaticDefaults()
        {

        }

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
            player.moveSpeed += 0.07f;
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.06f;
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.05f;
            thoriumPlayer.inspirationRegenBonus += 0.05f;
            thoriumPlayer.bardBuffDuration += 120;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<AerialiteBar>(5).AddIngredient(ItemID.SunplateBlock, 3)
            .AddIngredient(ItemID.Feather, 1)
            .AddTile(TileID.SkyMill)
            .Register();
        }
    }
}
