using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
    JumpPad,
    Slow,
    Teleporter,
    Flag,
    DirectionChanger,
    Reflect,
    Thorn,
    None,
    ButtonTile,
    Line
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

public class ObjectTileComparer : IEqualityComparer<ObjectTile>
{
    public bool Equals(ObjectTile x, ObjectTile y)
    {
        return x.myType.Equals(y.myType);
    }

    public int GetHashCode(ObjectTile obj)
    {
        return 0;
    }
}

public abstract class ObjectTile : MonoBehaviour, IPoolableComponent
{
    public string dataString;
    public TileType myType;
    public Vector2 worldPos;
    public Vector2 keyPos;
    public Vector3Int gridPos;
    public int btnIndex;
    public string btnString;

    public bool isTransitionTile = false;

    public void StartTransition()
    {
        StartCoroutine(Transition());
        isTransitionTile = true;
    }
    public abstract IEnumerator Transition();


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

    public abstract void InteractionTile(Ball tb);

    public virtual void SetDirection()
    {
        if (dataString.Equals(string.Empty))
        {
            return;
        }
    }

    public void Despawned()
    {
        StopCoroutine(Transition());
    }

    public void Spawned()
    {
        
    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
        isTransitionTile = false;
    }


}
