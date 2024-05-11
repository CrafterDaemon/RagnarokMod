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
using ThoriumMod.Items.BossThePrimordials.Dream;
using System;
using ThoriumMod.Empowerments;
using Microsoft.Xna.Framework;
using Terraria.ID;
using CalamityMod.Buffs.StatDebuffs;
using RagnarokMod.Utils;
using RagnarokMod.Projectiles;

namespace RagnarokMod.Items.HealerItems.Armor
{
    //I love auric tesla armor
    [AutoloadEquip(EquipType.Head)]
    public class AuricTeslaHealerHead : ThoriumItem
    {
     
        public override void SetStaticDefaults()
        {
            ArmorIDs.Head.Sets.PreventBeardDraw[this.Item.headSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = 505050;
            Item.defense = 27; //132
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
            modPlayer.silvaSet = true;
            modPlayer.auricSet = true;
            player.thorns += 3f;
            player.ignoreWater = true;
            player.crimsonRegen = true;
            player.GetRagnarokModPlayer().auricHealerSet = true;
		    player.GetRagnarokModPlayer().tarraHealer = true;
			player.GetRagnarokModPlayer().silvaHealer = true;
			player.GetRagnarokModPlayer().bloodflareHealer  = true;
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
            var modPlayer = player.Calamity();
            modPlayer.auricBoost = true;
            player.moveSpeed += 0.05f;
			player.manaCost *= 0.75f;
			ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
		    player.GetDamage(DamageClass.Generic) -= 0.7f;
			player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 1.4f;
			thoriumPlayer.healBonus += 11;
			player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 25f;
			player.statManaMax2 += 100;
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<DreamWeaversHood>().
                AddIngredient<DreamWeaversHelmet>().
				AddIngredient<SilvaHeadHealer>().
				AddIngredient<BloodflareHeadHealer>().
				AddIngredient<TarragonCowl>().
                AddIngredient<AuricBar>(12).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}