using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSorter : MonoBehaviour
{
    public bool isOnce;
    private Renderer rd;

    private void Awake()
    {
        rd = GetComponent<Renderer>();
        Sort();
    }

    private void LateUpdate()
    {
        if (!isOnce)
        {
            Sort();
        }
    }

    private void Sort()
    {
        float precisionMultiplier = 10.0f; // Sorting Order Á¤¹Ðµµ ¿ë
        rd.sortingOrder = (int)(-transform.position.y * precisionMultiplier);
    }
}
