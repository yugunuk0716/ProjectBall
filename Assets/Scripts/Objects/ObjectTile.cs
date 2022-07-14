using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Slow,
    Teleporter,
    Goal,
    DirectionChaner,
    Mirror,
    Wall
}

public enum TileDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

[System.Serializable]
public class ObjectTileInfo
{
    public int tileType;
}

public class ObjectTile : PoolableMono
{
    public TileType myType;
    public TileDirection myDirection;

    public virtual string ParseTileInfo()
    {
        return string.Empty;
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
