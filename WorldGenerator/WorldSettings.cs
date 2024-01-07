using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Main = WorldGenerator.WorldSettings;

namespace WorldGenerator;

internal static class WorldSettings
{
    public const bool drunkWorldGen = false, getGoodWorldGen = false, dontStarveWorldGen = false, notTheBees = false, gen = true, tenthAnniversaryWorld = false;

    internal const int maxTilesX = 1000, maxTilesY = 300;

    internal static int
        leftBeachEnd = 0, rightBeachStart = 0, waterLine = 0, lavaLine, jungleOriginX = 0, snowOriginLeft = 0, snowOriginRight = 0, numPyr = 0,
        beachDistance = 380, maxTunnels = 50, howFar = 0, beachBordersWidth = 275, beachSandRandomCenter = beachBordersWidth + 5 + 40,
        smallHolesBeachAvoidance = beachSandRandomCenter + 20, surfaceCavesBeachAvoidance2 = beachSandRandomCenter + 20, i2 = 0,
        snowTop = 0, snowBottom = 0, dungeonSide = 0, grassSpread = 0, SmallConsecutivesFound = 0, SmallConsecutivesEliminated = 0, tileCounterMax = 20;
    internal static int numTunnels, numMCaves, tileCounterNum;//values?

    internal static int[] PyrX = null, PyrY = null, tunnelX = new int[maxTunnels], mCaveX = new int[30], mCaveY = new int[30],
        snowMinX = new int[Main.maxTilesY], snowMaxX = new int[Main.maxTilesY], tileCounterX = new int[tileCounterMax], tileCounterY = new int[tileCounterMax];

    internal static double
        worldSurface = 0.0, worldSurfaceHigh = 0.0, worldSurfaceLow = 0.0,
        rockLayer = 0.0, rockLayerHigh = 0.0, rockLayerLow = 0.0;

    public static bool mudWall;//value?
    public static bool skipDesertTileCheck = false;

    public static Rectangle UndergroundDesertLocation = Rectangle.Empty, UndergroundDesertHiveLocation = Rectangle.Empty;

    public static readonly StructureMap structures = new StructureMap();
    public static UnifiedRandom genRand = new(Program.seed);
    public static Tilemap tile = new(maxTilesX, maxTilesY);

    public static bool[] tileStone = new bool[625], tileSolid = new bool[625];//this is bad

    #region util
    internal static Point RandomWorldPoint(int top = 0, int right = 0, int bottom = 0, int left = 0)
    {
        return new Point(genRand.Next(left, Main.maxTilesX - right), genRand.Next(top, Main.maxTilesY - bottom));
    }

    public static Vector2 ToVector2(this Point p)//how does this work?
    {
        return new Vector2(p.X, p.Y);
    }

    public static Point ToPoint(this Vector2 v)
    {
        return new Point((int)v.X, (int)v.Y);
    }

    public static float NextFloat(this UnifiedRandom r)
    {
        return (float)r.NextDouble();
    }

    public static void PlaceWall(int i, int j, int type, bool mute = false)
    {
        if (i <= 1 || j <= 1 || i >= Main.maxTilesX - 2 || j >= Main.maxTilesY - 2)
        {
            return;
        }

        Tile tile2;
        if (Main.tile[i, j] == null)
        {
            tile2 = (Main.tile[i, j] = default(Tile));
        }

        tile2 = Main.tile[i, j];
        if (tile2.wall == 0)
        {
            tile2 = Main.tile[i, j];
            tile2.wall = (ushort)type;
            //SquareWallFrame(i, j);
            /*if (!mute)
            {
                SoundEngine.PlaySound(0, i * 16, j * 16);
            }*/
        }
    }

    public static void TileRunner(int i, int j, double strength, int steps, int type, bool addTile = false, float speedX = 0f, float speedY = 0f, bool noYChange = false, bool overRide = true, int ignoreTileType = -1)
    {
        if (drunkWorldGen)
        {
            strength *= (double)(1f + (float)genRand.Next(-80, 81) * 0.01f);
            steps = (int)((float)steps * (1f + (float)genRand.Next(-80, 81) * 0.01f));
        }

        if (getGoodWorldGen && type != 57)
        {
            strength *= (double)(1f + (float)genRand.Next(-80, 81) * 0.015f);
            steps += genRand.Next(3);
        }

        double num = strength;
        float num2 = steps;
        Vector2 vector = default(Vector2);
        vector.X = i;
        vector.Y = j;
        Vector2 vector2 = default(Vector2);
        vector2.X = (float)genRand.Next(-10, 11) * 0.1f;
        vector2.Y = (float)genRand.Next(-10, 11) * 0.1f;
        if (speedX != 0f || speedY != 0f)
        {
            vector2.X = speedX;
            vector2.Y = speedY;
        }

        bool flag = type == 368;
        bool flag2 = type == 367;
        bool lava = false;
        if (getGoodWorldGen && genRand.Next(4) == 0)
        {
            lava = true;
        }

        while (num > 0.0 && num2 > 0f)
        {
            if (drunkWorldGen && genRand.Next(30) == 0)
            {
                vector.X += (float)genRand.Next(-100, 101) * 0.05f;
                vector.Y += (float)genRand.Next(-100, 101) * 0.05f;
            }

            if (vector.Y < 0f && num2 > 0f && type == 59)
            {
                num2 = 0f;
            }

            num = strength * (double)(num2 / (float)steps);
            num2 -= 1f;
            int num3 = (int)((double)vector.X - num * 0.5);
            int num4 = (int)((double)vector.X + num * 0.5);
            int num5 = (int)((double)vector.Y - num * 0.5);
            int num6 = (int)((double)vector.Y + num * 0.5);
            if (num3 < 1)
            {
                num3 = 1;
            }

            if (num4 > Main.maxTilesX - 1)
            {
                num4 = Main.maxTilesX - 1;
            }

            if (num5 < 1)
            {
                num5 = 1;
            }

            if (num6 > Main.maxTilesY - 1)
            {
                num6 = Main.maxTilesY - 1;
            }

            for (int k = num3; k < num4; k++)
            {
                if (k < beachDistance + 50 || k >= Main.maxTilesX - beachDistance - 50)
                {
                    lava = false;
                }

                for (int l = num5; l < num6; l++)
                {
                    if (drunkWorldGen && l < Main.maxTilesY - 300 && type == 57)
                    {
                        continue;
                    }

                    Tile tile;
                    if (ignoreTileType >= 0)
                    {
                        tile = Main.tile[k, l];
                        if (tile.active())
                        {
                            tile = Main.tile[k, l];
                            if (tile.type == ignoreTileType)
                            {
                                continue;
                            }
                        }
                    }

                    if (!((double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.5 * (1.0 + (double)genRand.Next(-10, 11) * 0.015)))
                    {
                        continue;
                    }

                    if (mudWall && (double)l > Main.worldSurface)
                    {
                        tile = Main.tile[k, l - 1];
                        if (tile.wall != 2 && l < Main.maxTilesY - 210 - genRand.Next(3) && (double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.45 * (1.0 + (double)genRand.Next(-10, 11) * 0.01))
                        {
                            if (l > lavaLine - genRand.Next(0, 4) - 50)
                            {
                                tile = Main.tile[k, l - 1];
                                if (tile.wall != 64)
                                {
                                    tile = Main.tile[k, l + 1];
                                    if (tile.wall != 64)
                                    {
                                        tile = Main.tile[k - 1, l];
                                        if (tile.wall != 64)
                                        {
                                            tile = Main.tile[k + 1, l];
                                            if (tile.wall != 64)
                                            {
                                                PlaceWall(k, l, 15, mute: true);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                tile = Main.tile[k, l - 1];
                                if (tile.wall != 15)
                                {
                                    tile = Main.tile[k, l + 1];
                                    if (tile.wall != 15)
                                    {
                                        tile = Main.tile[k - 1, l];
                                        if (tile.wall != 15)
                                        {
                                            tile = Main.tile[k + 1, l];
                                            if (tile.wall != 15)
                                            {
                                                PlaceWall(k, l, 64, mute: true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (type < 0)
                    {
                        tile = Main.tile[k, l];
                        if (tile.type == 53)
                        {
                            continue;
                        }

                        if (type == -2)
                        {
                            tile = Main.tile[k, l];
                            if (tile.active() && (l < waterLine || l > lavaLine))
                            {
                                tile = Main.tile[k, l];
                                tile.liquid = byte.MaxValue;
                                tile = Main.tile[k, l];
                                tile.lava(lava);
                                if (l > lavaLine)
                                {
                                    tile = Main.tile[k, l];
                                    tile.lava(lava: true);
                                }
                            }
                        }

                        tile = Main.tile[k, l];
                        tile.active(active: false);
                        continue;
                    }

                    if (flag && (double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.3 * (1.0 + (double)genRand.Next(-10, 11) * 0.01))
                    {
                        PlaceWall(k, l, 180, mute: true);
                    }

                    if (flag2 && (double)(Math.Abs((float)k - vector.X) + Math.Abs((float)l - vector.Y)) < strength * 0.3 * (1.0 + (double)genRand.Next(-10, 11) * 0.01))
                    {
                        PlaceWall(k, l, 178, mute: true);
                    }

                    if (!overRide)
                    {
                        tile = Main.tile[k, l];
                        if (tile.active())
                        {
                            goto IL_0827;
                        }
                    }

                    Tile tile2 = Main.tile[k, l];
                    bool flag3 = false;
                    flag3 = Main.tileStone[type] && tile2.type != 1;
                    if (!TileID.Sets.CanBeClearedDuringGeneration[tile2.type])
                    {
                        flag3 = true;
                    }

                    switch (tile2.type)
                    {
                        case 53:
                            if (type == 59 && UndergroundDesertLocation.Contains(k, l))
                            {
                                flag3 = true;
                            }

                            if (type == 40)
                            {
                                flag3 = true;
                            }

                            if ((double)l < Main.worldSurface && type != 59)
                            {
                                flag3 = true;
                            }

                            break;
                        case 45:
                        case 147:
                        case 189:
                        case 190:
                        case 196:
                        case 460:
                            flag3 = true;
                            break;
                        case 396:
                        case 397:
                            flag3 = !TileID.Sets.Ore[type];
                            break;
                        case 1:
                            if (type == 59 && (double)l < Main.worldSurface + (double)genRand.Next(-50, 50))
                            {
                                flag3 = true;
                            }

                            break;
                        case 367:
                        case 368:
                            if (type == 59)
                            {
                                flag3 = true;
                            }

                            break;
                    }

                    if (!flag3)
                    {
                        tile2.type = (ushort)type;
                    }

                    goto IL_0827;
                    IL_0827:
                    if (addTile)
                    {
                        tile = Main.tile[k, l];
                        tile.active(active: true);
                        tile = Main.tile[k, l];
                        tile.liquid = 0;
                        tile = Main.tile[k, l];
                        tile.lava(lava: false);
                    }

                    if (noYChange && (double)l < Main.worldSurface && type != 59)
                    {
                        tile = Main.tile[k, l];
                        tile.wall = 2;
                    }

                    if (type == 59 && l > waterLine)
                    {
                        tile = Main.tile[k, l];
                        if (tile.liquid > 0)
                        {
                            tile = Main.tile[k, l];
                            tile.lava(lava: false);
                            tile = Main.tile[k, l];
                            tile.liquid = 0;
                        }
                    }
                }
            }

            vector += vector2;
            if ((!drunkWorldGen || genRand.Next(3) != 0) && num > 50.0)
            {
                vector += vector2;
                num2 -= 1f;
                vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                if (num > 100.0)
                {
                    vector += vector2;
                    num2 -= 1f;
                    vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                    vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                    if (num > 150.0)
                    {
                        vector += vector2;
                        num2 -= 1f;
                        vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                        vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                        if (num > 200.0)
                        {
                            vector += vector2;
                            num2 -= 1f;
                            vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                            vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                            if (num > 250.0)
                            {
                                vector += vector2;
                                num2 -= 1f;
                                vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                if (num > 300.0)
                                {
                                    vector += vector2;
                                    num2 -= 1f;
                                    vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                    vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                    if (num > 400.0)
                                    {
                                        vector += vector2;
                                        num2 -= 1f;
                                        vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                        vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                        if (num > 500.0)
                                        {
                                            vector += vector2;
                                            num2 -= 1f;
                                            vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                            vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                            if (num > 600.0)
                                            {
                                                vector += vector2;
                                                num2 -= 1f;
                                                vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                                vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                                if (num > 700.0)
                                                {
                                                    vector += vector2;
                                                    num2 -= 1f;
                                                    vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                                    vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                                    if (num > 800.0)
                                                    {
                                                        vector += vector2;
                                                        num2 -= 1f;
                                                        vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                                        vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                                        if (num > 900.0)
                                                        {
                                                            vector += vector2;
                                                            num2 -= 1f;
                                                            vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                                                            vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
            if (drunkWorldGen)
            {
                vector2.X += (float)genRand.Next(-10, 11) * 0.25f;
            }

            if (vector2.X > 1f)
            {
                vector2.X = 1f;
            }

            if (vector2.X < -1f)
            {
                vector2.X = -1f;
            }

            if (!noYChange)
            {
                vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
                if (vector2.Y > 1f)
                {
                    vector2.Y = 1f;
                }

                if (vector2.Y < -1f)
                {
                    vector2.Y = -1f;
                }
            }
            else if (type != 59 && num < 3.0)
            {
                if (vector2.Y > 1f)
                {
                    vector2.Y = 1f;
                }

                if (vector2.Y < -1f)
                {
                    vector2.Y = -1f;
                }
            }

            if (type == 59 && !noYChange)
            {
                if ((double)vector2.Y > 0.5)
                {
                    vector2.Y = 0.5f;
                }

                if ((double)vector2.Y < -0.5)
                {
                    vector2.Y = -0.5f;
                }

                if ((double)vector.Y < Main.rockLayer + 100.0)
                {
                    vector2.Y = 1f;
                }

                if (vector.Y > (float)(Main.maxTilesY - 300))
                {
                    vector2.Y = -1f;
                }
            }
        }
    }

    public static void Mountinater(int i, int j)
    {
        double num = genRand.Next(80, 120);
        double num2 = num;
        float num3 = genRand.Next(40, 55);
        Vector2 vector = default(Vector2);
        vector.X = i;
        vector.Y = (float)j + num3 / 2f;
        Vector2 vector2 = default(Vector2);
        vector2.X = (float)genRand.Next(-10, 11) * 0.1f;
        vector2.Y = (float)genRand.Next(-20, -10) * 0.1f;
        while (num > 0.0 && num3 > 0f)
        {
            num -= (double)genRand.Next(4);
            num3 -= 1f;
            int num4 = (int)((double)vector.X - num * 0.5);
            int num5 = (int)((double)vector.X + num * 0.5);
            int num6 = (int)((double)vector.Y - num * 0.5);
            int num7 = (int)((double)vector.Y + num * 0.5);
            if (num4 < 0)
            {
                num4 = 0;
            }

            if (num5 > Main.maxTilesX)
            {
                num5 = Main.maxTilesX;
            }

            if (num6 < 0)
            {
                num6 = 0;
            }

            if (num7 > Main.maxTilesY)
            {
                num7 = Main.maxTilesY;
            }

            num2 = num * (double)genRand.Next(80, 120) * 0.01;
            for (int k = num4; k < num5; k++)
            {
                for (int l = num6; l < num7; l++)
                {
                    float num8 = Math.Abs((float)k - vector.X);
                    float num9 = Math.Abs((float)l - vector.Y);
                    if (Math.Sqrt(num8 * num8 + num9 * num9) < num2 * 0.4)
                    {
                        Tile tile = Main.tile[k, l];
                        if (!tile.active())
                        {
                            tile = Main.tile[k, l];
                            tile.active(active: true);
                            tile = Main.tile[k, l];
                            tile.type = 0;
                        }
                    }
                }
            }

            vector += vector2;
            vector2.X += (float)genRand.Next(-10, 11) * 0.05f;
            vector2.Y += (float)genRand.Next(-10, 11) * 0.05f;
            if ((double)vector2.X > 0.5)
            {
                vector2.X = 0.5f;
            }

            if ((double)vector2.X < -0.5)
            {
                vector2.X = -0.5f;
            }

            if ((double)vector2.Y > -0.5)
            {
                vector2.Y = -0.5f;
            }

            if ((double)vector2.Y < -1.5)
            {
                vector2.Y = -1.5f;
            }
        }
    }

    public static void Caverer(int X, int Y)
    {
        switch (genRand.Next(2))
        {
            case 0:
                {
                    int num4 = genRand.Next(7, 9);
                    float num5 = (float)genRand.Next(100) * 0.01f;
                    float num6 = 1f - num5;
                    if (genRand.Next(2) == 0)
                    {
                        num5 = 0f - num5;
                    }

                    if (genRand.Next(2) == 0)
                    {
                        num6 = 0f - num6;
                    }

                    Vector2 vector2 = new Vector2(X, Y);
                    for (int j = 0; j < num4; j++)
                    {
                        vector2 = digTunnel(vector2.X, vector2.Y, num5, num6, genRand.Next(6, 20), genRand.Next(4, 9));
                        num5 += (float)genRand.Next(-20, 21) * 0.1f;
                        num6 += (float)genRand.Next(-20, 21) * 0.1f;
                        if ((double)num5 < -1.5)
                        {
                            num5 = -1.5f;
                        }

                        if ((double)num5 > 1.5)
                        {
                            num5 = 1.5f;
                        }

                        if ((double)num6 < -1.5)
                        {
                            num6 = -1.5f;
                        }

                        if ((double)num6 > 1.5)
                        {
                            num6 = 1.5f;
                        }

                        float num7 = (float)genRand.Next(100) * 0.01f;
                        float num8 = 1f - num7;
                        if (genRand.Next(2) == 0)
                        {
                            num7 = 0f - num7;
                        }

                        if (genRand.Next(2) == 0)
                        {
                            num8 = 0f - num8;
                        }

                        Vector2 vector3 = digTunnel(vector2.X, vector2.Y, num7, num8, genRand.Next(30, 50), genRand.Next(3, 6));
                        TileRunner((int)vector3.X, (int)vector3.Y, genRand.Next(10, 20), genRand.Next(5, 10), -1);
                    }

                    break;
                }
            case 1:
                {
                    int num = genRand.Next(15, 30);
                    float num2 = (float)genRand.Next(100) * 0.01f;
                    float num3 = 1f - num2;
                    if (genRand.Next(2) == 0)
                    {
                        num2 = 0f - num2;
                    }

                    if (genRand.Next(2) == 0)
                    {
                        num3 = 0f - num3;
                    }

                    Vector2 vector = new Vector2(X, Y);
                    for (int i = 0; i < num; i++)
                    {
                        vector = digTunnel(vector.X, vector.Y, num2, num3, genRand.Next(5, 15), genRand.Next(2, 6), Wet: true);
                        num2 += (float)genRand.Next(-20, 21) * 0.1f;
                        num3 += (float)genRand.Next(-20, 21) * 0.1f;
                        if ((double)num2 < -1.5)
                        {
                            num2 = -1.5f;
                        }

                        if ((double)num2 > 1.5)
                        {
                            num2 = 1.5f;
                        }

                        if ((double)num3 < -1.5)
                        {
                            num3 = -1.5f;
                        }

                        if ((double)num3 > 1.5)
                        {
                            num3 = 1.5f;
                        }
                    }

                    break;
                }
        }
    }

    public static Vector2 digTunnel(float X, float Y, float xDir, float yDir, int Steps, int Size, bool Wet = false)
    {
        float num = X;
        float num2 = Y;
        try
        {
            float num3 = 0f;
            float num4 = 0f;
            float num5 = Size;
            num = MathHelper.Clamp(num, num5 + 1f, (float)Main.maxTilesX - num5 - 1f);
            num2 = MathHelper.Clamp(num2, num5 + 1f, (float)Main.maxTilesY - num5 - 1f);
            for (int i = 0; i < Steps; i++)
            {
                for (int j = (int)(num - num5); (float)j <= num + num5; j++)
                {
                    for (int k = (int)(num2 - num5); (float)k <= num2 + num5; k++)
                    {
                        if ((double)(Math.Abs((float)j - num) + Math.Abs((float)k - num2)) < (double)num5 * (1.0 + (double)genRand.Next(-10, 11) * 0.005) && j >= 0 && j < Main.maxTilesX && k >= 0 && k < Main.maxTilesY)
                        {
                            Tile tile = Main.tile[j, k];
                            tile.active(active: false);
                            if (Wet)
                            {
                                tile = Main.tile[j, k];
                                tile.liquid = byte.MaxValue;
                            }
                        }
                    }
                }

                num5 += (float)genRand.Next(-50, 51) * 0.03f;
                if ((double)num5 < (double)Size * 0.6)
                {
                    num5 = (float)Size * 0.6f;
                }

                if (num5 > (float)(Size * 2))
                {
                    num5 = (float)Size * 2f;
                }

                num3 += (float)genRand.Next(-20, 21) * 0.01f;
                num4 += (float)genRand.Next(-20, 21) * 0.01f;
                if (num3 < -1f)
                {
                    num3 = -1f;
                }

                if (num3 > 1f)
                {
                    num3 = 1f;
                }

                if (num4 < -1f)
                {
                    num4 = -1f;
                }

                if (num4 > 1f)
                {
                    num4 = 1f;
                }

                num += (xDir + num3) * 0.6f;
                num2 += (yDir + num4) * 0.6f;
            }
        }
        catch
        {
        }

        return new Vector2(num, num2);
    }

    private static void NotTheBees()
    {
        if (!notTheBees)
        {
            return;
        }

        /*for (int i = 0; i < Main.maxTilesX; i++)
        {
            int num = 0;
            while (num < Main.maxTilesY - 180)
            {
                Tile tile = Main.tile[i, num];
                if (tile.type == 52)
                {
                    tile = Main.tile[i, num];
                    tile.type = 62;
                }

                if (!SolidOrSlopedTile(i, num))
                {
                    bool[] crackedBricks = TileID.Sets.CrackedBricks;
                    tile = Main.tile[i, num];
                    if (!crackedBricks[tile.type])
                    {
                        goto IL_074f;
                    }
                }

                bool[] ore = TileID.Sets.Ore;
                tile = Main.tile[i, num];
                if (!ore[tile.type])
                {
                    tile = Main.tile[i, num];
                    if (tile.type != 123)
                    {
                        tile = Main.tile[i, num];
                        if (tile.type != 40)
                        {
                            tile = Main.tile[i, num];
                            if (tile.type != 191)
                            {
                                tile = Main.tile[i, num];
                                if (tile.type != 383)
                                {
                                    tile = Main.tile[i, num];
                                    if (tile.type != 192)
                                    {
                                        tile = Main.tile[i, num];
                                        if (tile.type != 384)
                                        {
                                            tile = Main.tile[i, num];
                                            if (tile.type != 151)
                                            {
                                                tile = Main.tile[i, num];
                                                if (tile.type != 189)
                                                {
                                                    tile = Main.tile[i, num];
                                                    if (tile.type != 196)
                                                    {
                                                        tile = Main.tile[i, num];
                                                        if (tile.type != 120)
                                                        {
                                                            tile = Main.tile[i, num];
                                                            if (tile.type != 158)
                                                            {
                                                                tile = Main.tile[i, num];
                                                                if (tile.type != 175)
                                                                {
                                                                    tile = Main.tile[i, num];
                                                                    if (tile.type != 45)
                                                                    {
                                                                        tile = Main.tile[i, num];
                                                                        if (tile.type != 119)
                                                                        {
                                                                            tile = Main.tile[i, num];
                                                                            if (tile.type >= 63)
                                                                            {
                                                                                tile = Main.tile[i, num];
                                                                                if (tile.type <= 68)
                                                                                {
                                                                                    tile = Main.tile[i, num];
                                                                                    tile.type = 230;
                                                                                    goto IL_074f;
                                                                                }
                                                                            }

                                                                            tile = Main.tile[i, num];
                                                                            if (tile.type != 57)
                                                                            {
                                                                                tile = Main.tile[i, num];
                                                                                if (tile.type != 76)
                                                                                {
                                                                                    tile = Main.tile[i, num];
                                                                                    if (tile.type != 75)
                                                                                    {
                                                                                        tile = Main.tile[i, num];
                                                                                        if (tile.type != 229)
                                                                                        {
                                                                                            tile = Main.tile[i, num];
                                                                                            if (tile.type != 230)
                                                                                            {
                                                                                                tile = Main.tile[i, num];
                                                                                                if (tile.type != 407)
                                                                                                {
                                                                                                    tile = Main.tile[i, num];
                                                                                                    if (tile.type != 404)
                                                                                                    {
                                                                                                        tile = Main.tile[i, num];
                                                                                                        if (tile.type == 224)
                                                                                                        {
                                                                                                            tile = Main.tile[i, num];
                                                                                                            tile.type = 229;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            tile = Main.tile[i, num];
                                                                                                            if (tile.type == 53)
                                                                                                            {
                                                                                                                if (i < beachDistance + genRand.Next(3) || i > Main.maxTilesX - beachDistance - genRand.Next(3))
                                                                                                                {
                                                                                                                    tile = Main.tile[i, num];
                                                                                                                    tile.type = 229;
                                                                                                                }
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if (i <= beachDistance - genRand.Next(3) || i >= Main.maxTilesX - beachDistance + genRand.Next(3))
                                                                                                                {
                                                                                                                    goto IL_0495;
                                                                                                                }

                                                                                                                tile = Main.tile[i, num];
                                                                                                                if (tile.type != 397)
                                                                                                                {
                                                                                                                    tile = Main.tile[i, num];
                                                                                                                    if (tile.type != 396)
                                                                                                                    {
                                                                                                                        goto IL_0495;
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            goto IL_074f;
                                        }
                                    }

                                    tile = Main.tile[i, num];
                                    tile.type = 384;
                                    goto IL_074f;
                                }
                            }

                            tile = Main.tile[i, num];
                            tile.type = 383;
                        }
                    }
                }

                goto IL_074f;
                IL_074f:
                tile = Main.tile[i, num];
                if (tile.wall != 15)
                {
                    tile = Main.tile[i, num];
                    if (tile.wall != 64)
                    {
                        tile = Main.tile[i, num];
                        if (tile.wall != 204)
                        {
                            tile = Main.tile[i, num];
                            if (tile.wall != 205)
                            {
                                tile = Main.tile[i, num];
                                if (tile.wall != 206)
                                {
                                    tile = Main.tile[i, num];
                                    if (tile.wall != 207)
                                    {
                                        tile = Main.tile[i, num];
                                        if (tile.wall != 23)
                                        {
                                            tile = Main.tile[i, num];
                                            if (tile.wall != 24)
                                            {
                                                tile = Main.tile[i, num];
                                                if (tile.wall != 42)
                                                {
                                                    tile = Main.tile[i, num];
                                                    if (tile.wall != 10)
                                                    {
                                                        tile = Main.tile[i, num];
                                                        if (tile.wall != 21)
                                                        {
                                                            tile = Main.tile[i, num];
                                                            if (tile.wall != 82)
                                                            {
                                                                tile = Main.tile[i, num];
                                                                if (tile.wall != 187)
                                                                {
                                                                    tile = Main.tile[i, num];
                                                                    if (tile.wall != 216)
                                                                    {
                                                                        tile = Main.tile[i, num];
                                                                        if (tile.wall != 34)
                                                                        {
                                                                            tile = Main.tile[i, num];
                                                                            if (tile.wall != 244)
                                                                            {
                                                                                tile = Main.tile[i, num];
                                                                                if (tile.wall == 87)
                                                                                {
                                                                                    tile = Main.tile[i, num];
                                                                                    tile.wallColor(15);
                                                                                }
                                                                                else
                                                                                {
                                                                                    bool[] wallDungeon = Main.wallDungeon;
                                                                                    tile = Main.tile[i, num];
                                                                                    if (wallDungeon[tile.wall])
                                                                                    {
                                                                                        tile = Main.tile[i, num];
                                                                                        tile.wallColor(14);
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        tile = Main.tile[i, num];
                                                                                        if (tile.wall == 2)
                                                                                        {
                                                                                            tile = Main.tile[i, num];
                                                                                            tile.wall = 2;
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            tile = Main.tile[i, num];
                                                                                            if (tile.wall == 196)
                                                                                            {
                                                                                                tile = Main.tile[i, num];
                                                                                                tile.wall = 196;
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                tile = Main.tile[i, num];
                                                                                                if (tile.wall == 197)
                                                                                                {
                                                                                                    tile = Main.tile[i, num];
                                                                                                    tile.wall = 197;
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    tile = Main.tile[i, num];
                                                                                                    if (tile.wall == 198)
                                                                                                    {
                                                                                                        tile = Main.tile[i, num];
                                                                                                        tile.wall = 198;
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        tile = Main.tile[i, num];
                                                                                                        if (tile.wall == 199)
                                                                                                        {
                                                                                                            tile = Main.tile[i, num];
                                                                                                            tile.wall = 199;
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            tile = Main.tile[i, num];
                                                                                                            if (tile.wall == 63)
                                                                                                            {
                                                                                                                tile = Main.tile[i, num];
                                                                                                                tile.wall = 64;
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                tile = Main.tile[i, num];
                                                                                                                if (tile.wall != 3)
                                                                                                                {
                                                                                                                    tile = Main.tile[i, num];
                                                                                                                    if (tile.wall != 83)
                                                                                                                    {
                                                                                                                        tile = Main.tile[i, num];
                                                                                                                        if (tile.wall != 73)
                                                                                                                        {
                                                                                                                            tile = Main.tile[i, num];
                                                                                                                            if (tile.wall != 13)
                                                                                                                            {
                                                                                                                                tile = Main.tile[i, num];
                                                                                                                                if (tile.wall != 14)
                                                                                                                                {
                                                                                                                                    tile = Main.tile[i, num];
                                                                                                                                    if (tile.wall > 0)
                                                                                                                                    {
                                                                                                                                        tile = Main.tile[i, num];
                                                                                                                                        tile.wall = 86;
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                tile = Main.tile[i, num];
                if (tile.liquid > 0 && num <= lavaLine + 2)
                {
                    if ((double)num > Main.rockLayer && (i < beachDistance + 200 || i > Main.maxTilesX - beachDistance - 200))
                    {
                        tile = Main.tile[i, num];
                        tile.honey(honey: false);
                    }
                    else
                    {
                        bool[] wallDungeon2 = Main.wallDungeon;
                        tile = Main.tile[i, num];
                        if (wallDungeon2[tile.wall])
                        {
                            tile = Main.tile[i, num];
                            tile.honey(honey: false);
                        }
                        else
                        {
                            tile = Main.tile[i, num];
                            tile.honey(honey: true);
                        }
                    }
                }

                num++;
                continue;
                IL_0495:
                tile = Main.tile[i, num];
                if (tile.type != 10)
                {
                    tile = Main.tile[i, num];
                    if (tile.type != 203)
                    {
                        tile = Main.tile[i, num];
                        if (tile.type != 25)
                        {
                            tile = Main.tile[i, num];
                            if (tile.type != 137)
                            {
                                tile = Main.tile[i, num];
                                if (tile.type != 138)
                                {
                                    tile = Main.tile[i, num];
                                    if (tile.type != 141)
                                    {
                                        bool[] tileDungeon = Main.tileDungeon;
                                        tile = Main.tile[i, num];
                                        if (!tileDungeon[tile.type])
                                        {
                                            bool[] crackedBricks2 = TileID.Sets.CrackedBricks;
                                            tile = Main.tile[i, num];
                                            if (!crackedBricks2[tile.type])
                                            {
                                                tile = Main.tile[i, num];
                                                if (tile.type == 226)
                                                {
                                                    tile = Main.tile[i, num];
                                                    tile.color(15);
                                                }
                                                else
                                                {
                                                    tile = Main.tile[i, num];
                                                    if (tile.type != 202)
                                                    {
                                                        tile = Main.tile[i, num];
                                                        if (tile.type != 70)
                                                        {
                                                            tile = Main.tile[i, num];
                                                            if (tile.type != 48)
                                                            {
                                                                tile = Main.tile[i, num];
                                                                if (tile.type != 232)
                                                                {
                                                                    bool[] grass = TileID.Sets.Conversion.Grass;
                                                                    tile = Main.tile[i, num];
                                                                    if (grass[tile.type])
                                                                    {
                                                                        if (num > lavaLine + genRand.Next(-2, 3) + 2)
                                                                        {
                                                                            tile = Main.tile[i, num];
                                                                            tile.type = 70;
                                                                        }
                                                                        else
                                                                        {
                                                                            tile = Main.tile[i, num];
                                                                            tile.type = 60;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        tile = Main.tile[i, num];
                                                                        if (tile.type != 0)
                                                                        {
                                                                            tile = Main.tile[i, num];
                                                                            if (tile.type != 59)
                                                                            {
                                                                                if (num > lavaLine + genRand.Next(-2, 3) + 2)
                                                                                {
                                                                                    tile = Main.tile[i, num];
                                                                                    tile.type = 230;
                                                                                }
                                                                                else
                                                                                {
                                                                                    tile = Main.tile[i, num];
                                                                                    tile.type = 225;
                                                                                }

                                                                                goto IL_074f;
                                                                            }
                                                                        }

                                                                        tile = Main.tile[i, num];
                                                                        tile.type = 59;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                goto IL_074f;
                                            }
                                        }

                                        tile = Main.tile[i, num];
                                        tile.color(14);
                                    }
                                }
                            }
                        }
                    }
                }

                goto IL_074f;
            }
        }*/
    }

    public static bool InWorld(int x, int y, int fluff = 0)
    {
        if (x < fluff || x >= Main.maxTilesX - fluff || y < fluff || y >= Main.maxTilesY - fluff)
        {
            return false;
        }

        return true;
    }

    public static void SpreadGrass(int i, int j, int dirt = 0, int grass = 2, bool repeat = true, byte color = 0)
    {
        try
        {
            if (!InWorld(i, j, 1))
            {
                return;
            }
            
            if (gen && (grass == 199 || grass == 23))
            {
                int num = beachDistance;
                if (((double)i > (double)Main.maxTilesX * 0.45 && (double)i <= (double)Main.maxTilesX * 0.55) || i < num || i >= Main.maxTilesX - num)
                {
                    return;
                }

                goto IL_00ca;
            }

            if (!gen && (grass == 199 || grass == 23))
            {
                goto IL_00ca;
            }

            Tile tile = Main.tile[i, j];
            if (tile.type == dirt)
            {
                tile = Main.tile[i, j];
                if (tile.active() && (!((double)j >= Main.worldSurface) || dirt != 0))
                {
                    goto IL_00ca;
                }
            }

            goto end_IL_0000;
            IL_020a:
            if (grass != 109)
            {
                goto IL_0230;
            }

            tile = Main.tile[i, j - 1];
            if (tile.type != 27)
            {
                goto IL_0230;
            }

            goto end_IL_0000;
            IL_0230:
            tile = Main.tile[i, j];
            tile.type = (ushort)grass;
            tile = Main.tile[i, j];
            tile.color(color);
            int num2;
            int num3;
            int num4;
            int num5;
            for (int k = num2; k < num3; k++)
            {
                for (int l = num4; l < num5; l++)
                {
                    tile = Main.tile[k, l];
                    if (!tile.active())
                    {
                        continue;
                    }

                    tile = Main.tile[k, l];
                    if (tile.type != dirt)
                    {
                        continue;
                    }

                    try
                    {
                        if (repeat && grassSpread < 1000)
                        {
                            grassSpread++;
                            SpreadGrass(k, l, dirt, grass, repeat: true, 0);
                            grassSpread--;
                        }
                    }
                    catch
                    {
                    }
                }
            }

            goto end_IL_0000;
            IL_01e6:
            if (grass != 199)
            {
                goto IL_020a;
            }

            tile = Main.tile[i, j - 1];
            if (tile.type != 27)
            {
                goto IL_020a;
            }

            goto end_IL_0000;
            IL_00ca:
            num2 = i - 1;
            num3 = i + 2;
            num4 = j - 1;
            num5 = j + 2;
            if (num2 < 0)
            {
                num2 = 0;
            }

            if (num3 > Main.maxTilesX)
            {
                num3 = Main.maxTilesX;
            }

            if (num4 < 0)
            {
                num4 = 0;
            }

            if (num5 > Main.maxTilesY)
            {
                num5 = Main.maxTilesY;
            }

            bool flag = true;
            for (int m = num2; m < num3; m++)
            {
                for (int num6 = num4; num6 < num5; num6++)
                {
                    tile = Main.tile[m, num6];
                    if (tile.active())
                    {
                        bool[] tileSolid = Main.tileSolid;
                        tile = Main.tile[m, num6];
                        if (tileSolid[tile.type])
                        {
                            goto IL_014e;
                        }
                    }

                    flag = false;
                    goto IL_014e;
                    IL_014e:
                    tile = Main.tile[m, num6];
                    if (tile.lava())
                    {
                        tile = Main.tile[m, num6];
                        if (tile.liquid > 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
            }

            if (flag)
            {
                return;
            }

            bool[] canBeClearedDuringGeneration = TileID.Sets.CanBeClearedDuringGeneration;
            tile = Main.tile[i, j];
            if (canBeClearedDuringGeneration[tile.type])
            {
                if (grass != 23)
                {
                    goto IL_01e6;
                }

                tile = Main.tile[i, j - 1];
                if (tile.type != 27)
                {
                    goto IL_01e6;
                }
            }

            end_IL_0000:;
        }
        catch
        {
        }
    }

    private static void ScanTileColumnAndRemoveClumps(int x)
    {
        int num = 0;
        int y = 0;
        for (int i = 10; i < Main.maxTilesY - 10; i++)
        {
            if (Main.tile[x, i].active() && Main.tileSolid[Main.tile[x, i].type] && TileID.Sets.CanBeClearedDuringGeneration[Main.tile[x, i].type])
            {
                if (num == 0)
                {
                    y = i;
                }

                num++;
                continue;
            }

            if (num > 0 && num < tileCounterMax)
            {
                SmallConsecutivesFound++;
                if (tileCounter(x, y) < tileCounterMax)
                {
                    SmallConsecutivesEliminated++;
                    tileCounterKill();
                }
            }

            num = 0;
        }
    }

    public static int tileCounter(int x, int y)
    {
        tileCounterNum = 0;
        tileCounterNext(x, y);
        return tileCounterNum;
    }

    public static void tileCounterNext(int x, int y)
    {
        if (tileCounterNum >= tileCounterMax || x < 5 || x > Main.maxTilesX - 5 || y < 5 || y > Main.maxTilesY - 5 || !Main.tile[x, y].active() || !Main.tileSolid[Main.tile[x, y].type] || !TileID.Sets.CanBeClearedDuringGeneration[Main.tile[x, y].type])
        {
            return;
        }

        for (int i = 0; i < tileCounterNum; i++)
        {
            if (tileCounterX[i] == x && tileCounterY[i] == y)
            {
                return;
            }
        }

        tileCounterX[tileCounterNum] = x;
        tileCounterY[tileCounterNum] = y;
        tileCounterNum++;
        tileCounterNext(x - 1, y);
        tileCounterNext(x + 1, y);
        tileCounterNext(x, y - 1);
        tileCounterNext(x, y + 1);
    }

    public static void tileCounterKill()
    {
        for (int i = 0; i < tileCounterNum; i++)
        {
            int x = tileCounterX[i];
            int y = tileCounterY[i];
            Main.tile[x, y].active(active: false);
        }
    }
    #endregion

    #region passes
    /*internal static GenPass Reset()
    {
        return new PassLegacy("Reset", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            numOceanCaveTreasure = 0;
            skipDesertTileCheck = false;
            gen = true;
            Liquid.ReInit();
            noTileActions = true;
            progress.Message = "";
            SetupStatueList();
            RandomizeWeather();
            Main.cloudAlpha = 0f;
            Main.maxRaining = 0f;
            Main.raining = false;
            heartCount = 0;
            Main.checkXMas();
            Main.checkHalloween();
            ResetGenerator();
            UndergroundDesertLocation = Rectangle.Empty;
            UndergroundDesertHiveLocation = Rectangle.Empty;
            numLarva = 0;
            hellChestItem = new int[hellChestItem.Length];
            for (int num911 = 0; num911 < hellChestItem.Length; num911++)
            {
                bool flag63 = true;
                while (flag63)
                {
                    flag63 = false;
                    hellChestItem[num911] = genRand.Next(hellChestItem.Length);
                    for (int num912 = 0; num912 < num911; num912++)
                    {
                        if (hellChestItem[num912] == hellChestItem[num911])
                        {
                            flag63 = true;
                        }
                    }
                }
            }

            int num913 = 86400;
            Main.slimeRainTime = -genRand.Next(num913 * 2, num913 * 3);
            Main.cloudBGActive = -genRand.Next(8640, 86400);
            skipFramingDuringGen = false;
            SavedOreTiers.Copper = 7;
            SavedOreTiers.Iron = 6;
            SavedOreTiers.Silver = 9;
            SavedOreTiers.Gold = 8;
            copperBar = 20;
            ironBar = 22;
            silverBar = 21;
            goldBar = 19;
            if (genRand.Next(2) == 0)
            {
                copper = 166;
                copperBar = 703;
                SavedOreTiers.Copper = 166;
            }

            if (!dontStarveWorldGen && genRand.Next(2) == 0)
            {
                iron = 167;
                ironBar = 704;
                SavedOreTiers.Iron = 167;
            }

            if (genRand.Next(2) == 0)
            {
                silver = 168;
                silverBar = 705;
                SavedOreTiers.Silver = 168;
            }

            if (!dontStarveWorldGen && genRand.Next(2) == 0)
            {
                gold = 169;
                goldBar = 706;
                SavedOreTiers.Gold = 169;
            }

            crimson = genRand.Next(2) == 0;
            if (WorldGenParam_Evil == 0)
            {
                crimson = false;
            }

            if (WorldGenParam_Evil == 1)
            {
                crimson = true;
            }

            if (jungleHut == 0)
            {
                jungleHut = 119;
            }
            else if (jungleHut == 1)
            {
                jungleHut = 120;
            }
            else if (jungleHut == 2)
            {
                jungleHut = 158;
            }
            else if (jungleHut == 3)
            {
                jungleHut = 175;
            }
            else if (jungleHut == 4)
            {
                jungleHut = 45;
            }

            Main.worldID = genRand.Next(int.MaxValue);
            RandomizeTreeStyle();
            RandomizeCaveBackgrounds();
            RandomizeBackgrounds(genRand);
            RandomizeMoonState();
            TreeTops.CopyExistingWorldInfoForWorldGeneration();
            dungeonSide = ((genRand.Next(2) != 0) ? 1 : (-1));
            if (dungeonSide == -1)
            {
                float num914 = 1f - (float)genRand.Next(15, 30) * 0.01f;
                jungleOriginX = (int)((float)Main.maxTilesX * num914);
            }
            else
            {
                float num915 = (float)genRand.Next(15, 30) * 0.01f;
                jungleOriginX = (int)((float)Main.maxTilesX * num915);
            }

            int num916 = genRand.Next(Main.maxTilesX);
            if (drunkWorldGen)
            {
                dungeonSide *= -1;
            }

            if (dungeonSide == 1)
            {
                while ((float)num916 < (float)Main.maxTilesX * 0.6f || (float)num916 > (float)Main.maxTilesX * 0.75f)
                {
                    num916 = genRand.Next(Main.maxTilesX);
                }
            }
            else
            {
                while ((float)num916 < (float)Main.maxTilesX * 0.25f || (float)num916 > (float)Main.maxTilesX * 0.4f)
                {
                    num916 = genRand.Next(Main.maxTilesX);
                }
            }

            if (drunkWorldGen)
            {
                dungeonSide *= -1;
            }

            int num917 = genRand.Next(50, 90);
            float num918 = Main.maxTilesX / 4200;
            num917 += (int)((float)genRand.Next(20, 40) * num918);
            num917 += (int)((float)genRand.Next(20, 40) * num918);
            int num919 = num916 - num917;
            num917 = genRand.Next(50, 90);
            num917 += (int)((float)genRand.Next(20, 40) * num918);
            num917 += (int)((float)genRand.Next(20, 40) * num918);
            int num920 = num916 + num917;
            if (num919 < 0)
            {
                num919 = 0;
            }

            if (num920 > Main.maxTilesX)
            {
                num920 = Main.maxTilesX;
            }

            snowOriginLeft = num919;
            snowOriginRight = num920;
            leftBeachEnd = genRand.Next(beachSandRandomCenter - beachSandRandomWidthRange, beachSandRandomCenter + beachSandRandomWidthRange);
            if (dungeonSide == 1)
            {
                leftBeachEnd += beachSandDungeonExtraWidth;
            }
            else
            {
                leftBeachEnd += beachSandJungleExtraWidth;
            }

            rightBeachStart = Main.maxTilesX - genRand.Next(beachSandRandomCenter - beachSandRandomWidthRange, beachSandRandomCenter + beachSandRandomWidthRange);
            if (dungeonSide == -1)
            {
                rightBeachStart -= beachSandDungeonExtraWidth;
            }
            else
            {
                rightBeachStart -= beachSandJungleExtraWidth;
            }

            int num921 = 50;
            if (dungeonSide == -1)
            {
                dungeonLocation = genRand.Next(leftBeachEnd + num921, (int)((double)Main.maxTilesX * 0.2));
            }
            else
            {
                dungeonLocation = genRand.Next((int)((double)Main.maxTilesX * 0.8), rightBeachStart - num921);
            }
        });
    }*/

    internal static GenPass Terrain()
    {
        return new TerrainPass().OnBegin(delegate (GenPass pass)
        {
            TerrainPass obj9 = pass as TerrainPass;
            obj9.LeftBeachSize = leftBeachEnd;
            obj9.RightBeachSize = Main.maxTilesX - rightBeachStart;
        }).OnComplete(delegate (GenPass pass)
        {
            TerrainPass obj8 = pass as TerrainPass;
            rockLayer = obj8.RockLayer;
            rockLayerHigh = obj8.RockLayerHigh;
            rockLayerLow = obj8.RockLayerLow;
            worldSurface = obj8.WorldSurface;
            worldSurfaceHigh = obj8.WorldSurfaceHigh;
            worldSurfaceLow = obj8.WorldSurfaceLow;
            waterLine = obj8.WaterLine;
            lavaLine = obj8.LavaLine;
        });
    }

    internal static GenPass Dunes(WorldGenConfiguration configuration)
    {
        return new PassLegacy("Dunes", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "DunesPass";//Lang.gen[1].Value;
            int random13 = 0;//passConfig.Get<WorldGenRange>("Count").GetRandom(/*genRand*/random);
            float num905 = 0.5f;//passConfig.Get<float>("ChanceOfPyramid");
            if (drunkWorldGen) num905 = 1f;

            float num906 = (float)Main.maxTilesX / 4200f;
            PyrX = new int[random13 + 3];
            PyrY = new int[random13 + 3];
            DunesBiome dunesBiome = configuration.CreateBiome<DunesBiome>();
            for (int num907 = 0; num907 < random13; num907++)
            {
                progress.Set((float)num907 / (float)random13);
                Point origin5 = Point.Zero;
                bool flag59 = false;
                int num908 = 0;
                while (!flag59)
                {
                    origin5 = RandomWorldPoint(0, 500, 0, 500);
                    bool flag60 = Math.Abs(origin5.X - jungleOriginX) < (int)(600f * num906);
                    bool flag61 = Math.Abs(origin5.X - Main.maxTilesX / 2) < 300;
                    bool flag62 = origin5.X > snowOriginLeft - 300 && origin5.X < snowOriginRight + 300;
                    num908++;
                    if (num908 >= Main.maxTilesX)
                    {
                        flag60 = false;
                    }

                    if (num908 >= Main.maxTilesX * 2)
                    {
                        flag62 = false;
                    }

                    flag59 = !(flag60 || flag61 || flag62);
                }

                dunesBiome.Place(origin5, structures);
                if (/*genRand.NextFloat()*/(float)genRand.NextDouble() <= num905)
                {
                    int num909 = genRand.Next(origin5.X - 200, origin5.X + 200);
                    for (int num910 = 0; num910 < Main.maxTilesY; num910++)
                    {
                        if (Main.tile[num909, num910].active())
                        {
                            PyrX[numPyr] = num909;
                            PyrY[numPyr] = num910 + 20;
                            numPyr++;
                            break;
                        }
                    }
                }
            }
        });
    }

    public static GenPass OceanSand()
    {
        return new PassLegacy("Ocean Sand", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "WorldGeneration.OceanSand";//Language.GetTextValue("WorldGeneration.OceanSand");
            for (int num894 = 0; num894 < 3; num894++)
            {
                progress.Set((float)num894 / 3f);
                int num895 = genRand.Next(Main.maxTilesX);
                while ((float)num895 > (float)Main.maxTilesX * 0.4f && (float)num895 < (float)Main.maxTilesX * 0.6f)
                {
                    num895 = genRand.Next(Main.maxTilesX);
                }

                int num896 = genRand.Next(35, 90);
                if (num894 == 1)
                {
                    float num897 = Main.maxTilesX / 4200;
                    num896 += (int)((float)genRand.Next(20, 40) * num897);
                }

                if (genRand.Next(3) == 0)
                {
                    num896 *= 2;
                }

                if (num894 == 1)
                {
                    num896 *= 2;
                }

                int num898 = num895 - num896;
                num896 = genRand.Next(35, 90);
                if (genRand.Next(3) == 0)
                {
                    num896 *= 2;
                }

                if (num894 == 1)
                {
                    num896 *= 2;
                }

                int num899 = num895 + num896;
                if (num898 < 0)
                {
                    num898 = 0;
                }

                if (num899 > Main.maxTilesX)
                {
                    num899 = Main.maxTilesX;
                }

                if (num894 == 0)
                {
                    num898 = 0;
                    num899 = leftBeachEnd;
                }
                else if (num894 == 2)
                {
                    num898 = rightBeachStart;
                    num899 = Main.maxTilesX;
                }
                else if (num894 == 1)
                {
                    continue;
                }

                int num900 = genRand.Next(50, 100);
                for (int num901 = num898; num901 < num899; num901++)
                {
                    if (genRand.Next(2) == 0)
                    {
                        num900 += genRand.Next(-1, 2);
                        if (num900 < 50)
                        {
                            num900 = 50;
                        }

                        if (num900 > 200)
                        {
                            num900 = 200;
                        }
                    }

                    for (int num902 = 0; (double)num902 < (Main.worldSurface + Main.rockLayer) / 2.0; num902++)
                    {
                        Tile tile35 = Main.tile[num901, num902];
                        if (tile35.active())
                        {
                            if (num901 == (num898 + num899) / 2 && genRand.Next(6) == 0)
                            {
                                PyrX[numPyr] = num901;
                                PyrY[numPyr] = num902;
                                numPyr++;
                            }

                            int num903 = num900;
                            if (num901 - num898 < num903)
                            {
                                num903 = num901 - num898;
                            }

                            if (num899 - num901 < num903)
                            {
                                num903 = num899 - num901;
                            }

                            num903 += genRand.Next(5);
                            for (int num904 = num902; num904 < num902 + num903; num904++)
                            {
                                if (num901 > num898 + genRand.Next(5) && num901 < num899 - genRand.Next(5))
                                {
                                    tile35 = Main.tile[num901, num904];
                                    tile35.type = 53;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        });
    }

    public static GenPass SandPatches()
    {
        return new PassLegacy("Sand Patches", delegate
        {
            int num890 = (int)((float)Main.maxTilesX * 0.013f);
            for (int num891 = 0; num891 < num890; num891++)
            {
                int num892 = genRand.Next(0, Main.maxTilesX);
                int num893 = genRand.Next((int)Main.worldSurface, (int)Main.rockLayer);
                while ((double)num892 > (double)Main.maxTilesX * 0.46 && (double)num892 < (double)Main.maxTilesX * 0.54 && (double)num893 < Main.worldSurface + 150.0)
                {
                    num892 = genRand.Next(0, Main.maxTilesX);
                    num893 = genRand.Next((int)Main.worldSurface, (int)Main.rockLayer);
                }

                TileRunner(num892, num893, genRand.Next(15, 70), genRand.Next(20, 130), 53);
            }
        });
    }

    public static GenPass Tunnels()
    {
        return new PassLegacy("Tunnels", delegate
        {
            for (int num885 = 0; num885 < (int)((double)Main.maxTilesX * 0.0015); num885++)
            {
                if (numTunnels >= maxTunnels - 1)
                {
                    break;
                }

                int[] array = new int[10];
                int[] array2 = new int[10];
                int num886 = genRand.Next(450, Main.maxTilesX - 450);
                while ((double)num886 > (double)Main.maxTilesX * 0.4 && (double)num886 < (double)Main.maxTilesX * 0.6)
                {
                    num886 = genRand.Next(450, Main.maxTilesX - 450);
                }

                int num887 = 0;
                bool flag58;
                do
                {
                    flag58 = false;
                    for (int num888 = 0; num888 < 10; num888++)
                    {
                        for (num886 %= Main.maxTilesX; !Main.tile[num886, num887].active(); num887++)
                        {
                        }

                        if (Main.tile[num886, num887].type == 53)
                        {
                            flag58 = true;
                        }

                        array[num888] = num886;
                        array2[num888] = num887 - genRand.Next(11, 16);
                        num886 += genRand.Next(5, 11);
                    }
                }
                while (flag58);
                tunnelX[numTunnels] = array[5];
                numTunnels++;
                for (int num889 = 0; num889 < 10; num889++)
                {
                    TileRunner(array[num889], array2[num889], genRand.Next(5, 8), genRand.Next(6, 9), 0, addTile: true, -2f, -0.3f);
                    TileRunner(array[num889], array2[num889], genRand.Next(5, 8), genRand.Next(6, 9), 0, addTile: true, 2f, -0.3f);
                }
            }
        });
    }//this gets caught in a loop

    public static GenPass MountCaves()
    {
        return new PassLegacy("Mount Caves", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            numMCaves = 0;
            progress.Message = "Mount Caves";//Lang.gen[2].Value;
            for (int num878 = 0; num878 < (int)((double)Main.maxTilesX * 0.001); num878++)
            {
                progress.Set((float)num878 / (float)Main.maxTilesX * 0.001f);
                int num879 = 0;
                bool flag56 = false;
                bool flag57 = false;
                int num880 = genRand.Next((int)((double)Main.maxTilesX * 0.25), (int)((double)Main.maxTilesX * 0.75));
                while (!flag57)
                {
                    flag57 = true;
                    while (num880 > Main.maxTilesX / 2 - 90 && num880 < Main.maxTilesX / 2 + 90)
                    {
                        num880 = genRand.Next((int)((double)Main.maxTilesX * 0.25), (int)((double)Main.maxTilesX * 0.75));
                    }

                    for (int num881 = 0; num881 < numMCaves; num881++)
                    {
                        if (Math.Abs(num880 - mCaveX[num881]) < 100)
                        {
                            num879++;
                            flag57 = false;
                            break;
                        }
                    }

                    if (num879 >= Main.maxTilesX / 5)
                    {
                        flag56 = true;
                        break;
                    }
                }

                if (!flag56)
                {
                    for (int num882 = 0; (double)num882 < Main.worldSurface; num882++)
                    {
                        if (Main.tile[num880, num882].active())
                        {
                            for (int num883 = num880 - 50; num883 < num880 + 50; num883++)
                            {
                                for (int num884 = num882 - 25; num884 < num882 + 25; num884++)
                                {
                                    if (Main.tile[num883, num884].active() && (Main.tile[num883, num884].type == 53 || Main.tile[num883, num884].type == 151 || Main.tile[num883, num884].type == 274))
                                    {
                                        flag56 = true;
                                    }
                                }
                            }

                            if (!flag56)
                            {
                                Mountinater(num880, num882);
                                mCaveX[numMCaves] = num880;
                                mCaveY[numMCaves] = num882;
                                numMCaves++;
                                break;
                            }
                        }
                    }
                }
            }
        });
    }

    public static GenPass DirtWallBackgrounds()
    {
        return new PassLegacy("Dirt Wall Backgrounds", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Dirt Wall Backgrounds";//Lang.gen[3].Value;
            for (int num875 = 1; num875 < Main.maxTilesX - 1; num875++)
            {
                ushort num876 = 2;
                float value20 = (float)num875 / (float)Main.maxTilesX;
                progress.Set(value20);
                bool flag55 = false;
                howFar += genRand.Next(-1, 2);
                if (howFar < 0)
                {
                    howFar = 0;
                }

                if (howFar > 10)
                {
                    howFar = 10;
                }

                for (int num877 = 0; (double)num877 < Main.worldSurface + 10.0 && !((double)num877 > Main.worldSurface + (double)howFar); num877++)
                {
                    Tile tile34 = Main.tile[num875, num877];
                    if (tile34.active())
                    {
                        tile34 = Main.tile[num875, num877];
                        num876 = (ushort)((tile34.type != 147) ? 2u : 40u);
                    }

                    if (flag55)
                    {
                        tile34 = Main.tile[num875, num877];
                        if (tile34.wall != 64)
                        {
                            tile34 = Main.tile[num875, num877];
                            tile34.wall = num876;
                        }
                    }

                    tile34 = Main.tile[num875, num877];
                    if (tile34.active())
                    {
                        tile34 = Main.tile[num875 - 1, num877];
                        if (tile34.active())
                        {
                            tile34 = Main.tile[num875 + 1, num877];
                            if (tile34.active())
                            {
                                tile34 = Main.tile[num875, num877 + 1];
                                if (tile34.active())
                                {
                                    tile34 = Main.tile[num875 - 1, num877 + 1];
                                    if (tile34.active())
                                    {
                                        tile34 = Main.tile[num875 + 1, num877 + 1];
                                        if (tile34.active())
                                        {
                                            flag55 = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    public static GenPass RocksInDirt()
    {
        return new PassLegacy("Rocks In Dirt", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Rocks In Dirt";//Lang.gen[4].Value;
            float num869 = (float)(Main.maxTilesX * Main.maxTilesY) * 0.00015f;
            for (int num870 = 0; (float)num870 < num869; num870++)
            {
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next(0, (int)worldSurfaceLow + 1), genRand.Next(4, 15), genRand.Next(5, 40), 1);
            }

            progress.Set(0.34f);
            num869 = (float)(Main.maxTilesX * Main.maxTilesY) * 0.0002f;
            for (int num871 = 0; (float)num871 < num869; num871++)
            {
                int num872 = genRand.Next(0, Main.maxTilesX);
                int num873 = genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh + 1);
                if (!Main.tile[num872, num873 - 10].active())
                {
                    num873 = genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh + 1);
                }

                TileRunner(num872, num873, genRand.Next(4, 10), genRand.Next(5, 30), 1);
            }

            progress.Set(0.67f);
            num869 = (float)(Main.maxTilesX * Main.maxTilesY) * 0.0045f;
            for (int num874 = 0; (float)num874 < num869; num874++)
            {
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next((int)worldSurfaceHigh, (int)rockLayerHigh + 1), genRand.Next(2, 7), genRand.Next(2, 23), 1);
            }
        });
    }

    public static GenPass DirtInRocks()
    {
        return new PassLegacy("Dirt In Rocks", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Dirt In Rocks";//Lang.gen[5].Value;
            float num867 = (float)(Main.maxTilesX * Main.maxTilesY) * 0.005f;
            for (int num868 = 0; (float)num868 < num867; num868++)
            {
                progress.Set((float)num868 / num867);
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next((int)rockLayerLow, Main.maxTilesY), genRand.Next(2, 6), genRand.Next(2, 40), 0);
            }
        });
    }

    public static GenPass Clay()
    {
        return new PassLegacy("Clay", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Clay";// Lang.gen[6].Value;
            for (int num861 = 0; num861 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num861++)
            {
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next(0, (int)worldSurfaceLow), genRand.Next(4, 14), genRand.Next(10, 50), 40);
            }

            progress.Set(0.25f);
            for (int num862 = 0; num862 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 5E-05); num862++)
            {
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh + 1), genRand.Next(8, 14), genRand.Next(15, 45), 40);
            }

            progress.Set(0.5f);
            for (int num863 = 0; num863 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 2E-05); num863++)
            {
                TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next((int)worldSurfaceHigh, (int)rockLayerHigh + 1), genRand.Next(8, 15), genRand.Next(5, 50), 40);
            }

            progress.Set(0.75f);
            for (int num864 = 5; num864 < Main.maxTilesX - 5; num864++)
            {
                for (int num865 = 1; (double)num865 < Main.worldSurface - 1.0; num865++)
                {
                    Tile tile33 = Main.tile[num864, num865];
                    if (tile33.active())
                    {
                        for (int num866 = num865; num866 < num865 + 5; num866++)
                        {
                            tile33 = Main.tile[num864, num866];
                            if (tile33.type == 40)
                            {
                                tile33 = Main.tile[num864, num866];
                                tile33.type = 0;
                            }
                        }

                        break;
                    }
                }
            }
        });
    }

    public static GenPass SmallHoles()
    {
        return new PassLegacy("Small Holes", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            i2 = 0;
            progress.Message = "Small Holes";//Lang.gen[7].Value;
            double num857 = worldSurfaceHigh;
            for (int num858 = 0; num858 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015); num858++)
            {
                float value19 = (float)((double)num858 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.0015));
                progress.Set(value19);
                int type12 = -1;
                if (genRand.Next(5) == 0)
                {
                    type12 = -2;
                }

                int num859 = genRand.Next(0, Main.maxTilesX);
                int num860 = genRand.Next((int)worldSurfaceHigh, Main.maxTilesY);
                while (((num859 < smallHolesBeachAvoidance || num859 > Main.maxTilesX - smallHolesBeachAvoidance) && (double)num860 < num857) || ((double)num859 > (double)Main.maxTilesX * 0.45 && (double)num859 < (double)Main.maxTilesX * 0.55 && (double)num860 < worldSurface))
                {
                    num859 = genRand.Next(0, Main.maxTilesX);
                    num860 = genRand.Next((int)worldSurfaceHigh, Main.maxTilesY);
                }

                TileRunner(num859, num860, genRand.Next(2, 5), genRand.Next(2, 20), type12);
                num859 = genRand.Next(0, Main.maxTilesX);
                num860 = genRand.Next((int)worldSurfaceHigh, Main.maxTilesY);
                while (((num859 < smallHolesBeachAvoidance || num859 > Main.maxTilesX - smallHolesBeachAvoidance) && (double)num860 < num857) || ((double)num859 > (double)Main.maxTilesX * 0.45 && (double)num859 < (double)Main.maxTilesX * 0.55 && (double)num860 < worldSurface))
                {
                    num859 = genRand.Next(0, Main.maxTilesX);
                    num860 = genRand.Next((int)worldSurfaceHigh, Main.maxTilesY);
                }

                TileRunner(num859, num860, genRand.Next(8, 15), genRand.Next(7, 30), type12);
            }
        });
    }

    public static GenPass DirtLayerCaves()
    {
        return new PassLegacy("Dirt Layer Caves", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Dirt Layer Caves";//Lang.gen[8].Value;
            double num853 = worldSurfaceHigh;
            for (int num854 = 0; num854 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05); num854++)
            {
                float value18 = (float)((double)num854 / ((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05));
                progress.Set(value18);
                if (rockLayerHigh <= (double)Main.maxTilesY)
                {
                    int type11 = -1;
                    if (genRand.Next(6) == 0)
                    {
                        type11 = -2;
                    }

                    int num855 = genRand.Next(0, Main.maxTilesX);
                    int num856 = genRand.Next((int)worldSurfaceLow, (int)rockLayerHigh + 1);
                    while (((num855 < smallHolesBeachAvoidance || num855 > Main.maxTilesX - smallHolesBeachAvoidance) && (double)num856 < num853) || ((double)num855 >= (double)Main.maxTilesX * 0.45 && (double)num855 <= (double)Main.maxTilesX * 0.55 && (double)num856 < Main.worldSurface))
                    {
                        num855 = genRand.Next(0, Main.maxTilesX);
                        num856 = genRand.Next((int)worldSurfaceLow, (int)rockLayerHigh + 1);
                    }

                    TileRunner(num855, num856, genRand.Next(5, 15), genRand.Next(30, 200), type11);
                }
            }
        });
    }

    public static GenPass RockLayerCaves()
    {
        return new PassLegacy("Rock Layer Caves", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Rock Layer Caves";//Lang.gen[9].Value;
            for (int num852 = 0; num852 < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013); num852++)
            {
                float value17 = (float)((double)num852 / ((double)(Main.maxTilesX * Main.maxTilesY) * 0.00013));
                progress.Set(value17);
                if (rockLayerHigh <= (double)Main.maxTilesY)
                {
                    int type10 = -1;
                    if (genRand.Next(10) == 0)
                    {
                        type10 = -2;
                    }

                    TileRunner(genRand.Next(0, Main.maxTilesX), genRand.Next((int)rockLayerHigh, Main.maxTilesY), genRand.Next(6, 20), genRand.Next(50, 300), type10);
                }
            }
        });
    }

    public static GenPass SurfaceCaves()
    {
        return new PassLegacy("Surface Caves", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Surface Caves";//Lang.gen[10].Value;
            for (int num842 = 0; num842 < (int)((double)Main.maxTilesX * 0.002); num842++)
            {
                i2 = genRand.Next(0, Main.maxTilesX);
                while (((float)i2 > (float)Main.maxTilesX * 0.45f && (float)i2 < (float)Main.maxTilesX * 0.55f) || i2 < leftBeachEnd + 20 || i2 > rightBeachStart - 20)
                {
                    i2 = genRand.Next(0, Main.maxTilesX);
                }

                for (int num843 = 0; (double)num843 < worldSurfaceHigh; num843++)
                {
                    if (Main.tile[i2, num843].active())
                    {
                        TileRunner(i2, num843, genRand.Next(3, 6), genRand.Next(5, 50), -1, addTile: false, (float)genRand.Next(-10, 11) * 0.1f, 1f);
                        break;
                    }
                }
            }

            progress.Set(0.2f);
            for (int num844 = 0; num844 < (int)((double)Main.maxTilesX * 0.0007); num844++)
            {
                i2 = genRand.Next(0, Main.maxTilesX);
                while (((float)i2 > (float)Main.maxTilesX * 0.43f && (float)i2 < (float)Main.maxTilesX * 0.57f) || i2 < leftBeachEnd + 20 || i2 > rightBeachStart - 20)
                {
                    i2 = genRand.Next(0, Main.maxTilesX);
                }

                for (int num845 = 0; (double)num845 < worldSurfaceHigh; num845++)
                {
                    if (Main.tile[i2, num845].active())
                    {
                        TileRunner(i2, num845, genRand.Next(10, 15), genRand.Next(50, 130), -1, addTile: false, (float)genRand.Next(-10, 11) * 0.1f, 2f);
                        break;
                    }
                }
            }

            progress.Set(0.4f);
            for (int num846 = 0; num846 < (int)((double)Main.maxTilesX * 0.0003); num846++)
            {
                i2 = genRand.Next(0, Main.maxTilesX);
                while (((float)i2 > (float)Main.maxTilesX * 0.4f && (float)i2 < (float)Main.maxTilesX * 0.6f) || i2 < leftBeachEnd + 20 || i2 > rightBeachStart - 20)
                {
                    i2 = genRand.Next(0, Main.maxTilesX);
                }

                for (int num847 = 0; (double)num847 < worldSurfaceHigh; num847++)
                {
                    if (Main.tile[i2, num847].active())
                    {
                        TileRunner(i2, num847, genRand.Next(12, 25), genRand.Next(150, 500), -1, addTile: false, (float)genRand.Next(-10, 11) * 0.1f, 4f);
                        TileRunner(i2, num847, genRand.Next(8, 17), genRand.Next(60, 200), -1, addTile: false, (float)genRand.Next(-10, 11) * 0.1f, 2f);
                        TileRunner(i2, num847, genRand.Next(5, 13), genRand.Next(40, 170), -1, addTile: false, (float)genRand.Next(-10, 11) * 0.1f, 2f);
                        break;
                    }
                }
            }

            progress.Set(0.6f);
            for (int num848 = 0; num848 < (int)((double)Main.maxTilesX * 0.0004); num848++)
            {
                i2 = genRand.Next(0, Main.maxTilesX);
                while (((float)i2 > (float)Main.maxTilesX * 0.4f && (float)i2 < (float)Main.maxTilesX * 0.6f) || i2 < leftBeachEnd + 20 || i2 > rightBeachStart - 20)
                {
                    i2 = genRand.Next(0, Main.maxTilesX);
                }

                for (int num849 = 0; (double)num849 < worldSurfaceHigh; num849++)
                {
                    if (Main.tile[i2, num849].active())
                    {
                        TileRunner(i2, num849, genRand.Next(7, 12), genRand.Next(150, 250), -1, addTile: false, 0f, 1f, noYChange: true);
                        break;
                    }
                }
            }

            progress.Set(0.8f);
            float num850 = Main.maxTilesX / 4200;
            for (int num851 = 0; (float)num851 < 5f * num850; num851++)
            {
                try
                {
                    Caverer(genRand.Next(surfaceCavesBeachAvoidance2, Main.maxTilesX - surfaceCavesBeachAvoidance2), genRand.Next((int)Main.rockLayer, Main.maxTilesY - 400));
                }
                catch
                {
                }
            }
        });
    }

    public static GenPass WavyCaves()
    {
        return new PassLegacy("Wavy Caves", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            if (dontStarveWorldGen)
            {
                progress.Message = "Wavy Caves";//Language.GetTextValue("WorldGeneration.WavyCaves");
                float num833 = (float)Main.maxTilesX / 4200f;
                num833 *= num833;
                int num834 = (int)(35f * num833);
                int num835 = 0;
                int num836 = 80;
                for (int num837 = 0; num837 < num834; num837++)
                {
                    float num838 = (float)num837 / (float)(num834 - 1);
                    progress.Set(num838);
                    int num839 = 0;//genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                    int num840 = 0;
                    while (Math.Abs(num839 - num835) < num836)
                    {
                        num840++;
                        if (num840 > 100)
                        {
                            break;
                        }

                        //num839 = genRand.Next((int)Main.worldSurface + 100, Main.UnderworldLayer - 100);
                    }

                    num835 = num839;
                    int num841 = 80;
                    int startX = num841 + (int)((float)(Main.maxTilesX - num841 * 2) * num838);
                    try
                    {
                        //WavyCaverer(startX, num839, 12f + (float)genRand.Next(3, 6), 0.25f + genRand.NextFloat(), genRand.Next(300, 500), -1);
                    }
                    catch
                    {
                    }
                }
            }
        });
    }

    public static GenPass GenerateIceBiome()
    {
        return new PassLegacy("Generate Ice Biome", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Generate Ice Biome";//Lang.gen[56].Value;
            snowTop = (int)Main.worldSurface;
            int num826 = lavaLine - genRand.Next(160, 200);
            int num827 = snowOriginLeft;
            int num828 = snowOriginRight;
            int num829 = 10;
            for (int num830 = 0; num830 <= lavaLine - 140; num830++)
            {
                progress.Set((float)num830 / (float)(lavaLine - 140));
                num827 += genRand.Next(-4, 4);
                num828 += genRand.Next(-3, 5);
                if (num830 > 0)
                {
                    num827 = (num827 + snowMinX[num830 - 1]) / 2;
                    num828 = (num828 + snowMaxX[num830 - 1]) / 2;
                }

                if (dungeonSide > 0)
                {
                    if (genRand.Next(4) == 0)
                    {
                        num827++;
                        num828++;
                    }
                }
                else if (genRand.Next(4) == 0)
                {
                    num827--;
                    num828--;
                }

                snowMinX[num830] = num827;
                snowMaxX[num830] = num828;
                for (int num831 = num827; num831 < num828; num831++)
                {
                    Tile tile32;
                    if (num830 < num826)
                    {
                        tile32 = Main.tile[num831, num830];
                        if (tile32.wall == 2)
                        {
                            tile32 = Main.tile[num831, num830];
                            tile32.wall = 40;
                        }

                        tile32 = Main.tile[num831, num830];
                        switch (tile32.type)
                        {
                            case 0:
                            case 2:
                            case 23:
                            case 40:
                            case 53:
                                tile32 = Main.tile[num831, num830];
                                tile32.type = 147;
                                break;
                            case 1:
                                tile32 = Main.tile[num831, num830];
                                tile32.type = 161;
                                break;
                        }
                    }
                    else
                    {
                        num829 += genRand.Next(-3, 4);
                        if (genRand.Next(3) == 0)
                        {
                            num829 += genRand.Next(-4, 5);
                            if (genRand.Next(3) == 0)
                            {
                                num829 += genRand.Next(-6, 7);
                            }
                        }

                        if (num829 < 0)
                        {
                            num829 = genRand.Next(3);
                        }
                        else if (num829 > 50)
                        {
                            num829 = 50 - genRand.Next(3);
                        }

                        for (int num832 = num830; num832 < num830 + num829; num832++)
                        {
                            tile32 = Main.tile[num831, num832];
                            if (tile32.wall == 2)
                            {
                                tile32 = Main.tile[num831, num832];
                                tile32.wall = 40;
                            }

                            tile32 = Main.tile[num831, num832];
                            switch (tile32.type)
                            {
                                case 0:
                                case 2:
                                case 23:
                                case 40:
                                case 53:
                                    tile32 = Main.tile[num831, num832];
                                    tile32.type = 147;
                                    break;
                                case 1:
                                    tile32 = Main.tile[num831, num832];
                                    tile32.type = 161;
                                    break;
                            }
                        }
                    }
                }

                if (snowBottom < num830)
                {
                    snowBottom = num830;
                }
            }
        });
    }//throws exception

    public static GenPass Grass()
    {
        return new PassLegacy("Grass", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            float num822 = (float)(Main.maxTilesX * Main.maxTilesY) * 0.002f;
            for (int num823 = 0; (float)num823 < num822; num823++)
            {
                progress.Set((float)num823 / num822);
                int num824 = genRand.Next(1, Main.maxTilesX - 1);
                int num825 = genRand.Next((int)worldSurfaceLow, (int)worldSurfaceHigh);
                if (num825 >= Main.maxTilesY)
                {
                    num825 = Main.maxTilesY - 2;
                }

                Tile tile31 = Main.tile[num824 - 1, num825];
                if (tile31.active())
                {
                    tile31 = Main.tile[num824 - 1, num825];
                    if (tile31.type == 0)
                    {
                        tile31 = Main.tile[num824 + 1, num825];
                        if (tile31.active())
                        {
                            tile31 = Main.tile[num824 + 1, num825];
                            if (tile31.type == 0)
                            {
                                tile31 = Main.tile[num824, num825 - 1];
                                if (tile31.active())
                                {
                                    tile31 = Main.tile[num824, num825 - 1];
                                    if (tile31.type == 0)
                                    {
                                        tile31 = Main.tile[num824, num825 + 1];
                                        if (tile31.active())
                                        {
                                            tile31 = Main.tile[num824, num825 + 1];
                                            if (tile31.type == 0)
                                            {
                                                tile31 = Main.tile[num824, num825];
                                                tile31.active(active: true);
                                                tile31 = Main.tile[num824, num825];
                                                tile31.type = 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                num824 = genRand.Next(1, Main.maxTilesX - 1);
                num825 = genRand.Next(0, (int)worldSurfaceLow);
                if (num825 >= Main.maxTilesY)
                {
                    num825 = Main.maxTilesY - 2;
                }

                tile31 = Main.tile[num824 - 1, num825];
                if (tile31.active())
                {
                    tile31 = Main.tile[num824 - 1, num825];
                    if (tile31.type == 0)
                    {
                        tile31 = Main.tile[num824 + 1, num825];
                        if (tile31.active())
                        {
                            tile31 = Main.tile[num824 + 1, num825];
                            if (tile31.type == 0)
                            {
                                tile31 = Main.tile[num824, num825 - 1];
                                if (tile31.active())
                                {
                                    tile31 = Main.tile[num824, num825 - 1];
                                    if (tile31.type == 0)
                                    {
                                        tile31 = Main.tile[num824, num825 + 1];
                                        if (tile31.active())
                                        {
                                            tile31 = Main.tile[num824, num825 + 1];
                                            if (tile31.type == 0)
                                            {
                                                tile31 = Main.tile[num824, num825];
                                                tile31.active(active: true);
                                                tile31 = Main.tile[num824, num825];
                                                tile31.type = 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });
    }

    /*public static GenPass Jungle()
    {
        return new JunglePass().OnBegin(delegate (GenPass pass)
        {
            JunglePass obj5 = pass as JunglePass;
            obj5.JungleOriginX = jungleOriginX;
            obj5.DungeonSide = dungeonSide;
            obj5.WorldSurface = worldSurface;
            obj5.LeftBeachEnd = leftBeachEnd;
            obj5.RightBeachStart = rightBeachStart;
        }).OnComplete(delegate (GenPass pass)
        {
            JungleX = (pass as JunglePass).JungleX;
        });
    }*/

    public static GenPass MudCavesToGrass()
    {
        return new PassLegacy("Mud Caves To Grass", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Mud Caves To Grass";//Lang.gen[77].Value;
            NotTheBees();
            for (int num817 = 0; num817 < Main.maxTilesX; num817++)
            {
                for (int num818 = 0; num818 < Main.maxTilesY; num818++)
                {
                    if (Main.tile[num817, num818].active())
                    {
                        grassSpread = 0;
                        SpreadGrass(num817, num818, 59, 60, repeat: true, 0);
                    }

                    progress.Set(0.2f * ((float)(num817 * Main.maxTilesY + num818) / (float)(Main.maxTilesX * Main.maxTilesY)));
                }
            }

            SmallConsecutivesFound = 0;
            SmallConsecutivesEliminated = 0;
            float num819 = Main.maxTilesX - 20;
            for (int num820 = 10; num820 < Main.maxTilesX - 10; num820++)
            {
                ScanTileColumnAndRemoveClumps(num820);
                float num821 = (float)(num820 - 10) / num819;
                progress.Set(0.2f + num821 * 0.8f);
            }
        });
    }

    public static GenPass FullDesert(WorldGenConfiguration configuration)
    {
        return new PassLegacy("Full Desert", delegate (GenerationProgress progress, GameConfiguration passConfig)
        {
            progress.Message = "Full Desert";//Lang.gen[78].Value;
            Main.tileSolid[484] = false;
            int num812 = 0;
            int num813 = dungeonSide;
            int num814 = Main.maxTilesX / 2;
            int num815 = genRand.Next(num814) / 8;
            num815 += num814 / 8;
            int x29 = num814 + num815 * -num813;
            if (drunkWorldGen)
            {
                num813 *= -1;
            }

            int num816 = 0;
            DesertBiome desertBiome = configuration.CreateBiome<DesertBiome>();
            while (!desertBiome.Place(new Point(x29, (int)worldSurfaceHigh + 25), structures))
            {
                num815 = genRand.Next(num814) / 2;
                num815 += num814 / 8;
                num815 += genRand.Next(num816 / 12);
                x29 = num814 + num815 * -num813;
                if (++num816 > Main.maxTilesX / 4)
                {
                    num813 *= -1;
                    num816 = 0;
                    num812++;
                    if (num812 >= 2)
                    {
                        skipDesertTileCheck = true;
                    }
                }
            }
        });
    }
    #endregion
}
