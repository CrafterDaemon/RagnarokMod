using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod;
using RagnarokMod.Utils;
using RagnarokMod.Projectiles;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Materials;
using CalamityMod.CalPlayer;
using CalamityMod.Rarities;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TarragonCowl : ThoriumItem
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
            Item.value = CalamityGlobalItem.RarityTurquoiseBuyPrice;
            Item.defense = 12;
            Item.rare = ModContent.RarityType<Turquoise>();

        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            calamity.TryFind<ModItem>("TarragonBreastplate", out ModItem tarragonb);
            calamity.TryFind<ModItem>("TarragonLeggings", out ModItem tarragonl);
            return (body.type == tarragonb.Type) && (legs.type == tarragonl.Type);
        }

        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadowSubtle = true;
            player.armorEffectDrawOutlines = true;
        }

        public override void UpdateArmorSet(Player player)
        {

            player.setBonus = "Increased heart pickup range\nEnemies have a chance to drop extra hearts on death\nA Guardian healer will assist you in healing your allies\nHeals ally life equal to your bonus healing + 5 health\nand grants them the Guardian Healers Blessing for 20 seconds";
            CalamityPlayer calamityPlayer = player.Calamity();
            calamityPlayer.tarraSet = true;
            player.GetRagnarokModPlayer().tarraHealer = true;
            if (Main.myPlayer == player.whoAmI)
            {
                int type = ModContent.ProjectileType<GuardianHealer>();
                if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[type] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromThis("tarraHealer"), player.Center, Vector2.Zero, type, 0, 0f, player.whoAmI, 0f, 0f, 0f);
                }
            }
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.05f;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.6f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.0f;
            thoriumPlayer.healBonus += 6;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 15f;
            player.statManaMax2 += 60;
            player.manaCost *= 0.85f;

        }
        public override void AddRecipes()
        {
            Recipe recipe = Recipe.Create(Item.type);
            recipe.AddIngredient(ModContent.ItemType<UelibloomBar>(), 7);
            recipe.AddIngredient(ModContent.ItemType<DivineGeode>(), 6);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}
