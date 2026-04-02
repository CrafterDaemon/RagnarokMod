using CalamityMod;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class RadiantMagic : DamageClass
    {
        public override bool GetPrefixInheritance(DamageClass damageClass)
            => damageClass == Magic || damageClass == ModContent.GetInstance<HealerDamage>()
            || damageClass == AprilChaosSystem.VoidMagic || damageClass == AprilChaosSystem.VoidRadiant;

        public override bool GetEffectInheritance(DamageClass damageClass)
            => damageClass == Magic || damageClass == ModContent.GetInstance<HealerDamage>()
            || damageClass == AprilChaosSystem.VoidMagic || damageClass == AprilChaosSystem.VoidRadiant;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Magic || damageClass == Generic || damageClass == ModContent.GetInstance<HealerDamage>()
            || damageClass == AprilChaosSystem.VoidMagic || damageClass == AprilChaosSystem.VoidRadiant)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
    }

    public class RogueRanger : DamageClass
    {
        public override bool GetPrefixInheritance(DamageClass damageClass)
            => damageClass == Throwing || damageClass == ModContent.GetInstance<RogueDamageClass>() || damageClass == Ranged
            || damageClass == AprilChaosSystem.VoidRanged || damageClass == AprilChaosSystem.VoidThrowing;

        public override bool GetEffectInheritance(DamageClass damageClass)
            => damageClass == Throwing || damageClass == ModContent.GetInstance<RogueDamageClass>() || damageClass == Ranged
            || damageClass == AprilChaosSystem.VoidRanged || damageClass == AprilChaosSystem.VoidThrowing;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == ModContent.GetInstance<RogueDamageClass>() || damageClass == Ranged || damageClass == Generic
            || damageClass == AprilChaosSystem.VoidRanged || damageClass == AprilChaosSystem.VoidThrowing)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
    }

    public class MeleeSummoner : DamageClass
    {
        public override bool GetPrefixInheritance(DamageClass damageClass)
            => damageClass == Melee || damageClass == Summon || damageClass == ModContent.GetInstance<TrueMeleeDamageClass>()
            || damageClass == AprilChaosSystem.VoidMelee || damageClass == AprilChaosSystem.VoidSummon;

        public override bool GetEffectInheritance(DamageClass damageClass)
            => damageClass == Melee || damageClass == Summon || damageClass == ModContent.GetInstance<TrueMeleeDamageClass>()
            || damageClass == AprilChaosSystem.VoidMelee || damageClass == AprilChaosSystem.VoidSummon;

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == Melee || damageClass == Summon || damageClass == ModContent.GetInstance<TrueMeleeDamageClass>() || damageClass == Generic
            || damageClass == AprilChaosSystem.VoidMelee || damageClass == AprilChaosSystem.VoidSummon)
                return StatInheritanceData.Full;
            return StatInheritanceData.None;
        }
    }

    public class AprilChaosGlobalItem : GlobalItem
    {
        private static bool Chaos => ModContent.GetInstance<Configs.AprilChaos>().MasterToggle;
        private static bool RadMage => ModContent.GetInstance<Configs.AprilChaos>().RadMage;
        private static bool MelSum => ModContent.GetInstance<Configs.AprilChaos>().MelSum;
        private static bool RogRan => ModContent.GetInstance<Configs.AprilChaos>().RogRan;

        public override bool InstancePerEntity => true;
        public bool WasInitiallyMelee;

        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);
            if (!Chaos) return;

            if (RadMage && (item.DamageType == DamageClass.Magic
                || item.DamageType == ThoriumDamageBase<HealerDamage>.Instance
                || item.DamageType == AprilChaosSystem.VoidMagic
                || item.DamageType == AprilChaosSystem.VoidRadiant))
                item.DamageType = ModContent.GetInstance<RadiantMagic>();

            if (MelSum)
            {
                if (item.DamageType == DamageClass.Melee
                    || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()
                    || item.DamageType == AprilChaosSystem.VoidMelee)
                {
                    WasInitiallyMelee = true;
                    item.DamageType = ModContent.GetInstance<MeleeSummoner>();
                }
                else if (item.DamageType == DamageClass.Summon
                    || item.DamageType == AprilChaosSystem.VoidSummon)
                    item.DamageType = ModContent.GetInstance<MeleeSummoner>();
            }

            if (RogRan && (item.DamageType == DamageClass.Ranged
                || item.DamageType == ModContent.GetInstance<RogueDamageClass>()
                || item.DamageType == AprilChaosSystem.VoidRanged
                || item.DamageType == AprilChaosSystem.VoidThrowing))
                item.DamageType = ModContent.GetInstance<RogueRanger>();
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(item, player, target, ref modifiers);
            if (!Chaos || !MelSum || !WasInitiallyMelee) return;

            target.AddBuff(BuffID.BoneWhipNPCDebuff, 240);
            player.MinionAttackTargetNPC = target.whoAmI;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(item, tooltips);
            if (!Chaos) return;

            foreach (TooltipLine line in tooltips)
            {
                if (RadMage)
                    line.Text = line.Text
                        .Replace("magic", "radiagic").Replace("Magic", "Radiagic").Replace("MAGIC", "RADIAGIC")
                        .Replace("radiant", "radiagic").Replace("Radiant", "Radiagic").Replace("RADIANT", "RADIAGIC");

                if (MelSum)
                    line.Text = line.Text
                        .Replace(" melee", " summelee").Replace(" Melee", " Summelee").Replace(" MELEE", " SUMMELEE")
                        .Replace("summon", "summelee").Replace("Summon", "Summelee").Replace("SUMMON", "SUMMELEE");

                if (RogRan)
                    line.Text = line.Text
                        .Replace("ranged", "roganged").Replace("Ranged", "Roganged").Replace("RANGED", "ROGANGED")
                        .Replace("rogue", "roganged").Replace("Rogue", "Roganged").Replace("ROGUE", "ROGANGED");
            }
        }
    }

    public class AprilChaosGlobalProjectile : GlobalProjectile
    {
        private static bool Chaos => ModContent.GetInstance<Configs.AprilChaos>().MasterToggle;
        private static bool RadMage => ModContent.GetInstance<Configs.AprilChaos>().RadMage;
        private static bool MelSum => ModContent.GetInstance<Configs.AprilChaos>().MelSum;
        private static bool RogRan => ModContent.GetInstance<Configs.AprilChaos>().RogRan;

        public override bool InstancePerEntity => true;
        public bool WasInitiallyMelee;

        public override void SetDefaults(Projectile projectile)
        {
            base.SetDefaults(projectile);
            if (!Chaos) return;

            if (RadMage && (projectile.DamageType == DamageClass.Magic
                || projectile.DamageType == ThoriumDamageBase<HealerDamage>.Instance
                || projectile.DamageType == AprilChaosSystem.VoidMagic
                || projectile.DamageType == AprilChaosSystem.VoidRadiant))
                projectile.DamageType = ModContent.GetInstance<RadiantMagic>();

            if (MelSum)
            {
                if (projectile.DamageType == DamageClass.Melee
                    || projectile.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>()
                    || projectile.DamageType == AprilChaosSystem.VoidMelee)
                {
                    WasInitiallyMelee = true;
                    projectile.DamageType = ModContent.GetInstance<MeleeSummoner>();
                }
                else if (projectile.DamageType == DamageClass.Summon
                    || projectile.DamageType == AprilChaosSystem.VoidSummon)
                    projectile.DamageType = ModContent.GetInstance<MeleeSummoner>();
            }

            if (RogRan && (projectile.DamageType == DamageClass.Ranged
                || projectile.DamageType == ModContent.GetInstance<RogueDamageClass>()
                || projectile.DamageType == AprilChaosSystem.VoidRanged
                || projectile.DamageType == AprilChaosSystem.VoidThrowing))
                projectile.DamageType = ModContent.GetInstance<RogueRanger>();
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(projectile, target, ref modifiers);
            if (!Chaos || !MelSum || !WasInitiallyMelee) return;

            target.AddBuff(BuffID.BoneWhipNPCDebuff, 240);
            Main.player[projectile.owner].MinionAttackTargetNPC = target.whoAmI;
        }
    }

    public class AprilChaosSystem : ModSystem
    {
        public static DamageClass VoidMelee;
        public static DamageClass VoidRanged;
        public static DamageClass VoidMagic;
        public static DamageClass VoidSummon;
        public static DamageClass VoidGeneric;
        public static DamageClass VoidRadiant;
        public static DamageClass VoidThrowing;
        public static DamageClass VoidSymphonic;

        public override void PostSetupContent()
        {
            if (ModLoader.TryGetMod("SOTS", out Mod sots))
            {
                sots.TryFind<DamageClass>("VoidMelee", out VoidMelee);
                sots.TryFind<DamageClass>("VoidRanged", out VoidRanged);
                sots.TryFind<DamageClass>("VoidMagic", out VoidMagic);
                sots.TryFind<DamageClass>("VoidSummon", out VoidSummon);
                sots.TryFind<DamageClass>("VoidGeneric", out VoidGeneric);
            }

            if (ModLoader.TryGetMod("SOTSBardHealer", out Mod sotsBH))
            {
                sotsBH.TryFind<DamageClass>("VoidRadiant", out VoidRadiant);
                sotsBH.TryFind<DamageClass>("VoidThrowing", out VoidThrowing);
                sotsBH.TryFind<DamageClass>("VoidSymphonic", out VoidSymphonic);
            }
        }
    }
}