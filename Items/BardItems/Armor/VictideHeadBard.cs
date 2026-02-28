using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor.Victide;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using ThoriumMod;

namespace RagnarokMod.Items.BardItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VictideHeadBard : ModItem
    {
        private readonly Mod calamity = ModLoader.GetMod("CalamityMod");
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
            player.setBonus = this.GetLocalizedValue("SetBonus") + "\n" + CalamityUtils.GetTextValueFromModItem<VictideBreastplate>("CommonSetBonus");
            player.Calamity().victideSet = true;
            player.ignoreWater = true;
            if (Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, false))
            {
                player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.1f;
                player.lifeRegen += 3;
            }
            player.aggro -= 200;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.05f;
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
