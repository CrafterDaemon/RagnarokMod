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
using RagnarokMod.Common.Configs;
using Terraria.ID;

namespace RagnarokMod.Common.GlobalItems
{
    public class TweakBossSummonsConsumable : GlobalItem
    {
        public override bool InstancePerEntity => true;
        ModItem[] bossSummons =
        [
            new StormFlare(),
            new JellyfishResonator(),
            new UnstableCore(),
            new AncientBlade(),
            new StarCaller(),
            new StriderTear(),
            new VoidLens(),
            new AbyssalShadow2(),
            new DoomSayersCoin()
        ];
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return bossSummons.Contains(item.ModItem);
        }

        public override void SetDefaults(Item item)
        {
            item.consumable = false;
        }
    }
}




