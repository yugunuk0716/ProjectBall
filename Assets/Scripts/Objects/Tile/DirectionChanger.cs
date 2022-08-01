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

    [SerializeField]
    private Sprite[] dirChangerSprites;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }


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

    public override void SetDirection()
    {
        base.SetDirection();

        switch (dataString)
        {
            case "กๆ":
                sr.sprite = dirChangerSprites[0];
                wallDirection = TileDirection.RIGHTUP;
                break;
            case "ก็":
                sr.sprite = dirChangerSprites[1];
                wallDirection = TileDirection.LEFTDOWN;
                break;
            case "ก่":
                sr.sprite = dirChangerSprites[2];
                wallDirection = TileDirection.LEFTUP;
                break;
            case "ก้":
                sr.sprite = dirChangerSprites[3];
                wallDirection = TileDirection.RIGHTDOWN;
                break;
        }
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
