using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        Destroy(tb.gameObject);
    }
    public override void Reset()
    {

    }
}
