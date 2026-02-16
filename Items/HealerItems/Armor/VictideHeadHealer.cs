using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VictideHeadHealer : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.PreventBeardDraw[base.Item.headSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = CalamityGlobalItem.RarityGreenBuyPrice;
            Item.rare = ItemRarityID.Green;
            Item.defense = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VictideBreastplate>() && legs.type == ModContent.ItemType<VictideGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = Language.GetTextValue("Mods.RagnarokMod.Items.VictideHeadHealer.SetBonus");
            player.Calamity().victideSet = true;
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, false))
            {
                player.GetDamage(DamageClass.Generic) -= 0.10f;
                player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.2f;
                player.lifeRegen += 3;
            }
            player.aggro -= 200;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.05f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<SeaRemains>(), 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
