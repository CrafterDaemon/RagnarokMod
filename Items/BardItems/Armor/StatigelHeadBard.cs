using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using CalamityMod.Items.Armor.Statigel;
using CalamityMod.ExtraJumps;
using CalamityMod.Tiles.Furniture.CraftingStations;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;


namespace RagnarokMod.Items.BardItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class StatigelHeadBard : BardItem
    {
        public override void SetBardDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 5405;
            Item.rare = 4;
            Item.defense = 5;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = this.GetLocalizedValue("SetBonus");
            player.Calamity().statigelSet = true;
            player.GetJumpState<StatigelJump>().Enable();
            Player.jumpHeight += 5;
            player.jumpSpeedBoost += 0.6f;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            thoriumPlayer.inspirationRegenBonus += 0.05f;
            thoriumPlayer.bardBuffDuration += 120;
            thoriumPlayer.bardResourceDropBoost += 0.1f;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<StatigelArmor>() && legs.type == ModContent.ItemType<StatigelGreaves>();
        }

        public override void UpdateEquip(Player player)
        {
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.10f;
            player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 7f;
        }

        public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<PurifiedGel>(5).AddIngredient<BlightedGel>(5)
                .AddTile<StaticRefiner>()
                .Register();
        }
    }
}
