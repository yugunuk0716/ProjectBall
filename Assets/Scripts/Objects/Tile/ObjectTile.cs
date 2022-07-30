using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
    JumpPad,
    Slow,
    Teleporter,
    Goal,
    DirectionChanger,
    Reflect,
    Wall,
    None
}

[System.Serializable]
public enum TileDirection
{
    LEFTUP,
    LEFTDOWN,
    RIGHTUP,
    RIGHTDOWN
}

[System.Serializable]
public class ObjectTileInfo
{
    public int tileType;
}

public abstract class ObjectTile : PoolableMono
{
    private void Awake()
    {
        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f;
        transform.position = myPos;
    }

    public TileType myType;
    //public TileDirection myDirection;

    public virtual string ParseTileInfo()
    {
        return string.Empty;
    }

    public virtual void SettingTile(string info)
    {
        IsometricManager.Instance.GetManager<StageManager>().objectTileList.Add(this);
    }

    public abstract void OnTriggerBall(Ball tb); // 공에 무엇을 해줄까요?


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball tb = collision?.GetComponent<Ball>();

        if(tb != null)
        {
            if (tb.collisionTileType == this.myType)
            {
                switch (tb.ballState)
                {
                    case BallState.Destroy:
                        Destroy(this.gameObject);
                        OnTriggerBall(tb);
                        break;

                    case BallState.Ignore:
                        return;
                }
                tb.GetComponent<Ball>().RemoveSpecialEffect();
            }
            else
            {
                OnTriggerBall(tb);
            }
        }
        
    }
}
