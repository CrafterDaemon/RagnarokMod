using CalamityMod.Items.Weapons.Magic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Utilities;
using Terraria.ID;
using Terraria.Audio;

namespace RagnarokMod.Common.GlobalItems
{
    public class Anahita : GlobalItem
    {
         public static void ModifyInspirationCost(Player player, ref int cost)
        {
        }
        public override bool AppliesToEntity(Item item, bool lateInstantiation)
        {
            return item.type == ModContent.ItemType<AnahitasArpeggio>();
        }

        public override void SetDefaults(Item item)
        {
            item.DamageType = (DamageClass)(object)ThoriumDamageBase<BardDamage>.Instance;
        }
    }
}
