using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroyParticle : MonoBehaviour, IPoolableComponent
{
    public ParticleSystem ps;

    public void Despawned()
    {
        
    }

    public void PlayParticle()
    {
        ps.Play();

    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }

    public void Spawned()
    {
    }
}
