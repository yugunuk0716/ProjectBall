using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        PoolManager.Instance.Push(tb);
    }

    public override void Reset()
    {

    }
}
