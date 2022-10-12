using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPoolableComponent
{
    public abstract void Despawned ();
    public abstract void Spawned   ();
    public abstract void SetDisable();
}