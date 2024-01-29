using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;

namespace RagnarokMod.Utils
{
	internal static class RagnarokModMiscHelper
	{
		
		/*
		// Token: 0x0600047B RID: 1147 RVA: 0x0002FCA5 File Offset: 0x0002DEA5
		public static void SetDefaults(this Item item, int type, int stack, bool noMatCheck = false)
		{
			item.SetDefaults(type, noMatCheck);
			item.stack = stack;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0002FCB8 File Offset: 0x0002DEB8
		public static void ThoriumChatMessage(string key, Color color, params string[] substitutions)
		{
			key = "Mods.ThoriumMod.Announcements." + key;
			if (Main.netMode == 2)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key, substitutions), color, -1);
				return;
			}
			if (Main.netMode == 0)
			{
				Main.NewText(Language.GetTextValue(key, substitutions), new Color?(color));
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0002FD08 File Offset: 0x0002DF08
		public static void ThoriumChatMessage(string key, Color color)
		{
			key = "Mods.ThoriumMod.Announcements." + key;
			if (Main.netMode == 2)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey(key, Array.Empty<object>()), color, -1);
				return;
			}
			if (Main.netMode == 0)
			{
				Main.NewText(Language.GetTextValue(key), new Color?(color));
			}
		}

	
		// Token: 0x0600047E RID: 1150 RVA: 0x0002FD58 File Offset: 0x0002DF58
		public static float DistanceSQ(this Rectangle rectangle, Vector2 point)
		{
			if (Utils.FloatIntersect((float)rectangle.Left, (float)rectangle.Top, (float)rectangle.Width, (float)rectangle.Height, point.X, point.Y, 0f, 0f))
			{
				return 0f;
			}
			if (point.X >= (float)rectangle.Left && point.X <= (float)rectangle.Right)
			{
				if (point.Y >= (float)rectangle.Top)
				{
					return (point.Y - (float)rectangle.Bottom) * (point.Y - (float)rectangle.Bottom);
				}
				return ((float)rectangle.Top - point.Y) * ((float)rectangle.Top - point.Y);
			}
			else if (point.Y >= (float)rectangle.Top && point.Y <= (float)rectangle.Bottom)
			{
				if (point.X >= (float)rectangle.Left)
				{
					return (point.X - (float)rectangle.Right) * (point.X - (float)rectangle.Right);
				}
				return ((float)rectangle.Left - point.X) * ((float)rectangle.Left - point.X);
			}
			else if (point.X < (float)rectangle.Left)
			{
				if (point.Y >= (float)rectangle.Top)
				{
					return Vector2.DistanceSquared(point, Utils.BottomLeft(rectangle));
				}
				return Vector2.DistanceSquared(point, Utils.TopLeft(rectangle));
			}
			else
			{
				if (point.Y >= (float)rectangle.Top)
				{
					return Vector2.DistanceSquared(point, Utils.BottomRight(rectangle));
				}
				return Vector2.DistanceSquared(point, Utils.TopRight(rectangle));
			}
		}

		*/
		public static bool CanHitLine(this Entity start, Entity end)
		{
			return RagnarokModMiscHelper.CanHitLine(Terraria.Utils.ToTileCoordinates(start.Center), Terraria.Utils.ToTileCoordinates(end.Center));
		}
		
		public static bool CanHitLine(Vector2 start, Vector2 end)
		{
			return RagnarokModMiscHelper.CanHitLine(Terraria.Utils.ToTileCoordinates(start), Terraria.Utils.ToTileCoordinates(end));
		}

		public static bool CanHitLine(Point start, Point end)
		{
			if (!WorldGen.InWorld(start.X, start.Y, 0) || !WorldGen.InWorld(end.X, end.Y, 0) || WorldGen.SolidTile3(Framing.GetTileSafely(start)))
			{
				return false;
			}
			
			int distX = Math.Abs(end.X - start.X);
			int distY = Math.Abs(end.Y - start.Y);
			int sign_x = Math.Sign(end.X - start.X);
			int sign_y = Math.Sign(end.Y - start.Y);
			
			int ix = 0;
			int iy = 0;
			
			while (ix < distX || iy < distY)
			{
				int xyDiff = ((1 + 2 * ix) * distY).CompareTo((1 + 2 * iy) * distX);
			
				if (xyDiff == 0)
				{
					int nextX = start.X + sign_x;
					int nextY = start.Y + sign_y;
			
					if (WorldGen.SolidTile3(Framing.GetTileSafely(nextX, start.Y)) || WorldGen.SolidTile3(Framing.GetTileSafely(start.X, nextY)))
					{
						return false;
					}
			
					start.X = nextX;
					start.Y = nextY;
					ix++;
					iy++;
				}
				else if (xyDiff < 0)
				{
					start.X += sign_x;
					ix++;
				}
				else
				{
					start.Y += sign_y;
					iy++;
				}
			
				if (WorldGen.SolidTile3(Framing.GetTileSafely(start)))
				{
					return false;
				}
			}
			return true;
		}

		/*
		// Token: 0x06000482 RID: 1154 RVA: 0x00030084 File Offset: 0x0002E284
		public static bool IsOnStandableGround(in float startX, float y, int width, bool onlySolid = false)
		{
			if (width <= 0)
			{
				throw new ArgumentException("width cannot be negative");
			}
			float fx = startX;
			for (;;)
			{
				Point point = Utils.ToTileCoordinates(new Vector2(fx, y + 0.01f));
				if ((onlySolid && MiscHelper.SolidTile(point.X, point.Y)) || MiscHelper.SolidOrSolidTopTile(point.X, point.Y))
				{
					break;
				}
				fx += 16f;
				if (fx >= startX + (float)width)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x000300F4 File Offset: 0x0002E2F4
		public static bool IsOnStandableGround(this Entity entity, float yOffset = 0f, bool onlySolid = false)
		{
			return MiscHelper.IsOnStandableGround(entity.BottomLeft.X, entity.BottomLeft.Y + yOffset, entity.width, onlySolid);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00030128 File Offset: 0x0002E328
		public static bool SolidTile(int i, int j)
		{
			return WorldGen.InWorld(i, j, 0) && MiscHelper.SolidTile(Main.tile[i, j]);
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00030147 File Offset: 0x0002E347
		public unsafe static bool SolidTile(Tile t)
		{
			return t.HasTile && !t.IsActuated && Main.tileSolid[(int)(*t.TileType)] && !Main.tileSolidTop[(int)(*t.TileType)];
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00030180 File Offset: 0x0002E380
		public static bool SolidOrSolidTopTile(int i, int j)
		{
			return WorldGen.InWorld(i, j, 0) && MiscHelper.SolidOrSolidTopTile(Main.tile[i, j]);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0003019F File Offset: 0x0002E39F
		public unsafe static bool SolidOrSolidTopTile(Tile t)
		{
			return t.HasTile && !t.IsActuated && (Main.tileSolid[(int)(*t.TileType)] || Main.tileSolidTop[(int)(*t.TileType)]);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x000301D8 File Offset: 0x0002E3D8
		public static string TenToRoman(byte number)
		{
			switch (number)
			{
			case 1:
				return "I";
			case 2:
				return "II";
			case 3:
				return "III";
			case 4:
				return "IV";
			case 5:
				return "V";
			case 6:
				return "VI";
			case 7:
				return "VII";
			case 8:
				return "VIII";
			case 9:
				return "IX";
			case 10:
				return "X";
			default:
				return "";
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00030258 File Offset: 0x0002E458
		public static void Log(object message, bool chat = false)
		{
			string msg = message.ToString();
			ThoriumMod.mod.Logger.Info(msg);
			if (!chat)
			{
				return;
			}
			if (Main.netMode == 2)
			{
				Console.WriteLine(msg);
				return;
			}
			Main.NewText(msg, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x000302A4 File Offset: 0x0002E4A4
		public static void ModifyVelocityForGravity(Vector2 position, Vector2 targetPos, in float gravity, ref Vector2 velocity, int ticksWithoutGravity = 0, float terminalCap = 16f, float factor = 1f, float offsetCap = 2.5f)
		{
			int ticksToReachX = (int)((targetPos - position).X / velocity.X);
			float traversedDistanceY = 0f;
			float traversedDistanceYNoGravity = 0f;
			float velocityYWithGravity = velocity.Y;
			for (int i = 0; i < ticksToReachX; i++)
			{
				if (i >= ticksWithoutGravity)
				{
					velocityYWithGravity += gravity;
					if (velocityYWithGravity > terminalCap)
					{
						velocityYWithGravity = terminalCap;
					}
				}
				traversedDistanceY += velocityYWithGravity;
				traversedDistanceYNoGravity += velocity.Y;
			}
			float velocityYOffset = (traversedDistanceY - traversedDistanceYNoGravity) / (float)ticksToReachX;
			velocityYOffset = Math.Min(velocityYOffset, offsetCap);
			velocity.Y -= factor * velocityYOffset;
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x0003032C File Offset: 0x0002E52C
		public static void ModifyPositionForUnstuckTowardsOrigin(Vector2 origin, ref Vector2 desiredPosition)
		{
			Vector2 directionToOrigin = Utils.SafeNormalize(origin - desiredPosition, Vector2.Zero);
			if (directionToOrigin == Vector2.Zero)
			{
				return;
			}
			Vector2 size;
			size..ctor(16f);
			Vector2 topLeft = desiredPosition - size / 2f;
			while (Collision.SolidCollision(topLeft, (int)size.X, (int)size.Y))
			{
				topLeft += directionToOrigin * 8f;
				if ((topLeft - origin).LengthSquared() < 256f)
				{
					desiredPosition = origin;
					return;
				}
			}
			desiredPosition = topLeft + size / 2f;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000303E0 File Offset: 0x0002E5E0
		public unsafe static IEnumerable<Point> GetClusterPositions(Point point, int tileCountMax, out int count)
		{
			Queue<Point> queue = new Queue<Point>();
			Queue<Point> filled = new Queue<Point>();
			queue.Enqueue(point);
			int[] xx = new int[] { 0, 1, 0, -1 };
			int[] array = new int[4];
			array[0] = -1;
			array[2] = 1;
			int[] yy = array;
			int tileType = (int)(*Framing.GetTileSafely(point).TileType);
			while (queue.Count > 0 && filled.Count < tileCountMax)
			{
				Point currentPoint = queue.Dequeue();
				if (!filled.Contains(currentPoint))
				{
					filled.Enqueue(currentPoint);
					for (int i = 0; i < 4; i++)
					{
						int offX = xx[i];
						int offY = yy[i];
						Point checkPoint;
						checkPoint..ctor(currentPoint.X + offX, currentPoint.Y + offY);
						int x = checkPoint.X;
						int y = checkPoint.Y;
						if (WorldGen.InWorld(x, y, 0) && !queue.Contains(checkPoint) && !filled.Contains(checkPoint))
						{
							Tile checkTile = Framing.GetTileSafely(checkPoint);
							if (checkTile.HasTile && (int)(*checkTile.TileType) == tileType)
							{
								queue.Enqueue(checkPoint);
							}
						}
					}
				}
			}
			count = filled.Count;
			return filled;
		}
		
		*/
	}
}
