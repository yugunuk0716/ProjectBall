using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class None : ObjectTile
{
    public override void InteractionTile(Ball tb)
    {
        tb.SetMove();
    }

    public override void Reset()
    {

    }
}
