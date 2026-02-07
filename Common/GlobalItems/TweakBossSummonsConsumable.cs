using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossTheGrandThunderBird;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossBoreanStrider;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossThePrimordials;

namespace RagnarokMod.Common.GlobalItems
{
    public class TweakBossSummonsConsumable : GlobalItem
    {
		public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return (
			item.type == ModContent.ItemType<StormFlare>() 
			|| item.type == ModContent.ItemType<JellyfishResonator>()
			|| item.type == ModContent.ItemType<UnstableCore>()
			|| item.type == ModContent.ItemType<AncientBlade>()
			|| item.type == ModContent.ItemType<StarCaller>()
			|| item.type == ModContent.ItemType<StriderTear>() 
			|| item.type == ModContent.ItemType<VoidLens>() 
			|| item.type == ModContent.ItemType<AbyssalShadow2>() 
			|| item.type == ModContent.ItemType<DoomSayersCoin>()
			);
        }
		
		public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<StormFlare>() || item.type == ModContent.ItemType<JellyfishResonator>() || item.type == ModContent.ItemType<UnstableCore>() ||
			item.type == ModContent.ItemType<AncientBlade>() || item.type == ModContent.ItemType<StarCaller>() || item.type == ModContent.ItemType<StriderTear>() ||
			item.type == ModContent.ItemType<VoidLens>() || item.type == ModContent.ItemType<AbyssalShadow2>() || item.type == ModContent.ItemType<DoomSayersCoin>())
			{
				item.consumable = false;
			}
        }
	}
}




 