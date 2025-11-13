using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using CalamityMod;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.ILEditing
{
    public class CalamityEdits : ModSystem
    {
        private static int downedBosses = 0;

        private static Mod Thorium => ModLoader.GetMod("ThoriumMod");
        private static Mod Calamity => ModLoader.GetMod("CalamityMod");
        private static Assembly calamityAssembly => Calamity?.GetType().Assembly;

        private static Type community = null;
        private static MethodInfo communitycalculatepower = null;
        private static ILHook community_hook = null;

        public override void OnModLoad()
        {
            if (Calamity == null) return;

            // Find the type once
            foreach (Type type in calamityAssembly.GetTypes())
            {
                if (type.Name == "TheCommunity")
                {
                    community = type;
                    break;
                }
            }

            if (community == null) return;

            // Get the method and apply the hook ONCE
            communitycalculatepower = community.GetMethod("CalculatePower", BindingFlags.NonPublic | BindingFlags.Static);
            if (communitycalculatepower != null)
            {
                community_hook = new ILHook(communitycalculatepower, tweakTheCommunity);
                community_hook.Apply();
            }

            downedBosses = getBossSlayCount();
        }

        public override void OnModUnload()
        {
            // Properly dispose the hook
            if (community_hook != null)
            {
                community_hook.Dispose();
                community_hook = null;
            }
        }

        private void tweakTheCommunity(ILContext il)
        {
            var c = new ILCursor(il);
            // The hook recalculates this value every time the method is called
            // So it will automatically use the updated boss count
            c.Emit(OpCodes.Ldc_R4, calculateCommunityPowerLerp());
            c.EmitRet();
        }
        public static int getBossSlayCount()
        {
            int count = 0;

            // Thorium bosses
            if (Thorium != null)
            {
                string[] thoriumBosses = new[]
                {
            "TheGrandThunderBird", "QueenJellyfish", "Viscount", "GraniteEnergyStorm",
            "BuriedChampion", "StarScouter", "BoreanStrider", "FallenBeholder",
            "Lich", "ForgottenOne", "ThePrimordials"
        };

                foreach (string boss in thoriumBosses)
                {
                    count += Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", boss));
                }
            }

            // Vanilla bosses
            bool[] vanillaBosses = new[]
            {
        NPC.downedSlimeKing, NPC.downedBoss1, NPC.downedBoss2, NPC.downedQueenBee,
        NPC.downedBoss3, NPC.downedDeerclops, Main.hardMode, NPC.downedQueenSlime,
        NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3,
        NPC.downedPlantBoss, NPC.downedGolemBoss, NPC.downedFishron,
        NPC.downedEmpressOfLight, NPC.downedAncientCultist, NPC.downedMoonlord
    };

            foreach (bool downed in vanillaBosses)
            {
                count += Terraria.Utils.ToInt(downed);
            }

            // Calamity bosses
            bool[] calamityBosses = new[]
            {
        DownedBossSystem.downedDesertScourge, DownedBossSystem.downedCrabulon,
        DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator,
        DownedBossSystem.downedSlimeGod, DownedBossSystem.downedCryogen,
        DownedBossSystem.downedAquaticScourge, DownedBossSystem.downedBrimstoneElemental,
        DownedBossSystem.downedCalamitasClone, DownedBossSystem.downedLeviathan,
        DownedBossSystem.downedAstrumAureus, DownedBossSystem.downedPlaguebringer,
        DownedBossSystem.downedRavager, DownedBossSystem.downedAstrumDeus,
        DownedBossSystem.downedGuardians, DownedBossSystem.downedDragonfolly,
        DownedBossSystem.downedProvidence, DownedBossSystem.downedCeaselessVoid,
        DownedBossSystem.downedStormWeaver, DownedBossSystem.downedSignus,
        DownedBossSystem.downedPolterghast, DownedBossSystem.downedBoomerDuke,
        DownedBossSystem.downedDoG, DownedBossSystem.downedYharon,
        DownedBossSystem.downedExoMechs, DownedBossSystem.downedCalamitas
    };

            foreach (bool downed in calamityBosses)
            {
                count += Terraria.Utils.ToInt(downed);
            }

            return count;
        }
        public static float calculateCommunityPower()
        {
            int downedbosscount = getBossSlayCount();
            float num = downedbosscount / 53f;
            return num;
        }

        public static float calculateCommunityPowerLerp()
        {
            float result = calculateCommunityPower();
            return MathHelper.Lerp(0.05f, 0.2f, result);
        }
    }
}