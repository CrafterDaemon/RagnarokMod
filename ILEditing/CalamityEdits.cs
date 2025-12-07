using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria;
using System;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Mono.Cecil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using CalamityMod;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.ILEditing
{
    public class CalamityEdits : ModSystem
    {
		private short timer = 0;
		private static int downedBosses = 0;
		
		private static Mod Thorium => ModLoader.GetMod("ThoriumMod");
		private static Mod Calamity = ModLoader.GetMod("CalamityMod");
        private static Assembly calamityAssembly = Calamity.GetType().Assembly;
		
        private static Type community = null;
        private static MethodInfo communitycalculatepower = null;
        private static ILHook community_hook = null;

        public override void OnModLoad() {
            bool loadCaught = false;
            while (!loadCaught){
				if (Calamity != null){ 
					downedBosses = getBossSlayCount();
					foreach (Type type in calamityAssembly.GetTypes()){
						if (type.Name == "TheCommunity"){
							community = type;
						}
					}
					communitycalculatepower = community.GetMethod("CalculatePower", BindingFlags.NonPublic | BindingFlags.Static);
					community_hook = new ILHook(communitycalculatepower, tweakTheCommunity);
					community_hook.Apply();
					
					loadCaught = true;
					break;
				}
            }
        }
		
		 public override void PostUpdateItems(){
				if(timer == 600){
					timer = 0;
					if(checkNewBossSlay(downedBosses)){
						community_hook.Undo();
						communitycalculatepower = community.GetMethod("CalculatePower", BindingFlags.NonPublic | BindingFlags.Static);
						community_hook = new ILHook(communitycalculatepower, tweakTheCommunity);
						community_hook.Apply();
						Main.NewText("A new boss has been slayed for the first time. The power of TheCommunity has increased!");
					}	
				}
				timer++;
        }
		
		 public override void OnModUnload(){
            if (Calamity != null){
				community_hook.Dispose();
            }
        }
		
		private void tweakTheCommunity(ILContext il) {
			var c = new ILCursor(il);
			c.Emit(OpCodes.Ldc_R4, calculateCommunityPowerLerp());
			c.EmitRet();
		}
		
		public static int getBossSlayCount() {
			int downedbosscount = 
			0
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "TheGrandThunderBird"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "QueenJellyfish"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "Viscount"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "GraniteEnergyStorm"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "BuriedChampion"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "StarScouter"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "BoreanStrider"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "FallenBeholder"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "Lich"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "ForgottenOne"))
			+ Terraria.Utils.ToInt((bool)Thorium.Call("GetDownedBoss", "ThePrimordials"))
			+ Terraria.Utils.ToInt(NPC.downedSlimeKing) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedDesertScourge) 
			+ Terraria.Utils.ToInt(NPC.downedBoss1) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedCrabulon) 
			+ Terraria.Utils.ToInt(NPC.downedBoss2) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator) 
			+ Terraria.Utils.ToInt(NPC.downedQueenBee) 
			+ Terraria.Utils.ToInt(NPC.downedBoss3) 
			+ Terraria.Utils.ToInt(NPC.downedDeerclops) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedSlimeGod) 
			+ Terraria.Utils.ToInt(Main.hardMode) 
			+ Terraria.Utils.ToInt(NPC.downedQueenSlime) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedCryogen) 
			+ Terraria.Utils.ToInt(NPC.downedMechBoss1) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedAquaticScourge) 
			+ Terraria.Utils.ToInt(NPC.downedMechBoss2) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedBrimstoneElemental) 
			+ Terraria.Utils.ToInt(NPC.downedMechBoss3) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedCalamitasClone)
			+ Terraria.Utils.ToInt(NPC.downedPlantBoss) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedLeviathan) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedAstrumAureus) 
			+ Terraria.Utils.ToInt(NPC.downedGolemBoss)
			+ Terraria.Utils.ToInt(DownedBossSystem.downedPlaguebringer) 
			+ Terraria.Utils.ToInt(NPC.downedFishron) 
			+ Terraria.Utils.ToInt(NPC.downedEmpressOfLight) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedRavager) 
			+ Terraria.Utils.ToInt(NPC.downedAncientCultist) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedAstrumDeus)
			+ Terraria.Utils.ToInt(NPC.downedMoonlord) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedGuardians) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedDragonfolly) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedProvidence) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedCeaselessVoid) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedStormWeaver) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedSignus) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedPolterghast) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedBoomerDuke) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedDoG) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedYharon) 
			+ Terraria.Utils.ToInt(DownedBossSystem.downedExoMechs)
			+ Terraria.Utils.ToInt(DownedBossSystem.downedCalamitas);
			return downedbosscount;
		}
		
		public static bool checkNewBossSlay(int oldcount) {
			int count = getBossSlayCount();
			if(count > oldcount) {
				downedBosses = count;
				return true;
			} else {return false;}
		}
		
		public static float calculateCommunityPower() {
			int downedbosscount = getBossSlayCount();
			float num = downedbosscount / 53f;
			return num;
		}
		
		public static int calculateCommunityPowerKillsOnly() {
			int result  = (int)(calculateCommunityPower() * 100);
			return result;
		}
		
		public static float calculateCommunityPowerLerp() {
			float result  = calculateCommunityPower();
			return MathHelper.Lerp(0.05f, 0.2f, result);
		}
	}
}