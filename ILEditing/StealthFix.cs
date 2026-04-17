using CalamityMod;
using CalamityMod.Items.Armor;
using log4net.Util;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.ILEdits
{
    /// <summary>
    /// Provides a system for patching and correcting the stealth consumption logic in the Calamity Mod
    /// </summary>
    /// <remarks>This system injects an early return in UpdateRogueStealth immediately after
    /// ProvideStealthStatBonuses(), skipping all of Calamity's weapon checks and
    /// stealth consumption logic. Our ModPlayer below re-implements consumption
    /// correctly, handling useTime being less than useAnimation and very low use times under
    /// attack speed buffs.</remarks>
    public class StealthFixSystem : ModSystem
    {
        private static Mod Calamity => ModLoader.GetMod("CalamityMod");
        private static Assembly calamityAssembly => Calamity.GetType().Assembly;

        private static Type calPlayerType = null;
        private static MethodInfo updateRogueStealthMethod = null;
        private static ILHook stealthAnimationCheck_hook = null;
        private static MethodInfo stealthAvailableMethod = null;
        private static Hook stealthAvailable_hook = null;

        private static new Mod Mod = ModLoader.GetMod("CalamityMod");
        public override void OnModLoad()
        {
            bool loadCaught = false;
            while (!loadCaught)
            {
                if (Calamity != null)
                {
                    foreach (Type type in calamityAssembly.GetTypes())
                    {
                        if (type.Name == "CalamityPlayer")
                        {
                            calPlayerType = type;
                            break;
                        }
                    }

                    updateRogueStealthMethod = calPlayerType.GetMethod("UpdateRogueStealth", BindingFlags.Public | BindingFlags.Instance);
                    stealthAnimationCheck_hook = new ILHook(updateRogueStealthMethod, PatchStealthAnimationCheck);
                    stealthAnimationCheck_hook.Apply();
                    stealthAvailableMethod = calPlayerType.GetMethod("StealthStrikeAvailable", BindingFlags.Public | BindingFlags.Instance);
                    stealthAvailable_hook = new Hook(stealthAvailableMethod, StealthAvailableDetour);
                    stealthAvailable_hook.Apply();
                    loadCaught = true;
                }
            }
        }

        public override void Unload()
        {
            stealthAnimationCheck_hook?.Dispose();
            stealthAnimationCheck_hook = null;
            calPlayerType = null;
            updateRogueStealthMethod = null;
            stealthAvailable_hook?.Dispose();
            stealthAvailable_hook = null;
        }
        private delegate bool StealthAvailableDelegate(ModPlayer self);

        private static bool StealthAvailableDetour(StealthAvailableDelegate orig, ModPlayer self)
        {
            // If orig says yes, always yes
            if (orig(self))
                return true;

            // If we're mid-clockwork stealth animation, keep returning true for sub-uses
            // even though stealth has already been consumed
            var fix = self.Player.GetModPlayer<StealthFixPlayer>();
            return fix.isStealthAnimation;
        }

        private static void PatchStealthAnimationCheck(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            // Find the call to ProvideStealthStatBonuses()
            // And kill it all after that.
            if (!c.TryGotoNext(MoveType.After,
                i => i.MatchCall(out var mr) && mr.Name == "ProvideStealthStatBonuses"
            ))
            {
                Mod.Logger.Error("StealthFix: could not find ProvideStealthStatBonuses call. Patch not applied.");
                return;
            }

            c.Emit(OpCodes.Ret);

            Mod.Logger.Info("StealthFix: patch applied successfully.");
        }
    }
    /// <summary>
    /// Provides corrected stealth consumption behavior for rogue weapons, ensuring consistent stealth usage across all
    /// scenarios when using Calamity Mod's rogue mechanics.
    /// </summary>
    /// <remarks>This player class should be used in conjunction with Calamity Mod to address edge cases where
    /// stealth is not consumed when using weapons with very low use times/animations or when useTime is less than useAnimation.
    /// It operates after Calamity's own stealth update logic, ensuring that stealth is consumed appropriately
    /// for all valid stealth strikes.</remarks>
    public class StealthFixPlayer : ModPlayer
    {
        public bool isStealthAnimation = false;
        private int lastHeldItemType = 0;

        private bool consumedThisSwing = false;

        public override void PostUpdateMiscEffects()
        {
            if (Player.HeldItem.type != lastHeldItemType)
            {
                isStealthAnimation = false;
                consumedThisSwing = false;
                lastHeldItemType = Player.HeldItem.type;
            }

            if (Player.itemAnimation == Player.itemAnimationMax)
            {
                consumedThisSwing = false;
                isStealthAnimation = false;
            }

            ApplyRogueUseTimeFix();
        }

        private void ApplyRogueUseTimeFix()
        {
            var cal = Player.Calamity();

            if (!cal.wearingRogueArmor)
                return;

            Item it = Player.HeldItem;

            bool hasDamage = it.damage > 0;
            bool hasHitboxes = !it.noMelee || it.shoot > ProjectileID.None;
            bool hasNonWeaponFunction = it.pick > 0 || it.axe > 0 || it.hammer > 0
                                     || it.createTile != -1 || it.channel;
            bool playerUsingWeapon = hasDamage && hasHitboxes && !hasNonWeaponFunction;

            if ((it.IsAir || (!it.CountsAsClass<RogueDamageClass>()) && cal.GemTechSet && cal.GemTechState.IsRedGemActive) || (it.CountsAsClass<SummonDamageClass>() && cal.forbiddenCirclet))
                playerUsingWeapon = false;

            if (!playerUsingWeapon)
            {
                // Non-weapon or excluded item,     drain stealth if it's not a rogue item
                if (Player.itemAnimation > 0 && !it.CountsAsClass<RogueDamageClass>()
                    && !(cal.GemTechSet && cal.GemTechState.IsRedGemActive))
                    cal.rogueStealth = 0f;
                return;
            }

            if (it.type == ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.MoltenAmputator>())
                return;
            if (it.type == ModContent.ItemType<CalamityMod.Items.Weapons.Rogue.DoomsdayDevice>())
                return;

            if (Player.itemAnimation <= 0)
            {
                isStealthAnimation = false;
                return;
            }

            float attackSpeed = Player.GetAttackSpeed<RogueDamageClass>()
                              * Player.GetAttackSpeed(DamageClass.Throwing)
                              * Player.GetAttackSpeed(DamageClass.Generic);

            bool isClockwork = it.useTime < it.useAnimation
                && it.useAnimation / it.useTime >= 2;
            if (isClockwork)
            {
                int strikesPerAnimation = it.useAnimation / it.useTime;
                int animDuration = Math.Max(Player.itemAnimationMax, strikesPerAnimation);
                int subUseDuration = Math.Max((animDuration + strikesPerAnimation - 1) / strikesPerAnimation, 1);

                if (!cal.stealthStrikeThisFrame && !consumedThisSwing)
                {
                    isStealthAnimation = cal.StealthStrikeAvailable();
                    if (isStealthAnimation)
                        cal.ConsumeStealthByAttacking();
                    consumedThisSwing = true;
                }

                if (isStealthAnimation)
                {
                    int frameInAnimation = Player.itemAnimationMax - Player.itemAnimation;
                    // Set the flag one frame early so it's present during ItemCheck
                    // when the sub-use projectile actually spawns
                    if ((frameInAnimation + 1) % subUseDuration == 0)
                        cal.stealthStrikeThisFrame = true;
                }
            }
            else
            {
                if (!cal.stealthStrikeThisFrame && !consumedThisSwing)
                {
                    if (cal.StealthStrikeAvailable())
                        cal.ConsumeStealthByAttacking();
                    else
                        cal.rogueStealth = 0f;

                    consumedThisSwing = true;
                }
            }
        }
    }
}