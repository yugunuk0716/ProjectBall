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
    LEFTUP,
    LEFTDOWN,
    RIGHTUP,
    RIGHTDOWN
}

[System.Serializable]
public class ObjectTileInfo
{
    public int tileType;
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
        StageManager.Instance.objectTileList.Add(this);
  
    }

}
