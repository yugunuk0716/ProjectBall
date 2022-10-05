using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Code by ForestJ (https://forestj.tistory.com)
/// PoolManager는 프래팹의 이름으로 구분한다.
/// 각 프리팹의 이름을 유니크하게 설정하여 사용하면 중복되지 않는다.
/// </summary>
public class PoolManager : IDisposable
{
    Transform Recycled_Object_Container;

    /// <summary>
    /// 매번 같은 프리팹 리소스를 불러 오는 것이 아니라 한번 가져왔던 prefab 리소스는 저장해 두었다가,
    /// 재사용 요청시 해당 prefab 리소스를 재사용 한다.
    /// </summary>
    GameObject Object_Prefab;

    /// <summary>
    /// 재사용할 게임 오브젝트를 담아두는 곳
    /// </summary>
    Stack<GameObject> Recycled_Objects = new Stack<GameObject>();

    /// <summary>
    /// 비활성시 필요에 따라 해당 GameObject의 위치를 화면 밖으로 이동시킨다.
    /// </summary>
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
                //  관리되는 자원 해제
                Clear();
            }
            disposed = true;
        }
    }

    public PoolManager(Transform recycled_container)
    {
        this.Recycled_Object_Container = recycled_container;
    }
    /// <summary>
    /// GameObject 를 찾아서 반환.
    /// 해당 GameObject가 없을 경우 생성하여 반환한다.
    /// Object_Prefab 도 재사용 가능하도록 추가한다.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
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
        //  prefab info 가 null 이면 새로 생성. 재사용 위해
        if (Object_Prefab == null)
        {
            Object_Prefab = Resources.Load<GameObject>(path);
        }
        //  신규 생성
        item = GameObject.Instantiate<GameObject>(Object_Prefab, Recycled_Object_Container);
        //  GameObject 이름 변경
        if (!string.IsNullOrEmpty(name))
        {
            item.name = name;
        }
        //  재사용 리스트에 등록
        Recycled_Objects.Push(item);
    }

    /// <summary>
    /// 사용하지 않는 GameObject를 반환한다.
    /// </summary>
    /// <param name="obj">반환할 GameObject</param>
    /// <param name="is_out_move">반환시 화면 밖으로 이동 시킬지 여부</param>
    public void Push(GameObject obj, bool is_out_move)
    {
        if (object.ReferenceEquals(obj.transform.parent, Recycled_Object_Container))
        {
            //  already in pool
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


    /// <summary>
    /// 모든 GameObject 삭제
    /// Prefab Object도 null 처리해준다.
    /// </summary>
    void Clear()
    {
        //  모두 클리어
        //while (Recycled_Objects.Count > 0)
        //{
        //    var obj = Recycled_Objects.Pop();
        //    GameObject.DestroyImmediate(obj);
        //}
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