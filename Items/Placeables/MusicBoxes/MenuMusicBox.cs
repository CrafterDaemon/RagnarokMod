using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using RagnarokMod.Items.Placeables.Paintings;
using RagnarokMod.Tiles.MusicBoxes;

namespace RagnarokMod.Items.Placeables.MusicBoxes
{
    public class MenuMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;
            Item.ResearchUnlockCount = 1;
            ItemID.Sets.CanGetPrefixes[Type] = false;
            ItemID.Sets.ShimmerTransformToItem[Type] = 576;
            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot("RagnarokMod/Sounds/Music/TitleMusic"), ModContent.ItemType<MenuMusicBox>(), ModContent.TileType<MenuMusicBoxTile>(), 0);
        }

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<MenuMusicBoxTile>();
            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(silver: 20);
            Item.accessory = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.MusicBox)
                .AddIngredient<FateoftheGods>()
                .AddTile(TileID.HeavyWorkBench)
                .Register();
        }
    }
}
