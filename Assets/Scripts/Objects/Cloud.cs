using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : PoolableMono
{
    public List<Sprite> cloudImgs;

    public override void Reset()
    {
    }

    public Sprite GetSprite()
    {
        int a = UnityEngine.Random.Range(0, cloudImgs.Count);
        return cloudImgs[a];
    }
}
