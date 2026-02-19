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
using ThoriumMod.Items.MagicItems;
using ThoriumMod.Items.ThrownItems;

namespace RagnarokMod.Common.GlobalItems
{
    public class ItemBalancer : GlobalItem
    {
        private static bool print_message = true;
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        private static Mod calamity = ModLoader.GetMod("CalamityMod");

        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return (item.damage > 0
            || item.defense > 0
            || item.type == shinobiSigilType
            || item.type == plagueLordFlaskType
            || item.type == throwingGuideType
            || item.type == throwingGuideVol2Type
            || item.type == throwingGuideVol3Type
            || item.type == theOmegaCoreType);
        }

        private static Dictionary<int, int> WeaponItemTypeToInteger;
        private static Dictionary<int, int> ArmorDefenseOverrides;
        private static Dictionary<int, Action<Item>> ToolPowerOverrides;
        private static Dictionary<int, float> ThrowingDamageAdjustments;
        private static Dictionary<int, float> ManaRegenDelayOnly;
        private static Dictionary<int, (float regenDelay, float manaCostAdd, float manaCostMult)> ManaCombos;
        private static int depthDiverHelmetType;
        private static int nagaSkinMaskType;
        private static int nagaSkinSuitType;
        private static int terrariansLastKnifeType;
        private static int shinobiSigilType;
        private static int plagueLordFlaskType;
        private static int throwingGuideType;
        private static int throwingGuideVol2Type;
        private static int throwingGuideVol3Type;
        private static int theOmegaCoreType;

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
        private static Dictionary<string, int> thorium_damage_tweak = new Dictionary<string, int> {
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

        private static Dictionary<string, int> thorium_armor_defense_tweak = new Dictionary<string, int>{
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

        private static int T(string name) => thorium.Find<ModItem>(name).Type;

        public override void Load()
        {
            WeaponItemTypeToInteger = new Dictionary<int, int>();
            foreach (var entry in thorium_damage_tweak)
            {
                WeaponItemTypeToInteger[T(entry.Key)] = entry.Value;
            }

            ArmorDefenseOverrides = new Dictionary<int, int>();
            foreach (var entry in thorium_armor_defense_tweak)
            {
                ArmorDefenseOverrides[T(entry.Key)] = entry.Value;
            }

            // Pre-cache special item types
            depthDiverHelmetType = T("DepthDiverHelmet");
            nagaSkinMaskType = T("NagaSkinMask");
            nagaSkinSuitType = T("NagaSkinSuit");
            terrariansLastKnifeType = T("TerrariansLastKnife");
            shinobiSigilType = T("ShinobiSigil");
            plagueLordFlaskType = T("PlagueLordFlask");
            throwingGuideType = T("ThrowingGuide");
            throwingGuideVol2Type = T("ThrowingGuideVolume2");
            throwingGuideVol3Type = T("ThrowingGuideVolume3");
            theOmegaCoreType = T("TheOmegaCore");

            // Tool power overrides
            ToolPowerOverrides = new Dictionary<int, Action<Item>>
            {
                { T("ValadiumPickaxe"), item => item.pick = 120 },
                { T("LodeStonePickaxe"), item => item.pick = 120 },
                { T("FleshPickAxe"), item => item.pick = 115 },
                { T("FleshDrill"), item => item.pick = 115 },
                { T("FleshChainSaw"), item => item.axe = 130 },
                { T("GeodePickaxe"), item => item.pick = 115 },
            };

            // Throwing damage adjustments
            ThrowingDamageAdjustments = new Dictionary<int, float>
            {
                { T("WhiteDwarfMask"), -0.05f },
                { T("WhiteDwarfGuard"), -0.05f },
                { T("WhiteDwarfGreaves"), -0.05f },
                { T("ShadeMasterMask"), -0.05f },
                { T("ShadeMasterTreads"), -0.075f },
                { T("LichCarapace"), -0.05f },
                { T("LichTalon"), -0.025f },
                { T("TideTurnersGaze"), -0.15f },
            };

            // Mana regen delay only adjustments
            ManaRegenDelayOnly = new Dictionary<int, float>
            {
                { T("NoviceClericCowl"), -1f },
                { T("NoviceClericPants"), -1f },
                { T("NoviceClericTabard"), -1f },
                { T("BloomingCrown"), -1f },
                { T("BloomingTabard"), -1f },
                { T("BloomingLeggings"), -1f },
                { T("SilkHat"), -1f },
                { T("SilkTabard"), -1f },
                { T("SilkLeggings"), -1f },
                { nagaSkinSuitType, -1f },
                { T("CryomancersTabard"), -3f },
                { T("PyromancerCowl"), -5f },
            };

            // Combo: mana regen delay + manaCost add/mult adjustments
            ManaCombos = new Dictionary<int, (float, float, float)>
            {
                { T("WarlockLeggings"), (0f, 0.15f, 0.85f) },
                { T("SacredHelmet"), (-2f, 0.07f, 0.93f) },
                { T("SacredBreastplate"), (-3f, 0.14f, 0.86f) },
                { T("SacredLeggings"), (-2f, 0.09f, 0.91f) },
                { T("HallowedCowl"), (-2f, 0.20f, 0.80f) },
                { T("BioTechHood"), (-2f, 0.12f, 0.88f) },
                { T("BioTechGarment"), (-3f, 0.1f, 0.9f) },
                { T("BioTechLeggings"), (-3f, 0.08f, 0.92f) },
                { T("LifeBinderMask"), (-2f, 0.18f, 0.82f) },
                { T("LifeBinderBreastplate"), (-1f, 0.12f, 0.88f) },
                { T("LifeBinderGreaves"), (-2f, 0.1f, 0.9f) },
                { T("FallenPaladinGreaves"), (0f, 0.15f, 0.85f) },
                { T("WhisperingLeggings"), (0f, 0.15f, 0.85f) },
                { T("CelestialCrown"), (-2f, 0.20f, 0.80f) },
                { T("CelestialVestment"), (-3f, 0.25f, 0.75f) },
                { T("CelestialLeggings"), (-2f, 0.25f, 0.75f) },
                { T("DreamWeaversHood"), (-4f, 0.35f, 0.65f) },
                { T("DreamWeaversTabard"), (-2f, 0.2f, 0.8f) },
                { T("DreamWeaversTreads"), (-2f, 0.20f, 0.80f) },
                { T("TitanHeadgear"), (0f, 0.15f, 0.85f) },
                { T("CryomancersCrown"), (0f, 0.15f, 0.85f) },
            };
        }

        public override void SetDefaults(Item item)
        {
            // Armor defense overrides
            if (item.defense > 0)
            {
                if (ArmorDefenseOverrides.TryGetValue(item.type, out int newDefense))
                {
                    item.defense = newDefense;
                }
            }
            else if (item.damage > 0 && item.ModItem != null)
            {
                if (ModContent.GetInstance<ItemBalancerConfig>().genericweaponchanges)
                {
                    // Overall damage tweaks
                    if (item.ModItem.Mod.Name == "ThoriumMod")
                    {
                        item.damage = (int)Math.Round(item.damage * 1.3f);
                    }

                    // Special damage tweaks
                    if (WeaponItemTypeToInteger.TryGetValue(item.type, out int newDamage))
                    {
                        item.damage = newDamage;
                    }

                    // Apply some other tweaks
                    if (item.type == terrariansLastKnifeType)
                    {
                        item.shootSpeed = 16f;
                        item.scale = 1.7f;
                    }
                }

                // Tool power overrides
                if (ToolPowerOverrides.TryGetValue(item.type, out var toolAction))
                {
                    toolAction(item);
                }
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            if (item.defense <= 0)
                return;

            // Complex items with unique logic
            if (item.type == depthDiverHelmetType)
            {
                if (Main.netMode == NetmodeID.MultiplayerClient && Main.myPlayer != player.whoAmI)
                {
                    Player localPlayer = Main.LocalPlayer;
                    if (localPlayer.DistanceSQ(player.Center) < 62500f)
                    {
                        localPlayer.AddBuff(ModContent.BuffType<DepthDiverAura>(), 30, true, false);
                    }
                }
                var calamityPlayer = player.Calamity();
                if (!calamityPlayer.ZoneAbyss)
                {
                    if (player.breath <= player.breathMax + 2)
                    {
                        player.breath = player.breathMax + 3;
                    }
                }
                else
                {
                    player.moveSpeed += 0.2f;
                    player.statDefense += 10;
                    if (player.breath < player.breathMax - 25 && player.breath > 5)
                    {
                        Random rnd = new Random();
                        if (rnd.Next(1, 600) == 1)
                        {
                            player.breath = player.breath + 20;
                        }
                    }
                }
                player.GetCritChance(DamageClass.Generic) += 6f;
                return;
            }

            if (item.type == nagaSkinMaskType)
            {
                var calamityPlayer = player.Calamity();
                if (!calamityPlayer.ZoneAbyss)
                {
                    if (player.breath <= player.breathMax + 2)
                    {
                        player.breath = player.breathMax + 3;
                    }
                }
                player.statManaMax2 += 60;
                return;
            }

            // Throwing damage adjustments
            if (ThrowingDamageAdjustments.TryGetValue(item.type, out float throwingAdj))
            {
                player.GetDamage(DamageClass.Throwing) += throwingAdj;
                return;
            }

            // Mana regen delay only adjustments
            if (ManaRegenDelayOnly.TryGetValue(item.type, out float regenDelay))
            {
                player.manaRegenDelayBonus += regenDelay;
                return;
            }

            // Combo: mana regen delay + mana cost adjustments
            if (ManaCombos.TryGetValue(item.type, out var combo))
            {
                if (combo.regenDelay != 0f)
                    player.manaRegenDelayBonus += combo.regenDelay;
                player.manaCost += combo.manaCostAdd;
                player.manaCost *= combo.manaCostMult;
                return;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (ModContent.GetInstance<ItemBalancerConfig>().OmegaCore)
            {
                if (item.type == theOmegaCoreType)
                {
                    player.moveSpeed -= 0.3f;
                    player.maxRunSpeed /= 1.3f;
                }
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ModContent.ItemType<WhiteDwarfMask>() && body.type == ModContent.ItemType<WhiteDwarfGuard>() && legs.type == ModContent.ItemType<WhiteDwarfGreaves>())
            {
                return "WhiteDwarfSet";
            }
            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "WhiteDwarfSet")
            {
                player.GetThoriumPlayer().setWhiteDwarf = false;
                player.GetRagnarokModPlayer().WhiteDwarf = true;
                player.setBonus = Language.GetTextValue("Mods.RagnarokMod.Tooltips.WhiteDwarfSetBonus");
            }
        }

        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.CountsAsClass(DamageClass.Throwing) && item.damage >= 1 && player.GetRagnarokModPlayer().blightAccFix && Terraria.Utils.NextBool(Main.rand, 5))
            {
                ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
                //ThoriumItem thoriumItem = item.ModItem as ThoriumItem;
                float spread = 0.25f;
                float baseSpeed = velocity.Length();
                double num = Math.Atan2((double)velocity.X, (double)velocity.Y);
                double randomAngle = num + (double)(0.25f * spread);
                double randomAngle2 = num - (double)(0.25f * spread);
                float randomSpeed = Terraria.Utils.NextFloat(Main.rand) * 0.2f + 0.95f;
                int daggerdamage = (int)(0.75f * damage);
                if (daggerdamage > 2000)
                {
                    daggerdamage = 2000;
                }
                Projectile.NewProjectile(source, position.X, position.Y, baseSpeed * randomSpeed * (float)Math.Sin(randomAngle), baseSpeed * randomSpeed * (float)Math.Cos(randomAngle), ModContent.ProjectileType<BlightDagger>(), daggerdamage, knockback, player.whoAmI, 0f, 0f, 0f);
                Projectile.NewProjectile(source, position.X, position.Y, baseSpeed * randomSpeed * (float)Math.Sin(randomAngle2), baseSpeed * randomSpeed * (float)Math.Cos(randomAngle2), ModContent.ProjectileType<BlightDagger>(), daggerdamage, knockback, player.whoAmI, 0f, 0f, 0f);
            }
            return true;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == shinobiSigilType)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("basic damage"))
                    {
                        tooltips[i].Text = "25% " + Language.GetTextValue("Mods.RagnarokMod.Compat.BasicDamage");
                    }
                }
            }
            if (item.type == plagueLordFlaskType)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("basic damage"))
                    {
                        tooltips[i].Text = "75% " + Language.GetTextValue("Mods.RagnarokMod.Compat.BasicDamage");
                    }
                }
            }
            if (item.type == throwingGuideType)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("duplicated"))
                    {
                        tooltips[i].Text = "12.5% " + Language.GetTextValue("Mods.RagnarokMod.Compat.DupeDamage");
                    }
                }
                var newLine = new TooltipLine(Mod, "throwingguide", Language.GetTextValue("Mods.RagnarokMod.Compat.DupeCap") + " 50." + "\n" + Language.GetTextValue("Mods.RagnarokMod.Compat.NoGuideStack"))
                {
                    OverrideColor = Color.Red
                };
                tooltips.Add(newLine);
            }
            if (item.type == throwingGuideVol2Type)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("duplicated"))
                    {
                        tooltips[i].Text = "15% " + Language.GetTextValue("Mods.RagnarokMod.Compat.DupeDamage");
                    }
                }
                var newLine = new TooltipLine(Mod, "throwingguide", Language.GetTextValue("Mods.RagnarokMod.Compat.DupeCap") + " 100." + "\n" + Language.GetTextValue("Mods.RagnarokMod.Compat.NoGuideStack"))
                {
                    OverrideColor = Color.Red
                };
                tooltips.Add(newLine);
            }
            if (item.type == throwingGuideVol3Type)
            {
                for (int i = 0; i < tooltips.Count; i++)
                {
                    if (tooltips[i].Text.Contains("duplicated"))
                    {
                        tooltips[i].Text = "17.5% " + Language.GetTextValue("Mods.RagnarokMod.Compat.DupeDamage");
                    }
                }
                var newLine = new TooltipLine(Mod, "throwingguide", Language.GetTextValue("Mods.RagnarokMod.Compat.DupeCap") + " 200." + "\n" + Language.GetTextValue("Mods.RagnarokMod.Compat.NoGuideStack"))
                {
                    OverrideColor = Color.Red
                };
                tooltips.Add(newLine);
            }
        }
    }
}