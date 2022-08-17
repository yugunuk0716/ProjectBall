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
    [SerializeField]
    private Sprite[] mySprite;
    private SpriteRenderer sr;

    public int portalIndex;
    public Teleporter pairTeleporter;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + ", \\\"portalIndex\\\":"  + portalIndex + "}";
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        print(info);
        base.SettingTile(info);
        TeleporterInfo teleporterInfo = JsonUtility.FromJson<TeleporterInfo>(info);
        myType = (TileType)teleporterInfo.tileType;
        portalIndex = teleporterInfo.portalIndex;
    }

    public void FindPair()
    {
        foreach (Teleporter tp in IsometricManager.Instance.GetManager<GameManager>().portalList)
        {

            if (tp != null)
            {
                if (tp.portalIndex == portalIndex && tp != this)
                {
                    if(mySprite != null)
                    {
                        sr.sprite = mySprite[portalIndex];
                        tp.sr.sprite = mySprite[portalIndex];
                    }
                    else
                    {
                        Debug.LogError("스프라이트 배열이 비었어");
                    }
                    pairTeleporter = tp;
                }
            }
        }
    }

    public override void SetDirection()
    {
        base.SetDirection();

        if (dataString.Contains("TP"))
        {
            IsometricManager.Instance.GetManager<GameManager>().portalList.Add(this);
        }
    }

    public override void Reset()
    {

    }

    public override void InteractionTile(Ball tb)
    {
        if (pairTeleporter != null && tb.tpCool + tb.tpLastTime < Time.time)
        {
            tb.tpLastTime = Time.time;
            tb.transform.position = pairTeleporter.transform.position - new Vector3(0, .25f, 0);
            tb.SetPos(pairTeleporter.keyPos);
            tb.SetMove();
        }
    }
}
