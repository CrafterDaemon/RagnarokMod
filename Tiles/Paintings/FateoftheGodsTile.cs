using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace RagnarokMod.Tiles.Paintings
{
    public class FateoftheGodsTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            var data = TileObjectData.newTile;
            data.CopyFrom(TileObjectData.Style3x3Wall);

            data.Width = 5;
            data.Height = 5;
            data.Origin = new Point16(2, 2);
            data.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
            data.CoordinateWidth = 16;
            data.CoordinatePadding = 2;

            TileObjectData.addTile(Type);

            AddMapEntry(new(120, 85, 60), Language.GetText("MapObject.Painting"));
            DustType = DustID.WoodFurniture;
        }
    }
}
