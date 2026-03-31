using CalamityMod;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;

namespace RagnarokMod.Common.GlobalItems
{
    public class AprilChaos : GlobalItem
    {

        // Handles damage type swaps and tooltip replacement
        public class AprilChaosGlobalItem : GlobalItem
        {
            private static bool Chaos => ModContent.GetInstance<Configs.AprilChaos>().MasterToggle;
            private static int RadMage => ModContent.GetInstance<Configs.AprilChaos>().RadMage;
            private static int MelSum => ModContent.GetInstance<Configs.AprilChaos>().MelSum;
            private static int RogRan => ModContent.GetInstance<Configs.AprilChaos>().RogRan;
            public override bool InstancePerEntity => true;

            public bool WasInitiallyMelee;
            public override void SetDefaults(Item item)
            {
                base.SetDefaults(item);
                if (!Chaos) return;

                switch (RadMage)
                {
                    case 1 when item.DamageType == DamageClass.Magic:
                        item.DamageType = ThoriumDamageBase<HealerDamage>.Instance; break;
                    case -1 when item.DamageType == ThoriumDamageBase<HealerDamage>.Instance:
                        item.DamageType = DamageClass.Magic; break;
                }

                switch (MelSum)
                {
                    case 1 when item.DamageType == DamageClass.Melee || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>():
                        WasInitiallyMelee = true; item.DamageType = DamageClass.Summon; break;
                    case -1 when item.DamageType == DamageClass.Melee || item.DamageType == ModContent.GetInstance<TrueMeleeDamageClass>():
                        WasInitiallyMelee = true; break;
                    case -1 when item.DamageType == DamageClass.Summon:
                        item.DamageType = DamageClass.Melee; break;
                }

                switch (RogRan)
                {
                    case 1 when item.DamageType == DamageClass.Ranged:
                        item.DamageType = ModContent.GetInstance<RogueDamageClass>(); break;
                    case -1 when item.DamageType == ModContent.GetInstance<RogueDamageClass>():
                        item.DamageType = DamageClass.Ranged; break;
                }
            }
            public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
            {
                base.ModifyHitNPC(item, player, target, ref modifiers);
                if (!Chaos || MelSum == 0 || !WasInitiallyMelee) return;

                target.AddBuff(BuffID.BoneWhipNPCDebuff, 240);
                player.MinionAttackTargetNPC = target.whoAmI;
            }

            public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
            {
                base.ModifyTooltips(item, tooltips);
                if (!Chaos) return;

                switch (RadMage)
                {
                    case 1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("magic", "radiant")
                                .Replace("Magic", "Radiant")
                                .Replace("MAGIC", "RADIANT");
                        }
                        break;
                    case -1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("radiant", "magic")
                                .Replace("Radiant", "Magic")
                                .Replace("RADIANT", "MAGIC");
                        }
                        break;
                }
                switch (MelSum)
                {
                    case 1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("melee", "summon")
                                .Replace("Melee", "Summon")
                                .Replace("MELEE", "SUMMON");
                        }
                        break;
                    case -1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("summon", "melee")
                                .Replace("Summon", "Melee")
                                .Replace("SUMMON", "MELEE");
                        }
                        break;
                }
                switch (RogRan)
                {
                    case 1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("ranged", "rogue")
                                .Replace("Ranged", "Rogue")
                                .Replace("RANGED", "ROGUE");
                        }
                        break;
                    case -1:
                        foreach (TooltipLine line in tooltips)
                        {
                            line.Text = line.Text
                                .Replace("rogue", "ranged")
                                .Replace("Rogue", "Ranged")
                                .Replace("ROGUE", "RANGED");
                        }
                        break;
                }
            }
        }

        public class AprilChaosSystem : ModSystem
        {
            private static bool Chaos => ModContent.GetInstance<Configs.AprilChaos>().MasterToggle;
            private static int RadMage => ModContent.GetInstance<Configs.AprilChaos>().RadMage;
            private static int MelSum => ModContent.GetInstance<Configs.AprilChaos>().MelSum;
            private static int RogRan => ModContent.GetInstance<Configs.AprilChaos>().RogRan;
            public override void PostSetupContent()
            {
                if (!Chaos) return;

                DamageClass magic = DamageClass.Magic;
                DamageClass radiant = ThoriumDamageBase<HealerDamage>.Instance;
                DamageClass summon = DamageClass.Summon;
                DamageClass melee = DamageClass.Melee;
                DamageClass ranged = DamageClass.Ranged;
                DamageClass rogue = ModContent.GetInstance<RogueDamageClass>();
                DamageClass truemelee = ModContent.GetInstance<TrueMeleeDamageClass>();

                if (RadMage != 0)
                {
                    magic.GetEffectInheritance(radiant);
                    magic.GetModifierInheritance(radiant);
                    magic.GetPrefixInheritance(radiant);
                    radiant.GetEffectInheritance(magic);
                    radiant.GetModifierInheritance(magic);
                    radiant.GetPrefixInheritance(magic);
                }

                if (MelSum != 0)
                {
                    summon.GetEffectInheritance(melee);
                    summon.GetModifierInheritance(melee);
                    summon.GetPrefixInheritance(melee);
                    melee.GetEffectInheritance(summon);
                    melee.GetModifierInheritance(summon);
                    melee.GetPrefixInheritance(summon);
                    truemelee.GetEffectInheritance(summon);
                    truemelee.GetModifierInheritance(summon);
                    truemelee.GetPrefixInheritance(summon);
                }

                if (RogRan != 0)
                {
                    ranged.GetEffectInheritance(rogue);
                    ranged.GetModifierInheritance(rogue);
                    ranged.GetPrefixInheritance(rogue);
                    rogue.GetEffectInheritance(ranged);
                    rogue.GetModifierInheritance(ranged);
                    rogue.GetPrefixInheritance(ranged);
                }
            }
        }
    }
}
