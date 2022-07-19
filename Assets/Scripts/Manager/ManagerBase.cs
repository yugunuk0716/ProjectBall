using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
    public abstract void Init();
    public abstract void UpdateState(eUpdateState state);
}
