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

    public TeleporterInfo teleporterInfo;

    public override string ParseTileInfo()
    {
        teleporterInfo.tileType = myType;
        teleporterInfo.portalIndex = portalIndex;

        return $"{{\\\"tileType\\\":" + teleporterInfo.tileType + ", \\\"portalIndex\\\":"  + teleporterInfo.portalIndex + "}";
    }

    public override void SettingTile(string info)
    {
        teleporterInfo = JsonUtility.FromJson<TeleporterInfo>(info);
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
