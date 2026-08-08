using CalamityMod;
using CalamityMod.Items.Accessories;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.ILEditing
{
    public class CalamityEdits : ModSystem
    {
        private static Hook calculatePowerHook;

        public override void Load()
        {
            MethodInfo calculatePowerMethod = typeof(TheCommunity).GetMethod("CalculatePower", BindingFlags.Static | BindingFlags.NonPublic);

            if (calculatePowerMethod is null)
                throw new Exception("Could not find TheCommunity.CalculatePower.");

            calculatePowerHook = new Hook(calculatePowerMethod, CalculatePower);
        }

        public override void Unload()
        {
            calculatePowerHook?.Dispose();
            calculatePowerHook = null;
        }

        public static float CalculatePower(bool killsOnly = false)
        {
            int numBosses = 0;

            numBosses += ThoriumWorld.downedTheGrandThunderBird.ToInt();
            numBosses += NPC.downedSlimeKing.ToInt();
            numBosses += DownedBossSystem.downedDesertScourge.ToInt();
            numBosses += NPC.downedBoss1.ToInt();
            numBosses += DownedBossSystem.downedCrabulon.ToInt();
            numBosses += NPC.downedBoss2.ToInt();
            numBosses += ThoriumWorld.downedQueenJellyfish.ToInt();
            numBosses += ThoriumWorld.downedViscount.ToInt();
            numBosses += (DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator).ToInt();
            numBosses += NPC.downedQueenBee.ToInt();
            numBosses += NPC.downedBoss3.ToInt();
            numBosses += ThoriumWorld.downedGraniteEnergyStorm.ToInt();
            numBosses += ThoriumWorld.downedBuriedChampion.ToInt();
            numBosses += NPC.downedDeerclops.ToInt();
            numBosses += DownedBossSystem.downedSlimeGod.ToInt();
            numBosses += ThoriumWorld.downedStarScouter.ToInt();
            numBosses += Main.hardMode.ToInt();
            numBosses += ThoriumWorld.downedBoreanStrider.ToInt();
            numBosses += ThoriumWorld.downedFallenBeholder.ToInt();
            numBosses += NPC.downedQueenSlime.ToInt();
            numBosses += DownedBossSystem.downedCryogen.ToInt();
            numBosses += NPC.downedMechBoss1.ToInt();
            numBosses += DownedBossSystem.downedAquaticScourge.ToInt();
            numBosses += NPC.downedMechBoss2.ToInt();
            numBosses += DownedBossSystem.downedBrimstoneElemental.ToInt();
            numBosses += NPC.downedMechBoss3.ToInt();
            numBosses += ThoriumWorld.downedLich.ToInt();
            numBosses += DownedBossSystem.downedCalamitasClone.ToInt();
            numBosses += NPC.downedPlantBoss.ToInt();
            numBosses += DownedBossSystem.downedLeviathan.ToInt();
            numBosses += DownedBossSystem.downedAstrumAureus.ToInt();
            numBosses += NPC.downedGolemBoss.ToInt();
            numBosses += ThoriumWorld.downedForgottenOne.ToInt();
            numBosses += DownedBossSystem.downedPlaguebringer.ToInt();
            numBosses += NPC.downedFishron.ToInt();
            numBosses += NPC.downedEmpressOfLight.ToInt();
            numBosses += DownedBossSystem.downedRavager.ToInt();
            numBosses += NPC.downedAncientCultist.ToInt();
            numBosses += DownedBossSystem.downedAstrumDeus.ToInt();
            numBosses += NPC.downedMoonlord.ToInt();
            numBosses += DownedBossSystem.downedGuardians.ToInt();
            numBosses += DownedBossSystem.downedDragonfolly.ToInt();
            numBosses += DownedBossSystem.downedProvidence.ToInt();
            numBosses += DownedBossSystem.downedCeaselessVoid.ToInt();
            numBosses += DownedBossSystem.downedStormWeaver.ToInt();
            numBosses += DownedBossSystem.downedSignus.ToInt();
            numBosses += DownedBossSystem.downedPolterghast.ToInt();
            numBosses += DownedBossSystem.downedBoomerDuke.ToInt();
            numBosses += DownedBossSystem.downedDoG.ToInt();
            numBosses += ThoriumWorld.downedThePrimordials.ToInt();
            numBosses += DownedBossSystem.downedYharon.ToInt();
            numBosses += DownedBossSystem.downedExoMechs.ToInt();
            numBosses += DownedBossSystem.downedCalamitas.ToInt();

            float bossDownedRatio = numBosses / (float)53;
            return killsOnly ? bossDownedRatio : MathHelper.Lerp(0.05f, 0.2f, bossDownedRatio);
        }
    }
}