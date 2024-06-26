using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour, IPoolableComponent
{
    public List<Sprite> cloudImgs;

    public void Despawned()
    {
        
    }

    public Sprite GetSprite()
    {
        int a = UnityEngine.Random.Range(0, cloudImgs.Count);
        return cloudImgs[a];
    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }

    public void Spawned()
    {
    }
}
