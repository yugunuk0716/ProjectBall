using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MirrorInfo : ObjectTileInfo
{
    public TileDirection wallDirection;
}



public class GellWall : ObjectTile
{

    public TileDirection wallDirection;


    public override string ParseTileInfo()
    {
        return $"{{\\\"tileType\\\":" + myType + ", \\\"wallDirection\\\":" + wallDirection + "}";
        
    }

    public override void SettingTile(string info)
    {
        base.SettingTile(info);
        info = info.Substring(1, info.Length - 2);
        print(info);
        MirrorInfo mirrorInfo = JsonUtility.FromJson<MirrorInfo>(info);
        myType = mirrorInfo.tileType;
        wallDirection = mirrorInfo.wallDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {
            Vector2 vec = Vector2.Reflect(tb.rigid.velocity.normalized, transform.up);
            if (vec.x != 1) vec.x = 0;
            if (vec.y != 1) vec.y = 0;
            
            Debug.Log($"{vec.x}, {vec.y}");
            tb.Move(vec, 5);
        }

    }


}
