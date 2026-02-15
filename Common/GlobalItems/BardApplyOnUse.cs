using RagnarokMod.Items.BardItems.Accessories;
using RagnarokMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

namespace RagnarokMod.Common.GlobalItems
{
    public class BardApplyOnUse : GlobalItem
    {
        public override bool? UseItem(Item item, Player player)
        {
            var ragPlayer = player.GetModPlayer<RagnarokModPlayer>();
            if (MiscHelper.MatchBardItem(item, BardInstrumentType.String)
                && ragPlayer.sirenScale == true)
            {
                ragPlayer.stringInstrumentUsed = true;
                ragPlayer.EnsureMiniAnahita();
            }

            return base.UseItem(item, player);
        }
    }
}
