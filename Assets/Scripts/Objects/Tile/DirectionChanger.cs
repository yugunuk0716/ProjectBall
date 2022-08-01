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

    public override void OnTriggerBall(Ball tb)
    {
        Vector3 vec = Vector3.zero;

        tb.transform.position = transform.position;
        switch (wallDirection)
        {
            case TileDirection.LEFTUP:
                vec.Set(-0.9f, 0.45f, 0);
                break;
            case TileDirection.RIGHTDOWN:
                vec.Set(0.9f, -0.45f, 0);
                break;
            case TileDirection.RIGHTUP:
                vec.Set(0.4f, 0.25f, 0);
                break;
            case TileDirection.LEFTDOWN:
                vec.Set(-0.4f, -0.25f, 0);
                break;
        }

        tb.Move(vec, 3.5f);
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":"  + (int)myType + ", \\\"wallDirection\\\":" + (int)wallDirection + "}";
    }

    public override void Reset()
    {
        
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        print(info);
        base.SettingTile(info);
        DirectionChangerInfo directionChangerInfo = JsonUtility.FromJson<DirectionChangerInfo>(info);
        myType = (TileType)directionChangerInfo.tileType;
        wallDirection = (TileDirection)directionChangerInfo.wallDirection;
    }


}
