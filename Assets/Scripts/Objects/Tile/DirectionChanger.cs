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

    int index = 0;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();
    }


    public override void InteractionTile(Ball tb)
    {
        Vector3 vec = Vector3.zero;

        SoundManager sm = IsometricManager.Instance.GetManager<SoundManager>();
        sm.Play("Arrow");

        tb.transform.position = transform.position - new Vector3(0, .25f, 0);
        switch (wallDirection)
        {
            case TileDirection.LEFTUP:
                tb.SetBall(Vector2.up, tb.speed);
                //vec.Set(-0.9f, 0.45f, 0);
                break;
            case TileDirection.RIGHTDOWN:
                tb.SetBall(Vector2.down, tb.speed);
                //vec.Set(0.9f, -0.45f, 0);
                break;
            case TileDirection.RIGHTUP:
                tb.SetBall(Vector2.right, tb.speed);
               // vec.Set(0.4f, 0.25f, 0);
                break;
            case TileDirection.LEFTDOWN:
                tb.SetBall(Vector2.left, tb.speed);
               // vec.Set(-0.4f, -0.25f, 0);
                break;
        }
        tb.SetMove();
        //tb.Move(vec, 3.5f);
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":"  + (int)myType + ", \\\"wallDirection\\\":" + (int)wallDirection + "}";
    }

    public override void SetDirection()
    {
        base.SetDirection();
        SetDatas();
    }

    private void SetDatas()
    {
        switch (dataString)
        {
            case "→":
                sr.sprite = dirChangerSprites[0];
                wallDirection = TileDirection.RIGHTUP;
                index = 0;
                break;
            case "↓":
                sr.sprite = dirChangerSprites[1];
                wallDirection = TileDirection.RIGHTDOWN;
                index = 1;
                break;
            case "←":
                sr.sprite = dirChangerSprites[2];
                wallDirection = TileDirection.LEFTDOWN;
                index = 2;
                break;
            case "↑":
                sr.sprite = dirChangerSprites[3];
                wallDirection = TileDirection.LEFTUP;
                index = 3;
                break;
        }
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

    int[] indexArr = { 4, 8, 2, 1 };
    public override IEnumerator Transition()
    {
        while (true)
        {
            index++;
            if (index == indexArr.Length) index = 0;
            sr.sprite = dirChangerSprites[index];

            wallDirection = (TileDirection)indexArr[index];
            yield return new WaitForSeconds(3f);
        }
    }

}
