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
using System.Linq;

namespace RagnarokMod.Common.GlobalItems
{
    public class TweakBossSummonsConsumable : GlobalItem
    {
        public override bool InstancePerEntity => true;

        private static int[] bossSummons =
        [
            ModContent.ItemType<StormFlare>(),
            ModContent.ItemType<JellyfishResonator>(),
            ModContent.ItemType<UnstableCore>(),
            ModContent.ItemType<AncientBlade>(),
            ModContent.ItemType<StarCaller>(),
            ModContent.ItemType<StriderTear>(),
            ModContent.ItemType<VoidLens>(),
            ModContent.ItemType<AbyssalShadow2>(),
            ModContent.ItemType<DoomSayersCoin>()
        ];

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return bossSummons.Contains(item.type);
        }

        public override void SetDefaults(Item item)
        {
            item.consumable = false;
        }
    }
}




