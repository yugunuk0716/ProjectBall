using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ReflectWall : ObjectTile
{
    public bool isHorizontalWall = true;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            Vector2 vec;
            if (isHorizontalWall)
            {
                 vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.up).normalized;
            }
            else
            {
                vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.right).normalized;
            }

            tb.Move(vec, 5);
        }
    }


}