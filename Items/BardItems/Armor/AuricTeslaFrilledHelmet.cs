using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using CalamityMod.Items;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using Terraria.ID;
using RagnarokMod.Utils;
using Terraria.Localization;

namespace RagnarokMod.Items.BardItems.Armor
{
    //I love auric tesla armor
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaFrilledHelmet : BardItem
    {
        public static readonly int Damage = 20;

        public static readonly int Crit = 10;

        public static readonly int InspirationRegen = 30;

        public static readonly int WindHomingSpeed = 20;
        public override LocalizedText DisplayName => ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing") ? this.GetLocalization("DisplayNameEclipse") : base.DisplayName;

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
        }
        public override void SetBardDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityVioletBuyPrice;
            Item.defense = 30; //132
            Item.rare = ModContent.RarityType<BurnishedAuric>();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AuricTeslaBodyArmor>() && legs.type == ModContent.ItemType<AuricTeslaCuisses>();
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
                player.setBonus = this.GetLocalizedValue("SetBonus");
            else
                player.setBonus = this.GetLocalizedValue("SetBonusHelheim");

            var modPlayer = player.Calamity();

            if (ModLoader.HasMod("ThoriumRework"))
                modPlayer.tarraSet = true;

            modPlayer.bloodflareSet = true;
            modPlayer.godSlayer = true;
            modPlayer.auricSet = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;

            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
                player.GetRagnarokModPlayer().auricBardSet = true;
            else
                player.GetRagnarokModPlayer().tarraBard = true;

            player.GetRagnarokModPlayer().godslayerBard = true;
            player.GetRagnarokModPlayer().bloodflareBard = true;

            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
        }

        public override void UpdateEquip(Player player)
        {
            //var modPlayer = player.Calamity();
            //modPlayer.auric = true; removed in Cal 2.1
            player.moveSpeed += 0.05f;

            player.GetRagnarokModPlayer().auricBoost = true;

            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.bardResourceMax2 += 15;
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
            player.GetAttackSpeed((DamageClass)(object)ThoriumDamageBase<BardDamage>.Instance) += Damage / 100f;
            player.GetCritChance((DamageClass)(object)ThoriumDamageBase<BardDamage>.Instance) += Crit;
            thoriumPlayer.inspirationRegenBonus += InspirationRegen / 100f;
            thoriumPlayer.bardHomingSpeedBonus += WindHomingSpeed / 100f;
            thoriumPlayer.bardBounceBonus += 2;
            thoriumPlayer.armInspirator = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            if (!ModLoader.HasMod("ThoriumRework") || ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
            {
                recipe.AddIngredient<SoloistHat>();
                recipe.AddIngredient<InspiratorsHelmet>();
            }
            else
            {
                recipe.AddIngredient<TarragonShroud>();
            }

            recipe.AddIngredient<BloodflareHeadBard>();
            recipe.AddIngredient<GodSlayerHeadBard>();

            if (ModLoader.HasMod("InfernalEclipseAPI") || ModLoader.HasMod("WHummusMultiModBalancing"))
                recipe.AddIngredient<ShadowspecBar>(12);
            else
                recipe.AddIngredient<AuricBar>(12);

            recipe.AddTile<CosmicAnvil>();
            recipe.Register();
        }
    }
}