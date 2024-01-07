using System;

namespace WorldGenerator;

[Obsolete]
internal static class TileUtilities
{
    /*public static bool TileEmpty(int i, int j)
    {
        if (Main.tile[i, j] != null && Main.tile[i, j].active())
        {
            return Main.tile[i, j].inActive();
        }

        return true;
    }*/

    /*public static bool CanPoundTile(int x, int y)
    {
        if (/*Main*Program.tile[x, y] == null)
        {
            /*Main*Program.tile[x, y] = default(Tile);
        }

        if (/*Main*Program.tile[x, y - 1] == null)
        {
            /*Main*Program.tile[x, y - 1] = default(Tile);
        }

        if (/*Main*Program.tile[x, y + 1] == null)
        {
            /*Main*Program.tile[x, y + 1] = default(Tile);
        }

        switch (/*Main*Program.tile[x, y].type)
        {
            case 10:
            case 48:
            case 137:
            case 138:
            case 232:
            case 380:
            case 387:
            case 388:
            case 476:
            case 484:
                return false;
            default:
                if (true/*gen*)
                {
                    if (/*Main*Program.tile[x, y].type == 190)
                    {
                        return false;
                    }

                    if (/*Main*Program.tile[x, y].type == 30)
                    {
                        return false;
                    }
                }

                if (/*Main*Program.tile[x, y - 1].active())
                {
                    if (TileID.Sets.BasicChest[/*Main*Program.tile[x, y - 1].type])
                    {
                        return false;
                    }

                    switch (/*Main*Program.tile[x, y - 1].type)
                    {
                        case 21:
                        case 26:
                        case 77:
                        case 88:
                        case 235:
                        case 237:
                        case 441:
                        case 467:
                        case 468:
                        case 470:
                        case 475:
                        case 488:
                        case 597:
                            return false;
                    }
                }

                return CanKillTile(x, y);
        }
    }*/

    /*public static bool CanKillTile(int i, int j)
    {
        bool blockDamaged;
        return CanKillTile(i, j, out blockDamaged);
    }*/

    /*public static bool SolidOrSlopedTile(int x, int y)
    {
        return SolidOrSlopedTile(Main.tile[x, y]);
    }*/

    /*public static bool SolidOrSlopedTile(Tile tile)
    {
        if (tile != null && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
        {
            return !tile.inActive();
        }

        return false;
    }*/

    /*public static bool CanKillTile(int i, int j, out bool blockDamaged)
    {
        blockDamaged = false;
        if (i < 0 || j < 0 || i >= /*Main*WorldSettings.maxTilesX || j >= /*Main*WorldSettings.maxTilesY)
        {
            return false;
        }

        Tile tile = /*Main*Program.tile[i, j];
        Tile tile2 = default(Tile);
        if (tile == null)
        {
            return false;
        }

        if (!tile.active())
        {
            return false;
        }

        /*if (!TileLoader.CanKillTile(i, j, tile.type, ref blockDamaged))
        {
            return false;
        }*

        if (j >= 1)
        {
            tile2 = /*Main*Program.tile[i, j - 1];
        }

        int type;
        if (tile2 != null && tile2.active())
        {
            type = tile2.type;
            if (TileID.Sets.IsATreeTrunk[type] && tile.type != type && (tile2.frameX != 66 || tile2.frameY < 0 || tile2.frameY > 44) && (tile2.frameX != 88 || tile2.frameY < 66 || tile2.frameY > 110) && tile2.frameY < 198)
            {
                return false;
            }

            int num = type;
            if (num == 323)
            {
                if (tile.type != type && (tile2.frameX == 66 || tile2.frameX == 220))
                {
                    return false;
                }
            }
            else
            {
                if (TileID.Sets.BasicDresser[type] || num == 21 || TileID.Sets.BasicChest[type])
                {
                    goto IL_018a;
                }

                if (num <= 77)
                {
                    if (num == 26 || num == 72 || num == 77)
                    {
                        goto IL_018a;
                    }
                }
                else if (num <= 88)
                {
                    if (num != 80)
                    {
                        if (num == 88)
                        {
                            goto IL_018a;
                        }
                    }
                    else if (tile.type != type)
                    {
                        int num2 = tile2.frameX / 18;
                        if ((uint)num2 <= 1u || (uint)(num2 - 4) <= 1u)
                        {
                            return false;
                        }
                    }
                }
                else if (num == 467 || num == 488)
                {
                    goto IL_018a;
                }
            }
        }

        goto IL_01bd;
        IL_0273:
        if (TileID.Sets.BasicChest[tile.type])
        {
            goto IL_0283;
        }

        ushort type2;
        /*if ((TileID.Sets.BasicDresser[tile.type] || type2 == 88) && !/*Chest.*CanDestroyChest(i - tile.frameX / 18 % 3, j - tile.frameY / 18))
        {
            return false;
        }*

        goto IL_02e0;
        IL_0283:
        /*if (!/*Chest.*CanDestroyChest(i - tile.frameX / 18 % 2, j - tile.frameY / 18))
        {
            return false;
        }*

        goto IL_02e0;
        IL_018a:
        if (tile.type != type)
        {
            return false;
        }

        goto IL_01bd;
        IL_02e0:
        return true;
        IL_01bd:
        type2 = tile.type;
        if ((uint)type2 <= 21u)
        {
            if (type2 != 10)
            {
                if (type2 != 21)
                {
                    goto IL_0273;
                }

                goto IL_0283;
            }

            if (IsLockedDoor(tile))
            {
                blockDamaged = true;
                return false;
            }
        }
        else if (type2 != 138)
        {
            if (type2 != 235)
            {
                goto IL_0273;
            }

            int num3 = i - tile.frameX % 54 / 18;
            for (int k = 0; k < 3; k++)
            {
                Tile t = /*Main*Program.tile[num3 + k, j - 1];
                if (t.active() && IsAContainer(t))
                {
                    blockDamaged = true;
                    return false;
                }
            }
        }
        else if (CheckBoulderChest(i, j))
        {
            blockDamaged = true;
            return false;
        }

        goto IL_02e0;
    }*/

    /*public static bool IsLockedDoor(Tile t)
    {
        if (t.type == 10 && t.frameY >= 594 && t.frameY <= 646)
        {
            return t.frameX < 54;
        }

        return false;
    }*/

    /*public static bool CheckBoulderChest(int i, int j)
    {
        int num = /*Main*Program.tile[i, j].frameX / 18 * -1;
        if (num < -1)
        {
            num += 2;
        }

        num += i;
        int num2;
        for (num2 = /*Main*Program.tile[i, j].frameY; num2 >= 36; num2 -= 36)
        {
        }

        num2 = j - num2 / 18;
        if (IsAContainer(/*Main*Program.tile[num, num2 - 1]) || IsAContainer(/*Main*Program.tile[num + 1, num2 - 1]))
        {
            return true;
        }

        return false;
    }*/

    /*public static bool IsAContainer(Tile t)
    {
        if (TileID.Sets.BasicDresser[t.type])
        {
            return true;
        }

        if (t.type != 88 && t.type != 470 && t.type != 475 && !TileID.Sets.BasicChest[t.type])
        {
            return TileID.Sets.BasicChestFake[t.type];
        }

        return true;
    }*/

    /*public static bool CanDestroyChest(int X, int Y)
    {
        for (int i = 0; i < 8000; i++)
        {
            Chest chest = Main.chest[i];
            if (chest == null || chest.x != X || chest.y != Y)
            {
                continue;
            }

            for (int j = 0; j < 40; j++)
            {
                if (chest.item[j] != null && chest.item[j].type > 0 && chest.item[j].stack > 0)
                {
                    return false;
                }
            }

            return true;
        }

        return true;
    }*/
}