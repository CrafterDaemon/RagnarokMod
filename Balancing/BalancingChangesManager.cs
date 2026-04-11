using CalamityMod.Balancing;
using CalamityMod.Projectiles.DraedonsArsenal;
using CalamityMod.Systems.Collections;
using RagnarokMod.Projectiles.BardPro.Electronic;
using RagnarokMod.Projectiles.BardPro.Percussion;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace RagnarokMod.Balancing
{
    public sealed class BalancingChangesManager : ModSystem
    {
        internal static List<NPCBalancingChange> NPCSpecificBalancingChanges = null;

        private static IBalancingRule[] Do(params IBalancingRule[] rules) => rules;

        private static NPCBalancingChange[] Bundle(IEnumerable<int> npcIDs, params IBalancingRule[] rules)
        {
            NPCBalancingChange[] changes = new NPCBalancingChange[npcIDs.Count()];
            for (int i = 0; i < changes.Length; i++)
                changes[i] = new NPCBalancingChange(npcIDs.ElementAt(i), rules);
            return changes;
        }

        public override void SetStaticDefaults()
        {
            NPCSpecificBalancingChanges = new List<NPCBalancingChange>();
            Bundle(CalamityNPCTypeSets.DevourerOfGods, Do(new ProjectileResistBalancingRule(0.5f, ProjectileType<RadioMicPro>())));
            Bundle(CalamityNPCTypeSets.DevourerOfGods, Do(new ProjectileResistBalancingRule(0.5f, ProjectileType<RadioMicShadowBurst>())));
            Bundle(CalamityNPCTypeSets.DevourerOfGods, Do(new ProjectileResistBalancingRule(0.5f, ProjectileType<EctambourinePro>())));
        }

        public override void Unload()
        {
            NPCSpecificBalancingChanges = null;
        }

        public static void ApplyFromProjectile(NPC npc, ref NPC.HitModifiers modifiers, Projectile proj)
        {
            // Apply NPC-specific balancing rules.
            foreach (NPCBalancingChange balanceChange in NPCSpecificBalancingChanges)
            {
                if (npc.type != balanceChange.NPCType)
                    continue;

                foreach (IBalancingRule balancingRule in balanceChange.BalancingRules)
                {
                    if (balancingRule.AppliesTo(npc, modifiers, proj))
                        balancingRule.ApplyBalancingChange(npc, ref modifiers);
                }
            }
        }
    }
}
