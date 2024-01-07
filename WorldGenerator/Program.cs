using static WorldGenerator.WorldSettings;
using static WorldGenerator.TilePalette.TileColor;

using System;

using ProjectFox.CoreEngine.Math;
using M = ProjectFox.CoreEngine.Math.Math;
using ProjectFox.CoreEngine.Collections;
using ProjectFox.GameEngine;
using ProjectFox.GameEngine.Visuals;
using ProjectFox.Windows;
using ProjectFox.GameEngine.Input;

namespace WorldGenerator;

public static class Program
{
    public const int seed = 0;
    
    public static GameWindow window;

    private static void Main()
    {
        Engine.Frequency = 15;
        Screen.position = new(0, 0);
        Screen.Size = new(1100, 500);
        Screen.Scale = 1f;
        Screen.OneToOne = true;
        Screen.FullScreen = false;
        Debug.DrawDebug = true;

        Scene scene = new(new("TestScn", 0))
        {
            ClearMode = Screen.ClearModes.Fill,
            bgColor = new(128, 128, 128),
        };

        new MapSetPiece(new("WrldMap", 0))
        {
            Scene = scene,
            layer = new(new("TstLayr", 0)) { Scene = scene },
            anim = GenerateMap(),
            drawTextureBounds = true,
            boundsColor = 0x00FFFFFF
        };

        Engine.SceneList.Add(scene);
        Engine.SceneList.ActiveScene = scene.Name;

        window = new("Test Window", 100, 100, true);
        window.Start();
    }
    
    private static TextureAnimation GenerateMap()
    {
        WorldGenConfiguration configuration = new(new());
        WorldGenerator g = new(seed, configuration);
        
        //g.Append(Reset());
        g.Append(Terrain());
        g.Append(Dunes(configuration));
        g.Append(OceanSand());
        g.Append(SandPatches());
        //g.Append(Tunnels());
        g.Append(MountCaves());
        g.Append(DirtWallBackgrounds());
        g.Append(RocksInDirt());
        g.Append(DirtInRocks());
        g.Append(Clay());
        g.Append(SmallHoles());
        g.Append(DirtLayerCaves());
        g.Append(RockLayerCaves());
        //g.Append(SurfaceCaves());
        g.Append(WavyCaves());
        //g.Append(GenerateIceBiome());
        g.Append(WorldSettings.Grass());
        //g.Append(Jungle());
        g.Append(MudCavesToGrass());
        //g.Append(FullDesert(configuration));

        g.GenerateWorld();

        return g.steps;
    }
    
    [Obsolete] private static bool CheckValues(ushort id, params ushort[] ids)
    {
        foreach (ushort u in ids) if (u == id) return true;
        return false;
    }
    [Obsolete] private static bool Grass(ushort id) => CheckValues(id, 2, 23, 60, 199, 109, 477, 492);
}

public sealed class TilePalette : IndexPalette//might need to make a ushort index palette base, how would this work with byte textures?
{
    public enum TileColor : uint
    {
        None = 0,
        Dirt = 0x201000FF,
        Stone = 0x404040FF,
        Grass = 0x00FF00FF,
        BladeGrass1 = 0xFF00FFFF,
        BladeGrass2 = 0x000000FF,

        ClayBlock = 0x804000FF,

        Sand = 0xFFFF80FF,

        Unspecified = 0xFFFFFFFF,
    }

    public static ColorTexture ColorizeMap(Tilemap tilemap/*, out PalettizedTexture texture, out TilePalette palette*/)
    {
        AutoSizedArray<TileIDs> tileIDs = new();

        Color[] map = new Color[maxTilesX * maxTilesY];
        for (int y = 0, x; y < maxTilesY; y++) for (x = 0; x < maxTilesX; x++)
            {
                TileIDs id = (TileIDs)tilemap[x, y].TileType;
                TileColor color = GetTileColor(id);
                map[y * maxTilesX + x] = (uint)color;

                if (color == Unspecified && !tileIDs.Contains(id))
                {
                    tileIDs.Add(id);
                    Debug.Console.QueueMessage($"Unknown tile id found {id}");
                }
            }
        return new(maxTilesX, maxTilesY, map);
    }

    public static Color GetColor(ushort id) => (uint)GetTileColor((TileIDs)id);

    public static TileColor GetTileColor(TileIDs id) => id switch
    {
        //-1 => None,
        TileIDs.Dirt => Dirt,
        TileIDs.Stone => Stone,
        TileIDs.Grass => TileColor.Grass,
        TileIDs.Plants => BladeGrass1,
        TileIDs.Torches => BladeGrass2,//whats with this one?

        TileIDs.ClayBlock => 0,

        TileIDs.Sand => Sand,

        _ => Unspecified,
    };

    public override Color this[byte index] => GetColor(index);

    public override IPalette Copy() => throw new System.NotImplementedException();
}

public sealed class MapSetPiece : SetPiece
{
    private const int dist = 20;

    public MapSetPiece(NameID name) : base(name) { }

    private readonly DigitalButton tab = new();

    public TextureAnimation anim = null;

    protected override void PreFrame()
    {
        KeyboardMouseState kbm = Program.window.KeyboardMouseState;

        tab.Value = kbm.Tab;
        if (tab)
        {

        }
        if (tab.TrueThisFrame)
        {
            anim.FrameIndex += kbm.Shift ? -1 : 1;
            Debug.Console.QueueMessage(anim.FrameIndex);
        }
        texture = anim.frames[anim.FrameIndex].texture;

        Vector pos = Position;
        switch (M.FindSign(kbm.Left, kbm.Right))
        {
            case M.Sign.Neg:
                pos.x -= dist;
                break;
            case M.Sign.Pos:
                pos.x += dist;
                break;
        }
        switch (M.FindSign(kbm.Up, kbm.Down))
        {
            case M.Sign.Neg:
                pos.y -= dist;
                break;
            case M.Sign.Pos:
                pos.y += dist;
                break;
        }
        Position = pos;
    }
}