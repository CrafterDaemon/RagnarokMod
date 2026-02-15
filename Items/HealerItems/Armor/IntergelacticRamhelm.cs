using System;
using RagnarokMod.Utils;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Rarities;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Utilities;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace RagnarokMod.Items.HealerItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class IntergelacticRamhelm : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                Item.rare = CatalystMod.Find<ModRarity>("SuperbossRarity").Type;
                Item.value = 1500000;
            }
            else
            {
                Item.value = 1;
                Item.rare = ItemRarityID.Blue;
            }
            Item.defense = 18;
        }

        public override bool IsLoadingEnabled(Mod mod)
        {
            return ModLoader.HasMod("CatalystMod");
        }

        public override void SetStaticDefaults()
        {
            base.Item.ResearchUnlockCount = 1;
            ArmorIDs.Head.Sets.PreventBeardDraw[base.Item.headSlot] = true;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                return body.type == CatalystMod.Find<ModItem>("IntergelacticBreastplate").Type && legs.type == CatalystMod.Find<ModItem>("IntergelacticGreaves").Type;
            }
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.endurance += 0.12f;
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetDamage(DamageClass.Generic) -= 0.4f;
            player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.7f;
            player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 15f;
            player.GetAttackSpeed(ThoriumDamageBase<HealerDamage>.Instance) += 0.15f;
            player.manaCost *= 0.9f;
            player.statManaMax2 += 60;
            thoriumPlayer.healBonus += 5;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                Item item = new Item();
                item.SetDefaults(CatalystMod.Find<ModItem>("IntergelacticHeadMelee").Type);
                item.DamageType = ThoriumDamageBase<HealerDamage>.Instance;
                if (item.ModItem != null)
                {
                    ModItem modItem = item.ModItem;
                    if (modItem != null && player.GetRagnarokModPlayer().asteroidexhaustioncounter == 0)
                    {
                        modItem.UpdateArmorSet(player);
                        player.setBonus = player.setBonus.Replace(" four ", " two ");
                        player.setBonus = player.setBonus + "\nHitting an enemy with an asteroid has an 5% chance to drop a heart\nPressing the 'Armor Ability' key will use up the asteroids to replenish 50 health and grants you a lifeshield of 100";
                    }
                    if (player.GetRagnarokModPlayer().asteroidexhaustioncounter != 0)
                    {
                        player.setBonus = player.setBonus + "\nThe power of your asteroids is currently exhausted";
                    }
                    player.noKnockback = true;
                    player.buffImmune[32] = true;
                    player.buffImmune[33] = true;
                    player.buffImmune[46] = true;
                    player.buffImmune[47] = true;
                    player.buffImmune[156] = true;
                    player.buffImmune[31] = true;
                    player.buffImmune[197] = true;
                    player.buffImmune[ModContent.BuffType<GlacialState>()] = true;
                    player.buffImmune[CatalystMod.Find<ModBuff>("AstralBlight").Type] = true;
                    player.GetRagnarokModPlayer().intergelacticHealer = true;
                }
            }
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                CreateRecipe(1).AddIngredient<StatigelHeadHealer>(1).AddIngredient(CatalystMod.Find<ModItem>("MetanovaBar").Type, 6)
               .AddTile(TileID.LunarCraftingStation)
               .Register();
            }
        }
    }
}
