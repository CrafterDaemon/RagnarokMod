using Terraria;
using Terraria.ModLoader;

namespace RagnarokMod.Items.TestingOnly
{
    public class TrueSquirrelScarf : ModItem
    {
        public override void SetDefaults()
        {
            ((Entity)this.Item).width = 18;
            ((Entity)this.Item).height = 18;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool isvis)
        {
            player.maxTurrets += 999;
        }
    }
}
