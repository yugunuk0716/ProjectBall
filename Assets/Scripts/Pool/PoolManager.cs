using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PoolManager : IDisposable
{
    Transform Recycled_Object_Container;


    GameObject Object_Prefab;


    Stack<GameObject> Recycled_Objects = new Stack<GameObject>();

    Vector2 Remove_Position = new Vector2(-100000, -100000);

    private bool disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Clear();
            }
            disposed = true;
        }
    }

    public PoolManager(Transform recycled_container)
    {
        this.Recycled_Object_Container = recycled_container;
    }

    public GameObject Pop(string path, Transform parent)
    {
        string name = System.IO.Path.GetFileNameWithoutExtension(path);
        GameObject item = null;
        if (Recycled_Objects.Count > 0)
        {
            item = Recycled_Objects.Pop();
            item.gameObject.SetActive(true);
            item.transform.SetParent(parent);
        }

        if (item == null)
        {
            if (Object_Prefab == null)
            {
                Object_Prefab = Resources.Load<GameObject>(path);
            }

            item = GameObject.Instantiate<GameObject>(Object_Prefab, parent);
            if (!string.IsNullOrEmpty(name))
            {
                item.name = name;
            }
        }

        var component = item.GetComponent<IPoolableComponent>();
        if (component != null)
        {
            component.Spawned();
        }

        return item;
    }

    public void PreLoad(string path)
    {
        string name = System.IO.Path.GetFileNameWithoutExtension(path);
        GameObject item = null;

        if (Object_Prefab == null)
        {
            Object_Prefab = Resources.Load<GameObject>(path);
        }

        item = GameObject.Instantiate<GameObject>(Object_Prefab, Recycled_Object_Container);

        if (!string.IsNullOrEmpty(name))
        {
            item.name = name;
        }

        Recycled_Objects.Push(item);
    }


    public void Push(GameObject obj, bool is_out_move)
    {
        if (object.ReferenceEquals(obj.transform.parent, Recycled_Object_Container))
        {

            return;
        }
        var component = obj.GetComponent<IPoolableComponent>();
        if (component != null)
        {
            component.Despawned();
        }
        obj.transform.SetParent(Recycled_Object_Container);
        if (is_out_move)
        {
            obj.transform.position = Remove_Position;
        }
        obj.gameObject.SetActive(false);
        Recycled_Objects.Push(obj);
    }



    void Clear()
    {

        Recycled_Objects.Clear();

        Object_Prefab = null;
    }

    public override string ToString()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("PoolManager : [{0}]", Recycled_Objects.Count).AppendLine();

        foreach (var item in Recycled_Objects)
        {
            sb.AppendFormat("[{0}]", item.name).AppendLine();
        }

        return sb.ToString();
    }
}