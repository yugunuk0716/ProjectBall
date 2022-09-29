using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public int count = 2;
    public bool isUsing = false;

    public abstract void Reset();
}