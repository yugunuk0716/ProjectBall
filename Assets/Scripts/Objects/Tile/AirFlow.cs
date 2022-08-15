using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AirFlowInfo : ObjectTileInfo
{
    public float flowAmount;
}

public class AirFlow : ObjectTile
{
    public float flowAmount;

    public override void InteractionTile(Ball tb)
    {
        if (tb.afCool + tb.afLastTime < Time.time)
        {
            print("에?");
            tb.afLastTime = Time.time;
            tb.SetBall(tb.direction, tb.speed + flowAmount);
            tb.SetMove();
        }
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + ", \\\"flowAmount\\\":" + flowAmount + "}";
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        print(info);
        base.SettingTile(info);
        AirFlowInfo airFlowInfo = JsonUtility.FromJson<AirFlowInfo>(info);
        myType = (TileType)airFlowInfo.tileType;
        flowAmount = airFlowInfo.flowAmount;
    }


    public override void Reset()
    {

    }
}
