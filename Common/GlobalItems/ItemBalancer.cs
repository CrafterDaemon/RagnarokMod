using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using CalamityMod;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Items.Depths;
using ThoriumMod.Items;
using ThoriumMod.Projectiles;
using ThoriumMod.Buffs;
using RagnarokMod.Common.Configs;
using RagnarokMod.Utils;

namespace RagnarokMod.Common.GlobalItems
{
    public class ItemBalancer : GlobalItem{
		private static bool print_message=true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
	
		public override bool AppliesToEntity(Item item, bool lateInstantiation){
            return (item.damage > 0 
			|| item.defense > 0 
			|| item.type == thorium.Find<ModItem>("ShinobiSigil").Type 
			|| item.type == thorium.Find<ModItem>("PlagueLordFlask").Type 
			|| item.type == thorium.Find<ModItem>("ThrowingGuide").Type
		    || item.type == thorium.Find<ModItem>("ThrowingGuideVolume2").Type
			|| item.type == thorium.Find<ModItem>("ThrowingGuideVolume3").Type
			|| item.type == thorium.Find<ModItem>("TheOmegaCore").Type)
			|| isHealerItem(item);
        }
		
		public bool isHealerItem(Item item) {
			if (item.ModItem is ThoriumItem thoriumItem){
				if (thoriumItem != null){
					if(thoriumItem.isHealer){
						return true;
					}
				}
			}
			return false;
		}
	
		private static Dictionary<int, int> WeaponItemTypeToInteger;
		
		private static List<string> pointblankshots = new List<string> {
			"FrostFury",
			"eSandStoneBow",
			"TalonBurst",
			"SteelBow",
			"ThoriumBow",
			"YewWoodBow",
			"FeatherFoe",
			"GrassStringBow",
			"BloomingBow",
			"StreamSting",
			"EternalNight",
			"ChampionsTrifectaShot",
			"CometCrossfire",
			"CupidString",
			"NagaRecurve",
			"FleshBow",
			"CinderString",
			"GlacialSting",
			"LodeStoneBow",
			"ValadiumBow",
			"ShusWrath",
			"DestroyersRage",
			"TitanBow",
			"DecayingSorrow",
			"DemonBloodBow",
			"ShadowFlareBow",
			"TheBlackBow",
			"TerrariumLongbow",
			"gDarkSteelCrossBow",
			"DurasteelRepeater",
			"DemonBloodCrossBow",
			"IllumiteShotbow",
			"YewWoodFlintlock",
			"HarpyPelter",
			"BuccaneerBlunderBuss",
			"SharkStorm",
			"GuanoGunner",
			"Slugger",
			"HellfireMinigun",
			"EnergyStormBolter",
			"HitScanner",
			"SpudBomber",
			"AquaPelter",
			"Trapper",
			"Webgun",
			"GoblinWarpipe",
			"ManHacker",
			"SSDevastator",
			"Shockbuster",
			"TommyGun",
			"BulletStorm",
			"MicroLauncher",
			"Obliterator",
			"PalladiumSubmachineGun",
			"TheZapper",
			"ElephantGun",
			"FreezeRay",
			"ChargedSplasher",
			"CobaltPopper",
			"DragonsGaze",
			"BloodBelcher",
			"RangedThorHammer",
			"MythrilPelter",
			"Trigun",
			"ArmorBane",
			"Funggat",
			"LodeStoneQuickDraw",
			"Scorn",
			"ValadiumFoeBlaster",
			"OrichalcumPelter",
			"UmbraBlaster",
			"TranquilizerGun",
			"VegaPhaser",
			"BeetleBlaster",
			"OgreSnotGun",
			"SpiritBreaker",
			"TitaniumRifle",
			"MineralLauncher",
			"SpineBuster",
			"Teslanator",
			"HandCannon",
			"IllumiteBlaster",
			"IllumiteBarrage",
			"AdamantiteCarbine",
			"MyceliumGattlingGun",
			"PhantomArmCannon",
			"DreadLauncher",
			"RejectsBlowpipe",
			"TrenchSpitter",
			"TerrariumPulseRifle",
			"LittleRed",
			"OmniCannon",
			"EmperorsWill",
			"DMR",
			"WyrmDecimator",
			"NovaRifle"
		};
	
		// only special damage tweaks
		private static Dictionary<string,int> thorium_damage_tweak = new Dictionary<string,int> {
			{"WoodenBaton", 9},
			{"Didgeridoo", 19},
			{"ThunderTalon", 17},
			{"TalonBurst", 7},
			{"IceShaver", 11},
			{"BatScythe", 21},
			{"DragonTooth", 47},
			{"TerrariansLastKnife", 285},
			{"SonicAmplifier", 325},
			{"BlackMIDI", 220},
			{"ShootingStarBlastGuitar", 210},
			{"TheSet", 325},
			{"EdgeofImagination", 210}
		};
		
		private static Dictionary<string,int> thorium_armor_defense_tweak = new Dictionary<string,int>{
			{"SandStoneMail", 4},
			{"SandStoneHelmet", 2},
			{"TideTurnerBreastplate", 26}, 
			{"TideTurnerGreaves", 25},
			{"TideTurnerHelmet", 27}, 
			{"AssassinsGuard", 29}, 
			{"AssassinsWalkers",29}, 
			{"RhapsodistBoots", 27}, 
			{"RhapsodistChestWoofer", 27},
			{"PyromancerTabard", 27},
			{"PyromancerLeggings", 24},
			{"DragonMask", 11},
			{"DragonGreaves", 12},
			{"DragonBreastplate", 15}	
		};
		
		public override void Load(){
			WeaponItemTypeToInteger = new Dictionary<int, int>();
			foreach (var entry in thorium_damage_tweak){
				int itemtype = (thorium.Find<ModItem>(entry.Key)).Type;
				WeaponItemTypeToInteger[itemtype] = entry.Value;
			}
		}
		
		public override void SetDefaults(Item item){
			// Armor
			if(item.defense > 0){
				foreach (var compareditem in thorium_armor_defense_tweak) {
						if ( item.type == thorium.Find<ModItem>(compareditem.Key).Type) {
							item.defense = compareditem.Value;
							break;
						}
					}
			} 
			else if (item.damage > 0 && item.ModItem != null){	
				if (ModContent.GetInstance<ItemBalancerConfig>().genericweaponchanges) {
					// Overall damage tweaks
					if (item.ModItem.Mod.Name == "ThoriumMod"){
						item.damage = (int)Math.Round(item.damage * 1.3f);
					}
				
					// Special damage tweaks
					if (WeaponItemTypeToInteger.TryGetValue(item.type, out int newDamage)){
						item.damage = newDamage;
					}
					
					// Apply some other tweaks
					if(item.type == thorium.Find<ModItem>("TerrariansLastKnife").Type) {
						item.shootSpeed = 16f;
						item.scale = 1.7f;
					}
				}
				
				//Toolpowers
				if(item.type == thorium.Find<ModItem>("ValadiumPickaxe").Type) {
					item.pick = 120;
				}
				else if(item.type == thorium.Find<ModItem>("FleshPickAxe").Type) {
					item.pick = 115;
				}
				else if(item.type == thorium.Find<ModItem>("FleshDrill").Type) {
					item.pick = 115;
				}
				else if(item.type == thorium.Find<ModItem>("FleshChainSaw").Type) {
					item.axe = 130;
				}
				else if(item.type == thorium.Find<ModItem>("GeodePickaxe").Type) {
					item.pick = 115;
				}
			}
		}
		
		public override void UpdateEquip(Item item, Player player) 
		{
			if (item.defense > 0) {
				if(item.type == thorium.Find<ModItem>("DepthDiverHelmet").Type) {
					if (Main.netMode == 1 && Main.myPlayer != player.whoAmI){
						Player localPlayer = Main.LocalPlayer;
						if (localPlayer.DistanceSQ(player.Center) < 62500f){
							localPlayer.AddBuff(ModContent.BuffType<DepthDiverAura>(), 30, true, false);
						}
					}
					var calamityPlayer = player.Calamity();
					if (!calamityPlayer.ZoneAbyss){
						if (player.breath <= player.breathMax + 2){
							player.breath = player.breathMax + 3;
						}	
					} 
					else{
						player.moveSpeed += 0.2f;
						player.statDefense += 10;
						if(player.breath < player.breathMax - 25 && player.breath > 5) {	
							Random rnd = new Random(); 
							if(rnd.Next(1, 600) == 1) {
								player.breath = player.breath + 20;
							}
						}
					}
					player.GetCritChance(DamageClass.Generic) += 6f;
				}
				else if(item.type == thorium.Find<ModItem>("WhiteDwarfMask").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.05f;
				}
				else if(item.type == thorium.Find<ModItem>("WhiteDwarfGuard").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.05f;
				}
				else if(item.type == thorium.Find<ModItem>("WhiteDwarfGreaves").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.05f;
				}
				else if(item.type == thorium.Find<ModItem>("ShadeMasterMask").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.05f;
				}
				else if(item.type == thorium.Find<ModItem>("ShadeMasterTreads").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.075f;
				}
				else if(item.type == thorium.Find<ModItem>("LichCarapace").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.05f;
				}
				else if(item.type == thorium.Find<ModItem>("LichTalon").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.025f;
				}
				else if(item.type == thorium.Find<ModItem>("TideTurnersGaze").Type) {
					player.GetDamage(DamageClass.Throwing) -= 0.15f;
				}
				// Healer armor
				else if(item.type == thorium.Find<ModItem>("NoviceClericCowl").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("NoviceClericPants").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("NoviceClericTabard").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("BloomingCrown").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("BloomingTabard").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("BloomingLeggings").Type){
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("WarlockLeggings").Type){
					player.manaCost += 0.15f;
					player.manaCost *= 0.85f;
				}
				else if(item.type == thorium.Find<ModItem>("SacredHelmet").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.07f;
					player.manaCost *= 0.93f;	
				}
				else if(item.type == thorium.Find<ModItem>("SacredBreastplate").Type) {
					player.manaRegenDelayBonus -= 3f;
					player.manaCost += 0.14f;
					player.manaCost *= 0.86f;
				}
				else if(item.type == thorium.Find<ModItem>("SacredLeggings").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.09f;
					player.manaCost *= 0.91f;
				}
				else if(item.type == thorium.Find<ModItem>("HallowedCowl").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.20f;
					player.manaCost *= 0.80f;
				}
				else if(item.type == thorium.Find<ModItem>("BioTechHood").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.12f;
					player.manaCost *= 0.88f;
				}
				else if(item.type == thorium.Find<ModItem>("BioTechGarment").Type) {
					player.manaRegenDelayBonus -= 3f;
					player.manaCost += 0.1f;
					player.manaCost *= 0.9f;
				}
				else if(item.type == thorium.Find<ModItem>("BioTechLeggings").Type) {
					player.manaRegenDelayBonus -= 3f;
					player.manaCost += 0.08f;
					player.manaCost *= 0.92f;
				}
				else if(item.type == thorium.Find<ModItem>("LifeBinderMask").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.18f;
					player.manaCost *= 0.82f;
				}
				else if(item.type == thorium.Find<ModItem>("LifeBinderBreastplate").Type) {
					player.manaRegenDelayBonus -= 1f;
					player.manaCost += 0.12f;
					player.manaCost *= 0.88f;
				}
				else if(item.type == thorium.Find<ModItem>("LifeBinderGreaves").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.1f;
					player.manaCost *= 0.9f;
				}
				else if(item.type == thorium.Find<ModItem>("FallenPaladinGreaves").Type) {
					player.manaCost += 0.15f;
					player.manaCost *= 0.85f;
				}
				else if(item.type == thorium.Find<ModItem>("WhisperingLeggings").Type) {
					player.manaCost += 0.15f;
					player.manaCost *= 0.85f;
				}
				else if(item.type == thorium.Find<ModItem>("CelestialCrown").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.20f;
					player.manaCost *= 0.80f;
				}
				else if(item.type == thorium.Find<ModItem>("CelestialVestment").Type) {
					player.manaRegenDelayBonus -= 3f;
					player.manaCost += 0.25f;
					player.manaCost *= 0.75f;
				}
				else if(item.type == thorium.Find<ModItem>("CelestialLeggings").Type) {
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.25f;
					player.manaCost *= 0.75f;
				}
				else if(item.type == thorium.Find<ModItem>("DreamWeaversHood").Type) {
					player.manaRegenDelayBonus -= 4f;
					player.manaCost += 0.35f;
					player.manaCost *= 0.65f;
				}
				else if(item.type == thorium.Find<ModItem>("DreamWeaversTabard").Type){
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.2f;
					player.manaCost *= 0.8f;
				}
				else if(item.type == thorium.Find<ModItem>("DreamWeaversTreads").Type){
					player.manaRegenDelayBonus -= 2f;
					player.manaCost += 0.20f;
					player.manaCost *= 0.80f;
				}
				// Mage Armor
				else if(item.type == thorium.Find<ModItem>("SilkHat").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("SilkTabard").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("SilkLeggings").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("NagaSkinMask").Type) {
					// Add conditional underwater breathing when not in the abyss
					var calamityPlayer = player.Calamity();
					if (!calamityPlayer.ZoneAbyss)
					{
						if (player.breath <= player.breathMax + 2)
						{
							player.breath = player.breathMax + 3;
						}	
					} 
					// Add old buffs again
					player.statManaMax2 += 60;
				}
				else if(item.type == thorium.Find<ModItem>("NagaSkinSuit").Type) {
					player.manaRegenDelayBonus -= 1f;
				}
				else if(item.type == thorium.Find<ModItem>("TitanHeadgear").Type) {
					player.manaCost += 0.15f;
					player.manaCost *= 0.85f;
				}
				else if(item.type == thorium.Find<ModItem>("CryomancersCrown").Type) {
					player.manaCost += 0.15f;
					player.manaCost *= 0.85f;
				}
				else if(item.type == thorium.Find<ModItem>("CryomancersTabard").Type) {
					player.manaRegenDelayBonus -= 3f;
				}	
				else if(item.type == thorium.Find<ModItem>("PyromancerCowl").Type) {
					player.manaRegenDelayBonus -= 5f;
				}
			}
		}
		
		public override void UpdateAccessory(Item item, Player player, bool hideVisual) {
			if (ModContent.GetInstance<ItemBalancerConfig>().OmegaCore){
				if(item.type == thorium.Find<ModItem>("TheOmegaCore").Type) {
						player.moveSpeed -= 0.3f;
						player.maxRunSpeed /= 1.3f;
				}
            }
		}

		public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback){
			if (item.CountsAsClass(DamageClass.Throwing) && item.damage >= 1 && player.GetRagnarokModPlayer().blightAccFix && Terraria.Utils.NextBool(Main.rand, 5)){
				ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
				//ThoriumItem thoriumItem = item.ModItem as ThoriumItem;
				float spread = 0.25f;
				float baseSpeed = velocity.Length();
				double num = Math.Atan2((double)velocity.X, (double)velocity.Y);
				double randomAngle = num + (double)(0.25f * spread);
				double randomAngle2 = num - (double)(0.25f * spread);
				float randomSpeed = Terraria.Utils.NextFloat(Main.rand) * 0.2f + 0.95f;
				int daggerdamage = (int)(0.75f * damage);
				if(daggerdamage > 2000) 
				{
					daggerdamage = 2000;
				}
				Projectile.NewProjectile(source, position.X, position.Y, baseSpeed * randomSpeed * (float)Math.Sin(randomAngle), baseSpeed * randomSpeed * (float)Math.Cos(randomAngle), ModContent.ProjectileType<BlightDagger>(), daggerdamage, knockback, player.whoAmI, 0f, 0f, 0f);
				Projectile.NewProjectile(source, position.X, position.Y, baseSpeed * randomSpeed * (float)Math.Sin(randomAngle2), baseSpeed * randomSpeed * (float)Math.Cos(randomAngle2), ModContent.ProjectileType<BlightDagger>(), daggerdamage, knockback, player.whoAmI, 0f, 0f, 0f);
			}
			return true;
		}	
		
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips){
			if(item.defense > 0) {
				if(item.type == thorium.Find<ModItem>("DepthDiverHelmet").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("breath underwater")){
							tooltips[i].Text = tooltips[i].Text + "\nDoes not work in the Abyss but instead grants you and your allies +10 defense and +20% movement speed\nYou also randomly get some oxygen back";
							break;
						}
					}
				}
				if(item.type == thorium.Find<ModItem>("NagaSkinMask").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("breath underwater")){
							tooltips[i].Text = tooltips[i].Text + "\nUnderwater breath does not work in the Abyss";
							break;
						}
					}
				}
			}
			if(item.type == thorium.Find<ModItem>("ShinobiSigil").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("basic damage")){
							tooltips[i].Text = "25% basic damage";
						}
					}
					var newLine = new TooltipLine(Mod, "shinobisigil", "Projectile damage caps at 500"){
						OverrideColor = Color.Red
					};
					tooltips.Add(newLine);
				}
			if(item.type == thorium.Find<ModItem>("PlagueLordFlask").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("basic damage")){
							tooltips[i].Text = "75% basic damage";
						}
					}
					var newLine = new TooltipLine(Mod, "plaguelordflask", "Projectile damage caps at 2000"){
						OverrideColor = Color.Red
					};
					tooltips.Add(newLine);
				}
			if(item.type == thorium.Find<ModItem>("ThrowingGuide").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("duplicated")){
							tooltips[i].Text = "12.5% of your rogue damage is duplicated";
						}
					}
					var newLine = new TooltipLine(Mod, "throwingguide", "Duplication damage caps at 50. Effect does not stack with other Guides"){
						OverrideColor = Color.Red
					};
					tooltips.Add(newLine);
				}
			if(item.type == thorium.Find<ModItem>("ThrowingGuideVolume2").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("duplicated")){
							tooltips[i].Text = "15% of your rogue damage is duplicated";
						}
					}
					var newLine = new TooltipLine(Mod, "throwingguide2", "Duplication damage caps at 100. Effect does not stack with other Guides"){
						OverrideColor = Color.Red
					};
					tooltips.Add(newLine);
				}
			if(item.type == thorium.Find<ModItem>("ThrowingGuideVolume3").Type) {
					for (int i = 0; i < tooltips.Count; i++){
						if (tooltips[i].Text.Contains("duplicated")){
							tooltips[i].Text = "17.5% of your rogue damage is duplicated";
						}
					}
					var newLine = new TooltipLine(Mod, "throwingguide2", "Duplication damage caps at 200. Effect does not stack with other Guides"){
						OverrideColor = Color.Red
					};
					tooltips.Add(newLine);
				}	
		}
	}
}