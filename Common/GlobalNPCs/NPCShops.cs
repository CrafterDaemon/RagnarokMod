using CalamityMod;
using CalamityMod.NPCs.TownNPCs;
using RagnarokMod.Items.HealerItems.Scythes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace RagnarokMod.Common.GlobalNPCs
{
    public class NPCShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType != ModContent.NPCType<Archmage>())
                return;
            shop.Add<GlacialHarvester>();
        }
    }
}
