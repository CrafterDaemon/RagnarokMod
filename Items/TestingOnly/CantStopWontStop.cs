using CalamityMod;
using RagnarokMod.Riffs;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items;
using ThoriumMod.Utilities;

namespace RagnarokMod.Items.TestingOnly
{
    public class CantStopWontStop : BardItem
    {
        public override string Texture => "RagnarokMod/Items/BardItems/Consumable/InspirationSingularity";
        public override void SetBardDefaults()
        {
            ((Entity)this.Item).width = 18;
            ((Entity)this.Item).height = 18;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool isvis)
        {
            player.GetThoriumPlayer().bardResource = player.GetThoriumPlayer().bardResourceMax;
            if (player.Calamity().cooldowns.TryGetValue(RiffLoader.Cooldown.ID, out var cooldown))
                cooldown.timeLeft = 0;
        }
    }
}
