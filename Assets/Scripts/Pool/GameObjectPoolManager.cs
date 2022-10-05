using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Code by ForestJ (https://forestj.tistory.com)
/// 미리 GameObject를 생성해둘 필요는 없으며,
/// 외부에서 Instance를 호출하면 Instance 생성 여부에 따라 자동으로 GameObjectPoolMananger를 Sigleton으로 생성해 준다.
/// 필요한 경우 각 씬마다 1개씩 생성하여 사용이 가능
/// DontDestory 모드가 아니기 때문에 씬 변경시 모두 삭제됨
/// </summary>
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

    /// <summary>
    /// 회수된 오브젝트를 보관 할 컨테이너
    /// </summary>
    Transform Recycled_Object_Container;


    /// <summary>
    /// Object Pool 리스트
    /// </summary>
    Dictionary<string, PoolManager> Pool_List = new Dictionary<string, PoolManager>();


    /// <summary>
    /// 회수 후 보관할 컨테이너 생성
    /// </summary>
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
    /// <summary>
    /// path의 prefab을 GameObject로 생성 반환.
    /// path를 이용하여 name을 찾고, name 별로 PoolManager를 보관하고 있으며,
    /// 해당 PoolManager에 GameObject가 존재할 경우 찾아서 반환
    /// PoolManager가 없거나 GameObject가 없을 경우 새로 생성하여 반환한다
    /// PoolManager를 새로 생성할 때는 name을 Key로 사용하여 Map에 저장하여 재사용 할 수 있도록 한다.
    /// </summary>
    /// <param name="path">Resources 아래의 Prefab Path</param>
    /// <param name="parent">GameObject 의 상위 Transform</param>
    /// <returns></returns>
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
    /// <summary>
    /// path의 prefab을 GameObject로 생성하여 재사용 컨테이너에 저장해둔다.
    /// 게임내에서 사용하기 전에 미리 생성해두는 것으로, 실시간 생성시 노드가 발생할 수 있으니,
    /// Loading 화면에서 미리 로드할 수있는 GameObject를 알고 있다면 미리 로드하는 것도 좋다.
    /// </summary>
    /// <param name="path"></param>
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

    /// <summary>
    /// GameObject 회수
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="is_out_move">화면밖으로 강제 이동 시킬 것인지 여부. 기본값은 true</param>
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
        Resources.UnloadUnusedAssets(); //  Resources로 호출되었던 사용하지 않는 에셋만 해제
    }
}