using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.CalPlayer;
using CalamityMod.Cooldowns;
using Microsoft.Xna.Framework;
using RagnarokMod.Buffs;
using RagnarokMod.Common.Configs;
using RagnarokMod.ILEditing;
using RagnarokMod.Items.BardItems.Accessories;
using RagnarokMod.Items.BardItems.Armor;
using RagnarokMod.Items.HealerItems;
using RagnarokMod.Items.HealerItems.Accessories;
using RagnarokMod.Projectiles.Accessories;
using RagnarokMod.Riffs;
using RagnarokMod.Sounds;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Buffs;
using ThoriumMod.Buffs.Bard;
using ThoriumMod.Buffs.Healer;
using ThoriumMod.Buffs.Thrower;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BossThePrimordials.Dream;
using ThoriumMod.Items.BossThePrimordials.Rhapsodist;
using ThoriumMod.Projectiles.Healer;
using ThoriumMod.Projectiles.Thrower;
using ThoriumMod.Sounds;
using ThoriumMod.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace RagnarokMod.Utils
{
    public class RagnarokModPlayer : ModPlayer
    {
        private static Mod thorium = ModLoader.GetMod("ThoriumMod");
        private static Mod calamity = ModLoader.GetMod("CalamityMod");


        public float oneTimeDamageReduction = 0;
        public bool brimstoneFlamesOnHit = false;
        public byte activeRiffType;
        public float savedMusicVolume = -1f;
        public static List<Action<Player, Player>> OnHealEffects = [];
        public bool stringInstrumentUsed = false;
        public bool batpoop = false;

        public bool auricBardSet = false;
        public bool auricHealerSet = false;
        public bool tarraHealer = false;
        public bool tarraBard = false;
        public bool daedalusHealer = false;
        public bool daedalusBard = false;
        public bool godslayerBard = false;
        public bool silvaHealer = false;
        public bool bloodflareHealer = false;
        public bool bloodflareBard = false;
        public bool intergelacticBard = false;
        public bool intergelacticHealer = false;
        public bool nightfallen = false;
        public bool WhiteDwarf = false;
        public bool leviathanHeart = false;
        public bool sirenScale = false;
        public ThoriumItem lastHeldItem;
        public int origLifeCost = 0;
        private int lastSeenHeal = 0;
        public int godslayerBardcurrentemp = 0;
        public int godslayerBardcurrentemplevel = 0;
        public int intergelacticBardcurrentemp = 0;
        public int intergelacticBardcurrentemplevel = 0;
        public int asteroidexhaustioncounter = 0;
        private int bloodflarebloodlust = 0;
        private int bloodflarepointtimer = 0;
        public int elementalReaperCD = 0;
        private const int maxbloodlustpoints = 150;
        public int elementalReaperIndex = 0;
        public bool accShinobiSigilFix = false;
        public bool blightAccFix = false;
        public bool throwGuideFix = false;
        public bool throwGuide2Fix = false;
        public bool throwGuide3Fix = false;
        public bool auricBoost;

        public SlotId fretSlot;
        public bool fretPlaying;

        public void EnsureMiniAnahita()
        {
            if (Player.whoAmI != Main.myPlayer)
                return;

            int type = ModContent.ProjectileType<MiniAnahitaCompanion>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == Player.whoAmI && p.type == type)
                {
                    return; // already exists
                }
            }

            Item scale = null;
            foreach (Item item in Player.armor)
            {
                if (item.type == ModContent.ItemType<SirenScale>())
                {
                    scale = item;
                    break;
                }
            }

            if (scale == null)
                return;
            int index = Projectile.NewProjectile(
                Player.GetSource_Accessory(scale),
                Player.Center,
                Microsoft.Xna.Framework.Vector2.Zero,
                type,
                // Damage scaled from player's bard damage
                (int)(Player.HeldItem.damage),
                2f,
                Player.whoAmI
            );
        }
        public override void OnHurt(Player.HurtInfo info)
        {
            if (bloodflareHealer || bloodflareBard)
            {
                RemoveBloodFlareBloodlustPoints(25);
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (oneTimeDamageReduction != 0)
            {
                modifiers.FinalDamage *= (1 - oneTimeDamageReduction);
                oneTimeDamageReduction = 0;
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (ThoriumHotkeySystem.ArmorKey.JustPressed)
            {
                if (auricBardSet && !((ModPlayer)this).Player.HasBuff(ModContent.BuffType<CreativeBlock>()) && BardItem.ConsumeInspiration(((ModPlayer)this).Player, 20))
                {
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<SoloistsHatSetBuff>(), 600, true, false);
                    ModContent.GetInstance<InspiratorsHelmet>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                    ((ModPlayer)this).Player.AddBuff(ModContent.BuffType<CreativeBlock>(), 2400, true, false);
                    for (int l = 0; l < 5; l++)
                    {
                        Vector2 offset3 = new Vector2(Main.rand.Next(-50, 51), Main.rand.Next(-50, 51));
                        Dust obj4 = Dust.NewDustDirect(((Entity)((ModPlayer)this).Player).position + offset3, ((Entity)((ModPlayer)this).Player).width, ((Entity)((ModPlayer)this).Player).height, DustID.GemTopaz, 0f, 0f, 0, default(Color), 2f);
                        obj4.noGravity = true;
                        obj4.velocity = -offset3 * 0.075f;
                    }
                }

                if (auricHealerSet && !base.Player.HasBuff(ModContent.BuffType<DreamWeaversHoodDebuff>()) && base.Player.CheckMana(200, true, false))
                {
                    base.Player.AddBuff(ModContent.BuffType<DreamWeaversHoodDreamBuff>(), 600, true, false);
                    base.Player.AddBuff(ModContent.BuffType<DreamWeaversMaskBuff>(), 900, true, false);
                    base.Player.AddBuff(ModContent.BuffType<DreamWeaversHoodDebuff>(), 3660, true, false);
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    for (int n = 0; n < 20; n++)
                    {
                        Dust.NewDustDirect(base.Player.position, base.Player.width, base.Player.height, DustID.ShadowbeamStaff, (float)Main.rand.Next(-8, 9), (float)Main.rand.Next(-8, 9), 0, default(Color), 2f).noGravity = true;
                    }
                    for (int k2 = 0; k2 < 10; k2++)
                    {
                        Dust.NewDustDirect(base.Player.position, base.Player.width, base.Player.height, DustID.DemonTorch, (float)Main.rand.Next(-8, 9), (float)Main.rand.Next(-8, 9), 0, default(Color), 1.4f).noGravity = true;
                    }
                }

                if (daedalusHealer && !base.Player.HasBuff(ModContent.BuffType<AntarcticExhaustionDebuff>()) && base.Player.CheckMana(100, true, false))
                {
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                    this.Player.AddBuff(ModContent.BuffType<AntarcticExhaustionDebuff>(), 3600, true, false);
                    this.Player.AddBuff(ModContent.BuffType<AntarcticReinforcementBuff>(), 900, true, false);
                    thoriumPlayer.shieldHealth += 50;
                    this.Player.statLife += 50;
                }

                if (daedalusBard && !base.Player.HasBuff(ModContent.BuffType<AntarcticExhaustionDebuff>()) && BardItem.ConsumeInspiration(((ModPlayer)this).Player, 12))
                {
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    ModContent.GetInstance<DaedalusHeadBard>().NetApplyEmpowerments(base.Player, 0);
                    this.Player.AddBuff(ModContent.BuffType<AntarcticExhaustionDebuff>(), 3600, true, false);
                    this.Player.AddBuff(ModContent.BuffType<AntarcticCreativityBuff>(), 900, true, false);
                }

                if (intergelacticBard && asteroidexhaustioncounter == 0)
                {
                    asteroidexhaustioncounter = 1800;
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    base.Player.GetModPlayer<ThoriumPlayer>().HealInspiration(100, false);
                }
                if (intergelacticHealer && asteroidexhaustioncounter == 0)
                {
                    asteroidexhaustioncounter = 1800;
                    SoundEngine.PlaySound(ThoriumSounds.PassbySurge, (Vector2?)null, (SoundUpdateCallback)null);
                    this.Player.statLife += 50;
                    ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                    thoriumPlayer.shieldHealth += 100;
                }
            }
        }

        public override void PreUpdateBuffs()
        {
            if (base.Player.HasBuff(ModContent.BuffType<LastStandBuff>()))
            {
                int bufftypeindex = -100;
                for (int i = 0; i < base.Player.buffType.Length; i++)
                {
                    if (base.Player.buffType[i] == ModContent.BuffType<LastStandBuff>())
                    {
                        bufftypeindex = i;
                    }
                }
                if (base.Player.buffTime[bufftypeindex] > 480 && bufftypeindex != -100)
                {
                    base.Player.buffTime[bufftypeindex] = 480;
                }
            }
            else if (base.Player.HasBuff(ModContent.BuffType<OceansBuffer>()))
            {
                int bufftypeindex = -100;
                for (int i = 0; i < base.Player.buffType.Length; i++)
                {
                    if (base.Player.buffType[i] == ModContent.BuffType<OceansBuffer>())
                    {
                        bufftypeindex = i;
                    }
                }
                if (base.Player.buffTime[bufftypeindex] > 360 && bufftypeindex != -100)
                {
                    base.Player.buffTime[bufftypeindex] = 360;
                }
            }
        }

        public override void PostUpdateMiscEffects()
        {
            if (tarraBard)
            {
                ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                thoriumPlayer.setOrnate = true;
                thoriumPlayer.bardBuffDurationX *= 1.5f;
                thoriumPlayer.bardBuffDurationXDisplay *= 1.5f;
            }
            if (godslayerBard && base.Player.HeldItem.DamageType == ThoriumDamageBase<BardDamage>.Instance)
            {
                Random rnd = new Random();
                int num = rnd.Next(180);
                if (num == 0)
                {
                    godslayerBardcurrentemp = rnd.Next(1, 18);
                    godslayerBardcurrentemplevel = rnd.Next(1, 4);
                    ModContent.GetInstance<GodSlayerHeadBard>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                    godslayerBardcurrentemp = 0;
                    godslayerBardcurrentemplevel = 0;
                }
            }
            if (silvaHealer)
            {
                ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                thoriumPlayer.setBlooming = true;
                thoriumPlayer.setLifeBinder = true;
                Random rnd = new Random();
                int triggerchance = 400;
                if (base.Player.statLife >= 0.8 * base.Player.statLifeMax2 && base.Player.statLife < 0.9 * base.Player.statLifeMax2)
                {
                    triggerchance = 360;
                }
                else if (base.Player.statLife >= 0.5 * base.Player.statLifeMax2 && base.Player.statLife < 0.8 * base.Player.statLifeMax2)
                {
                    triggerchance = 240;
                }
                else if (base.Player.statLife >= 0.2 * base.Player.statLifeMax2 && base.Player.statLife < 0.5 * base.Player.statLifeMax2)
                {
                    triggerchance = 160;
                }
                else if (base.Player.statLife < 0.2 * base.Player.statLifeMax2)
                {
                    triggerchance = 80;
                }
                int num = rnd.Next(triggerchance);
                if (num == 1)
                {
                    if (thoriumPlayer.shieldHealth + 7 <= 50)
                    {
                        thoriumPlayer.shieldHealth += 7;
                        this.Player.statLife += 7;
                    }
                }
                else if (num == 2)
                {
                    this.Player.Heal(12);
                }
            }
            if (bloodflareHealer)
            {
                if (bloodflarepointtimer == 0)
                {
                    bloodflarepointtimer = 30;
                    AddBloodFlareBloodlustPoints(1);
                }
                else
                {
                    bloodflarepointtimer--;
                }
                if (bloodflarebloodlust >= 100)
                {
                    base.Player.moveSpeed += 0.2f;
                    ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                    thoriumPlayer.healBonus += 3;
                    base.Player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) += 0.20f;
                    base.Player.GetCritChance(ThoriumDamageBase<HealerDamage>.Instance) += 20f;
                }
            }
            if (bloodflareBard)
            {
                if (bloodflarepointtimer == 0)
                {
                    bloodflarepointtimer = 30;
                    AddBloodFlareBloodlustPoints(1);
                }
                else
                {
                    bloodflarepointtimer--;
                }
                if (bloodflarebloodlust >= 100)
                {
                    base.Player.moveSpeed += 0.25f;
                    ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                    thoriumPlayer.inspirationRegenBonus += 0.1f;
                    base.Player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.20f;
                    base.Player.GetCritChance(ThoriumDamageBase<BardDamage>.Instance) += 20f;
                    thoriumPlayer.bardBuffDuration += 180;
                }
            }
            if (intergelacticBard || intergelacticHealer)
            {
                if (asteroidexhaustioncounter > 0)
                {
                    asteroidexhaustioncounter--;
                    if (intergelacticBard)
                    {
                        if (asteroidexhaustioncounter == 1799)
                        {
                            ModContent.GetInstance<IntergelacticRobohelm>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                        }
                    }
                }
                else
                {
                    asteroidexhaustioncounter = 0;
                }
            }

            if (batpoop)
            {
                long effectivecoinvalue = 0;
                // for every possible coin the 4 coin slots
                for (int i = 50; i < 54; i++)
                {
                    // is it a coin?
                    if (base.Player.inventory[i].type >= ItemID.CopperCoin && base.Player.inventory[i].type <= ItemID.PlatinumCoin)
                    {
                        effectivecoinvalue += (long)(Math.Pow(100, base.Player.inventory[i].type - 71) * base.Player.inventory[i].stack);
                    }
                }
                float damagemodifier = (float)Math.Pow(((float)effectivecoinvalue / 10000000), 0.2) / 10;
                base.Player.GetDamage(DamageClass.Generic) += damagemodifier;
            }

            if (elementalReaperCD > 0)
            {
                elementalReaperCD--;
            }

            OnHealEffects.Add((healer, target) =>
            {
                var ragHealer = healer.GetModPlayer<RagnarokModPlayer>();
                if (!ragHealer.leviathanHeart)
                    return;

                var healerThorium = healer.GetModPlayer<ThoriumPlayer>();
                if (healerThorium.darkAura)
                    return;


                if (target.whoAmI == Main.myPlayer)
                {
                    target.AddBuff(ModContent.BuffType<LeviathanHeartBubble>(), 5 * 60);
                }
                else
                {
                    ModPacket packet = ModContent.GetInstance<RagnarokMod>().GetPacket();
                    packet.Write((byte)0);
                    packet.Write((byte)target.whoAmI);
                    packet.Send();
                }
            });

            // Fixes the RogueUseTime problem, that occurs, because Thorium implements features like buffs that can effect item use times, which is incompatible with the Calamity Rogue usetime / attackspeed check.
            this.ApplyRogueUseTimeFix();

            // Fixes problems with thorium thrower accessories by disablng the thoriumplayer flags for the setbonuses and reimplementing it
            ThoriumPlayer thoriumPlayer2 = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
            if (thoriumPlayer2.accShinobiSigil)
            {
                thoriumPlayer2.accShinobiSigil = false;
                accShinobiSigilFix = true;
            }
            if (thoriumPlayer2.blightAcc)
            {
                thoriumPlayer2.blightAcc = false;
                blightAccFix = true;
            }
            if (thoriumPlayer2.throwGuide)
            {
                thoriumPlayer2.throwGuide = false;
                throwGuideFix = true;
            }
            if (thoriumPlayer2.throwGuide2)
            {
                thoriumPlayer2.throwGuide2 = false;
                throwGuide2Fix = true;
            }
            if (thoriumPlayer2.throwGuide3)
            {
                thoriumPlayer2.throwGuide3 = false;
                throwGuide3Fix = true;
            }

            // Applies the damage modifiers from the Configs
            // This code always has to be a the end of this function (except debug functions) to properly calculate the effective damage!!!
            base.Player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) *= ModContent.GetInstance<ClassBalancerConfig>().BardDamageModifier;
            base.Player.GetDamage(ThoriumDamageBase<HealerDamage>.Instance) *= ModContent.GetInstance<ClassBalancerConfig>().HealerDamageModifier;
            base.Player.GetDamage(DamageClass.Throwing) *= ModContent.GetInstance<ClassBalancerConfig>().RogueDamageModifier;
            base.Player.GetDamage(DamageClass.Magic) *= ModContent.GetInstance<ClassBalancerConfig>().MagicDamageModifier;
            base.Player.GetDamage(DamageClass.Summon) *= ModContent.GetInstance<ClassBalancerConfig>().SummonDamageModifier;
            base.Player.GetDamage(DamageClass.Ranged) *= ModContent.GetInstance<ClassBalancerConfig>().RangedDamageModifier;
            base.Player.GetDamage(DamageClass.Melee) *= ModContent.GetInstance<ClassBalancerConfig>().MeleeDamageModifier;

            // Apply Healing Multiplier
            thoriumPlayer2.healBonus = (int)Math.Round(thoriumPlayer2.healBonus * ModContent.GetInstance<ClassBalancerConfig>().HealingBonusModifier);
        }

        // Fixes the RogueUseTime problem, that occurs, because Thorium implements features like buffs that can effect item use times, which is incompatible with the Calamity Rogue Stealthmode usetime / attackspeed check.
        private void ApplyRogueUseTimeFix()
        {
            var calamityPlayer = base.Player.Calamity();
            Item it = !Main.mouseItem.IsAir ? Main.mouseItem : Player.HeldItem;
            if (!calamityPlayer.wearingRogueArmor || it.useAnimation == it.useTime) // When the player does not wear a rogue armor or the item animation and usetime is the same, the fix does not have to be applied.
            {
                return;
            }

            bool flag = it.damage > 0;
            bool hasHitboxes = !it.noMelee || it.shoot > ProjectileID.None;
            bool flag2 = it.pick > 0;
            bool isAxe = it.axe > 0;
            bool isHammer = it.hammer > 0;
            bool isPlaced = it.createTile != -1;
            bool isChannelable = it.channel;
            bool hasNonWeaponFunction = flag2 || isAxe || isHammer || isPlaced || isChannelable;
            bool playerUsingWeapon = flag && hasHitboxes && !hasNonWeaponFunction;
            if ((it.IsAir || !it.CountsAsClass<RogueDamageClass>()) && calamityPlayer.GemTechSet && calamityPlayer.GemTechState.IsRedGemActive)
            {
                return;
            }

            // Besides Rogue attackspeed, Thoriums own attack speed changes for throwing and general class have to be calculated into the attackspeed.
            float attackSpeed = base.Player.GetAttackSpeed<RogueDamageClass>() * base.Player.GetAttackSpeed(DamageClass.Throwing) * base.Player.GetAttackSpeed(DamageClass.Generic);
            int adjustedUseTime = (int)(it.useTime / attackSpeed); // Calculates the effective use time

            bool animationCheck = (base.Player.itemTime == adjustedUseTime);
            if (!calamityPlayer.stealthStrikeThisFrame && animationCheck)
            {
                if (calamityPlayer.StealthStrikeAvailable())
                {
                    calamityPlayer.ConsumeStealthByAttacking();
                    return;
                }
                calamityPlayer.rogueStealth = 0f;
            }
        }

        public override void PostUpdateEquips()
        {
            RiffLoader.UpdateRiffs(Player);
            if (base.Player.armor[0].type == thorium.Find<ModItem>("SandStoneHelmet").Type
           && base.Player.armor[1].type == thorium.Find<ModItem>("SandStoneMail").Type
           && base.Player.armor[2].type == thorium.Find<ModItem>("SandStoneGreaves").Type)
            {
                AddStealth(60);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("FlightMask").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("FlightMail").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("FlightBoots").Type)
            {
                AddStealth(70);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("BronzeHelmet").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("BronzeBreastplate").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("BronzeGreaves").Type)
            {
                AddStealth(85);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("PlagueDoctorsMask").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("PlagueDoctorsGarb").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("PlagueDoctorsLeggings").Type)
            {
                AddStealth(100);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("FungusHat").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("FungusGuard").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("FungusLeggings").Type)
            {
                AddStealth(105);
            }
            else if ((base.Player.armor[0].type == thorium.Find<ModItem>("HallowedGuise").Type
            || base.Player.armor[0].type == thorium.Find<ModItem>("AncientHallowedGuise").Type)
            && (base.Player.armor[1].type == ItemID.HallowedPlateMail || base.Player.armor[1].type == ItemID.AncientHallowedPlateMail)
            && (base.Player.armor[2].type == ItemID.HallowedGreaves || base.Player.armor[2].type == ItemID.AncientHallowedGreaves))
            {
                AddStealth(110);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("LichCowl").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("LichCarapace").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("LichTalon").Type)
            {
                AddStealth(110);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("ShadeMasterMask").Type
            && base.Player.armor[1].type == thorium.Find<ModItem>("ShadeMasterGarb").Type
            && base.Player.armor[2].type == thorium.Find<ModItem>("ShadeMasterTreads").Type)
            {
                AddStealth(110);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("WhiteDwarfMask").Type
           && base.Player.armor[1].type == thorium.Find<ModItem>("WhiteDwarfGuard").Type
           && base.Player.armor[2].type == thorium.Find<ModItem>("WhiteDwarfGreaves").Type)
            {
                AddStealth(115);
            }
            else if (base.Player.armor[0].type == thorium.Find<ModItem>("TideTurnersGaze").Type
           && base.Player.armor[1].type == thorium.Find<ModItem>("TideTurnerBreastplate").Type
           && base.Player.armor[2].type == thorium.Find<ModItem>("TideTurnerGreaves").Type)
            {
                AddStealth(120);
            }
        }

        public override void PostUpdate()
        {
            if (!fretPlaying)
            {
                return;
            }
            if (SoundEngine.TryGetActiveSound(fretSlot, out var sound))
            {
                sound.Position = Player.Center;
            }
            else
            {
                fretPlaying = false;
            }
        }

        // Function to add stealth to thorium throwing armors
        public void AddStealth(int stealth)
        {
            CalamityPlayer calamityPlayer = base.Player.Calamity();
            calamityPlayer.rogueStealthMax += ((float)stealth / 100);
            base.Player.Calamity().wearingRogueArmor = true;
            base.Player.setBonus = base.Player.setBonus + "\n+" + stealth + " maximum stealth";
        }

        public void InstrumentRotationModifier(float modifier)
        {
            ThoriumPlayer thoriumPlayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
            if (!thoriumPlayer.bardBigInstrumentRotationShift)
            {
                thoriumPlayer.bardBigInstrumentRotation -= (1f - modifier) * thoriumPlayer.bardBigInstrumentRotationSpeed;
            }
            else
            {
                thoriumPlayer.bardBigInstrumentRotation += (1f - modifier) * thoriumPlayer.bardBigInstrumentRotationSpeed;
            }
        }

        public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (brimstoneFlamesOnHit)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            }
            if (this.bloodflareHealer && projectile.DamageType == ThoriumDamageBase<HealerDamage>.Instance)
            {
                ApplyBloodFlareOnHit(target, damageDone);
            }
            if (this.bloodflareBard && projectile.DamageType == ThoriumDamageBase<BardDamage>.Instance)
            {
                ApplyBloodFlareOnHit(target, damageDone);
            }
            if (this.intergelacticBard)
            {
                ModProjectile modProjectile = projectile.ModProjectile;
                if (((modProjectile != null) ? modProjectile.Mod.Name : null) == "CatalystMod")
                {
                    ModProjectile modProjectile2 = projectile.ModProjectile;
                    if (((modProjectile2 != null) ? modProjectile2.Name : null) == "AstralRocksProj")
                    {
                        Random rnd = new Random();
                        if (rnd.Next(0, 20) == 0)
                        {
                            intergelacticBardcurrentemp = rnd.Next(1, 18);
                            intergelacticBardcurrentemplevel = rnd.Next(1, 4);
                            ModContent.GetInstance<IntergelacticRobohelm>().NetApplyEmpowerments(((ModPlayer)this).Player, 0);
                            intergelacticBardcurrentemp = 0;
                            intergelacticBardcurrentemplevel = 0;
                        }
                    }
                }
            }
            if (intergelacticHealer)
            {
                ModProjectile modProjectile = projectile.ModProjectile;
                if (((modProjectile != null) ? modProjectile.Mod.Name : null) == "CatalystMod")
                {
                    ModProjectile modProjectile2 = projectile.ModProjectile;
                    if (((modProjectile2 != null) ? modProjectile2.Name : null) == "AstralRocksProj")
                    {
                        Random rnd = new Random();
                        if (rnd.Next(0, 20) == 0)
                        {
                            Projectile astralrock = modProjectile2.Projectile;
                            Projectile.NewProjectile(astralrock.GetSource_FromThis(), astralrock.Center, Vector2.Zero, ModContent.ProjectileType<HeartWandPro2>(), 0, 0, astralrock.owner);
                        }
                    }
                }
            }
            if (projectile.IsThrown())
            {
                if (accShinobiSigilFix)
                {
                    ThoriumPlayer thoriumplayer = ThoriumMod.Utilities.PlayerHelper.GetThoriumPlayer(base.Player);
                    if (hit.Crit)
                    {
                        int shinobi = ModContent.ProjectileType<ShinobiSigilPro>();
                        if (projectile.type != shinobi)
                        {
                            thoriumplayer.accShinobiSigilCrit++;
                            if (thoriumplayer.accShinobiSigilCrit >= 2)
                            {
                                thoriumplayer.accShinobiSigilCrit = 0;
                                SoundEngine.PlaySound(SoundID.Item103, new Vector2?(projectile.Center), null);
                                this.Player.AddBuff(ModContent.BuffType<ThrowingSpeed>(), 300, true, false);
                                int shinobiDamage = (int)((float)projectile.damage * 0.25f);
                                if (shinobiDamage > 500)
                                {
                                    shinobiDamage = 500;
                                }
                                IEntitySource source = projectile.GetSource_OnHit(target, null);
                                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0f, -1f, shinobi, shinobiDamage, 1f, projectile.owner, 0f, 0f, 0f);
                                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 0.75f, 1f, shinobi, shinobiDamage, 1f, projectile.owner, 0f, 0f, 0f);
                                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, -1f, -0.75f, shinobi, shinobiDamage, 1f, projectile.owner, 0f, 0f, 0f);
                                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, 1f, -0.75f, shinobi, shinobiDamage, 1f, projectile.owner, 0f, 0f, 0f);
                                Projectile.NewProjectile(source, target.Center.X, target.Center.Y, -0.75f, 1f, shinobi, shinobiDamage, 1f, projectile.owner, 0f, 0f, 0f);
                            }
                        }
                    }
                    else
                    {
                        thoriumplayer.accShinobiSigilCrit = 0;
                    }
                }
                int guideDamage = 0;
                if (throwGuide3Fix)
                {
                    guideDamage = (int)((double)projectile.damage * 0.175);
                    if (guideDamage > 200)
                    {
                        guideDamage = 200;
                    }
                }
                else if (throwGuide2Fix)
                {
                    guideDamage = (int)((double)projectile.damage * 0.15);
                    if (guideDamage > 100)
                    {
                        guideDamage = 100;
                    }
                }
                else if (throwGuideFix)
                {
                    guideDamage = (int)((double)projectile.damage * 0.125);
                    if (guideDamage > 50)
                    {
                        guideDamage = 50;
                    }
                }
                if (guideDamage > 0)
                {
                    IEntitySource source = projectile.GetSource_OnHit(target, null);
                    Projectile.NewProjectile(source, target.Center, Vector2.Zero, ModContent.ProjectileType<ThrowingGuideFollowup>(), guideDamage, 0f, projectile.owner, (float)target.whoAmI, 0f, 0f);
                }
            }
            if (WhiteDwarf && hit.Crit)
            {
                int damage;
                if (Player.GetWeaponDamage(Player.HeldItem) > target.damage)
                {
                    damage = Player.GetWeaponDamage(Player.HeldItem);
                }
                else
                {
                    damage = target.damage;
                }
                SoundEngine.PlaySound(SoundID.Item92, target.position);
                Projectile.NewProjectile(projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<WhiteFlare>(), damage, 0.0f, Main.myPlayer, 0.0f, 0.0f, 0.0f);
            }
            OnHitNPCWithAny(target, hit, damageDone);
        }

        public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (brimstoneFlamesOnHit)
            {
                target.AddBuff(ModContent.BuffType<BrimstoneFlames>(), 300);
            }
            if (this.bloodflareHealer && item.DamageType == ThoriumDamageBase<HealerDamage>.Instance)
            {
                ApplyBloodFlareOnHit(target, damageDone);
            }
            if (this.bloodflareBard && item.DamageType == ThoriumDamageBase<BardDamage>.Instance)
            {
                ApplyBloodFlareOnHit(target, damageDone);
            }
            OnHitNPCWithAny(target, hit, damageDone);
        }

        public void ApplyBloodFlareOnHit(NPC target, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<BurningBlood>(), 240, false);
            if (bloodflarebloodlust > 100 && this.bloodflareHealer)
            {
                this.Player.Heal((int)Math.Sqrt(damageDone / 100));
            }
        }

        public void RemoveBloodFlareBloodlustPoints(int points)
        {
            if (bloodflarebloodlust - points >= 0)
            {
                bloodflarebloodlust -= points;
            }
            else
            {
                bloodflarebloodlust = 0;
            }
        }

        public void AddBloodFlareBloodlustPoints(int points)
        {
            if (bloodflarebloodlust + points <= maxbloodlustpoints)
            {
                bloodflarebloodlust += points;
            }
            else
            {
                bloodflarebloodlust = maxbloodlustpoints;
            }
            if (bloodflarebloodlust == 100)
            {
                SoundEngine.PlaySound(SoundID.NPCHit13, (Vector2?)null, (SoundUpdateCallback)null);
            }
        }

        private void SetFalse()
        {
            this.brimstoneFlamesOnHit = false;
            this.tarraHealer = false;
            this.tarraBard = false;
            this.daedalusHealer = false;
            this.daedalusBard = false;
            this.godslayerBard = false;
            this.silvaHealer = false;
            this.bloodflareHealer = false;
            this.bloodflareBard = false;
            this.auricBardSet = false;
            this.auricHealerSet = false;
            WhiteDwarf = false;
            nightfallen = false;
            this.batpoop = false;
            this.accShinobiSigilFix = false;
            this.blightAccFix = false;
            this.throwGuideFix = false;
            this.throwGuide2Fix = false;
            this.throwGuide3Fix = false;
            this.intergelacticBard = false;
            this.intergelacticHealer = false;
            auricBoost = false;
            leviathanHeart = false;
            sirenScale = false;
        }
        public override void ResetEffects()
        {
            if (lastHeldItem != null)
            {
                lastHeldItem.radiantLifeCost = origLifeCost;
                lastHeldItem = null;
                origLifeCost = 0;
            }
            if (this.tarraBard == false && this.tarraHealer == false)
                bloodflarebloodlust = 0;
            SetFalse();
        }

        public override void UpdateDead()
        {
            SetFalse();
        }

        public override void ModifyWeaponKnockback(Item item, ref StatModifier knockback)
        {
            if (auricBoost)
                knockback.Flat += item.knockBack * 0.5f;
        }

        public void OnHitNPCWithAny(NPC npc, NPC.HitInfo hit, int damageDone)
        {
            if (nightfallen)
            {
                npc.AddBuff(ModContent.BuffType<NightfallenDebuff>(), 240);
            }
        }

        public override void Unload()
        {
            OnHealEffects?.Clear();
            OnHealEffects = null;
            base.Unload();
        }
    }
}
