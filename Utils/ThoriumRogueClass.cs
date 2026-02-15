using System;
using CalamityMod;
using Terraria.ModLoader;

namespace RagnarokMod.Utils
{
    public class ThoriumRogueClass : DamageClass
    {
        public override bool GetPrefixInheritance(DamageClass damageClass)
        {
            return damageClass == DamageClass.Ranged;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            return (damageClass == ModContent.GetInstance<RogueDamageClass>()) || (damageClass == DamageClass.Throwing);
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if (damageClass == DamageClass.Throwing || damageClass == DamageClass.Generic || damageClass == ModContent.GetInstance<RogueDamageClass>())
            {
                return StatInheritanceData.Full;
            }
            return StatInheritanceData.None;
        }
    }
}
