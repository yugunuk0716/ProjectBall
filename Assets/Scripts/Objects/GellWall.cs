using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MirrorInfo : ObjectTileInfo
{
    public int wallDirection;
}



public class GellWall : ObjectTile
{

    public TileDirection wallDirection;


    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + (int)myType + ", \\\"wallDirection\\\":" + (int)wallDirection + "}";
        
    }

    public override void SettingTile(string info)
    {
        info = info.Substring(1, info.Length - 2);
        print(info);
        base.SettingTile(info);
        MirrorInfo mirrorInfo = JsonUtility.FromJson<MirrorInfo>("{\"tileType\":Mirror, \"wallDirection\":LEFT}");
        myType = (TileType)mirrorInfo.tileType;
        wallDirection = (TileDirection)mirrorInfo.wallDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            Vector2 vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.up).normalized;

            if(Mathf.Abs(tb.rigid.velocity.x) > 0)
            {
                vec.x = 0;
            }
            else
            {
                vec.y = 0;
            }

            //if (vec.x == 0 && vec.y == 0) vec.y = 1; // up을 기준으로 외적해서 위로 향해야 할 때, 멈춰버리는 듯?

            tb.Move(vec, 5);
        }
    }


}
