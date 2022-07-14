using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
    Slow,
    Teleporter,
    Goal,
    DirectionChaner,
    Mirror,
    Wall
}

[System.Serializable]
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
    public TileType tileType;
}

public class ObjectTile : MonoBehaviour
{
    public TileType myType;
    //public TileDirection myDirection;

    public virtual string ParseTileInfo()
    {
        return string.Empty;
    }

    public virtual void SettingTile(string info)
    {
        StageManager.instance.objectTileList.Add(this);
    }

}
