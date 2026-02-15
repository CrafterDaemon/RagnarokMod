using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using RagnarokMod.Common.Configs;

namespace RagnarokMod.Common.ModSystems
{
    public class AddThoriumBossRush : ModSystem
    {
        public override void PostSetupContent()
        {
            //Thorium Rework is still bugged, so bossrush is always Thorium Rework for now
            if (OtherModsCompat.tbr_loaded)
            {
                return;
            }
            if (!(ModContent.GetInstance<BossConfig>().bossrush == ThoriumBossRework_selection_mode.Ragnarok)) // If Ragnarok is not enabled do not add thorium bosses to the bossrush
            {
                return;
            }
            Mod calamity;
            if (ModLoader.TryGetMod("CalamityMod", out calamity))
            {
                try
                {
                    List<ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>> list = calamity.Call(new object[] { "GetBossRushEntries" }) as List<ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>>;
                    List<ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>> bossRushEntries = new List<ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>>();
                    Action<int> action_old;
                    foreach (ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>> entry in list)
                    {
                        if (entry.Item1 == 50) // Before King Slime add Grand Thunderbird
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("TheGrandThunderBird"), 1, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[] { this.ThoriumNPC("TheGrandThunderBird") }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("StormHatchling"),
                                this.ThoriumNPC("TheGrandThunderBird")
                            })));
                        }
                        bossRushEntries.Add(entry);
                        if (entry.Item1 == 266)
                        { // After BoC add QueenJellyfish and Viscount
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("QueenJellyfish"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[]
                            {
                                this.ThoriumNPC("ZealousJelly"),
                                this.ThoriumNPC("DistractJelly"),
                                this.ThoriumNPC("SpittingJelly"),
                                this.ThoriumNPC("QueenJellyfish")
                            }, new ValueTuple<int[]>(new int[] { this.ThoriumNPC("QueenJellyfish") })));

                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("Viscount"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, 0, true, 0f, new int[]
                            {
                                this.ThoriumNPC("Viscount"),
                                this.ThoriumNPC("BiteyBaby")
                            }, new ValueTuple<int[]>(new int[] { this.ThoriumNPC("Viscount") })));
                        }
                        else if (entry.Item1 == 35) // After Skeletron add Buried Champion, Granite Energy Storm and Star Scouter
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("BuriedChampion"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, false, 0f, new int[]
                            {
                                this.ThoriumNPC("BuriedChampion"),
                                this.ThoriumNPC("FallenChampion1"),
                                this.ThoriumNPC("FallenChampion2"),
                                this.ThoriumNPC("MagicalBurst")
                            }, new ValueTuple<int[]>(new int[] { this.ThoriumNPC("BuriedChampion") })));
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("GraniteEnergyStorm"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[]
                            {
                                this.ThoriumNPC("GraniteEnergyStorm"),
                                this.ThoriumNPC("CoalescedEnergy"),
                                this.ThoriumNPC("EncroachingEnergy"),
                                this.ThoriumNPC("EnergyConduit")
                            }, new ValueTuple<int[]>(new int[] { this.ThoriumNPC("GraniteEnergyStorm") })));

                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("StarScouter"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[]
                            {
                                this.ThoriumNPC("StarScouter"),
                                this.ThoriumNPC("BioCore"),
                                this.ThoriumNPC("PyroCore"),
                                this.ThoriumNPC("CryoCore")
                            }, new ValueTuple<int[]>(new int[] { this.ThoriumNPC("StarScouter") })));
                        }
                        else if (entry.Item1 == 113) // After Wall of Flesh add Borean Strider and FallenBeholder
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("BoreanStrider"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, false, 0f, new int[]
                            {
                                this.ThoriumNPC("BoreanMyte1"),
                                this.ThoriumNPC("BoreanHopper"),
                                this.ThoriumNPC("BoreanStrider"),
                                this.ThoriumNPC("BoreanStriderPopped")
                            }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("BoreanStrider"),
                                this.ThoriumNPC("BoreanStriderPopped")
                            })));

                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("FallenBeholder"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[]
                            {
                                this.ThoriumNPC("EnemyBeholder"),
                                this.ThoriumNPC("Beholder"),
                                this.ThoriumNPC("FallenBeholder"),
                                this.ThoriumNPC("FallenBeholder2")
                            }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("FallenBeholder"),
                                this.ThoriumNPC("FallenBeholder2")
                            })));
                        }
                        else if (entry.Item1 == 127) // After Skeletron Prime add Lich
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("Lich"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, true, 0f, new int[]
                            {
                                this.ThoriumNPC("ThousandSoulPhalactry"),
                                this.ThoriumNPC("Lich"),
                                this.ThoriumNPC("LichHeadless")
                            }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("Lich"),
                                this.ThoriumNPC("LichHeadless")
                            })));
                        }
                        else if (entry.Item1 == 245) // After Golem add Forgotten One
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("ForgottenOne"), 0, delegate (int type)
                            {
                                NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
                            }, -1, false, 0f, new int[]
                            {
                                this.ThoriumNPC("AbyssalSpawn"),
                                this.ThoriumNPC("ForgottenOne"),
                                this.ThoriumNPC("ForgottenOneCracked"),
                                this.ThoriumNPC("ForgottenOneReleased")
                            }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("ForgottenOne"),
                                this.ThoriumNPC("ForgottenOneCracked"),
                                this.ThoriumNPC("ForgottenOneReleased")
                            })));
                        }
                        else if (entry.Item1 == calamity.Find<ModNPC>("Providence").Type) // After Providence add The Primordials
                        {
                            bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("DreamEater"), 0, delegate (int type)
                            {
                                byte b = Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0);
                                NPC.SpawnOnPlayer((int)b, this.ThoriumNPC("SlagFury"));
                                NPC.SpawnOnPlayer((int)b, this.ThoriumNPC("Aquaius"));
                                NPC.SpawnOnPlayer((int)b, this.ThoriumNPC("Omnicide"));
                            }, -1, false, 0f, new int[]
                            {
                                this.ThoriumNPC("AquaiusBubble"),
                                this.ThoriumNPC("SlagFury"),
                                this.ThoriumNPC("Aquaius"),
                                this.ThoriumNPC("Omnicide")
                            }, new ValueTuple<int[]>(new int[]
                            {
                                this.ThoriumNPC("SlagFury"),
                                this.ThoriumNPC("Aquaius"),
                                this.ThoriumNPC("Omnicide")
                            })));

                            /*
							if(Main.expertMode) 
							{
								bossRushEntries.Add(new ValueTuple<int, int, Action<int>, int, bool, float, int[], ValueTuple<int[]>>(this.ThoriumNPC("DreamEater"), 0, delegate(int type)
								{
									NPC.SpawnOnPlayer((int)Player.FindClosest(new Vector2((float)Main.maxTilesX, (float)Main.maxTilesY) * 8f, 0, 0), type);
								}, -1, false, 0f, new int[]
								{
									this.ThoriumNPC("LucidBubble"),
									this.ThoriumNPC("DreamEater")
								}, new ValueTuple<int[]>(new int[]
								{
									this.ThoriumNPC("DreamEater")
								})));
							}
							*/
                        }
                    }
                    calamity.Call(new object[] { "SetBossRushEntries", bossRushEntries });
                }
                catch { }
            }
        }
        private int ThoriumNPC(string name)
        {
            return ModLoader.GetMod("ThoriumMod").Find<ModNPC>(name).Type;
        }
    }
}