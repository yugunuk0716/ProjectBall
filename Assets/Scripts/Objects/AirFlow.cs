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

    public AirFlowInfo airFlowInfo = new AirFlowInfo();

    public override string ParseTileInfo()
    {
        airFlowInfo.tileType = (int)myType;
        airFlowInfo.flowAmount = flowAmount;

        string s = $"{{\"tileType\":" + "\"" + airFlowInfo.tileType + "\"" + ", \"flowAmount\":" + "\"" + airFlowInfo.flowAmount + "\"}";

        return s;
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
