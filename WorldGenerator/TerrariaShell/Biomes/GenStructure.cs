﻿using Microsoft.Xna.Framework;

namespace WorldGenerator;

public abstract class GenStructure : GenBase
{
    public abstract bool Place(Point origin, StructureMap structures);
}