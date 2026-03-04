using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

using CalamityMod;
using CalamityMod.Rarities;
using CalamityMod.CalPlayer;
using CalamityMod.Items;
using CalamityMod.Projectiles.Typeless;

using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

using RagnarokMod.Projectiles.CalamityOverrides;

namespace RagnarokMod.Items.HealerItems.CalamityOverrides{
    public class RelicOfConvergenceOverride : ThoriumItem, ILocalizedModType{
        public override string Texture => "CalamityMod/Items/Tools/RelicOfConvergence";
        public override void SetDefaults(){
			base.Item.width = 32;
			base.Item.height = 46;
			base.Item.useAnimation = (base.Item.useTime = 25);
			base.Item.useStyle = 5;
			base.Item.mana = 200;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.channel = true;
			base.Item.value = CalamityGlobalItem.RarityPurpleBuyPrice;
			base.Item.rare = 11;
			base.Item.shoot = ModContent.ProjectileType<RelicOfConvergenceCrystalOverride>();
			this.isHealer = true;
            this.healDisplay = true;
		}
		
		public string LocalizationCategory{
			get{
				return "Items";
			}
		}
		
		public override LocalizedText Tooltip{
			get{
				return base.Tooltip.WithFormatArgs(new object[]{
					RelicOfConvergenceOverride.HealValue
				});
			}
		}
		
		public override void SetStaticDefaults(){
			ItemID.Sets.CanBePlacedOnWeaponRacks[base.Type] = true;
		}
		public override void HoldItem(Player player){
			player.Calamity().mouseWorldListener = true;
		}
		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup){
			itemGroup = (ContentSamples.CreativeHelper.ItemGroup)820;
		}

		public override bool CanUseItem(Player player){
			return player.ownedProjectileCounts[base.Item.shoot] <= 0 && player.ownedProjectileCounts[ModContent.ProjectileType<RelicOfDeliveranceSpear>()] <= 0;
		}
        public static int HealValue = 25;
		public static int BonusHealMultiplier = 4;
		public static float IncomingDamageMultiplier = 1.5f;
		public static float DefenseMultiplier = 0.5f;
    }
}
