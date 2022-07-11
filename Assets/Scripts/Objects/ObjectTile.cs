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

[System.Serializable]
public class ObjectTileInfo
{
    public int tileType;
}


public class ObjectTile : MonoBehaviour
{
    public TileType myType;


    public virtual string ParseTileInfo()
    {
        return string.Empty;
    }


}
