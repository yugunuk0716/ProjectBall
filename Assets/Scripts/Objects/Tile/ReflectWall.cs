using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectWall : ObjectTile
{
    public bool isHorizontalWall = true;

    public Sprite[] sprites;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void InteractionTile(Ball tb)
    {
        if (isHorizontalWall)
        {
            tb.direction = tb.direction.y > 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
        }
        else
        {
            tb.direction = tb.direction.x > 0 ? new Vector2(0, 1) : new Vector2(0, -1);
        }
        tb.SetMove();
    }

    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + "}";
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        base.SettingTile(info);
        ObjectTileInfo mirrorInfo = JsonUtility.FromJson<ObjectTileInfo>(info);
        myType = (TileType)mirrorInfo.tileType;
    }

    public override void SetDirection()
    {
        base.SetDirection();

        if (dataString.Equals("\\"))
        {
            isHorizontalWall = true;
            sr.sprite = sprites[0];
        }
        else if (dataString.Equals("/"))
        {
            isHorizontalWall = false;
            sr.sprite = sprites[1];
        }
    }

    public override void Reset()
    {

    }
}
