using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionChangerInfo : ObjectTileInfo
{
    public TileDirection wallDirection;
}


public class DirectionChanger : ObjectTile
{
    public TileDirection wallDirection;

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":"  + myType + ", \\\"wallDirection\\\":" + wallDirection + "}";
    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length - 2);
        DirectionChangerInfo directionChangerInfo = JsonUtility.FromJson<DirectionChangerInfo>(info);
        myType = directionChangerInfo.tileType;
        wallDirection = directionChangerInfo.wallDirection;
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
