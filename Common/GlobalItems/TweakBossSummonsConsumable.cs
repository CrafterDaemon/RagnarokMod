using Terraria.GameContent.ItemDropRules;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.BossTheGrandThunderBird;
using RagnarokMod.Items.Materials;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossGraniteEnergyStorm;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossBoreanStrider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossQueenJellyfish;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossViscount;
using ThoriumMod.Items.BossBuriedChampion;
using ThoriumMod.Items.BossStarScouter;
using ThoriumMod.Items.BossBoreanStrider;
using ThoriumMod.Items.BossFallenBeholder;
using ThoriumMod.Items.BossForgottenOne;
using ThoriumMod.Items.BossThePrimordials;
using RagnarokMod.Items.BardItems.Percussion;
using RagnarokMod.Items.HealerItems.Scythes;

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




 