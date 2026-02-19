using CalamityMod.Rarities;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Items.Armor.Bloodflare;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;
using Terraria.ID;

namespace RagnarokMod.Items.BardItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodflareHeadBard : BardItem
    {

        public override void SetBardDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityPureGreenBuyPrice;
            Item.defense = 25;
            Item.rare = ModContent.RarityType<PureGreen>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<BloodflareBodyArmor>() && legs.type == ModContent.ItemType<BloodflareCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            CalamityPlayer calamityPlayer = player.Calamity();
            calamityPlayer.bloodflareSet = true;
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<BloodflareBodyArmor>("CommonSetBonus");
            player.crimsonRegen = true;
            player.GetRagnarokModPlayer().bloodflareBard = true;
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
            player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 12f;
            thoriumPlayer.inspirationRegenBonus += 0.15f;
            thoriumPlayer.bardResourceMax2 += 6;
            thoriumPlayer.bardResourceDropBoost += 0.15f;
        }

        public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
        {
            empPool.Add<AttackSpeed>(4);
            empPool.Add<Damage>(4);
            empPool.Add<MovementSpeed>(4);
            empPool.Add<LifeRegeneration>(4);
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<BloodstoneCore>(11).AddIngredient<RuinousSoul>(2)
                .AddTile(TileID.LunarCraftingStation)
                .Register();
        }
    }
}
