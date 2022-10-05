

using UnityEngine;

public class TargetPointUI : MonoBehaviour, IPoolableComponent
{
    public void Despawned()
    {
        
    }

    public void SetDisable()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(gameObject);
    }

    public void Spawned()
    {
    }
}
