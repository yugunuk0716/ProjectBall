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

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + myType + ", \\\"flowAmount\\\":"  + flowAmount + "}";
    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length -2);
        AirFlowInfo airFlowInfo = JsonUtility.FromJson<AirFlowInfo>(info);
        myType = airFlowInfo.tileType;
        flowAmount = airFlowInfo.flowAmount;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            if (tb.afCool + tb.afLastTime < Time.time)
            {

                tb.afLastTime = Time.time;
                tb.rigid.velocity *= flowAmount;
            }
        }
    }
}
