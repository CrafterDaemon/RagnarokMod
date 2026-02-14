using Microsoft.Xna.Framework;
using RagnarokMod.Utils;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Effects;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;

namespace RagnarokMod.Items.BardItems.Accessories
{
    public class SirenScale : BardItem
    {
        public override void SetBardDefaults()
        {
            Item.width  = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(gold: 15);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var thoriumPlayer = player.GetModPlayer<ThoriumPlayer>();

            player.GetDamage(ThoriumDamageBase<BardDamage>.Instance) += 0.15f;

            thoriumPlayer.inspirationRegenBonus *= 1.20f;

            var ragPlayer = player.GetRagnarokModPlayer();
            ragPlayer.sirenScale = true;
        }
    }
}
