using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricObj : MonoBehaviour
{
    private void Awake()
    {
        Vector3 myPos = transform.position;
        myPos.z = transform.position.y * -0.1f;
    }
}
