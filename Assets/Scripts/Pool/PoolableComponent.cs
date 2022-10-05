using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prefab으로 사용할 GameObject의 MonoBehavior와 같이 상속 받아서 구현해주면 된다.
/// 해당 Interface를 사용할 경우 생성/삭제시 해당 Spawned() / Despawned()를 자동 호출해주어
/// 초기화 및 삭제시 해야하는 작업을 추가해 줄 수 있다.
/// </summary>
public interface IPoolableComponent
{
    public abstract void Despawned ();
    public abstract void Spawned   ();
    public abstract void SetDisable();
}