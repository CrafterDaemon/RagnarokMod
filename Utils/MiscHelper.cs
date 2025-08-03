using Terraria.ModLoader;
using Terraria;
using ThoriumMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RagnarokMod.Utils
{
    public static class MiscHelper
    {
		public static bool SolidTile(int i, int j)
		{
			return WorldGen.InWorld(i, j, 0) && MiscHelper.SolidTile(Main.tile[i, j]);
		}

		public static bool SolidTile(Tile t)
		{
			 if (t == null){
				return false;
			}	
			return t.HasTile && !t.IsActuated && (Main.tileSolid[t.TileType] || !Main.tileSolidTop[t.TileType]);
		}
		public static bool IsOnStandableGround(in float startX, float y, int width, bool onlySolid = false)
		{
			if (width <= 0){
				throw new ArgumentException("width cannot be negative");
			}
			float num = startX;
			for (;;){
				Point point = Terraria.Utils.ToTileCoordinates(new Vector2(num, y + 0.01f));
				if ((onlySolid && MiscHelper.SolidTile(point.X, point.Y)) || MiscHelper.SolidOrSolidTopTile(point.X, point.Y))
				{
					break;
				}
				num += 16f;
				if (num >= startX + (float)width){
					return false;
				}
			}
			return true;
		}
		public static bool IsOnStandableGround(this Entity entity, float yOffset = 0f, bool onlySolid = false){
			return MiscHelper.IsOnStandableGround(entity.BottomLeft.X, entity.BottomLeft.Y + yOffset, entity.width, onlySolid);
		}
		public static bool SolidOrSolidTopTile(int i, int j){
			return WorldGen.InWorld(i, j, 0) && MiscHelper.SolidOrSolidTopTile(Main.tile[i, j]);
		}
		public static bool SolidOrSolidTopTile(Tile t)
		{
			 if (t == null)
			{
				return false;
			}	
			return t.HasTile && !t.IsActuated && (Main.tileSolid[t.TileType] || Main.tileSolidTop[t.TileType]);
		}
		
	}
}