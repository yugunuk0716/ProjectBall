using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IsometricManager : MonoBehaviour
{
    

    private static IsometricManager instance;
    public static IsometricManager Instance
    {
        get
        {
            return instance;
        }
    }

    public List<ManagerBase> managers;

    private void Awake()
    {
        instance = this;

        managers.Add(GetComponent<UIManager>());

        managers.Add(gameObject.AddComponent<GameManager>());
        managers.Add(gameObject.AddComponent<StageManager>());
        managers.Add(gameObject.AddComponent<SaveManager>());

        Application.targetFrameRate = 300;
        
        UpdateState(eUpdateState.Init);
    }

    public static Vector2 GetIsoDir(TileDirection dir) // 등각투형에 걸맞는 벡터로..
    {
        Vector2 vec = Vector2.zero;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                vec = Vector2.right;
                break;
            case TileDirection.LEFTDOWN:
                vec = Vector2.left;
                break;
            case TileDirection.LEFTUP:
                vec = Vector2.up;
                break;
            case TileDirection.RIGHTDOWN:
                vec = Vector2.down;
                break;
        }
        return vec;
    }

    public static Vector2 GetRealDir(TileDirection dir)
    {
        Vector2 vec = Vector2.zero;
        switch (dir)
        {
            case TileDirection.RIGHTUP:
                vec = new Vector2(1,1);
                break;
            case TileDirection.LEFTDOWN:
                vec = new Vector2(-1, -1);
                break;
            case TileDirection.LEFTUP:
                vec = new Vector2(-1, 1);
                break;
            case TileDirection.RIGHTDOWN:
                vec = new Vector2(1, -1);
                break;
        }
        return vec;
    }

    public void UpdateState(eUpdateState state)
    {
        foreach (var item in managers)
        {
            item.UpdateState(state);
        }
    }

    public T GetManager<T>() where T : ManagerBase
    {
        var value = default(T);

        foreach (var component in managers.OfType<T>())
            value = component;

        return value;
    }
}
