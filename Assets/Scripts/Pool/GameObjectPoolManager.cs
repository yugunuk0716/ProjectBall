using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameObjectPoolManager : MonoBehaviour
{
    private static GameObjectPoolManager _instance = null;
    public static GameObjectPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = FindObjectOfType<GameObjectPoolManager>();
                if (obj != null)
                {
                    _instance = obj;
                }
                else
                {
                    var new_obj = new GameObject();
                    _instance = new_obj.AddComponent<GameObjectPoolManager>();
                    new_obj.name = _instance.GetType().Name;
                    _instance.CreateRecycledObjectContainer();
                    _instance.Is_Alive = true;
                }

            }
            return _instance;
        }
    }

    public bool Is_Alive { get; private set; } = false;


    Transform Recycled_Object_Container;



    Dictionary<string, PoolManager> Pool_List = new Dictionary<string, PoolManager>();



    void CreateRecycledObjectContainer()
    {
        if (Recycled_Object_Container == null)
        {
            var container = new GameObject();
            container.transform.SetParent(this.transform);
            container.SetActive(false);
            container.name = nameof(Recycled_Object_Container);
            Recycled_Object_Container = container.GetComponent<Transform>();
        }
    }

    public GameObject GetGameObject(string path, Transform parent)
    {
        if (!Is_Alive)
        {
            return null;
        }
        string name = System.IO.Path.GetFileNameWithoutExtension(path);
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        PoolManager p = null;
        if (Pool_List.ContainsKey(name))
        {
            p = Pool_List[name];
        }
        else
        {
            p = new PoolManager(Recycled_Object_Container);
            Pool_List.Add(name, p);
        }

        if (p != null)
        {
            return p.Pop(path, parent);
        }
        return null;
    }

    public void PreloadGameObject(string path)
    {
        if (!Is_Alive)
        {
            return;
        }
        string name = System.IO.Path.GetFileNameWithoutExtension(path);
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        PoolManager p = null;
        if (Pool_List.ContainsKey(name))
        {
            p = Pool_List[name];
        }
        else
        {
            p = new PoolManager(Recycled_Object_Container);
            Pool_List.Add(name, p);
        }
        if (p != null)
        {
            //  todo
            p.PreLoad(path);
        }
    }


    public void UnusedGameObject(GameObject obj, bool is_out_move = true)
    {
        if (!Is_Alive)
        {
            return;
        }
        PoolManager p = null;
        string name = obj.name;
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        if (Pool_List.ContainsKey(name))
        {
            p = Pool_List[name];
        }
        else
        {
            p = new PoolManager(Recycled_Object_Container);
            Pool_List.Add(name, p);
        }
        if (p != null)
        {
            p.Push(obj, is_out_move);
        }
        else
        {
            Debug.Assert(false);
        }

    }

    [ContextMenu("ConsoleLogToStringGameObjectPoolManager")]
    public void ToStringGameObjectPoolManager()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("Pool_List Count [{0}]", Pool_List.Count).AppendLine();
        sb.AppendLine("==========");
        foreach (var item in Pool_List)
        {
            var p = item.Value;
            sb.AppendFormat("[{0}] => {1}", item.Key, p.ToString()).AppendLine();
            sb.AppendLine("==========");
        }

        Debug.Log(sb.ToString());
    }

    public void OnDestroy()
    {
        foreach (var item in Pool_List)
        {
            var p = item.Value;
            p.Dispose();
        }
        Pool_List.Clear();
        Is_Alive = false;
        Resources.UnloadUnusedAssets(); 
    }
}