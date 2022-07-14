using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TeleporterInfo : ObjectTileInfo
{
    public int portalIndex;
}


public class Teleporter : ObjectTile
{
    public int portalIndex;
    public Teleporter pairTeleporter;

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + myType + ", \\\"portalIndex\\\":"  + portalIndex + "}";
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        TeleporterInfo teleporterInfo = JsonUtility.FromJson<TeleporterInfo>(info);

        myType = teleporterInfo.tileType;
        portalIndex = teleporterInfo.portalIndex;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            if(tb.tpCool + tb.tpLastTime < Time.time)
            {
              
                tb.tpLastTime = Time.time;
                tb.transform.position = pairTeleporter.transform.position;
            }
        }
    }
}
