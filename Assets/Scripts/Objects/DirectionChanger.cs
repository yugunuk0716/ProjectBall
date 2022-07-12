using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionChangerInfo : ObjectTileInfo
{
    public int wallDirection;
}


public class DirectionChanger : ObjectTile
{
    public TileDirection wallDirection;

    public DirectionChangerInfo directionChangerInfo = new DirectionChangerInfo();

    public override string ParseTileInfo()
    {
        directionChangerInfo.tileType = (int)myType;
        directionChangerInfo.wallDirection = (int)wallDirection;

        string s = $"{{\"tileType\":" + "\"" + directionChangerInfo.tileType + "\"" + ", \"wallDirection\":" + "\"" + directionChangerInfo.wallDirection + "\"}";

        DirectionChangerInfo dc = JsonUtility.FromJson<DirectionChangerInfo>(s);
        print(s);

        print((TileType)dc.tileType);
        print((GellWallDirection)dc.wallDirection);
        return s;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {

            Vector3 vec = Vector3.zero;

            tb.transform.position = transform.position;
            switch (wallDirection)
            {
                case TileDirection.UP:
                    vec = Vector2.up;
                    break;
                case TileDirection.RIGHT:
                    vec = Vector2.right;
                    break;
                case TileDirection.LEFT:
                    vec = Vector2.left;
                    break;
                case TileDirection.DOWN:
                    vec = Vector2.down;
                    break;
            }

            tb.Move(vec, 5);
        }

    }
}
