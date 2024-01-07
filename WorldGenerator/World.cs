using static WorldGenerator.WorldSettings;

namespace WorldGenerator;

public sealed class World
{
    public World(ushort width, ushort height, int seed)
    {
        maxTilesX = width;
        maxTilesY = height;

        tile = new(maxTilesX, maxTilesY);

        random = new(seed);

        generator = new(random.Next(), configuration);

        AppendPasses();

        generator.GenerateWorld();
    }

    private readonly UnifiedRandom random;

    private readonly Tilemap tile;

    private readonly StructureMap structures = new();

    private readonly WorldGenConfiguration configuration = new(/*null*/new());

    private readonly WorldGenerator generator;

    private readonly ushort maxTilesX, maxTilesY;

    private int leftBeachEnd = 0, rightBeachStart = 0, waterLine = 0, lavaLine, jungleOriginX = 0, snowOriginLeft = 0, snowOriginRight = 0, numPyr = 0;

    private int[] PyrX = null, PyrY = null;

    private double
        worldSurface = 0.0, worldSurfaceHigh = 0.0, worldSurfaceLow = 0.0,
        rockLayer = 0.0, rockLayerHigh = 0.0, rockLayerLow = 0.0;

    private void AppendPasses()
    {
        //generator.Append(Reset());
        generator.Append(Terrain());
        //generator.Append(Dunes(configuration));
    }
}