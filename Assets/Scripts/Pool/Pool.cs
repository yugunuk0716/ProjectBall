using System;
using System.Collections.Generic;
using UnityEngine;

class Pool<T> where T : PoolableMono
{
    private Queue<T> _pool = new Queue<T>();
    private T _prefab; //???????? ????
    private Transform _parent;

    public Pool(T prefab, Transform parent, int count = 2)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < count; i++)
        {
            T obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");  //??????? ??? ????????? ??? ?? ?? ????.
            if (obj.CompareTag("Ball")) obj.gameObject.name += i.ToString();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public T Pop()
    {
        T obj = null;
        if (_pool.Count <= 0)
        {
            obj = GameObject.Instantiate(_prefab, _parent);
            obj.gameObject.SetActive(true);
            obj.gameObject.name = obj.gameObject.name.Replace("(Clone)", "");

        }
        else
        {
            obj = _pool.Dequeue();
            if(obj.isUsing)
            {
                Push(obj);
                obj = Pop();
            }
            
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
}