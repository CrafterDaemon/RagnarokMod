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

namespace RagnarokMod.Items.BardItems.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class IntergelacticRobohelm : BardItem
    {
        public override void SetBardDefaults()
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
            Item.defense = 24;
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
            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
            player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 20f;
            player.GetAttackSpeed(ThoriumDamageBase<BardDamage>.Instance) += 0.2f;
            thoriumPlayer.inspirationRegenBonus += 0.15f;
            thoriumPlayer.bardResourceMax2 += 6;
            thoriumPlayer.bardResourceDropBoost += 0.15f;
            thoriumPlayer.bardBuffDuration += 180;
        }

        public override void UpdateArmorSet(Player player)
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                Item item = new Item();
                item.SetDefaults(CatalystMod.Find<ModItem>("IntergelacticHeadMelee").Type);
                item.DamageType = ThoriumDamageBase<BardDamage>.Instance;
                if (item.ModItem != null)
                {
                    ModItem modItem = item.ModItem;
                    if (modItem != null && player.GetRagnarokModPlayer().asteroidexhaustioncounter == 0)
                    {
                        modItem.UpdateArmorSet(player);
                        player.setBonus = player.setBonus.Replace(" four ", " two ");
                        player.setBonus = player.setBonus + "\nHitting an enemy with an asteroid has an 5% chance to apply a random empowerment on level 1 to 3\nPressing the 'Armor Ability' key will exhaust the asteroids to replenish 100 inspiration and apply all resource empowerments on level 4";
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
                    player.GetRagnarokModPlayer().intergelacticBard = true;
                }
            }
        }

        public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool)
        {
            empPool.Clear();
            if (player.GetRagnarokModPlayer().asteroidexhaustioncounter > 0)
            {
                empPool.Add<ResourceRegen>((byte)4);
                empPool.Add<ResourceMaximum>((byte)4);
                empPool.Add<EmpowermentProlongation>((byte)4);
                empPool.Add<ResourceGrabRange>((byte)4);
                empPool.Add<ResourceConsumptionChance>((byte)4);
                return;
            }
            switch (player.GetRagnarokModPlayer().intergelacticBardcurrentemp)
            {
                case 1:
                    empPool.Add<AttackSpeed>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 2:
                    empPool.Add<CriticalStrikeChance>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 3:
                    empPool.Add<Damage>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 4:
                    empPool.Add<FlatDamage>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 5:
                    empPool.Add<AquaticAbility>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 6:
                    empPool.Add<FlightTime>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 7:
                    empPool.Add<JumpHeight>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 8:
                    empPool.Add<MovementSpeed>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 9:
                    empPool.Add<DamageReduction>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 10:
                    empPool.Add<Defense>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 11:
                    empPool.Add<InvincibilityFrames>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 12:
                    empPool.Add<LifeRegeneration>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 13:
                    empPool.Add<ResourceMaximum>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 14:
                    empPool.Add<ResourceConsumptionChance>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 15:
                    empPool.Add<ResourceGrabRange>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 16:
                    empPool.Add<ResourceRegen>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                case 17:
                    empPool.Add<EmpowermentProlongation>((byte)player.GetRagnarokModPlayer().intergelacticBardcurrentemplevel);
                    break;
                default:
                    break;
            }
        }

        public override void AddRecipes()
        {
            if (ModLoader.TryGetMod("CatalystMod", out Mod CatalystMod))
            {
                CreateRecipe(1).AddIngredient<StatigelHeadBard>(1).AddIngredient(CatalystMod.Find<ModItem>("MetanovaBar").Type, 6)
               .AddTile(TileID.LunarCraftingStation)
               .Register();
            }
        }
    }
}
