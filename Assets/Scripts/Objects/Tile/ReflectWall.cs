using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ReflectWall : ObjectTile
{
    public bool isHorizontalWall = true;

    public override void OnTriggerBall(Ball tb)
    {
        Debug.Log("½ÇÇà");
        Vector2 vec;
        if (isHorizontalWall)
        {
            vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.up).normalized;
        }
        else
        {
            vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.right).normalized;
        }

        tb.Move(vec, 3.5f);
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
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (dataString.Equals("/"))
        {
            isHorizontalWall = false;
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }

    public override void Reset()
    {

    }
}
