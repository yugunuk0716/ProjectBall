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

public class ObjectTile : MonoBehaviour
{
    
    public TileType myType;


}
