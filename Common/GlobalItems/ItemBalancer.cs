using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using CalamityMod;
using ThoriumMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class ItemBalancer : GlobalItem
    {
		private static bool print_message=true;
		private static Mod thorium = ModLoader.GetMod("ThoriumMod");
		private static Mod calamity = ModLoader.GetMod("CalamityMod");
		public override void SetDefaults(Item item)
        {
			if(item.defense > 0) 
			{
				if (item.type == thorium.Find<ModItem>("SandStoneMail").Type) 
				{
					item.defense = 4;
				}	
				else if (item.type == thorium.Find<ModItem>("SandStoneHelmet").Type) 
				{
					item.defense = 2;
				}	
			} 
			
			else if (item.damage > 0) 
			{	
				if (item.type == thorium.Find<ModItem>("ThunderTalon").Type) 
				{
					item.damage = 14;
				}
				else if (item.type == thorium.Find<ModItem>("Didgeridoo").Type) 
				{
					item.damage = 15;
				}
				else if (item.type == thorium.Find<ModItem>("LifeQuartzClaymore").Type)
				{	
					item.damage = 14;
				}			
				else if (item.type == thorium.Find<ModItem>("PrehistoricAmberStaff").Type)
				{
					item.damage= 14;
				}
				else if (item.type == thorium.Find<ModItem>("gSandStoneThrowingKnife").Type)
				{
					item.damage= 13;
				}	
				else if (item.type == thorium.Find<ModItem>("StoneThrowingSpear").Type)
				{
					item.damage= 10;
				}
				else if (item.type == thorium.Find<ModItem>("CactusNeedle").Type)
				{		
					item.damage= 9;
				}
				else if (item.type == thorium.Find<ModItem>("CoralCaltrop").Type)
				{
					item.damage= 11;
				}
				else if (item.type == thorium.Find<ModItem>("IronTomahawk").Type)
				{
					item.damage= 12;
				}
				else if (item.type == thorium.Find<ModItem>("IcyCaltrop").Type)
				{
					item.damage= 12;
				}
				else if (item.type == thorium.Find<ModItem>("LeadTomahawk").Type)
				{	
					item.damage= 14;
				}
				else if (item.type == thorium.Find<ModItem>("ShinobiSlicer").Type)
				{	
					item.damage= 11;
				}
				else if (item.type == thorium.Find<ModItem>("DemoniteTomahawk").Type)
				{	
					item.damage= 16;
				}
				else if (item.type == thorium.Find<ModItem>("CrimtaneTomahawk").Type)
				{	
					item.damage= 17;
				}
				else if (item.type == thorium.Find<ModItem>("LastingPliers").Type)
				{	
					item.damage= 12;
				}
				else if (item.type == thorium.Find<ModItem>("SeveredHand").Type)
				{	
					item.damage= 14;
				}
				else if (item.type == thorium.Find<ModItem>("GelGlove").Type)
				{	
					item.damage= 16;
				}
				else if (item.type == thorium.Find<ModItem>("SteelBattleAxe").Type)
				{	
					item.damage= 16;
				}
				else if (item.type == thorium.Find<ModItem>("BloomingShuriken").Type)
				{	
					item.damage= 18;
				}
				else if (item.type == thorium.Find<ModItem>("HarpiesBarrage").Type)
				{	
					item.damage= 18;
				}
				else if (item.type == thorium.Find<ModItem>("MeteoriteClusterBomb").Type)
				{	
					item.damage= 18;
				}
				else if (item.type == thorium.Find<ModItem>("ObsidianStriker").Type)
				{	
					item.damage= 18;
				}
				else if (item.type == thorium.Find<ModItem>("PhaseChopper").Type)
				{	
					item.damage= 18;
				}
				else if (item.type == thorium.Find<ModItem>("ThoriumDagger").Type)
				{	
					item.damage= 19;
				}
				else if (item.type == thorium.Find<ModItem>("BaseballBat").Type)
				{	
					item.damage= 21;
				}
				else if (item.type == thorium.Find<ModItem>("DraculaFang").Type)
				{	
					item.damage= 20;
				}
				else if (item.type == thorium.Find<ModItem>("EnchantedKnife").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("GoblinWarSpear").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("NaiadShiv").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("SeedBomb").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("StarfishSlicer").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("SpikyCaltrop").Type)
				{
					item.damage=20;
				}
				else if (item.type == thorium.Find<ModItem>("Bolas").Type)
				{
					item.damage=21;
				}
				else if (item.type == thorium.Find<ModItem>("WackWrench").Type)
				{
					item.damage=21;
				}
				else if (item.type == thorium.Find<ModItem>("AquaiteKnife").Type)
				{
					item.damage=22;
				}
				else if (item.type == thorium.Find<ModItem>("ArcaneAnelace").Type)
				{
					item.damage=22;
				}
				else if (item.type == thorium.Find<ModItem>("GraniteThrowingAxe").Type)
				{
					item.damage=22;
				}
				else if (item.type == thorium.Find<ModItem>("SpikeBomb").Type)
				{
					item.damage=22;
				}
				else if (item.type == thorium.Find<ModItem>("BlackDagger").Type)
				{
					item.damage=22;
				}
				else if (item.type == thorium.Find<ModItem>("BronzeThrowing").Type)
				{
					item.damage=24;
				}
				else if (item.type == thorium.Find<ModItem>("FungalPopper").Type)
				{
					item.damage=24;
				}
				else if (item.type == thorium.Find<ModItem>("Embowelment").Type)
				{
					item.damage=26;
				}
				else if (item.type == thorium.Find<ModItem>("MoltenKnife").Type)
				{
					item.damage=26;
				}
				else if (item.type == thorium.Find<ModItem>("Chum").Type)
				{
					item.damage=28;
				}
				else if (item.type == thorium.Find<ModItem>("LightAnguish").Type)
				{
					item.damage=28;
				}
				else if (item.type == thorium.Find<ModItem>("Kunai").Type)
				{
					item.damage=30;
				}
				else if (item.type == thorium.Find<ModItem>("GaussFlinger").Type)
				{
					item.damage=31;
				}
				else if (item.type == thorium.Find<ModItem>("TheCryoFang").Type)
				{
					item.damage=31;
				}
				else if (item.type == thorium.Find<ModItem>("MorelGrenade").Type)
				{
					item.damage=32;
				}
				else if (item.type == thorium.Find<ModItem>("CorrupterBalloon").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("CrystalBalloon").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("EvisceratingClaw").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("FesteringBalloon").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("GasContainer").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("RocketFist").Type)
				{
					item.damage=33;
				}
				else if (item.type == thorium.Find<ModItem>("CaptainsPoniard").Type)
				{
					item.damage=34;
				}
				else if (item.type == thorium.Find<ModItem>("AphrodisiacVial").Type)
				{
					item.damage=35;
				}
				else if (item.type == thorium.Find<ModItem>("CombustionFlask").Type)
				{
					item.damage=35;
				}
				else if (item.type == thorium.Find<ModItem>("CorrosionBeaker").Type)
				{
					item.damage=35;
				}
				else if (item.type == thorium.Find<ModItem>("NitrogenVial").Type)
				{
					item.damage=35;
				}
				else if (item.type == thorium.Find<ModItem>("ChampionsGodHand").Type)
				{
					item.damage=36;
				}
				else if (item.type == thorium.Find<ModItem>("VenomKunai").Type)
				{
					item.damage=36;
				}
				else if (item.type == thorium.Find<ModItem>("CobaltThrowingSpear").Type)
				{
					item.damage=37;
				}
				else if (item.type == thorium.Find<ModItem>("DurasteelJavelin").Type)
				{
					item.damage=37;
				}
				else if (item.type == thorium.Find<ModItem>("HotPot").Type)
				{
					item.damage=37;
				}
				else if (item.type == thorium.Find<ModItem>("LegionOrnament").Type)
				{
					item.damage=37;
				}
				else if (item.type == thorium.Find<ModItem>("AxeBlade").Type)
				{
					item.damage=38;
				}
				else if (item.type == thorium.Find<ModItem>("ClockWorkBomb").Type)
				{
					item.damage=38;
				}
				else if (item.type == thorium.Find<ModItem>("PalladiumThrowingSpear").Type)
				{
					item.damage=39;
				}
				else if (item.type == thorium.Find<ModItem>("HellRoller").Type)
				{
					item.damage=40;
				}
				else if (item.type == thorium.Find<ModItem>("AdamantiteGlaive").Type)
				{
					item.damage=41;
				}
				else if (item.type == thorium.Find<ModItem>("Carnwennan").Type)
				{
					item.damage=41;
				}
				else if (item.type == thorium.Find<ModItem>("TrueEmbowelment").Type)
				{
					item.damage=41;
				}
				else if (item.type == thorium.Find<ModItem>("ValadiumBattleAxe").Type)
				{
					item.damage=41;
				}
				else if (item.type == thorium.Find<ModItem>("LodestoneJavelin").Type)
				{
					item.damage=43;
				}
				else if (item.type == thorium.Find<ModItem>("RiftTearer").Type)
				{
					item.damage=43;
				}
				else if (item.type == thorium.Find<ModItem>("TitaniumGlaive").Type)
				{
					item.damage=43;
				}
				else if (item.type == thorium.Find<ModItem>("TrueLightAnguish").Type)
				{
					item.damage=43;
				}
				else if (item.type == thorium.Find<ModItem>("SparkTaser").Type)
				{
					item.damage=45;
				}
				else if (item.type == thorium.Find<ModItem>("TrueCarnwennan").Type)
				{
					item.damage=45;
				}
				else if (item.type == thorium.Find<ModItem>("TitanJavelin").Type)
				{
					item.damage=48;
				}
				else if (item.type == thorium.Find<ModItem>("ChlorophyteTomahawk").Type)
				{
					item.damage=52;
				}
				else if (item.type == thorium.Find<ModItem>("ShadowTippedJavelin").Type)
				{
					item.damage=52;
				}
				else if (item.type == thorium.Find<ModItem>("MagicCard").Type)
				{
					item.damage=53;
				}
				else if (item.type == thorium.Find<ModItem>("StalkersSnippers").Type)
				{
					item.damage=54;
				}
				else if (item.type == thorium.Find<ModItem>("Omniwrench").Type)
				{
					item.damage=56;
				}
				else if (item.type == thorium.Find<ModItem>("SoulCleaver").Type)
				{
					item.damage=58;
				}
				else if (item.type == thorium.Find<ModItem>("VoltHatchet").Type)
				{
					item.damage=58;
				}
				else if (item.type == thorium.Find<ModItem>("ShadowPurgeCaltrop").Type)
				{
					item.damage=59;
				}
				else if (item.type == thorium.Find<ModItem>("SwampSpike").Type)
				{
					item.damage=59;
				}
				else if (item.type == thorium.Find<ModItem>("SoftServeSunderer").Type)
				{
					item.damage=60;
				}
				else if (item.type == thorium.Find<ModItem>("BudBomb").Type)
				{
					item.damage=63;
				}
				else if (item.type == thorium.Find<ModItem>("HadronCollider").Type)
				{
					item.damage=63;
				}
				else if (item.type == thorium.Find<ModItem>("TerraKnife").Type)
				{
					item.damage=63;
				}
				else if (item.type == thorium.Find<ModItem>("SoulBomb").Type)
				{
					item.damage=67;
				}
				else if (item.type == thorium.Find<ModItem>("LihzahrdKukri").Type)
				{
					item.damage=69;
				}
				else if (item.type == thorium.Find<ModItem>("PlasmaVial").Type)
				{
					item.damage=69;
				}
				else if (item.type == thorium.Find<ModItem>("Soulslasher").Type)
				{
					item.damage=69;
				}
				else if (item.type == thorium.Find<ModItem>("BugenkaiShuriken").Type)
				{
					item.damage=73;
				}
				else if (item.type == thorium.Find<ModItem>("CosmicDagger").Type)
				{
					item.damage=76;
				}
				else if (item.type == thorium.Find<ModItem>("ShadeKunai").Type)
				{
					item.damage=78;
				}
				else if (item.type == thorium.Find<ModItem>("Witchblade").Type)
				{
					item.damage=80;
				}
				else if (item.type == thorium.Find<ModItem>("DragonFang").Type)
				{
					item.damage=81;
				}
				else if (item.type == thorium.Find<ModItem>("Brinefang").Type)
				{
					item.damage=86;
				}
				else if (item.type == thorium.Find<ModItem>("ProximityMine").Type)
				{
					item.damage=91;
				}
				else if (item.type == thorium.Find<ModItem>("StarEater").Type)
				{
					item.damage=95;
				}
				else if (item.type == thorium.Find<ModItem>("TerrariumRippleKnife").Type)
				{
					item.damage=95;
				}
				else if (item.type == thorium.Find<ModItem>("ElectroRebounder").Type)
				{
					item.damage=97;
				}
				else if (item.type == thorium.Find<ModItem>("WhiteDwarfKunai").Type)
				{
					item.damage=104;
				}
				else if (item.type == thorium.Find<ModItem>("ShadeKusarigama").Type)
				{
					item.damage=108;
				}
				else if (item.type == thorium.Find<ModItem>("FireAxe").Type)
				{
					item.damage=129;
				}
				else if (item.type == thorium.Find<ModItem>("TidalWave").Type)
				{
					item.damage=145;
				}
				else if (item.type == thorium.Find<ModItem>("DeitysTrefork").Type)
				{
					item.damage=215;
				}
				else if (item.type == thorium.Find<ModItem>("AngelsEnd").Type)
				{
					item.damage=300;
				}
			}	
			
			else 
			{
				return;
			}
        }
	}
	
}