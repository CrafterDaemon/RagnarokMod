using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;

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
            if (t == null)
            {
                return false;
            }
            return t.HasTile && !t.IsActuated && (Main.tileSolid[t.TileType] || !Main.tileSolidTop[t.TileType]);
        }
        public static bool IsOnStandableGround(in float startX, float y, int width, bool onlySolid = false)
        {
            if (width <= 0)
            {
                throw new ArgumentException("width cannot be negative");
            }
            float num = startX;
            while (true)
            {
                Point point = Terraria.Utils.ToTileCoordinates(new Vector2(num, y + 0.01f));
                if ((onlySolid && MiscHelper.SolidTile(point.X, point.Y)) || MiscHelper.SolidOrSolidTopTile(point.X, point.Y))
                {
                    break;
                }
                num += 16f;
                if (num >= startX + (float)width)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsOnStandableGround(this Entity entity, float yOffset = 0f, bool onlySolid = false)
        {
            return MiscHelper.IsOnStandableGround(entity.BottomLeft.X, entity.BottomLeft.Y + yOffset, entity.width, onlySolid);
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
        }


        public static bool MatchBardItem(Item item, BardInstrumentType type)
        {
            return (item.ModItem is BardItem bardItem && bardItem.InstrumentType == type);
        }
        public static bool CanHitLine(this Entity start, Entity end)
        {
            return CanHitLine(Terraria.Utils.ToTileCoordinates(start.Center), Terraria.Utils.ToTileCoordinates(end.Center));
        }

        public static bool CanHitLine(Vector2 start, Vector2 end)
        {
            return CanHitLine(Terraria.Utils.ToTileCoordinates(start), Terraria.Utils.ToTileCoordinates(end));
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
        public static IEnumerable<Player> GetPlayersInRange(Player player, float range)
        {
            float rangeSQ = range * range;
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
                if (target.active && !target.dead && target != player
                    && target.DistanceSQ(player.Center) <= rangeSQ)
                    yield return target;
            }
        }

        public static string TenToRoman(int number)
        {
            string roman;
            switch (number)
            {
                case 1:
                    roman = "I";
                    break;
                case 2:
                    roman = "II";
                    break;
                case 3:
                    roman = "III";
                    break;
                case 4:
                    roman = "IV";
                    break;
                case 5:
                    roman = "V";
                    break;
                case 6:
                    roman = "VI";
                    break;
                case 7:
                    roman = "VII";
                    break;
                case 8:
                    roman = "VIII";
                    break;
                case 9:
                    roman = "IX";
                    break;
                case 10:
                    roman = "X";
                    break;
                default:
                    roman = string.Empty;
                    break;
            }
            return roman;
        }
    }
}