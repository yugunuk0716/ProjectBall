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


public class ObjectTile : MonoBehaviour
{
    public TileType myType;
    public TileDirection myDirection;

}
