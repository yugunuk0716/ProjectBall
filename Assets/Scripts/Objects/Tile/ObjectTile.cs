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
    None,
}

[System.Serializable]
[System.Flags]
public enum TileDirection
{
    LEFTUP = 1,
    LEFTDOWN = 2,
    RIGHTUP = 4,
    RIGHTDOWN = 8,
}

[System.Serializable]
public class ObjectTileInfo
{
    public int tileType;
}

public abstract class ObjectTile : PoolableMono
{
    public string dataString;
    public TileType myType;
    public Vector2 worldPos;
    public Vector2 index;


    protected virtual void Awake()
    {
        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f;
        transform.position = myPos;
    }

    public virtual string ParseTileInfo()
    {
        return string.Empty;
    }

    public virtual void SettingTile(string info)
    {
        IsometricManager.Instance.GetManager<StageManager>().objectTileList.Add(this);
    }

    public abstract void InteractionTile(Ball tb); // 공에 무엇을 해줄까요?

    public void CheckTile(Ball tb)
    {
        if (tb.collisionTileType == this.myType)
        {
            switch (tb.ballState)
            {
                case BallState.Destroy:
                    Destroy(this.gameObject);
                    InteractionTile(tb);
                    break;

                case BallState.Ignore:
                    break;
            }
            tb.GetComponent<Ball>().RemoveSpecialEffect();
        }
        else
        {
            InteractionTile(tb);
        }
    }

    public virtual void SetDirection()
    {
        if (dataString.Equals(string.Empty))
        {
            return;
        }
    }
}
