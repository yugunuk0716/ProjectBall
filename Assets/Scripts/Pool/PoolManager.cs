using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("PoolManager");
                obj.AddComponent<PoolManager>();
                obj.transform.position = new Vector3(1000f, 1000f);
                instance = obj.GetComponent<PoolManager>();
            }

            return instance;
        }
    }

    private Dictionary<string, Pool<PoolableMono>> pools = new Dictionary<string, Pool<PoolableMono>>();



    public void CreatePool(PoolableMono prefab, string name = null, int count = 2)
    {
        if (name == null)
            name = prefab.gameObject.name;
        else
            prefab.gameObject.name = name;

        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, transform, count);
        pools.Add(name, pool);
    }



    public PoolableMono Pop(string prefabName)
    {
        if (!pools.ContainsKey(prefabName))
        {
            Debug.LogError("Prefab doesnt exist on pool");
            Debug.Log(prefabName);
            return null;
        }

        PoolableMono item = pools[prefabName].Pop();
        item.Reset();
        return item;
    }



    public void Push(PoolableMono obj)
    {
        //print($"{obj.name} ??????");

        pools[obj.name].Push(obj);
        obj.transform.SetParent(this.transform);
    }




}