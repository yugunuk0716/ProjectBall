using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DirectionChangerInfo : ObjectTileInfo
{
    public GellWallDirection wallDirection;
}


public class DirectionChanger : ObjectTile
{
    public GellWallDirection wallDirection;

    public DirectionChangerInfo directionChangerInfo = new DirectionChangerInfo();

    public override string ParseTileInfo()
    {
        directionChangerInfo.tileType = myType;
        directionChangerInfo.wallDirection = wallDirection;

        return $"{{\\\"tileType\\\":"  + directionChangerInfo.tileType + ", \\\"wallDirection\\\":" + directionChangerInfo.wallDirection + "}";
    }

    public override void SettingTile(string info)
    {
        directionChangerInfo = JsonUtility.FromJson<DirectionChangerInfo>(info);
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
                case GellWallDirection.UP:
                    vec = Vector2.up;
                    break;
                case GellWallDirection.RIGHT:
                    vec = Vector2.right;
                    break;
                case GellWallDirection.LEFT:
                    vec = Vector2.left;
                    break;
                case GellWallDirection.DOWN:
                    vec = Vector2.down;
                    break;
            }

            tb.Move(vec, 5);
        }

    }
}
