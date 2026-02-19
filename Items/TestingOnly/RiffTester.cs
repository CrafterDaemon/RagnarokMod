using CalamityMod;
using Microsoft.Xna.Framework;
using RagnarokMod.Riffs;
using RagnarokMod.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RagnarokMod.Items
{
    public class RiffTester : ModItem
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Consumable/InspirationEssence";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.MowTheLawn;
            Item.holdStyle = ItemHoldStyleID.HoldGuitar;
            Item.noMelee = true;
        }

        public override bool? UseItem(Player player)
        {
            if (player.Calamity().cooldowns.TryGetValue(RiffLoader.Cooldown.ID, out var cooldown))
                cooldown.timeLeft = 0;
            return true;
        }
    }
}
