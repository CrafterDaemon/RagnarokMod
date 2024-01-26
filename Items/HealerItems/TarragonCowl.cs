using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod.Items.Armor.Auric;
using CalamityMod.CalPlayer.Dashes;
using CalamityMod;
using CalamityMod.Items;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using System;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using ThoriumMod.Buffs.Bard;
using RagnarokMod.Utils;

namespace RagnarokMod.Items.HealerItems
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonCowl : ModItem
    {
        

        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.PreventBeardDraw[this.Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.defense = 34; //132
            Item.rare = ModContent.RarityType<Violet>();
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
            player.setBonus = this.GetLocalizedValue("SetBonus");
            var modPlayer = player.Calamity();
            modPlayer.tarraSet = true;
            modPlayer.bloodflareSet = true;
            modPlayer.godSlayer = true;
            modPlayer.auricSet = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetRagnarokModPlayer().auricBardSet = true;



            if (modPlayer.godSlayerDashHotKeyPressed || player.dashDelay != 0 && modPlayer.LastUsedDashID == GodslayerArmorDash.ID)
                modPlayer.DeferredDashID = GodslayerArmorDash.ID;
        }

        public override void UpdateEquip(Player player)
        {
            var modPlayer = player.Calamity();
            modPlayer.auricBoost = true;
            player.moveSpeed += 0.05f;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.bardResourceMax2 += 15;
            thoriumPlayer.bardBounceBonus += 2;
            thoriumPlayer.armInspirator = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<SoloistHat>().
                AddIngredient<AuricBar>(12).
                AddTile(TileID.LunarCraftingStation).
                Register();
        }
    }
}