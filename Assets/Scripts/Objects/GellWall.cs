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

    public MirrorInfo mirrorInfo = new MirrorInfo();

    public override string ParseTileInfo()
    {
        mirrorInfo.tileType = myType;
        mirrorInfo.wallDirection = wallDirection;

        return $"{{\\\"tileType\\\":" + mirrorInfo.tileType  + ", \\\"wallDirection\\\":" +  mirrorInfo.wallDirection + "}";
        
    }

    public override void SettingTile(string info)
    {
        print(info);
        mirrorInfo = JsonUtility.FromJson<MirrorInfo>(info);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision.gameObject.GetComponent<Ball>();

        if (tb != null)
        {

            Vector3 vec = Vector3.zero;
            Vector3 vec2 = tb.rigid.velocity;

            //print(collision.contacts[0].normal);
            tb.transform.position = transform.position;
            vec = Vector3.Cross(Vector3.forward, vec2).normalized;

            int co = 1;

            switch (wallDirection)
            {
                case TileDirection.UP:
                case TileDirection.RIGHT:
                    co = -1;
                    break;
            }

            tb.Move(vec * co, 5);
        }

    }


}