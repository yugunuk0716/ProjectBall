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

    public override void OnTriggerBall(Ball tb)
    {
        if (tb.afCool + tb.afLastTime < Time.time)
        {
            tb.afLastTime = Time.time;
            tb.rigid.velocity *= flowAmount;
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
