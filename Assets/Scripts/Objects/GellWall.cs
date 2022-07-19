using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class GellWall : ObjectTile
{

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
            Vector2 vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.up).normalized;
           
            //if(Mathf.Abs(tb.rigid.velocity.x) > 0)
            //{
            //    vec.x = 0;
            //}
            //else
            //{
            //    vec.y = 0;
            //}

            //if (vec.x == 0 && vec.y == 0) vec.y = 1; // up�� �������� �����ؼ� ���� ���ؾ� �� ��, ��������� ��?

            tb.Move(vec, 5);
        }
    }


}
