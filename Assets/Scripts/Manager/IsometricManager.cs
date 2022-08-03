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


        UpdateState(eUpdateState.Init);
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
