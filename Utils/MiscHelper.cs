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
			//return t.HasTile && !t.IsActuated && Main.tileSolid[(int)(*t.TileType)] && !Main.tileSolidTop[(int)(*t.TileType)];
			 if (t == null)
			{
				return false;
			}	
			return t.HasTile && !t.IsActuated && (Main.tileSolid[t.TileType] || !Main.tileSolidTop[t.TileType]);

		}
		
		
		
		public static bool SolidOrSolidTopTile(int i, int j)
		{
			return WorldGen.InWorld(i, j, 0) && MiscHelper.SolidOrSolidTopTile(Main.tile[i, j]);
		}
		public static bool SolidOrSolidTopTile(Tile t)
		{
			 if (t == null)
			{
				return false;
			}	
			return t.HasTile && !t.IsActuated && (Main.tileSolid[t.TileType] || Main.tileSolidTop[t.TileType]);
			//return t.HasTile && !t.IsActuated && (Main.tileSolid[(int)(*t.TileType)] || Main.tileSolidTop[(int)(*t.TileType)]);
		}
		
	}
}