using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Utils;
using ThoriumMod;
using ThoriumMod.Utilities;
using CalamityMod.Items;
using CalamityMod;
using Terraria.DataStructures;

namespace RagnarokMod.Items.RevItems
{
    public class StormFeather : ModItem{
        public override void SetStaticDefaults(){
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 8));
            ItemID.Sets.AnimatesAsSoul[Type] = true;
        }
        public override void SetDefaults(){
            Item.width = 10;
            Item.height = 10;
            Item.scale = 0.75f;
            Item.accessory = true;
            Item.Calamity().revengeanceItem = true;
            Item.value = Item.sellPrice(0, 4);
            Item.rare = ItemRarityID.Blue;
        }
		public override void UpdateAccessory(Player player, bool hideVisual){
            ThoriumPlayer thoriumPlayer = player.GetThoriumPlayer();
            player.GetRagnarokModPlayer().stormfeather = true;
        }
    }
}
