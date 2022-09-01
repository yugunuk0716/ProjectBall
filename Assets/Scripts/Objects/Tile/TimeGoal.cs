using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TimeGoal : Goal
{


    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + "}";
    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length - 2);
        ObjectTileInfo goalInfo = JsonUtility.FromJson<ObjectTileInfo>(info);
        myType = (TileType)goalInfo.tileType;
    }

 
    public override void InteractionTile(Ball tb)
    {
        base.InteractionTile(tb);
    }

    public override void Reset()
    {
        base.Reset();
    }

    public override IEnumerator Transition()
    {
        throw new System.NotImplementedException();
    }
}
